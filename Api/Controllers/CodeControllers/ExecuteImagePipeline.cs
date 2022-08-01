using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Reflection;

namespace Api.Controllers.CodeControllers;

#pragma warning disable CS1998, IDE0051
public partial class CodeController
{
    /// <summary>
    /// <b>Available scripts :</b><br/>
    /// Grayscale<br/>
    /// Invert<br/>
    /// FlipHorizontal<br/>
    /// FlipVertical<br/>
    /// Blur<br/>
    /// Rotate-XXX (degrees)<br/>
    /// Resize-XXX (%)<br/>
    /// </summary>
    /// <param name="scripts"></param>
    /// <param name="fileInput"></param>
    /// <returns></returns>
    [HttpPost("imagePipeline")]
    public async Task<ActionResult<PipelineResult>> ExecuteImagePipeline(
        [FromForm] string scripts,
        [FromForm] byte[] fileInput)
    {
        byte[] fileBytes = null;
        if (Request != null)
        {
            IFormFile file = Request.Form.Files.GetFile(nameof(fileInput));
            if (file == null || file.Length == 0)
            {
                return BadRequest();
            }

            using (MemoryStream ms = new MemoryStream())
            {
                file.CopyTo(ms);
                fileBytes = ms.ToArray();
            }
        }
        
        foreach (string script in scripts.Split("."))
        {
            string methodToCall = script.Split("-")[0];
            MethodInfo method = this.GetType().GetMethod(methodToCall, BindingFlags.NonPublic | BindingFlags.Instance);
            
            if (method == null)
            {
                return BadRequest(new { error = $"Unknown script '{methodToCall}'" });
            }

            IImageFormat format;
            Image scriptResult = (Image) method.Invoke(this, script.Contains("-") ?
                    new object[] { Image.Load<Rgba32>(fileBytes, out format), int.Parse(script.Split("-")[1]) } :
                    new [] { Image.Load<Rgba32>(fileBytes, out format) });
            
            using (var ms = new MemoryStream())
            {
                scriptResult.Save(ms, format);
                fileBytes = ms.ToArray();
            }
        }

        return Ok(new PipelineResult()
        {
            Output = fileBytes,
        });
    }

    private Image Grayscale(Image image)
    {
        image.Mutate(i => i.Grayscale());
        return image;
    }

    private Image Invert(Image image)
    {
        image.Mutate(i => i.Invert());
        return image;
    }

    private Image FlipHorizontal(Image image)
    {
        image.Mutate(i => i.Flip(FlipMode.Horizontal));
        return image;
    }

    private Image FlipVertical(Image image)
    {
        image.Mutate(i => i.Flip(FlipMode.Vertical));
        return image;
    }

    private Image Blur(Image image)
    {
        image.Mutate(i => i.GaussianBlur());
        return image;
    }

    private Image Rotate(Image image, int degrees)
    {
        image.Mutate(i => i.Rotate(degrees));
        return image;
    }

    private Image Resize(Image image, int sizeMultiplierPercent)
    {
        if (sizeMultiplierPercent <= 0)
        {
            return image;
        }
        
        int newWidth = image.Width * sizeMultiplierPercent / 100;
        int newHeight = image.Height * sizeMultiplierPercent / 100;
        image.Mutate(i => i.Resize(newWidth, newHeight));
        return image;
    }
}
