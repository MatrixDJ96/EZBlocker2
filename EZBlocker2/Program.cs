using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Threading;
using System.Windows.Forms;

namespace EZBlocker2
{
    static class Program
    {
        // Instance ID and name
        public static Mutex mutex = null;
        public static string mutexName;

        // EZBlocker 2
        public static readonly string ezBlockerFullExe = Application.ExecutablePath.Replace("/", "\\");
        public static readonly string ezBlockerExe = Path.GetFileName(ezBlockerFullExe);

        // StringComparison
        public static StringComparison comp = StringComparison.OrdinalIgnoreCase;

        public static bool IsFirstInstance()
        {
            mutexName = ((GuidAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), false)[0]).Value.ToString();
            mutex = new Mutex(true, mutexName, out bool firstInstance);
            return firstInstance;
        }

        public static void CleanMutex()
        {
            if (mutex != null)
            {
                mutex.Close();
                mutex = null;
            }
        }

        public static void KillEZBlocker() => Process.GetCurrentProcess().Kill();

        public static void StartProcess(string filename, string arguments = null)
        {
            using (var process = new Process())
            {
                process.StartInfo.FileName = filename;
                process.StartInfo.Arguments = arguments ?? "";

                process.Start();
            }
        }

        public static string[] StartProcess(ProcessStartInfo startInfo, bool output = false)
        {
            using (var process = new Process())
            {
                process.StartInfo = startInfo;

                if (output)
                {
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                }

                process.Start();

                if (output)
                {
                    process.WaitForExit(); // wait before read output
                    string logFile = Path.GetTempFileName();
                    File.AppendAllText(logFile, process.StandardOutput.ReadToEnd());
                    return File.ReadAllLines(logFile);
                }
                else
                    return null;
            }
        }

        public static bool RestartEZBlocker(bool admin = false)
        {
            CleanMutex();
            try
            {
                CleanMutex();
                StartProcess(new ProcessStartInfo()
                {
                    FileName = ezBlockerFullExe,
                    UseShellExecute = true,
                    Verb = admin ? "runas" : ""
                });
                KillEZBlocker();
                return true; // useless but necessary (to compile XD)
            }
            catch
            {
                mutex = new Mutex(true, mutexName);
                return false;
            }
        }

        public static bool IsAdmin => new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);

        private static bool CompareFiles(byte[] first, byte[] second)
        {
            if (first.Length != second.Length)
                return false;

            var firstMD5 = MD5.Create().ComputeHash(first);
            var secondMD5 = MD5.Create().ComputeHash(second);

            for (int i = 0; i < firstMD5.Length; i++)
                if (firstMD5[i] != secondMD5[i])
                    return false;

            return true;
        }

        public static void ExtractFile(string path, byte[] bytes)
        {
            bool extract = false;
            if (File.Exists(path))
            {
                if (!CompareFiles(File.ReadAllBytes(path), bytes))
                {
                    File.Delete(path);
                    extract = true;
                }
            }
            else
                extract = true;
            if (extract)
                File.WriteAllBytes(path, bytes);
        }

        public static void DeleteFile(string path)
        {
            if (File.Exists(path))
                File.Delete(path);
        }

        [STAThread]
        static void Main()
        {
            if (IsFirstInstance())
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolTypeExtensions.Tls11 | SecurityProtocolTypeExtensions.Tls12; // fix connection to GitHub

                Application.Run(new UpdateForm());
                Application.Run(new MainForm());
            }
            else
                MessageBox.Show("There is already another instance running!", "EZBlocker 2", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
