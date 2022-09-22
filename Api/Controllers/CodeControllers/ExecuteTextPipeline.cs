using System.Diagnostics;

namespace Api.Controllers.CodeControllers;

public partial class CodeController
{
    [HttpPost("textPipeline")]
    public async Task<ActionResult<PipelineResult>> ExecuteTextPipeline(
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
        
        Guid fileGuid = Guid.NewGuid();
        string filepath = new DirectoryInfo(Directory.GetCurrentDirectory()) + $@"/Files/TextPipeline_{fileGuid}";
        string result = Encoding.Default.GetString(fileBytes);
        
        foreach (string script in scripts.Split("."))
        {
            await System.IO.File.WriteAllTextAsync(filepath, result);

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
                result = await resultStream.ReadToEndAsync();
            }
        }

        System.IO.File.Delete(filepath);

        return Ok(new PipelineResult()
        {
            Output = Encoding.Default.GetBytes(result),
        });
    }
}
