using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Reflection;

namespace Api.Controllers.CodeControllers;

#pragma warning disable CS1998
public partial class CodeController
{
    /// <summary>
    /// Available scripts : Grayscale Invert Rotate45 Rotate90 FlipHorizontal FlipVertical
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
            IImageFormat format;
            Image scriptResult = (Image) this.GetType()
                .GetMethod(script, BindingFlags.NonPublic | BindingFlags.Instance)
                .Invoke(this, new[] { Image.Load<Rgba32>(fileBytes, out format) });
            
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

    private Image Rotate45(Image image)
    {
        image.Mutate(i => i.Rotate(45));
        return image;
    }

    private Image Rotate90(Image image)
    {
        image.Mutate(i => i.Rotate(90));
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
}
