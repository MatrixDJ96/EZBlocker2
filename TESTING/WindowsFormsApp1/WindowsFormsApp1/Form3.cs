using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.JSON;

namespace WindowsFormsApp1
{
    public partial class Form3 : Form
    {
        private MyWebClient client = null;

        public Form3(MyWebClient client)
        {
            InitializeComponent();
            this.client = client;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            GetINFO();
        }

        public void GetINFO()
        {
            timer1.Enabled = false;

            byte[] result = client.DownloadData("https://api.spotify.com/v1/me/player/currently-playing");

            if (result != null && result.Length > 0)
            {
                Response status = JsonConvert.DeserializeObject<Response>(Encoding.UTF8.GetString(result));

                playingCb.Checked = status.Is_playing;
                privateCb.Checked = false;

                if (status.Item != null)
                {
                    progressBar1.Maximum = status.Item.Duration_ms;
                    progressBar1.Value = status.Progress_ms;

                    adsCb.Checked = false;

                    songLbl.Text = status.Item.Name;
                    albumLbl.Text = status.Item.Album.Name;

                    string artists = "";
                    foreach (var artist in status.Item.Artists)
                    {
                        if (artists != string.Empty)
                            artists += " - ";

                        artists += artist.Name;
                    }

                    artistsLbl.Text = artists;
                }
                else
                {
                    progressBar1.Value = 0;

                    adsCb.Checked = true;

                    songLbl.Text = "";
                    albumLbl.Text = "";
                    artistsLbl.Text = "";
                }
            }
            else
            {
                progressBar1.Value = 0;

                playingCb.Checked = false;
                adsCb.Checked = false;
                privateCb.Checked = true;
                
                songLbl.Text = "";
                albumLbl.Text = "";
                artistsLbl.Text = "";
            }

            timer1.Enabled = true;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            GetINFO();
        }
    }
}
