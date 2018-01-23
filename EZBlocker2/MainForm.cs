using Microsoft.Win32;
using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Principal;
using System.Threading;
using System.Windows.Forms;
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
        private string spotifyPrefsFullFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Spotify\prefs";

        // Current SessionId
        private readonly int currentSessionId = Process.GetCurrentProcess().SessionId;

        // Websites
        private readonly string issue_website = "https://github.com/MatrixDJ96/EZBlocker2/issues";
        private readonly string developer_website = "https://github.com/MatrixDJ96";
        private readonly string original_website = "https://github.com/Xeroday/Spotify-Ad-Blocker";
        private readonly string designer_website = "https://github.com/Bruske";

        // Hosts patches
        private readonly string[] hostsPatches = {
            "ads.pubmatic.com",
            "gads.pubmatic.com",
            "pubads.g.doubleclisck.net",
            "securepubads.g.doubleclick.net",
            "spclient.wg.spotify.com",
            "www.googletagservices.com"
        };
        
        // Form movement
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        // Spotify system volume
        private bool muted = false;

        // Useful booleans
        private bool winStoreApp = false;
        private bool spotifyNotInstalled = false;
        private bool execSpotify = false;
        private bool exiting = false;

        // Countdown timer
        private int countdown = 30;

        // Label message
        private string[] message; // useful to store info

        /* Constructor */
        public MainForm()
        {
            InitializeComponent();

            contextMenuStrip.Renderer = new CustomToolStripRenderer();
            titleLabel.Text = "v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            if (IsAdmin)
                titleLabel.Text += " - Admin mode";
            labelMessage.UseMnemonic = false; // display the ampersand character

            if (Properties.Settings.Default.StartOnLogin || Properties.Settings.Default.StartMinimized)
                HideEZBlocker();
        }

        /* Callable functions */
        private void ShowMessage(string text, string hint = "", string album = "")
        {
            if (text.Length > 35)
                text = text.Substring(0, 35) + "...";
            if (!labelMessage.Text.Equals(text))
                labelMessage.Text = text;
            if (!toolTip.GetToolTip(labelMessage).Equals(hint))
                toolTip.SetToolTip(labelMessage, hint);
            if (!toolTip.GetToolTip(imgSong).Equals(album))
                toolTip.SetToolTip(imgSong, album);
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
            if (timeout != 0)
                Thread.Sleep(5000);
            Application.Exit();
        }

        private bool IsSpotifyRunning()
        {
            Process[] processes = Process.GetProcessesByName("spotify");
            Process[] processesCurrentSessionId = processes.Where(x => x.SessionId == currentSessionId).ToArray();

            if (processesCurrentSessionId.Length > 0)
                return true;
            else
                return false;
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
                        {
                            if (muted)
                                sessions[i].SimpleAudioVolume.Mute = true;
                            else
                                sessions[i].SimpleAudioVolume.Mute = false;
                        }
                    }
                }
            }
        }

        private void StartOnLogin()
        {
            RegistryKey startupKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            foreach (string name in startupKey.GetValueNames())
            {
                if ((name.ToLower().Contains("ezblocker") && !name.Equals("EZBlocker 2")) || (name.Equals("EZBlocker 2") && !checkBoxStartOnLogin.Checked))
                    startupKey.DeleteValue(name); // Clean
            }
            if (checkBoxStartOnLogin.Checked)
            {
                string name = "EZBlocker 2";
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

        private bool BlockAds(bool loading = false, bool write = false, string[] currentLines = null, List<string> newLines = null)
        {
            try
            {
                if (currentLines == null || newLines == null)
                {
                    string address = "0.0.0.0";
                    List<string> patches = new List<string>(hostsPatches);

                    currentLines = File.ReadAllLines(hostsFullFile);
                    newLines = new List<string>(currentLines);

                    foreach (string line in currentLines)
                    {
                        foreach (string patch in hostsPatches)
                        {
                            // check with space and tab to avoid conflit (for example 'pubads.g.doubleclick.net' and 'securepubads.g.doubleclick.net')
                            if (line.Contains(" " + patch) || line.Contains("\t" + patch))
                            {
                                if (!line.Replace(patch, "").Trim(new[] { ' ', '\t' }).Equals(address))
                                {
                                    newLines[newLines.IndexOf(line)] = address + " " + patch;
                                    write = true;
                                }
                                else
                                    patches.RemoveAt(patches.IndexOf(patch));
                            }
                        }
                    }

                    if (loading)
                    {
                        if (patches.Count == hostsPatches.Length || patches.Count == 0)
                        {
                            if (patches.Count == hostsPatches.Length)
                                checkBoxBlockAds.Checked = false;
                            else if (patches.Count == 0)
                                checkBoxBlockAds.Checked = true;

                            Properties.Settings.Default.BlockAds = checkBoxBlockAds.Checked;
                            // do not save settings !!!

                            return true;
                        }
                    }

                    if (checkBoxBlockAds.Checked)
                    {
                        foreach (string patch in patches)
                        {
                            newLines.Add(address + " " + patch);
                            write = true;
                        }
                    }
                    else
                    {
                        foreach (string patch in hostsPatches)
                        {
                            int index = newLines.FindIndex(x => x.Contains(patch));
                            if (index != -1)
                            {
                                newLines.RemoveAt(index);
                                write = true;
                            }
                        }
                    }
                }

                if (write)
                    File.WriteAllLines(hostsFullFile, newLines);
            }
            catch
            {
                if (!IsAdmin)
                {
                    string text = (loading ? "An error has been found while checking hosts file...\r\nTo solve this problem" : "To " + (checkBoxBlockAds.Checked ? "enable" : "disable") + " this option") + " administrator rights are required.\r\n\r\nDo you want to restart EZBlocker 2 in admin mode?";
                    if (MessageBox.Show(text, "EZBlocker 2", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes && RestartEZBlocker(true))
                        return true;
                    else
                        return false;
                }
                else
                {
                    if (File.GetAttributes(hostsFullFile).HasFlag(FileAttributes.ReadOnly))
                    {
                        File.SetAttributes(hostsFullFile, File.GetAttributes(hostsFullFile) & ~FileAttributes.ReadOnly); // remove flag
                        BlockAds(loading, write, currentLines, newLines);
                        File.SetAttributes(hostsFullFile, File.GetAttributes(hostsFullFile) | FileAttributes.ReadOnly); // add flag
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("An error has been found while checking hosts file...\r\nEZBlocker 2 is unable to resolve this problem...", "EZBlocker 2", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }
            }
            return true;
        }

        /* Function executed by events */
        private void Main_Load(object sender, EventArgs e)
        {
            // Check Spotify version (classic or Windows Store)
            if (!File.Exists(spotifyPrefsFullFile) && !File.Exists(spotifyFullExe))
            {
                if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Packages")) // Windows 8+
                {
                    List<string> lines = new List<string>(Directory.GetDirectories(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Packages"));
                    string folder = lines.Find(x => x.Contains("SpotifyAB.SpotifyMusic"));
                    if (folder != null)
                    {
                        spotifyPrefsFullFile = folder + @"\LocalState\Spotify\prefs";
                        winStoreApp = true;
                    }
                    if (!winStoreApp)
                        spotifyNotInstalled = true;
                }
                else
                    spotifyNotInstalled = true;
            }

            // Enable WebHelper
            if (!spotifyNotInstalled)
            {
                if (!IsSpotifyRunning())
                    execSpotify = true; // Start Spotify

                try
                {
                    if (!File.Exists(spotifyPrefsFullFile))
                        File.Create(spotifyPrefsFullFile).Close();

                    List<string> lines = new List<string>(File.ReadAllLines(spotifyPrefsFullFile));

                    bool webhelperEnabled = false;
                    for (int i = 0; i < lines.Count; i++)
                    {
                        if (lines[i].Contains("webhelper.enabled"))
                        {
                            if (!lines[i].Contains("true"))
                                lines.RemoveAt(i);
                            else
                                webhelperEnabled = true;
                            break;
                        }
                    }
                    if (!webhelperEnabled)
                    {
                        lines.Add("webhelper.enabled=true");
                        File.WriteAllLines(spotifyPrefsFullFile, lines);
                        execSpotify = true; // Restart Spotify
                    }
                }
                catch
                {
                    MessageBox.Show("Unable to enable 'Allow Spotify to be opened from the web'.\r\nEnable it in 'Edit' -> 'Preferences' -> 'Advanced settings'", "EZBlocker 2", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    KillEZBlocker();
                }
            }
            else
            {
                MessageBox.Show("Spotify seems not to be installed on your system.\r\nEZBlocker 2 is useless for you...", "EZBlocker 2", MessageBoxButtons.OK, MessageBoxIcon.Information);
                KillEZBlocker();
            }

            // Load settings
            checkBoxMuteAds.Checked = Properties.Settings.Default.MuteAds;
            checkBoxBlockAds.Checked = Properties.Settings.Default.BlockAds;
            checkBoxStartOnLogin.Checked = Properties.Settings.Default.StartOnLogin;
            if (checkBoxStartOnLogin.Checked)
                checkBoxStartMinimized.Checked = true;
            else
                checkBoxStartMinimized.Checked = Properties.Settings.Default.StartMinimized;

            bool enableBlockAds = BlockAds(true);
            StartOnLogin();

            checkBoxMuteAds.Enabled = true;
            checkBoxBlockAds.Enabled = enableBlockAds;
            checkBoxStartOnLogin.Enabled = true;
            if (!checkBoxStartOnLogin.Checked)
                checkBoxStartMinimized.Enabled = true;

            checkBoxMuteAds.CheckedChanged += new EventHandler(CheckBoxMuteAds_CheckedChanged);
            checkBoxBlockAds.CheckedChanged += new EventHandler(CheckBoxBlockAds_CheckedChanged);
            checkBoxStartOnLogin.CheckedChanged += new EventHandler(CheckBoxStartOnLogin_CheckedChanged);
            if (checkBoxStartMinimized.Enabled)
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
                    MessageBox.Show("Error while extracting dependencies!\r\nEZBlocker 2 will be closed immediately", "EZBlocker 2", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    if (MessageBox.Show("Error while extracting dependencies...\r\nDo you want to retry in admin mode?", "EZBlocker 2", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        RestartEZBlocker(true);
                }
                KillEZBlocker();
            }

            if (execSpotify)
            {
                KillSpotify();
                ExecSpotify();
            }

            timerSleep.Enabled = true;
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.StartOnLogin || Properties.Settings.Default.StartMinimized)
                MinimizeEZBlocker();
        }

        private void TimerSleep_Tick(object sender, EventArgs e)
        {
            timerSleep.Interval = 1000;

            if (IsSpotifyRunning() || countdown == 0)
            {
                timerSleep.Enabled = false;
                timerMain.Enabled = true;
            }
            else if (countdown > 0)
            {
                ShowMessage("Waiting for Spotify...");
                countdown--;
            }
        }

        private void TimerMain_Tick(object sender, EventArgs e)
        {
            timerMain.Enabled = false; // wait...
            timerMain.Interval = 700;

            bool enable = true; // start?
            message = new[] { labelMessage.Text, toolTip.GetToolTip(labelMessage) };

            SpotilocalStatus status = Spotilocal.GetStatus();
            if (!status.IsError)
            {
                if (status.IsPrivateSession)
                    ShowMessage("Spotify is in private session", "Disable private session to allow EZBlocker 2 to work");
                else
                {
                    if (status.IsPlaying)
                    {
                        Mute(status.IsAd && checkBoxMuteAds.Checked);
                        if (status.IsAd)
                        {
                            if (checkBoxMuteAds.Checked)
                                ShowMessage("Muting: Ad");
                            else
                                ShowMessage("Playing: Ad");
                        }
                        else
                            ShowMessage("Playing: " + status.Track.Song, "Artist: " + status.Track.Artist, "Album: " + status.Track.Album);
                    }
                    else
                        ShowMessage("Spotify is in pause");
                }
            }
            else
            {
                if (IsSpotifyRunning())
                    ShowMessage("Error while hooking to Spotify...", "Error: " + status.Error.Message);
                else
                {
                    enable = false; // stop!
                    MinimizeEZBlocker();
                    notifyIcon.ShowBalloonTip(5000, "EZBlocker 2", "Exiting from EZBlocker 2...", ToolTipIcon.Info);
                    CloseEZBlocker(5000);
                }
            }

            if (enable)
                timerMain.Enabled = true; // go!
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = Location;
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        private void MainForm_MouseUp(object sender, MouseEventArgs e) => dragging = false;

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
                MessageBox.Show("Could not open system volume mixer.", "EZBlocker 2", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                Properties.Settings.Default.Save();

                if (!BlockAds())
                {
                    Properties.Settings.Default.BlockAds = !checkBoxBlockAds.Checked;
                    checkBoxBlockAds.CheckedChanged -= new EventHandler(CheckBoxBlockAds_CheckedChanged);
                    checkBoxBlockAds.Checked = Properties.Settings.Default.BlockAds;
                    checkBoxBlockAds.CheckedChanged += new EventHandler(CheckBoxBlockAds_CheckedChanged);
                }
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
                Properties.Settings.Default.Save();

                if (checkBoxStartOnLogin.Checked)
                {
                    checkBoxStartMinimized.Enabled = false;
                    checkBoxStartMinimized.CheckedChanged -= new EventHandler(CheckBoxStartMinimized_CheckedChanged);
                    checkBoxStartMinimized.Checked = true;
                }
                else
                {
                    checkBoxStartMinimized.Enabled = true;
                    checkBoxStartMinimized.Checked = Properties.Settings.Default.StartMinimized;
                    checkBoxStartMinimized.CheckedChanged += new EventHandler(CheckBoxStartMinimized_CheckedChanged);
                }
                StartOnLogin();
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

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e) => CloseEZBlocker();
    }
}
