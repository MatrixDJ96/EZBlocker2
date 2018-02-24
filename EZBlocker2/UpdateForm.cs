using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;
using static EZBlocker2.Program;

namespace EZBlocker2
{
    public partial class UpdateForm : Form
    {
        // Resources
        private readonly string ezBlockerFullExeOld = ezBlockerFullExe + ".old";
        private readonly string _7zaFullExe = Application.StartupPath + @"\7za.exe";


        // Download stuff
        private readonly string website = "https://github.com/MatrixDJ96/EZBlocker2/releases/latest";
        private string updateFullFile = null;
        private Uri address = null;
        WebClient client = null;

        // Versions
        private string slatest = null;
        private string scurrent = null;
        private int ilatest = 0;
        private int icurrent = 0;

        // Form movement
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            double bytesIn = double.Parse(e.BytesReceived.ToString());
            double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
            double percentage = bytesIn / totalBytes * 100;

            Text = "Downloading: " + Math.Round(percentage, 2) + "%"; // title
            labelMessage.Text = Text;
            progressBar.Value = Convert.ToInt32(percentage);
        }

        void Client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                try
                {
                    bool success = false; // must be false at the beginning

                    ExtractFile(_7zaFullExe, Properties.Resources._7za);

                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        FileName = _7zaFullExe
                    };

                    string entry = null;
                    {
                        startInfo.Arguments = $"l \"{updateFullFile}\"";
                        List<string> lines = new List<string>(StartProcess(startInfo, true));

                        string line = lines.Find(x => x.IndexOf("ezblocker", comp) > -1 && x.Substring(x.Length - 4).Equals(".exe", comp));

                        if (line != null)
                            entry = line.Substring(line.IndexOf("ezblocker", comp));
                    }

                    if (entry != null)
                    {
                        File.Move(ezBlockerFullExe, ezBlockerFullExeOld);
                        DeleteFile(Application.StartupPath + $@"\{entry}");

                        startInfo.Arguments = $"e \"{updateFullFile}\" \"{entry}\"";
                        List<string> lines = new List<string>(StartProcess(startInfo, true));

                        if (lines.Find(x => x.ToLower().Contains("error")) == null)
                            success = true;
                    }

                    if (success)
                    {
                        if (!entry.Equals(ezBlockerExe))
                            File.Move(Application.StartupPath + $@"\{entry}", ezBlockerFullExe);

                        DeleteFile(_7zaFullExe);
                        DeleteFile(updateFullFile);

                        RestartEZBlocker();
                    }
                    else
                        MessageBox.Show("Unable to detect executable in update file...\r\nYou can continue to use this older version", "EZBlocker 2", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch
                {
                    if (IsAdmin)
                        MessageBox.Show("Error while updating EZBlocker 2!\r\nYou can continue to use this older version", "EZBlocker 2", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                    {
                        if (MessageBox.Show("Error while updating EZBlocker 2...\r\nDo you want to retry in admin mode?", "EZBlocker 2", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                            RestartEZBlocker(true);
                    }
                }
                Close();
            }
        }

        private void StartDownload()
        {
            client = new WebClient();
            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(Client_DownloadProgressChanged);
            client.DownloadFileCompleted += new AsyncCompletedEventHandler(Client_DownloadFileCompleted);
            client.DownloadFileAsync(address, updateFullFile);
        }

        public UpdateForm()
        {
            InitializeComponent();
        }

        private void UpdateForm_Load(object sender, EventArgs e)
        {
            try
            {
                DeleteFile(ezBlockerFullExeOld);

                WebRequest request = WebRequest.Create(website);
                request.Timeout = Spotilocal.Timeout;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();

                reader.Close();
                dataStream.Close();
                response.Close();

                slatest = response.ResponseUri.OriginalString.Substring(response.ResponseUri.OriginalString.LastIndexOf("/") + 1);
                ilatest = Convert.ToInt32(slatest.Replace("v", "").Replace(".", ""));
                scurrent = "v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                icurrent = Convert.ToInt32(scurrent.Replace("v", "").Replace(".", ""));

                if (ilatest > icurrent && MessageBox.Show($"A newer version of EZBlocker 2 has been found!\r\nWould you like to update? ({scurrent} to {slatest})", "EZBlocker 2", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    int end = responseFromServer.IndexOf(".zip") + 4;
                    int start = end - 4;
                    while (responseFromServer[start] != '/')
                        start--;

                    string updateFile = null;
                    for (int i = start + 1; i < end; i++)
                        updateFile += responseFromServer[i];

                    updateFullFile = Application.StartupPath + $@"\{updateFile}";
                    address = new Uri(response.ResponseUri.OriginalString.Replace("tag", "download") + $"/{updateFile}");

                    StartDownload();
                }
                else
                    Close();
            }
            catch 
            {
                DeleteFile(updateFullFile); // remove downloaded file
                Close();
            }
        }

        private void UpdateForm_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = Location;
        }

        private void UpdateForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        private void UpdateForm_MouseUp(object sender, MouseEventArgs e) => dragging = false;

        private void BtnExit_Click(object sender, EventArgs e)
        {
            if (client != null)
                client.CancelAsync();

            if (MessageBox.Show("Do you want to abort update?", "EZBlocker 2", MessageBoxButtons.YesNo, MessageBoxIcon.Information) != DialogResult.Yes)
            {
                labelMessage.Text = "Restarting download...";
                client.DownloadFileAsync(address, updateFullFile);
            }
            else
            {
                try
                {
                    DeleteFile(updateFullFile);
                }
                catch { }

                Close();
            }
        }
    }
}