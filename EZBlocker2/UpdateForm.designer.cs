namespace EZBlocker2
{
    partial class UpdateForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdateForm));
            this.panelStatusBar = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.titleLabel = new System.Windows.Forms.Label();
            this.panelMain = new System.Windows.Forms.Panel();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.labelMessage = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.panelStatusBar.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelStatusBar
            // 
            this.panelStatusBar.BackColor = System.Drawing.Color.Transparent;
            this.panelStatusBar.Controls.Add(this.btnExit);
            this.panelStatusBar.Controls.Add(this.titleLabel);
            this.panelStatusBar.Location = new System.Drawing.Point(0, 0);
            this.panelStatusBar.Margin = new System.Windows.Forms.Padding(0);
            this.panelStatusBar.Name = "panelStatusBar";
            this.panelStatusBar.Size = new System.Drawing.Size(350, 30);
            this.panelStatusBar.TabIndex = 0;
            // 
            // btnExit
            // 
            this.btnExit.BackgroundImage = global::EZBlocker2.Properties.Resources.Exit;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btnExit.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Location = new System.Drawing.Point(300, 0);
            this.btnExit.Margin = new System.Windows.Forms.Padding(0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(50, 30);
            this.btnExit.TabIndex = 1;
            this.btnExit.TabStop = false;
            this.toolTip.SetToolTip(this.btnExit, "Abort update");
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.BtnExit_Click);
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
            this.titleLabel.Size = new System.Drawing.Size(350, 30);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "Updater";
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelMain
            // 
            this.panelMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.panelMain.Controls.Add(this.progressBar);
            this.panelMain.Controls.Add(this.labelMessage);
            this.panelMain.Controls.Add(this.panelStatusBar);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Margin = new System.Windows.Forms.Padding(0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(350, 114);
            this.panelMain.TabIndex = 1;
            // 
            // progressBar
            // 
            this.progressBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.progressBar.Location = new System.Drawing.Point(15, 75);
            this.progressBar.Margin = new System.Windows.Forms.Padding(0);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(320, 25);
            this.progressBar.TabIndex = 3;
            // 
            // labelMessage
            // 
            this.labelMessage.BackColor = System.Drawing.Color.Transparent;
            this.labelMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMessage.ForeColor = System.Drawing.Color.White;
            this.labelMessage.Location = new System.Drawing.Point(0, 30);
            this.labelMessage.Margin = new System.Windows.Forms.Padding(0);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(350, 40);
            this.labelMessage.TabIndex = 2;
            this.labelMessage.Text = "Starting download...";
            this.labelMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UpdateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(350, 114);
            this.Controls.Add(this.panelMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "UpdateForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Updater";
            this.Load += new System.EventHandler(this.UpdateForm_Load);
            this.panelStatusBar.ResumeLayout(false);
            this.panelMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelStatusBar;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Label labelMessage;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.ToolTip toolTip;
    }
}