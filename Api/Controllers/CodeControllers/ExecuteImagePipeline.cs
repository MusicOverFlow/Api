using System.Diagnostics;

namespace Api.Controllers.CodeControllers;

public partial class CodeController
{
    [HttpPost("imagePipeline")]
    public async Task<ActionResult<PipelineResult>> ExecuteImagePipeline(
        [FromForm] string scripts,
        [FromForm] byte[] fileInput)
    {
        byte[] fileBytes = null;
        IFormFile file = null;
        if (Request != null)
        {
            file = Request.Form.Files.GetFile(nameof(fileInput));
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

        Guid fileGuid = Guid.NewGuid();
        string filepath = new DirectoryInfo(Directory.GetCurrentDirectory()) + $@"/Files/ImagePipeline_{fileGuid}.{Path.GetExtension(file.FileName)}";
        
        foreach (string script in scripts.Split("."))
        {
            await System.IO.File.WriteAllBytesAsync(filepath, fileBytes);

            Process compiler = Process.Start(new ProcessStartInfo()
            {
                FileName = "python",
                Arguments =
                    $"{new DirectoryInfo(Directory.GetCurrentDirectory()).Parent}" +
                    $"/PipelineScripts/{script}.py " +
                    $"{filepath}",
                RedirectStandardError = true,
                RedirectStandardOutput = true,
            });

            using (StreamReader resultStream = compiler.StandardOutput)
            {
                Console.WriteLine(await resultStream.ReadToEndAsync());
            }
            using (StreamReader resultStream = compiler.StandardError)
            {
                Console.WriteLine(await resultStream.ReadToEndAsync());
            }

            fileBytes = await System.IO.File.ReadAllBytesAsync(filepath);
        }

        System.IO.File.Delete(filepath);
        
        return Ok(new PipelineResult()
        {
            Output = fileBytes,
        });
    }
}
