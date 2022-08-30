using NAudio.Wave;

namespace Api.Controllers.CodeControllers;

[ApiController]
[Route("api/converter")]
#pragma warning disable CS1998
public class Converter : ControllerBase
{
    private readonly Blob blob;

    public Converter(Blob blob)
    {
        this.blob = blob;
    }

    [HttpPost]
    public async Task<ActionResult<string>> Convert(
        [FromForm] string format,
        [FromForm] byte[] fileInput)
    {
        format = format.ToLower();

        Stream fileStream = new MemoryStream();
        IFormFile file = null;
        if (Request != null)
        {
            file = Request.Form.Files.GetFile(nameof(fileInput));
            if (file == null)
            {
                return BadRequest();
            }

            if (!this.IsInputFormatValid(file.FileName) || !this.IsWantedFormatValid(format))
            {
                return BadRequest(new { error = "Wrong input file format or wanted output format" });
            }

            if (Path.GetExtension(file.FileName).Equals(format))
            {
                return BadRequest(new { error = $"Input file format is already a {format}" });
            }

            fileStream = file.OpenReadStream();
        }

        string fileFormat = Path.GetExtension(file.FileName);
        switch (format)
        {
            case ".wav":
                if (fileFormat.Equals(".mp3"))
                {
                    this.ConvertMp3ToWav(ref fileStream);
                }
                else if (fileFormat.Equals(".mp4"))
                {
                    this.ConvertMp4ToWav(ref fileStream);
                }
                break;
            case ".mp3":
                if (fileFormat.Equals(".wav"))
                {
                    this.ConvertWavToMp3(ref fileStream);
                }
                else if (fileFormat.Equals(".mp4"))
                {
                    this.ConvertMp4ToMp3(ref fileStream);
                }
                break;
        }

        return Ok(new { Output = Blob.GetConvertedSoundUrl(
            sound: this.ReadBytes(fileStream),
            filename: Path.GetFileNameWithoutExtension(file.FileName) + format).Result });
    }

    private void ConvertMp3ToWav(ref Stream fileStream)
    {
        using (Mp3FileReader mp3Sound = new Mp3FileReader(fileStream))
        {
            this.ConvertAs("wav", mp3Sound, ref fileStream);
        }
    }

    private void ConvertWavToMp3(ref Stream fileStream)
    {
        using (WaveFileReader wavSound = new WaveFileReader(fileStream))
        {
            this.ConvertAs("mp3", wavSound, ref fileStream);
        }
    }

    private void ConvertMp4ToMp3(ref Stream fileStream)
    {
        using (StreamMediaFoundationReader video = new StreamMediaFoundationReader(fileStream))
        {
            this.ConvertAs("mp3", video, ref fileStream);
        }
    }

    private void ConvertMp4ToWav(ref Stream fileStream)
    {
        using (StreamMediaFoundationReader video = new StreamMediaFoundationReader(fileStream))
        {
            this.ConvertAs("wav", video, ref fileStream);
        }
    }

    private void ConvertAs(string format, WaveStream input, ref Stream fileStream)
    {
        using (WaveStream pcmStream = WaveFormatConversionStream.CreatePcmStream(input))
        {
            Guid guid = Guid.NewGuid();
            WaveFileWriter.CreateWaveFile(new DirectoryInfo(Directory.GetCurrentDirectory()) + $@"/Files/{guid}.{format}", pcmStream);
            fileStream = new FileStream(new DirectoryInfo(Directory.GetCurrentDirectory()) + $@"/Files/{guid}.{format}", FileMode.Open);
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

    private bool IsInputFormatValid(string filepath)
    {
        string format = Path.GetExtension(filepath);
        return format == ".mp3" || format == ".wav" || format == ".mp4";
    }
    
    private bool IsWantedFormatValid(string format)
    {
        return format == ".mp3" || format == ".wav";
    }
}
