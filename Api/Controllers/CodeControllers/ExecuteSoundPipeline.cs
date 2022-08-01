using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.Globalization;
using System.Reflection;

namespace Api.Controllers.CodeControllers;

#pragma warning disable CS1998, IDE0051
public partial class CodeController
{
    /// <summary>
    /// <b>Available scripts :</b><br/>
    /// Pitch-X (ex: Pitch-1,25)<br/>
    /// FadeIn-X (s)<br/>
    /// FadeOut-X (s)<br/>
    /// </summary>
    /// <param name="scripts"></param>
    /// <param name="fileInput"></param>
    /// <returns></returns>
    [HttpPost("soundPipeline")]
    public async Task<ActionResult<string>> ExecuteSoundPipeline(
        [FromForm] string scripts,
        [FromForm] byte[] fileInput)
    {
        Stream fileStream = new MemoryStream();
        IFormFile file = null;
        if (Request != null)
        {
            file = Request.Form.Files.GetFile(nameof(fileInput));
            if (file == null)
            {
                return BadRequest();
            }
            fileStream = file.OpenReadStream();
        }
        
        if (Path.GetExtension(file.FileName).Equals(".mp3"))
        {
            this.ConvertMp3ToWav(ref fileStream);
        }
        
        Guid fileGuid = Guid.NewGuid();
        string filepath = new DirectoryInfo(Directory.GetCurrentDirectory()) + $@"/Files/SoundPipeline_{fileGuid}";

        int scriptCount = 0;
        foreach (string script in scripts.Split("."))
        {
            string methodToCall = script.Split("-")[0];
            MethodInfo method = this.GetType().GetMethod(methodToCall, BindingFlags.NonPublic | BindingFlags.Instance);

            if (method == null)
            {
                return BadRequest(new { error = $"Unknown script '{methodToCall}'" });
            }

            method.Invoke(this, script.Contains("-") ?
                new object[] { fileStream, float.Parse(script.Replace(",", ".").Split("-")[1], CultureInfo.InvariantCulture), $"{filepath}{scriptCount}" } :
                new object[] { });
            
            fileStream = new FileStream($"{filepath}{scriptCount}", FileMode.Open);
            scriptCount += 1;
        }

        return Ok(new { Output = this.blob.GetPipelineSoundUrl(sound: this.ReadBytes(fileStream), file.FileName.Replace(".mp3", ".wav")).Result });
    }
    
    private void Pitch(Stream fileStream, float rate, string filepath)
    {
        using (WaveFileReader fileReader = new WaveFileReader(fileStream))
        {
            SmbPitchShiftingSampleProvider pitch = new SmbPitchShiftingSampleProvider(fileReader.ToSampleProvider());
            pitch.PitchFactor = rate;

            WaveFileWriter.CreateWaveFile(filepath, pitch.ToWaveProvider());
        }
    }

    private void FadeIn(Stream fileStream, float time, string filepath)
    {
        using (WaveFileReader fileReader = new WaveFileReader(fileStream))
        {
            FadeInOutSampleProvider fade = new FadeInOutSampleProvider(fileReader.ToSampleProvider());
            fade.BeginFadeIn(time * 1_000);

            WaveFileWriter.CreateWaveFile(filepath, fade.ToWaveProvider());
        }
    }

    private void FadeOut(Stream fileStream, float time, string filepath)
    {
        using (WaveFileReader fileReader = new WaveFileReader(fileStream))
        {
            FadeInOutSampleProvider fade = new FadeInOutSampleProvider(fileReader.ToSampleProvider());
            fade.BeginFadeOut(time * 1_000);

            WaveFileWriter.CreateWaveFile(filepath, fade.ToWaveProvider());
        }
    }

    private void ConvertMp3ToWav(ref Stream fileStream)
    {
        using (Mp3FileReader reader = new Mp3FileReader(fileStream))
        {
            using (WaveStream pcmStream = WaveFormatConversionStream.CreatePcmStream(reader))
            {
                Guid guid = Guid.NewGuid();
                WaveFileWriter.CreateWaveFile(new DirectoryInfo(Directory.GetCurrentDirectory()) + $@"/Files/{guid}.wav", pcmStream);
                fileStream = new FileStream(new DirectoryInfo(Directory.GetCurrentDirectory()) + $@"/Files/{guid}.wav", FileMode.Open);
            }
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
