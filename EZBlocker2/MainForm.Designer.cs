using System;

namespace EZBlocker2
{
    partial class MainForm
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.btnMinimize = new System.Windows.Forms.Button();
            this.btnReportIssue = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.checkBoxStartOnLogin = new System.Windows.Forms.CheckBox();
            this.checkBoxStartMinimized = new System.Windows.Forms.CheckBox();
            this.checkBoxBlockAds = new System.Windows.Forms.CheckBox();
            this.checkBoxMuteAds = new System.Windows.Forms.CheckBox();
            this.btnSndVol = new System.Windows.Forms.Button();
            this.imgSpotifyLogo = new System.Windows.Forms.PictureBox();
            this.titleLabel = new System.Windows.Forms.Label();
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelStatusBar = new System.Windows.Forms.Panel();
            this.imgEZBlockerLogo = new System.Windows.Forms.PictureBox();
            this.panelContainer = new System.Windows.Forms.Panel();
            this.linkLabelDesigner = new System.Windows.Forms.LinkLabel();
            this.linkLabelOriginalProject = new System.Windows.Forms.LinkLabel();
            this.linkLabelDeveloper = new System.Windows.Forms.LinkLabel();
            this.panelSeparatorDown = new System.Windows.Forms.Panel();
            this.labelMessage = new System.Windows.Forms.Label();
            this.panelSeparatorUp = new System.Windows.Forms.Panel();
            this.imgSong = new System.Windows.Forms.PictureBox();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.spotifyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.timerStatus = new System.Windows.Forms.Timer(this.components);
            this.timerSpotify = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.imgSpotifyLogo)).BeginInit();
            this.panelMain.SuspendLayout();
            this.panelStatusBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgEZBlockerLogo)).BeginInit();
            this.panelContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgSong)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnMinimize
            // 
            this.btnMinimize.BackgroundImage = global::EZBlocker2.Properties.Resources.Minimize;
            this.btnMinimize.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnMinimize.FlatAppearance.BorderSize = 0;
            this.btnMinimize.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btnMinimize.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btnMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimize.Location = new System.Drawing.Point(450, 0);
            this.btnMinimize.Margin = new System.Windows.Forms.Padding(0);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(50, 30);
            this.btnMinimize.TabIndex = 1;
            this.btnMinimize.TabStop = false;
            this.toolTip.SetToolTip(this.btnMinimize, "Minimize EZBlocker 2");
            this.btnMinimize.UseVisualStyleBackColor = true;
            this.btnMinimize.Click += new System.EventHandler(this.BtnMinimize_Click);
            // 
            // btnReportIssue
            // 
            this.btnReportIssue.BackgroundImage = global::EZBlocker2.Properties.Resources.ReportIssue;
            this.btnReportIssue.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnReportIssue.FlatAppearance.BorderSize = 0;
            this.btnReportIssue.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btnReportIssue.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btnReportIssue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReportIssue.Location = new System.Drawing.Point(500, 0);
            this.btnReportIssue.Margin = new System.Windows.Forms.Padding(0);
            this.btnReportIssue.Name = "btnReportIssue";
            this.btnReportIssue.Size = new System.Drawing.Size(50, 30);
            this.btnReportIssue.TabIndex = 2;
            this.btnReportIssue.TabStop = false;
            this.toolTip.SetToolTip(this.btnReportIssue, "Report issue on GitHub");
            this.btnReportIssue.UseVisualStyleBackColor = true;
            this.btnReportIssue.Click += new System.EventHandler(this.BtnReportIssue_Click);
            // 
            // btnExit
            // 
            this.btnExit.BackgroundImage = global::EZBlocker2.Properties.Resources.Exit;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btnExit.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Location = new System.Drawing.Point(550, 0);
            this.btnExit.Margin = new System.Windows.Forms.Padding(0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(50, 30);
            this.btnExit.TabIndex = 3;
            this.btnExit.TabStop = false;
            this.toolTip.SetToolTip(this.btnExit, "Exit from EZBlocker 2");
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // checkBoxStartOnLogin
            // 
            this.checkBoxStartOnLogin.AutoSize = true;
            this.checkBoxStartOnLogin.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxStartOnLogin.Enabled = false;
            this.checkBoxStartOnLogin.FlatAppearance.BorderSize = 0;
            this.checkBoxStartOnLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxStartOnLogin.ForeColor = System.Drawing.Color.White;
            this.checkBoxStartOnLogin.Location = new System.Drawing.Point(411, 93);
            this.checkBoxStartOnLogin.Margin = new System.Windows.Forms.Padding(0);
            this.checkBoxStartOnLogin.Name = "checkBoxStartOnLogin";
            this.checkBoxStartOnLogin.Size = new System.Drawing.Size(114, 22);
            this.checkBoxStartOnLogin.TabIndex = 7;
            this.checkBoxStartOnLogin.TabStop = false;
            this.checkBoxStartOnLogin.Text = "Start on login";
            this.toolTip.SetToolTip(this.checkBoxStartOnLogin, "Start EZBlocker 2 after user login");
            this.checkBoxStartOnLogin.UseVisualStyleBackColor = false;
            // 
            // checkBoxStartMinimized
            // 
            this.checkBoxStartMinimized.AutoSize = true;
            this.checkBoxStartMinimized.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxStartMinimized.Enabled = false;
            this.checkBoxStartMinimized.FlatAppearance.BorderSize = 0;
            this.checkBoxStartMinimized.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxStartMinimized.ForeColor = System.Drawing.Color.White;
            this.checkBoxStartMinimized.Location = new System.Drawing.Point(411, 126);
            this.checkBoxStartMinimized.Margin = new System.Windows.Forms.Padding(0);
            this.checkBoxStartMinimized.Name = "checkBoxStartMinimized";
            this.checkBoxStartMinimized.Size = new System.Drawing.Size(129, 22);
            this.checkBoxStartMinimized.TabIndex = 8;
            this.checkBoxStartMinimized.TabStop = false;
            this.checkBoxStartMinimized.Text = "Start minimized";
            this.toolTip.SetToolTip(this.checkBoxStartMinimized, "Start EZBlocker 2 minimized in icon tray");
            this.checkBoxStartMinimized.UseVisualStyleBackColor = false;
            // 
            // checkBoxBlockAds
            // 
            this.checkBoxBlockAds.AutoSize = true;
            this.checkBoxBlockAds.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxBlockAds.Enabled = false;
            this.checkBoxBlockAds.FlatAppearance.BorderSize = 0;
            this.checkBoxBlockAds.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxBlockAds.ForeColor = System.Drawing.Color.White;
            this.checkBoxBlockAds.Location = new System.Drawing.Point(280, 126);
            this.checkBoxBlockAds.Margin = new System.Windows.Forms.Padding(0);
            this.checkBoxBlockAds.Name = "checkBoxBlockAds";
            this.checkBoxBlockAds.Size = new System.Drawing.Size(93, 22);
            this.checkBoxBlockAds.TabIndex = 6;
            this.checkBoxBlockAds.TabStop = false;
            this.checkBoxBlockAds.Text = "Block ads";
            this.toolTip.SetToolTip(this.checkBoxBlockAds, "Apply hosts patches to block all ads (could require admin privileges)");
            this.checkBoxBlockAds.UseVisualStyleBackColor = false;
            // 
            // checkBoxMuteAds
            // 
            this.checkBoxMuteAds.AutoSize = true;
            this.checkBoxMuteAds.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxMuteAds.Enabled = false;
            this.checkBoxMuteAds.FlatAppearance.BorderSize = 0;
            this.checkBoxMuteAds.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxMuteAds.ForeColor = System.Drawing.Color.White;
            this.checkBoxMuteAds.Location = new System.Drawing.Point(280, 93);
            this.checkBoxMuteAds.Margin = new System.Windows.Forms.Padding(0);
            this.checkBoxMuteAds.Name = "checkBoxMuteAds";
            this.checkBoxMuteAds.Size = new System.Drawing.Size(88, 22);
            this.checkBoxMuteAds.TabIndex = 5;
            this.checkBoxMuteAds.TabStop = false;
            this.checkBoxMuteAds.Text = "Mute ads";
            this.toolTip.SetToolTip(this.checkBoxMuteAds, "While ad is playing set Spotify system volume to zero");
            this.checkBoxMuteAds.UseVisualStyleBackColor = false;
            // 
            // btnSndVol
            // 
            this.btnSndVol.BackColor = System.Drawing.Color.Transparent;
            this.btnSndVol.BackgroundImage = global::EZBlocker2.Properties.Resources.SndVol_Leave;
            this.btnSndVol.FlatAppearance.BorderSize = 0;
            this.btnSndVol.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.btnSndVol.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.btnSndVol.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSndVol.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.btnSndVol.Location = new System.Drawing.Point(487, 10);
            this.btnSndVol.Margin = new System.Windows.Forms.Padding(0);
            this.btnSndVol.Name = "btnSndVol";
            this.btnSndVol.Size = new System.Drawing.Size(53, 60);
            this.btnSndVol.TabIndex = 4;
            this.btnSndVol.TabStop = false;
            this.toolTip.SetToolTip(this.btnSndVol, "Open system volume mixer");
            this.btnSndVol.UseVisualStyleBackColor = false;
            this.btnSndVol.Click += new System.EventHandler(this.BtnSndVol_Click);
            this.btnSndVol.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtnSndVol_MouseDown);
            this.btnSndVol.MouseEnter += new System.EventHandler(this.BtnSndVol_MouseEnter);
            this.btnSndVol.MouseLeave += new System.EventHandler(this.BtnSndVol_MouseLeave);
            // 
            // imgSpotifyLogo
            // 
            this.imgSpotifyLogo.BackColor = System.Drawing.Color.Transparent;
            this.imgSpotifyLogo.ErrorImage = null;
            this.imgSpotifyLogo.Image = global::EZBlocker2.Properties.Resources.Spotify_Logo;
            this.imgSpotifyLogo.InitialImage = null;
            this.imgSpotifyLogo.Location = new System.Drawing.Point(60, 83);
            this.imgSpotifyLogo.Margin = new System.Windows.Forms.Padding(0);
            this.imgSpotifyLogo.Name = "imgSpotifyLogo";
            this.imgSpotifyLogo.Size = new System.Drawing.Size(180, 78);
            this.imgSpotifyLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imgSpotifyLogo.TabIndex = 4;
            this.imgSpotifyLogo.TabStop = false;
            this.toolTip.SetToolTip(this.imgSpotifyLogo, "I would rather you spent your money on Spotify Premium!");
            // 
            // titleLabel
            // 
            this.titleLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.titleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.ForeColor = System.Drawing.Color.White;
            this.titleLabel.Location = new System.Drawing.Point(0, 0);
            this.titleLabel.Margin = new System.Windows.Forms.Padding(0);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.titleLabel.Size = new System.Drawing.Size(450, 30);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "EZBlocker 2";
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelMain
            // 
            this.panelMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.panelMain.BackgroundImage = global::EZBlocker2.Properties.Resources.Background;
            this.panelMain.Controls.Add(this.panelStatusBar);
            this.panelMain.Controls.Add(this.imgEZBlockerLogo);
            this.panelMain.Controls.Add(this.panelContainer);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Margin = new System.Windows.Forms.Padding(0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(600, 420);
            this.panelMain.TabIndex = 0;
            // 
            // panelStatusBar
            // 
            this.panelStatusBar.BackColor = System.Drawing.Color.Transparent;
            this.panelStatusBar.Controls.Add(this.titleLabel);
            this.panelStatusBar.Controls.Add(this.btnMinimize);
            this.panelStatusBar.Controls.Add(this.btnReportIssue);
            this.panelStatusBar.Controls.Add(this.btnExit);
            this.panelStatusBar.Location = new System.Drawing.Point(0, 0);
            this.panelStatusBar.Margin = new System.Windows.Forms.Padding(0);
            this.panelStatusBar.Name = "panelStatusBar";
            this.panelStatusBar.Size = new System.Drawing.Size(600, 30);
            this.panelStatusBar.TabIndex = 0;
            // 
            // imgEZBlockerLogo
            // 
            this.imgEZBlockerLogo.BackColor = System.Drawing.Color.Transparent;
            this.imgEZBlockerLogo.ErrorImage = null;
            this.imgEZBlockerLogo.Image = global::EZBlocker2.Properties.Resources.EZBlocker2_Logo;
            this.imgEZBlockerLogo.InitialImage = null;
            this.imgEZBlockerLogo.Location = new System.Drawing.Point(0, 30);
            this.imgEZBlockerLogo.Margin = new System.Windows.Forms.Padding(0);
            this.imgEZBlockerLogo.Name = "imgEZBlockerLogo";
            this.imgEZBlockerLogo.Size = new System.Drawing.Size(600, 184);
            this.imgEZBlockerLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.imgEZBlockerLogo.TabIndex = 0;
            this.imgEZBlockerLogo.TabStop = false;
            // 
            // panelContainer
            // 
            this.panelContainer.Controls.Add(this.checkBoxStartOnLogin);
            this.panelContainer.Controls.Add(this.checkBoxStartMinimized);
            this.panelContainer.Controls.Add(this.checkBoxBlockAds);
            this.panelContainer.Controls.Add(this.checkBoxMuteAds);
            this.panelContainer.Controls.Add(this.imgSpotifyLogo);
            this.panelContainer.Controls.Add(this.linkLabelDesigner);
            this.panelContainer.Controls.Add(this.linkLabelOriginalProject);
            this.panelContainer.Controls.Add(this.linkLabelDeveloper);
            this.panelContainer.Controls.Add(this.panelSeparatorDown);
            this.panelContainer.Controls.Add(this.labelMessage);
            this.panelContainer.Controls.Add(this.panelSeparatorUp);
            this.panelContainer.Controls.Add(this.imgSong);
            this.panelContainer.Controls.Add(this.btnSndVol);
            this.panelContainer.Location = new System.Drawing.Point(0, 214);
            this.panelContainer.Margin = new System.Windows.Forms.Padding(0);
            this.panelContainer.Name = "panelContainer";
            this.panelContainer.Size = new System.Drawing.Size(600, 206);
            this.panelContainer.TabIndex = 0;
            // 
            // linkLabelDesigner
            // 
            this.linkLabelDesigner.ActiveLinkColor = System.Drawing.SystemColors.MenuHighlight;
            this.linkLabelDesigner.AutoSize = true;
            this.linkLabelDesigner.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabelDesigner.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkLabelDesigner.LinkColor = System.Drawing.Color.LightGray;
            this.linkLabelDesigner.Location = new System.Drawing.Point(404, 176);
            this.linkLabelDesigner.Margin = new System.Windows.Forms.Padding(0);
            this.linkLabelDesigner.Name = "linkLabelDesigner";
            this.linkLabelDesigner.Size = new System.Drawing.Size(171, 16);
            this.linkLabelDesigner.TabIndex = 11;
            this.linkLabelDesigner.TabStop = true;
            this.linkLabelDesigner.Text = "Design insipired by: Bruske";
            this.linkLabelDesigner.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabelDesigner_LinkClicked);
            // 
            // linkLabelOriginalProject
            // 
            this.linkLabelOriginalProject.ActiveLinkColor = System.Drawing.SystemColors.MenuHighlight;
            this.linkLabelOriginalProject.AutoSize = true;
            this.linkLabelOriginalProject.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabelOriginalProject.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkLabelOriginalProject.LinkColor = System.Drawing.Color.LightGray;
            this.linkLabelOriginalProject.Location = new System.Drawing.Point(207, 176);
            this.linkLabelOriginalProject.Margin = new System.Windows.Forms.Padding(0);
            this.linkLabelOriginalProject.Name = "linkLabelOriginalProject";
            this.linkLabelOriginalProject.Size = new System.Drawing.Size(186, 16);
            this.linkLabelOriginalProject.TabIndex = 10;
            this.linkLabelOriginalProject.TabStop = true;
            this.linkLabelOriginalProject.Text = "Original project by: Eric Zhang";
            this.linkLabelOriginalProject.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabelOriginalProject_LinkClicked);
            // 
            // linkLabelDeveloper
            // 
            this.linkLabelDeveloper.ActiveLinkColor = System.Drawing.SystemColors.MenuHighlight;
            this.linkLabelDeveloper.AutoSize = true;
            this.linkLabelDeveloper.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabelDeveloper.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkLabelDeveloper.LinkColor = System.Drawing.Color.LightGray;
            this.linkLabelDeveloper.Location = new System.Drawing.Point(22, 176);
            this.linkLabelDeveloper.Margin = new System.Windows.Forms.Padding(0);
            this.linkLabelDeveloper.Name = "linkLabelDeveloper";
            this.linkLabelDeveloper.Size = new System.Drawing.Size(166, 16);
            this.linkLabelDeveloper.TabIndex = 9;
            this.linkLabelDeveloper.TabStop = true;
            this.linkLabelDeveloper.Text = "Developed by: MatrixDJ96";
            this.linkLabelDeveloper.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabelDeveloper_LinkClicked);
            // 
            // panelSeparatorDown
            // 
            this.panelSeparatorDown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.panelSeparatorDown.Location = new System.Drawing.Point(25, 161);
            this.panelSeparatorDown.Margin = new System.Windows.Forms.Padding(0);
            this.panelSeparatorDown.Name = "panelSeparatorDown";
            this.panelSeparatorDown.Size = new System.Drawing.Size(550, 3);
            this.panelSeparatorDown.TabIndex = 0;
            // 
            // labelMessage
            // 
            this.labelMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMessage.ForeColor = System.Drawing.Color.White;
            this.labelMessage.Location = new System.Drawing.Point(120, 19);
            this.labelMessage.Margin = new System.Windows.Forms.Padding(0);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(367, 42);
            this.labelMessage.TabIndex = 0;
            this.labelMessage.Text = "Loading EZBlocker 2...";
            this.labelMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelSeparatorUp
            // 
            this.panelSeparatorUp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.panelSeparatorUp.Location = new System.Drawing.Point(60, 80);
            this.panelSeparatorUp.Margin = new System.Windows.Forms.Padding(0);
            this.panelSeparatorUp.Name = "panelSeparatorUp";
            this.panelSeparatorUp.Size = new System.Drawing.Size(480, 3);
            this.panelSeparatorUp.TabIndex = 0;
            // 
            // imgSong
            // 
            this.imgSong.BackColor = System.Drawing.Color.Transparent;
            this.imgSong.BackgroundImage = global::EZBlocker2.Properties.Resources.Song;
            this.imgSong.Location = new System.Drawing.Point(60, 19);
            this.imgSong.Margin = new System.Windows.Forms.Padding(0);
            this.imgSong.Name = "imgSong";
            this.imgSong.Size = new System.Drawing.Size(60, 42);
            this.imgSong.TabIndex = 0;
            this.imgSong.TabStop = false;
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.spotifyToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.ShowImageMargin = false;
            this.contextMenuStrip.Size = new System.Drawing.Size(79, 44);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.openToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Padding = new System.Windows.Forms.Padding(0);
            this.openToolStripMenuItem.Size = new System.Drawing.Size(78, 20);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.OpenToolStripMenuItem_Click);
            // 
            // spotifyToolStripMenuItem
            // 
            this.spotifyToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.spotifyToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.spotifyToolStripMenuItem.Name = "spotifyToolStripMenuItem";
            this.spotifyToolStripMenuItem.Padding = new System.Windows.Forms.Padding(0);
            this.spotifyToolStripMenuItem.Size = new System.Drawing.Size(251, 20);
            this.spotifyToolStripMenuItem.Text = "Show status";
            this.spotifyToolStripMenuItem.Click += new System.EventHandler(this.SpotifyToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.exitToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Padding = new System.Windows.Forms.Padding(0);
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(78, 20);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.contextMenuStrip;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "EZBlocker 2";
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.NotifyIcon_MouseDoubleClick);
            // 
            // timerStatus
            // 
            this.timerStatus.Interval = 500;
            this.timerStatus.Tick += new System.EventHandler(this.TimerStatus_Tick);
            // 
            // timerSpotify
            // 
            this.timerSpotify.Interval = 1000;
            this.timerSpotify.Tick += new System.EventHandler(this.TimerSpotify_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 420);
            this.Controls.Add(this.panelMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "EZBlocker 2";
            this.Load += new System.EventHandler(this.Main_Load);
            this.Shown += new System.EventHandler(this.Main_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.imgSpotifyLogo)).EndInit();
            this.panelMain.ResumeLayout(false);
            this.panelStatusBar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imgEZBlockerLogo)).EndInit();
            this.panelContainer.ResumeLayout(false);
            this.panelContainer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgSong)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel panelStatusBar;
        private System.Windows.Forms.Panel panelContainer;
        private System.Windows.Forms.Button btnMinimize;
        private System.Windows.Forms.Button btnReportIssue;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.PictureBox imgEZBlockerLogo;
        private System.Windows.Forms.Button btnSndVol;
        private System.Windows.Forms.PictureBox imgSong;
        private System.Windows.Forms.Panel panelSeparatorUp;
        private System.Windows.Forms.Label labelMessage;
        private System.Windows.Forms.LinkLabel linkLabelDeveloper;
        private System.Windows.Forms.Panel panelSeparatorDown;
        private System.Windows.Forms.LinkLabel linkLabelDesigner;
        private System.Windows.Forms.LinkLabel linkLabelOriginalProject;
        private System.Windows.Forms.PictureBox imgSpotifyLogo;
        private System.Windows.Forms.CheckBox checkBoxMuteAds;
        private System.Windows.Forms.CheckBox checkBoxBlockAds;
        private System.Windows.Forms.CheckBox checkBoxStartOnLogin;
        private System.Windows.Forms.CheckBox checkBoxStartMinimized;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem spotifyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Timer timerStatus;
        private System.Windows.Forms.Timer timerSpotify;
    }
}

