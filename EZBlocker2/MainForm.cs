using Microsoft.Win32;
using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
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
            "adclick.g.doublecklick.net",
            "adeventtracker.spotify.com",
            "adnxs.com",
            "adnxs.comadplexmedia.adk2x.com",
            "ads-fa.spotify.com",
            "ads.spotify.com",
            "analytics.spotify.com",
            "audio-sp-sto.spotify.com",
            "audio2.spotify.com",
            "b.scorecardresearch.com",
            "bounceexchange.com",
            "bs.serving-sys.com",
            "content.bitsontherun.com",
            "core.insightexpressai.com",
            "crashdump.spotify.com",
            "cs126.wpc.edgecastcdn.net",
            "d2gi7ultltnc2u.cloudfront.net",
            "d3rt1990lpmkn.cloudfront.net",
            "desktop.spotify.com",
            "doubleclick.net",
            "ds.serving-sys.com",
            "gew1.ap.spotify.com",
            "googleadapis.l.google.com",
            "googleads.g.doubleclick.net",
            "googleads4.g.doubleclick.net",
            "googleadservices.com",
            "gtssl2-ocsp.geotrust.com",
            "js.moatads.com",
            "log.spotify.com",
            "lon6-accesspoint-a33.lon6.spotify.com",
            "media-match.com",
            "omaze.com",
            "pagead2.googlesyndication.com",
            "pagead46.l.doubleclick.net",
            "partner.googleadservices.com",
            "pubads.g.doubleclick.net",
            "redirector.gvt1.com",
            "s0.2mdn.net",
            "securepubads.g.doubleclick.net",
            "seen-on-screen.thewhizmarketing.com",
            "server-54-230-216-203.mrs50.r.cloudfront.net",
            "tpc.googlesyndication.com",
            "u.scdn.co",
            "v.jwpcdn.com",
            "video-ad-stats.googlesyndication.com",
            "weblb-wg.gslb.spotify.com",
            "www.googleadservices.com",
            "www.googletagservices.com",
            "www.omaze.com",
            // Spotify Update
            "beta.spotify.map.fastly.net",
            "prod.spotify.map.fastlylb.net",
            "upgrade.scdn.co",
            "upgrade.spotify.com",
            "sto3-accesspoint-a88.sto3.spotify.net",
            "www.spotify-desktop.com"
        };

        // Hosts patches (wrong)
        private readonly string[] otherHostsPatches = {
            "ads.pubmatic.com",
            "apresolve.spotify.com",
            "fastly.net",
            "gads.pubmatic.com",
            "pubads.g.doubleclisck.net",
            "spclient.wg.spotify.com",
            "t.scdn.co"
        };

        // Form movement
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        // Spotify system volume variable
        private bool muted = false;

        // Useful booleans
        private bool winStoreApp = false;
        private bool spotifyNotInstalled = false;
        private bool execSpotify = false;
        private bool exiting = false;

        // Countdown timer
        private int countdown = 30; // seconds

        // Label message
        private string[] message = { "", "", "" }; // useful to store info

        // Listener
        private CustomListener listener;

        /* Constructor */
        public MainForm()
        {
            InitializeComponent();

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
        }

        /* Callable functions */
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

        private string MyTrim(string str)
        {
            string tmp = str.Replace("\t", "");
            while (tmp.Contains("  "))
                tmp = tmp.Replace("  ", " ");
            return tmp;
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
                    foreach (string patch in otherHostsPatches)
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
                    MessageBox.Show("Hosts file has been " + (fix ? "fixed" : "updated") + " successfully!", "EZBlocker 2", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
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
                        BlockAds(loading);
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
                    string spotifyPrefsPath = Path.GetDirectoryName(spotifyPrefsFullFile);
                    if (!Directory.Exists(spotifyPrefsPath))
                        Directory.CreateDirectory(spotifyPrefsPath);
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
            if (Properties.Settings.Default.StartMinimized)
                MinimizeEZBlocker();
        }

        private void TimerSleep_Tick(object sender, EventArgs e)
        {
            timerSleep.Interval = 1000;

            if (IsSpotifyRunning() || countdown == 0)
            {
                timerSleep.Enabled = false;

                listener = new CustomListener(this);
                Spotilocal.emitter.NewStatus += listener.StatusHandler;

                ShowMessage("Checking Spotify port...");

                timerStatus.Enabled = true;
            }
            else if (countdown > 0)
            {
                ShowMessage("Waiting for Spotify...");
                countdown--;
            }
        }

        private void TimerStatus_Tick(object sender, EventArgs e)
        {
            timerStatus.Enabled = false; // wait...
            Spotilocal.GetStatus();
        }

        internal void Main_Status(SpotilocalStatus status)
        {
            bool enable = true; // start?

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
                if (!IsSpotifyRunning())
                {
                    enable = false; // stop!
                    MinimizeEZBlocker();
                    notifyIcon.ShowBalloonTip(3000, "EZBlocker 2", "Exiting from EZBlocker 2...", ToolTipIcon.Info);
                    CloseEZBlocker(3000);
                }
                else
                {
                    string text = "Error: " + status.Message;
                    ShowMessage(text, text);
                }
            }

            if (enable)
                timerStatus.Enabled = true; // go!
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
            if (checkBoxBlockAds.Checked)
                MessageBox.Show("Automatic update for Spotify will be disabled.", "EZBlocker 2", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            try
            {
                Properties.Settings.Default.BlockAds = checkBoxBlockAds.Checked;

                if (!BlockAds())
                {
                    Properties.Settings.Default.BlockAds = !checkBoxBlockAds.Checked;
                    checkBoxBlockAds.CheckedChanged -= new EventHandler(CheckBoxBlockAds_CheckedChanged);
                    checkBoxBlockAds.Checked = Properties.Settings.Default.BlockAds;
                    checkBoxBlockAds.CheckedChanged += new EventHandler(CheckBoxBlockAds_CheckedChanged);
                }
                else
                {
                    MessageBox.Show("It could be necessary to restart Spotify or your computer for this changes to take effect.", "EZBlocker 2", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Properties.Settings.Default.Save();
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

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e) => CloseEZBlocker();
    }
}
