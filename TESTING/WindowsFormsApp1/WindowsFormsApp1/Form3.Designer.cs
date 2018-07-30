namespace WindowsFormsApp1
{
    partial class Form3
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
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.songLbl = new System.Windows.Forms.Label();
            this.albumLbl = new System.Windows.Forms.Label();
            this.artistsLbl = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.adsCb = new System.Windows.Forms.CheckBox();
            this.privateCb = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.playingCb = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 37);
            this.label1.Margin = new System.Windows.Forms.Padding(5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Track:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 83);
            this.label3.Margin = new System.Windows.Forms.Padding(5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Artists:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 60);
            this.label2.Margin = new System.Windows.Forms.Padding(5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Albulm:";
            // 
            // songLbl
            // 
            this.songLbl.AutoSize = true;
            this.songLbl.Location = new System.Drawing.Point(90, 37);
            this.songLbl.Margin = new System.Windows.Forms.Padding(5);
            this.songLbl.Name = "songLbl";
            this.songLbl.Size = new System.Drawing.Size(0, 13);
            this.songLbl.TabIndex = 3;
            // 
            // albumLbl
            // 
            this.albumLbl.AutoSize = true;
            this.albumLbl.Location = new System.Drawing.Point(90, 60);
            this.albumLbl.Margin = new System.Windows.Forms.Padding(5);
            this.albumLbl.Name = "albumLbl";
            this.albumLbl.Size = new System.Drawing.Size(0, 13);
            this.albumLbl.TabIndex = 4;
            // 
            // artistsLbl
            // 
            this.artistsLbl.AutoSize = true;
            this.artistsLbl.Location = new System.Drawing.Point(90, 83);
            this.artistsLbl.Margin = new System.Windows.Forms.Padding(5);
            this.artistsLbl.Name = "artistsLbl";
            this.artistsLbl.Size = new System.Drawing.Size(0, 13);
            this.artistsLbl.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(220, 14);
            this.label4.Margin = new System.Windows.Forms.Padding(5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "IS ADS?";
            // 
            // adsCb
            // 
            this.adsCb.AutoSize = true;
            this.adsCb.Enabled = false;
            this.adsCb.Location = new System.Drawing.Point(276, 14);
            this.adsCb.Name = "adsCb";
            this.adsCb.Size = new System.Drawing.Size(15, 14);
            this.adsCb.TabIndex = 8;
            this.adsCb.UseVisualStyleBackColor = true;
            // 
            // privateCb
            // 
            this.privateCb.AutoSize = true;
            this.privateCb.Enabled = false;
            this.privateCb.Location = new System.Drawing.Point(197, 14);
            this.privateCb.Name = "privateCb";
            this.privateCb.Size = new System.Drawing.Size(15, 14);
            this.privateCb.TabIndex = 10;
            this.privateCb.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(117, 14);
            this.label5.Margin = new System.Windows.Forms.Padding(5);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "IS PRIVATE?";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(17, 104);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(274, 23);
            this.progressBar1.Step = 1;
            this.progressBar1.TabIndex = 11;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // playingCb
            // 
            this.playingCb.AutoSize = true;
            this.playingCb.Enabled = false;
            this.playingCb.Location = new System.Drawing.Point(94, 14);
            this.playingCb.Name = "playingCb";
            this.playingCb.Size = new System.Drawing.Size(15, 14);
            this.playingCb.TabIndex = 14;
            this.playingCb.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 14);
            this.label6.Margin = new System.Windows.Forms.Padding(5);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "IS PLAYING?";
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(306, 136);
            this.Controls.Add(this.playingCb);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.privateCb);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.adsCb);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.artistsLbl);
            this.Controls.Add(this.albumLbl);
            this.Controls.Add(this.songLbl);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form3";
            this.Text = "Form3";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label songLbl;
        private System.Windows.Forms.Label albumLbl;
        private System.Windows.Forms.Label artistsLbl;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox adsCb;
        private System.Windows.Forms.CheckBox privateCb;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.CheckBox playingCb;
        private System.Windows.Forms.Label label6;
    }
}