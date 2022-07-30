using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using SoundTouch;
using System.Globalization;
using System.Reflection;

namespace Api.Controllers.CodeControllers;

public partial class CodeController
{
    [HttpPost("soundPipeline")]
    public async Task<ActionResult<PipelineResult>> ExecuteSoundPipeline(
        [FromForm] string scripts,
        [FromForm] byte[] fileInput)
    {
        Stream fileStream = new MemoryStream();
        if (Request != null)
        {
            IFormFile file = Request.Form.Files.GetFile(nameof(fileInput));
            if (file == null)
            {
                return BadRequest();
            }

            fileStream = file.OpenReadStream();
        }

        foreach (string script in scripts.Split("."))
        {
            string methodToCall = script.Split("-")[0];
            MethodInfo method = this.GetType().GetMethod(methodToCall, BindingFlags.NonPublic | BindingFlags.Instance);

            if (method == null)
            {
                return BadRequest(new { error = $"Unknown script '{methodToCall}'" });
            }

            method.Invoke(this, script.Contains("-") ?
                    new object[] { fileStream, float.Parse(script.Replace(",", ".").Split("-")[1], CultureInfo.InvariantCulture) } :
                    new object[] { });
        }

        return Ok(new PipelineResult()
        {
            Output = this.ReadByte(fileStream),
        });
    }

    private void Pitch(Stream fileStream, float rate)
    {
        using (Mp3FileReader fileReader = new Mp3FileReader(fileStream))
        {
            SmbPitchShiftingSampleProvider pitch = new SmbPitchShiftingSampleProvider(fileReader.ToSampleProvider());
            pitch.PitchFactor = rate;

            // Test code to see if the pitch is working
            // Headphone warning
            WaveOutEvent device = new WaveOutEvent();
            device.Init(pitch.Take(TimeSpan.FromSeconds(fileReader.TotalTime.TotalSeconds)));
            device.Play();
            Thread.Sleep((int)fileReader.TotalTime.TotalSeconds * 1000);
        }
    }

    private byte[] ReadByte(Stream input)
    {
        byte[] buffer = new byte[16 * 1024];
        using (MemoryStream ms = new MemoryStream())
        {
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                ms.Write(buffer, 0, read);
            }
            return ms.ToArray();
        }
    }
}
