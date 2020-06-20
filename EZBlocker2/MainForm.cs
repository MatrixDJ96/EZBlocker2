using Microsoft.Win32;
using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using static EZBlocker2.HostsPatches;
using static EZBlocker2.Program;

namespace EZBlocker2
{
    public partial class MainForm : Form
    {
        // Resources
        private readonly string nAudioFullDll = Application.StartupPath + @"\NAudio.dll";
        private readonly string newtonsoftJsonFullDll = Application.StartupPath + @"\Newtonsoft.Json.dll";

        // Windows files
        private readonly string sndVolFullExe = Environment.GetFolderPath(Environment.SpecialFolder.Windows) + @"\System32\SndVol.exe";
        private readonly string sndVol32FullExe = Environment.GetFolderPath(Environment.SpecialFolder.Windows) + @"\System32\SndVol32.exe";
        private readonly string hostsFullFile = Environment.GetFolderPath(Environment.SpecialFolder.Windows) + @"\System32\drivers\etc\hosts";

        // Classic Spotify files
        private readonly string spotifyFullExe = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Spotify\Spotify.exe";
        
        // Current SessionId
        private readonly int currentSessionId = Process.GetCurrentProcess().SessionId;

        // Websites
        private readonly string issue_website = "https://github.com/MatrixDJ96/EZBlocker2/issues";
        private readonly string developer_website = "https://github.com/MatrixDJ96";
        private readonly string original_website = "https://github.com/Xeroday/Spotify-Ad-Blocker";
        private readonly string designer_website = "https://github.com/Bruske";

        // WebServer
        private CustomWebServer server = null;
        
        // Form movement and location
        private CustomMovement movement;
        private Point centerLocation;

        // Spotify system volume variable
        private bool muted = false;

        // Countdown timer
        private int countdown = 30; // seconds

        // Useful booleans
        private bool winStoreApp = false;
        private bool spotifyNotInstalled = false;
        private bool execSpotify = false;
        private bool exiting = false;

        // Label message
        private string[] message = { "", "", "" }; // useful to store info
        private Process processTmp;

        /* Constructor */
        public MainForm()
        {
            InitializeComponent();

            try
            {
                Point point = new Point(Convert.ToInt32(Properties.Settings.Default.PositionX), Convert.ToInt32(Properties.Settings.Default.PositionY));
                Location = point;
            }
            catch
            {
                StartPosition = FormStartPosition.CenterScreen;
            }

            // set keydown event
            SetCustomEvent(Controls);

            movement = new CustomMovement(this);
            movement.NewPosition += SaveLocation;

            movement.Exclude(typeof(Button));
            movement.Exclude(typeof(CheckBox));
            movement.Exclude(typeof(LinkLabel));

            movement.SetMovement(Controls);

            contextMenuStrip.Renderer = new CustomToolStripRenderer();

            titleLabel.Text = "v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            if (IsAdmin)
            {
                titleLabel.Text += " - Admin mode";
                string checkBoxBlockAdsToolTip = toolTip.GetToolTip(checkBoxBlockAds);
                toolTip.SetToolTip(checkBoxBlockAds, checkBoxBlockAdsToolTip.Substring(0, checkBoxBlockAdsToolTip.IndexOf('(') - 1));
            }

            labelMessage.UseMnemonic = false; // display the ampersand character

            if (Properties.Settings.Default.StartMinimized)
                HideEZBlocker();

            // Create .ini config file on first startup so the user can modify Client Id+Secret
            Properties.Settings.Default.Save();
        }

        /* Callable functions */
        private void CleanLog()
        {
            if (File.Exists(ezBlockerLog))
            {
                try
                {
                    string oldEZBlockerLog = ezBlockerLog + ".old";
                    DeleteFile(oldEZBlockerLog);
                    File.Move(ezBlockerLog, oldEZBlockerLog);
                }
                catch { }
            }
        }

        private void SetCustomEvent(Control.ControlCollection collection)
        {
            foreach (Control control in collection)
            {
                if (control.HasChildren)
                    SetCustomEvent(control.Controls);
                else
                {
                    if (control is LinkLabel || control is Button)
                        control.KeyDown += MainForm_KeyDown;
                }
            }
        }

        private void SaveLocation()
        {
            try
            {
                Properties.Settings.Default.PositionX = Location.X.ToString();
                Properties.Settings.Default.PositionY = Location.Y.ToString();
                Properties.Settings.Default.Save();
            }
            catch { }
        }

        private void ShowMessage(string text, string hint = "", string album = "")
        {
            if (text.Length > 35)
                text = text.Substring(0, 35) + "...";

            if (!message[0].Equals(text))
                labelMessage.Text = text;

            if (!message[1].Equals(hint))
                toolTip.SetToolTip(labelMessage, hint);

            if (!message[2].Equals(album))
                toolTip.SetToolTip(imgSong, album);

            message = new[] { text, hint, album };
        }

        private void HideEZBlocker()
        {
            ShowInTaskbar = false;
            WindowState = FormWindowState.Minimized;
        }

        private void MinimizeEZBlocker()
        {
            Hide();
            notifyIcon.Visible = true;
        }

        private void ShowEZBlocker()
        {
            if (!exiting)
            {
                Show();
                ShowInTaskbar = true;
                WindowState = FormWindowState.Normal;
                notifyIcon.Visible = false;
            }
        }

        private void CloseEZBlocker(int timeout = 0)
        {
            exiting = true;

            // Could it be listening?
            server.Stop();

            if (timeout != 0)
                Thread.Sleep(timeout);

            Application.Exit();
        }

        private bool IsSpotifyRunning()
        {
            Process[] processes = Process.GetProcessesByName("spotify");
            Process[] processesCurrentSessionId = processes.Where(x => x.SessionId == currentSessionId).ToArray();            
            return processesCurrentSessionId.Length > 0 ? true : false;
        }

        private void KillSpotify()
        {
            Process[] processes = Process.GetProcessesByName("spotify");
            Process[] processesCurrentSessionId = processes.Where(x => x.SessionId == currentSessionId).ToArray();

            foreach (var process in processesCurrentSessionId)
                process.Kill();
        }

        private void ExecSpotify()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();

            if (winStoreApp)
            {
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = "/C start spotify";
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            }
            else
                startInfo.FileName = spotifyFullExe;

            StartProcess(startInfo);
        }

        private void Mute(bool enable)
        {
            if (muted == enable)
                return;

            muted = enable;

            using (var enumerator = new MMDeviceEnumerator())
            {
                using (var device = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia))
                {
                    var sessions = device.AudioSessionManager.Sessions;
                    for (int i = 0; i < sessions.Count; i++)
                    {
                        if (sessions[i].GetSessionIdentifier.ToLower().Contains("spotify"))
                            sessions[i].SimpleAudioVolume.Mute = muted;
                    }
                }
            }
        }

        private void StartOnLogin()
        {
            RegistryKey startupKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            foreach (string name in startupKey.GetValueNames())
            {
                if ((name.ToLower().Contains("ezblocker") && !name.Equals(Program.ProductName)) || (name.Equals(Program.ProductName) && !checkBoxStartOnLogin.Checked))
                    startupKey.DeleteValue(name); // Clean
            }
            if (checkBoxStartOnLogin.Checked)
            {
                string name = Program.ProductName;
                var value = startupKey.GetValue(name);
                if (value == null || !value.Equals(ezBlockerFullExe))
                    startupKey.SetValue(name, ezBlockerFullExe);
                using (var key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\Run", true))
                {
                    if (key != null)
                        key.SetValue(name, new byte[] { 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, RegistryValueKind.Binary);
                }
            }
        }

        private bool BlockAds(bool loading = false)
        {
            bool fix = false;
            bool write = false;

            try
            {
                string address = "0.0.0.0";

                string[] currentLines = File.ReadAllLines(hostsFullFile);
                List<string> tmpLines = new List<string>(currentLines);
                List<string> newLines = new List<string>();

                foreach (string line in currentLines)
                {
                    string tmp = MyTrim(line);

                    // fix wrong patches
                    foreach (string patch in hostsPatches)
                    {
                        // check with space and tab to avoid conflit (for example 'pubads.g.doubleclick.net' and 'securepubads.g.doubleclick.net' or 'adnxs.com' and 'adnxs.comadplexmedia.adk2x.com')
                        if (tmp.ToLower().Contains(" " + patch) && tmp.EndsWith(patch, comp))
                        {
                            if (!tmp.Replace(patch, "").Trim().Equals(address))
                            {
                                int index = tmpLines.IndexOf(line);
                                if (index > -1)
                                {
                                    tmpLines[index] = address + " " + patch;
                                    fix = true;
                                }
                            }
                        }
                    }

                    // delete wrong patches
                    foreach (string patch in wrongHostsPatches)
                    {
                        if (tmp.ToLower().Contains(" " + patch) && tmp.EndsWith(patch, comp))
                        {
                            int index = tmpLines.FindIndex(x => x.ToLower().Contains(patch));
                            while (index > -1)
                            {
                                tmpLines.RemoveAt(index);
                                index = tmpLines.FindIndex(x => x.ToLower().Contains(patch));
                                fix = true;
                            }
                        }
                    }
                }

                // delete duplicated patches
                foreach (string line in tmpLines)
                {
                    if (!newLines.Contains(line))
                        newLines.Add(line);
                    else
                    {
                        string tmp = MyTrim(line);
                        foreach (string patch in hostsPatches)
                        {
                            if (!(tmp.ToLower().Contains(patch)) || tmp.Trim()[0] == '#')
                            {
                                newLines.Add(line);
                                break;
                            }
                            else
                                fix = true;
                        }
                    }
                }

                if (checkBoxBlockAds.Checked)
                {
                    foreach (string patch in hostsPatches)
                    {
                        if (newLines.FindIndex(x => x.Contains(patch)) == -1)
                        {
                            newLines.Add(address + " " + patch);
                            write = true;
                        }
                    }
                }
                else
                {
                    foreach (string patch in hostsPatches)
                    {
                        int index = newLines.FindIndex(x => x.Contains(patch));
                        while (index > -1)
                        {
                            newLines.RemoveAt(index);
                            index = newLines.FindIndex(x => x.Contains(patch));
                            write = true;
                        }
                    }
                }

                if (fix || write)
                {
                    File.WriteAllLines(hostsFullFile, newLines);
                    MessageBox.Show("Hosts file has been " + (fix ? "fixed" : "updated") + " successfully!", Program.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {
                if (!IsAdmin)
                {
                    string text = (loading ? "An error has been found while checking hosts file...\r\nTo solve this problem" : "To " + (checkBoxBlockAds.Checked ? "enable" : "disable") + " this option") + " administrator rights are required.\r\n\r\nDo you want to restart " + Program.ProductName + " in admin mode?";
                    if (MessageBox.Show(text, Program.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes && RestartEZBlocker(true))
                        return true;
                    else
                        return false;
                }
                else
                {
                    if (File.GetAttributes(hostsFullFile).HasFlag(FileAttributes.ReadOnly))
                    {
                        File.SetAttributes(hostsFullFile, File.GetAttributes(hostsFullFile) & ~FileAttributes.ReadOnly); // remove flag
                        BlockAds(loading);
                        File.SetAttributes(hostsFullFile, File.GetAttributes(hostsFullFile) | FileAttributes.ReadOnly); // add flag
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("An error has been found while checking hosts file...\r\n" + Program.ProductName + " is unable to resolve this problem...", Program.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }
            }
            return true;
        }

        /* Functions executed by events */
        private void Main_Load(object sender, EventArgs e)
        {
            // Check Spotify version (classic or Windows Store)
            if (!File.Exists(spotifyFullExe))
            {
                if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Packages")) // Windows 8+
                {
                    List<string> lines = new List<string>(Directory.GetDirectories(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Packages"));
                    string folder = lines.Find(x => x.ToLower().Contains("spotifyab.spotifymusic"));

                    if (folder != null)
                        winStoreApp = true;

                    if (!winStoreApp)
                        spotifyNotInstalled = true;
                }
                else
                    spotifyNotInstalled = true;
            }

            if (spotifyNotInstalled)
            {
                MessageBox.Show("Spotify seems not to be installed on your system.\r\n" + Program.ProductName + " is useless for you...", Program.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                KillEZBlocker();
            }
            else if (!IsSpotifyRunning())
                execSpotify = true; // Start Spotify
            
            // Load settings
            checkBoxMuteAds.Checked = Properties.Settings.Default.MuteAds;
            checkBoxBlockAds.Checked = Properties.Settings.Default.BlockAds;
            checkBoxStartOnLogin.Checked = Properties.Settings.Default.StartOnLogin;
            checkBoxStartMinimized.Checked = Properties.Settings.Default.StartMinimized;

            bool enableBlockAds = BlockAds(true);
            StartOnLogin();

            checkBoxMuteAds.Enabled = true;
            checkBoxBlockAds.Enabled = enableBlockAds;
            checkBoxStartOnLogin.Enabled = true;
            checkBoxStartMinimized.Enabled = true;

            checkBoxMuteAds.CheckedChanged += new EventHandler(CheckBoxMuteAds_CheckedChanged);
            checkBoxBlockAds.CheckedChanged += new EventHandler(CheckBoxBlockAds_CheckedChanged);
            checkBoxStartOnLogin.CheckedChanged += new EventHandler(CheckBoxStartOnLogin_CheckedChanged);
            checkBoxStartMinimized.CheckedChanged += new EventHandler(CheckBoxStartMinimized_CheckedChanged);

            // Extract dependencies
            try
            {
                ExtractFile(nAudioFullDll, Properties.Resources.NAudio);
                ExtractFile(newtonsoftJsonFullDll, Properties.Resources.Newtonsoft_Json);
            }
            catch
            {
                if (IsAdmin)
                    MessageBox.Show("Error while extracting dependencies!\r\n" + Program.ProductName + " will be closed immediately", Program.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    if (MessageBox.Show("Error while extracting dependencies...\r\nDo you want to retry in admin mode?", Program.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        RestartEZBlocker(true);
                }
                KillEZBlocker();
            }

            if (execSpotify)
            {
                KillSpotify();
                ExecSpotify();
            }

            CleanLog();

            // Start server
            server = new CustomWebServer();
            Task.Run(server.Start);
            
            timerSpotify.Enabled = true;
        }

        private void Main_Shown(object sender, EventArgs e)
        {
            if (StartPosition == FormStartPosition.CenterScreen)
                centerLocation = Location;
            else
            {
                int x = (Screen.PrimaryScreen.WorkingArea.Width / 2) - (Width / 2);
                int y = (Screen.PrimaryScreen.WorkingArea.Height / 2) - (Height / 2);

                centerLocation = new Point(x, y);
            }

            if (Properties.Settings.Default.StartMinimized)
                MinimizeEZBlocker();
        }

        private void TimerSpotify_Tick(object sender, EventArgs e)
        {
            if (!IsSpotifyRunning())
            {
                if (countdown > 0)
                {
                    ShowMessage("Waiting for Spotify...");
                    countdown--;
                }
                else
                    CloseEZBlocker();
            }
            else
            {
                timerSpotify.Enabled = false;

                ShowMessage("Hooking to Spotify...");

                Spotify.WebAPI.OnNewStatus += Main_Status;
                Spotify.WebAPI.RedirectUri = Uri.EscapeUriString(server.Prefix);

                NameValueCollection data = HttpUtility.ParseQueryString(string.Empty);

                data.Add("scope", Spotify.WebAPI.Scope);
                data.Add("client_id", Spotify.WebAPI.ClientID);
                data.Add("redirect_uri", Spotify.WebAPI.RedirectUri);
                data.Add("response_type", Spotify.WebAPI.ResponseType);

                Spotify.WebAPI.AuthorizeUrl = "https://accounts.spotify.com/authorize?" + data.ToString();

                // Open Spotify authorization page
                processTmp = Process.Start(Spotify.WebAPI.AuthorizeUrl);

                timerStatus.Enabled = true;
            }
        }

        private void TimerStatus_Tick(object sender, EventArgs e)
        {
            if (Spotify.WebAPI.APIToken != null)
            {
                timerStatus.Enabled = false; // wait...

                try
                {
                    if (processTmp != null)
                    {
                        if (!processTmp.HasExited)
                            processTmp.Kill();

                        processTmp = null;
                    }
                }
                catch { }
                
                Spotify.WebAPI.GetStatus();
            }
        }
        
        private void Main_Status()
        {
            bool enable = true; // start?
            
            if (Spotify.WebAPI.Status.Retry_After == 0)
            {
                timerStatus.Interval = 500; // 0.5s

                if (IsSpotifyRunning())
                {
                    if (Spotify.WebAPI.Status.Error == null)
                    {
                        if (!Spotify.WebAPI.Status.Is_Private)
                        {
                            if (Spotify.WebAPI.Status.Is_Playing)
                            {
                                Mute(Spotify.WebAPI.Status.Is_Ads && checkBoxMuteAds.Checked);

                                if (!Spotify.WebAPI.Status.Is_Ads)
                                {
                                    string artists = "";

                                    foreach (var artist in Spotify.WebAPI.Status.Item.Artists)
                                    {
                                        if (artists != string.Empty)
                                            artists += " - ";

                                        artists += artist.Name;
                                    }

                                    ShowMessage("Playing: " + Spotify.WebAPI.Status.Item.Name, "Artists: " + artists, "Album: " + Spotify.WebAPI.Status.Item.Album.Name);
                                }
                                else
                                    ShowMessage((checkBoxMuteAds.Checked ? "Muting" : "Playing") + ": Ads");
                            }
                            else
                                ShowMessage("Spotify is in pause");
                        }
                        else
                            ShowMessage("Spotify is in private session", "Disable private session to allow " + Program.ProductName + " to work");
                    }
                    else
                        ShowMessage("Error: " + Spotify.WebAPI.Status.Error.Message, "Error: " + Spotify.WebAPI.Status.Error.Message);
                }
                else
                {
                    enable = false; // stop!
                    MinimizeEZBlocker();
                    notifyIcon.ShowBalloonTip(3000, Program.ProductName, "Exiting from " + Program.ProductName + "...", ToolTipIcon.Info);
                    CloseEZBlocker(3000);
                }
            }
            else
                timerStatus.Interval = Spotify.WebAPI.Status.Retry_After;

            if (enable)
                timerStatus.Enabled = true; // go!
        }
                
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                if (MessageBox.Show("Do you want to move " + Program.ProductName + " in the middle of the screen?", Program.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Location = centerLocation;
                    SaveLocation();
                }
            }
        }

        private void BtnMinimize_Click(object sender, EventArgs e) => MinimizeEZBlocker();

        private void BtnReportIssue_Click(object sender, EventArgs e) => StartProcess(issue_website);

        private void BtnExit_Click(object sender, EventArgs e) => CloseEZBlocker();

        private void BtnSndVol_Click(object sender, EventArgs e)
        {
            Size btnLoc = new Size(btnSndVol.Location) + new Size(panelContainer.Location);
            Point pos = Point.Add(Location, btnLoc);

            UInt16 x = (UInt16)(((UInt32)(pos.X)) & 0xffff);
            UInt32 y = (UInt16)(((UInt32)(pos.Y)) & 0xffff);
            Int64 l = (x | y << 16);

            if (File.Exists(sndVolFullExe))
                StartProcess(sndVolFullExe, "-m " + l.ToString());
            else if (File.Exists(sndVol32FullExe))
                StartProcess(sndVol32FullExe, "-m " + l.ToString());
            else
                MessageBox.Show("Could not open system volume mixer.", Program.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void BtnSndVol_MouseLeave(object sender, EventArgs e) => btnSndVol.BackgroundImage = Properties.Resources.SndVol_Leave;

        private void BtnSndVol_MouseEnter(object sender, EventArgs e) => btnSndVol.BackgroundImage = Properties.Resources.SndVol_Enter;

        private void BtnSndVol_MouseDown(object sender, MouseEventArgs e) => btnSndVol.BackgroundImage = Properties.Resources.SndVol_Down;

        private void LinkLabelDeveloper_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => StartProcess(developer_website);

        private void LinkLabelOriginalProject_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => StartProcess(original_website);

        private void LinkLabelDesigner_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => StartProcess(designer_website);

        private void CheckBoxMuteAds_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.MuteAds = checkBoxMuteAds.Checked;
                Properties.Settings.Default.Save();
            }
            catch
            {
                Properties.Settings.Default.MuteAds = !checkBoxMuteAds.Checked;
                checkBoxMuteAds.CheckedChanged -= new EventHandler(CheckBoxMuteAds_CheckedChanged);
                checkBoxMuteAds.Checked = Properties.Settings.Default.MuteAds;
                checkBoxMuteAds.CheckedChanged += new EventHandler(CheckBoxMuteAds_CheckedChanged);
            }

        }

        private void CheckBoxBlockAds_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.BlockAds = checkBoxBlockAds.Checked;

                if (BlockAds())
                {
                    MessageBox.Show("It could be necessary to restart Spotify or your computer for this changes to take effect.", Program.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Properties.Settings.Default.Save();
                }
                else
                    throw new Exception();
            }
            catch
            {
                Properties.Settings.Default.BlockAds = !checkBoxBlockAds.Checked;
                checkBoxBlockAds.CheckedChanged -= new EventHandler(CheckBoxBlockAds_CheckedChanged);
                checkBoxBlockAds.Checked = Properties.Settings.Default.BlockAds;
                checkBoxBlockAds.CheckedChanged += new EventHandler(CheckBoxBlockAds_CheckedChanged);
            }
        }

        private void CheckBoxStartOnLogin_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.StartOnLogin = checkBoxStartOnLogin.Checked;

                StartOnLogin();

                Properties.Settings.Default.Save();
            }
            catch
            {
                Properties.Settings.Default.StartOnLogin = !checkBoxStartOnLogin.Checked;
                checkBoxStartOnLogin.CheckedChanged -= new EventHandler(CheckBoxStartOnLogin_CheckedChanged);
                checkBoxStartOnLogin.Checked = Properties.Settings.Default.StartOnLogin;
                checkBoxStartOnLogin.CheckedChanged += new EventHandler(CheckBoxStartOnLogin_CheckedChanged);
            }
        }

        private void CheckBoxStartMinimized_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.StartMinimized = checkBoxStartMinimized.Checked;
                Properties.Settings.Default.Save();
            }
            catch
            {
                Properties.Settings.Default.StartMinimized = !checkBoxStartMinimized.Checked;
                checkBoxStartMinimized.CheckedChanged -= new EventHandler(CheckBoxStartMinimized_CheckedChanged);
                checkBoxStartMinimized.Checked = Properties.Settings.Default.StartMinimized;
                checkBoxStartMinimized.CheckedChanged += new EventHandler(CheckBoxStartMinimized_CheckedChanged);
            }
        }

        private void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e) => ShowEZBlocker();

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e) => ShowEZBlocker();

        private void SpotifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string text = message[0];

            if (!string.IsNullOrWhiteSpace(message[1]))
                text += "\r\n" + message[1];

            if (!string.IsNullOrWhiteSpace(message[2]))
                text += "\r\n" + message[2];

            notifyIcon.ShowBalloonTip(3000, Program.ProductName, text, ToolTipIcon.Info);
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e) => CloseEZBlocker();
    }
}
