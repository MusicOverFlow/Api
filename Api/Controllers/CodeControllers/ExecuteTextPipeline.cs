using System.Diagnostics;
using System.Text;

namespace Api.Controllers.CodeControllers;

public partial class CodeController
{
    [HttpPost("textPipeline")]
    public ActionResult<PipelineResult> ExecuteTextPipeline(
        [FromForm] string scripts,
        [FromForm] byte[] fileInput)
    {
        PipelineResult pipelineResult = new PipelineResult();

        byte[] fileBytes = null;
        if (Request != null)
        {
            IFormFile file = Request.Form.Files.GetFile(nameof(fileInput));

            if (file == null || file.Length == 0)
            {
                return BadRequest("nike");
            }
            
            using (MemoryStream ms = new MemoryStream())
            {
                file.CopyTo(ms);
                fileBytes = ms.ToArray();
            }
        }

        scripts.Split(".").ToList().ForEach(async script =>
        {
            Process compiler = Process.Start(new ProcessStartInfo()
            {
                FileName = "python",
                Arguments =
                    $"{new DirectoryInfo(Directory.GetCurrentDirectory())}" +
                    $"/Controllers/CodeControllers/PipelineScripts/Text/{script}.py " +
                    $"{Encoding.Default.GetString(fileBytes)}",
                RedirectStandardError = true,
                RedirectStandardOutput = true,
            });

            string result = string.Empty;
            using (StreamReader resultStream = compiler.StandardOutput)
            {
                result = await resultStream.ReadToEndAsync();
                Console.WriteLine(result);
            }
            pipelineResult.Output = Encoding.ASCII.GetBytes(result);
        });

        return Ok(pipelineResult);
    }
}
