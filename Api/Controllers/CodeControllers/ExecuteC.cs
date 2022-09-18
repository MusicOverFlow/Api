using System.Diagnostics;
using System.Text;

namespace Api.Controllers.CodeControllers;

public partial class CodeController
{
    [HttpPost("c")]
    public async Task<ActionResult> HandleC()
    {
        string script = string.Empty;
        using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
        {
            script = await reader.ReadToEndAsync();
        }
        
        Guid guid = Guid.NewGuid();
        string scriptFilepath = new DirectoryInfo(Directory.GetCurrentDirectory()) + $@"/Files";
        await System.IO.File.WriteAllTextAsync(scriptFilepath + $@"/codeSample_{guid}.c", script);

        Process process = Process.Start(new ProcessStartInfo()
        {
            FileName = "docker",
            Arguments = $"run --rm -v {scriptFilepath}:/files gtouchet/pa-script-execution c {guid}",
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardOutput = true,
        });

        string result = string.Empty;
        using (StreamReader resultStream = process.StandardOutput)
        {
            result = await resultStream.ReadToEndAsync();
        }

        System.IO.File.Delete(scriptFilepath + $@"/codeSample_{guid}.c");

        return Ok(result);
    }
}
