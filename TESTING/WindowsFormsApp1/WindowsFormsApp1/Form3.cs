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
            byte[] result = client.DownloadData("https://api.spotify.com/v1/me/player/currently-playing");

            Response status = JsonConvert.DeserializeObject<Response>(Encoding.UTF8.GetString(result));

            if (status.Item != null)
            {
                checkBox1.Checked = false;

                songLbl.Text = status.Item.Name;
                albumLbl.Text = status.Item.Album.Name;

                artistsLbl.Text = "";
                foreach (var artist in status.Item.Artists)
                {
                    if (artistsLbl.Text != string.Empty)
                        artistsLbl.Text += " - ";

                    artistsLbl.Text += artist.Name;
                }
            }
            else
            {
                checkBox1.Checked = true;

                songLbl.Text = "";
                albumLbl.Text = "";
                artistsLbl.Text = "";
            }
        }
    }
}
