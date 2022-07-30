using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.Globalization;
using System.Reflection;

namespace Api.Controllers.CodeControllers;

#pragma warning disable CS1998, IDE0051
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

        // TODO: stocker sur un container et renvoyer le lien ? Oui carrément en fait.
        return Ok(new PipelineResult()
        {
            Output = this.ReadBytes(fileStream),
        });
    }

    private void Pitch(Stream fileStream, float rate)
    {
        using (Mp3FileReader fileReader = new Mp3FileReader(fileStream))
        {
            SmbPitchShiftingSampleProvider pitch = new SmbPitchShiftingSampleProvider(fileReader.ToSampleProvider());
            pitch.PitchFactor = rate;

            // TODO: virer ce code quand les tests seront terminés
            WaveOutEvent device = new WaveOutEvent();
            device.Init(pitch.Take(TimeSpan.FromSeconds(fileReader.TotalTime.TotalSeconds)));
            device.Play();
            Thread.Sleep((int)fileReader.TotalTime.TotalSeconds * 1000);
        }
    }

    private byte[] ReadBytes(Stream input)
    {
        byte[] result = new byte[16 * 1024];
        using (MemoryStream ms = new MemoryStream())
        {
            int read;
            while ((read = input.Read(result, 0, result.Length)) > 0)
            {
                ms.Write(result, 0, read);
            }
            return ms.ToArray();
        }
    }
}
