using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Windows.Forms;
using static EZBlocker2.Program;

namespace EZBlocker2
{
    public partial class UpdateForm : Form
    {
        // Resources
        private readonly string ezBlockerFullExeOld = ezBlockerFullExe + ".old";

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

                    using (ZipArchive archive = ZipFile.OpenRead(updateFullFile))
                    {
                        foreach (ZipArchiveEntry file in archive.Entries)
                        {
                            if (file.FullName.ToLower().Contains("ezblocker") && file.FullName.Substring(file.FullName.Length - 4).Equals(".exe", comp))
                            {
                                File.Move(ezBlockerFullExe, ezBlockerFullExeOld);
                                file.ExtractToFile(Application.StartupPath + $@"\{file.FullName}");
                                if (!file.FullName.Equals(ezBlockerExe))
                                    File.Move(Application.StartupPath + $@"\{file.FullName}", ezBlockerFullExe);
                                success = true;
                                break;
                            }
                        }
                    }

                    if (success)
                    {
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
            client.DownloadProgressChanged += Client_DownloadProgressChanged;
            client.DownloadFileCompleted += Client_DownloadFileCompleted;
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
                request.Timeout = 3000; // 3 seconds
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string page = reader.ReadToEnd();

                reader.Close();
                dataStream.Close();
                response.Close();

                slatest = response.ResponseUri.OriginalString.Substring(response.ResponseUri.OriginalString.LastIndexOf("/") + 1);
                ilatest = Convert.ToInt32(slatest.Replace("v", "").Replace(".", ""));
                scurrent = "v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                icurrent = Convert.ToInt32(scurrent.Replace("v", "").Replace(".", ""));

                if (ilatest > icurrent)
                {
                    // Check if zip file is available
                    string updateFile = "";
                    int start = page.IndexOf("/releases/download/" + slatest);
                    if (start != -1)
                    {
                        int length = slatest.Length;
                        while (page.Substring(start, length) != slatest)
                            start++;

                        start += length + 1;

                        while (page.Substring(start, 4) != ".zip")
                            updateFile += page[start++];

                        updateFile += ".zip";
                        updateFullFile = Application.StartupPath + $@"\{updateFile}";
                                                
                        address = new Uri(response.ResponseUri.OriginalString.Replace("/tag/", "/download/") + $"/{updateFile}");
                    }

                    // Check if changelog is available
                    string changelog = "";
                    start = page.IndexOf(slatest.Substring(1) + ":");
                    if (start != -1)
                    {
                        while (page.Substring(start, 4) != "<li>")
                            start++;

                        changelog += "\r\n\r\nChangelog:\r\n";

                        while (page.Substring(start, 5) != "</ul>")
                            changelog += page[start++];

                        changelog = changelog.Replace("<li>", "- ").Replace("</li>", "");
                    }
                    
                    if (address != null && MessageBox.Show($"A newer version of EZBlocker 2 has been found!\r\nWould you like to update? ({scurrent} to {slatest}){changelog}", "EZBlocker 2", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        StartDownload();
                        return;
                    }
                }
            }
            catch { }
            Close();
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