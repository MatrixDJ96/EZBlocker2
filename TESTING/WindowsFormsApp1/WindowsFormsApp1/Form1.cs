﻿using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Windows.Forms;
using WindowsFormsApp1.JSON;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private MyWebServer server = null;
        private MyWebClient client = null;

        private Form form = null;

        private string code = null;
        JSON.Authorization auth = null;

        public Form1()
        {
            InitializeComponent();

            server = new MyWebServer(5515);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            server.Stop();
            base.OnFormClosing(e);
        }

        private void Start_Server(object sender, EventArgs e)
        {
            server.Start();
            label1.Text = "Server: " + server.Address + ":" + server.Port;

            stopBtn.Enabled = true;
            startBtn.Enabled = false;
        }

        private void Stop_Server(object sender, EventArgs e)
        {
            server.Stop();
            label1.Text = "Server: Offline";

            startBtn.Enabled = true;
            stopBtn.Enabled = false;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            startBtn.PerformClick();

            form = new Form2();
            form.Show();

            try
            {                
                NameValueCollection data = HttpUtility.ParseQueryString(string.Empty);

                data.Add("client_id", "b6058737c9a048ea88a7282b25374c2c");
                data.Add("response_type", "code");
                data.Add("redirect_uri", Uri.EscapeUriString(server.Address + ":" + server.Port));
                data.Add("scope", Uri.EscapeDataString("user-read-currently-playing"));

                ((Form2)form).webBrowser1.Navigated += WebBrowser1_Navigated;
                ((Form2)form).webBrowser1.Navigate("https://accounts.spotify.com/authorize?" + data.ToString());
            }
            catch (WebException ex)
            {
                StreamReader stream = new StreamReader(ex.Response.GetResponseStream());
                string response = "";
                while (stream.Peek() != -1)
                {
                    response += stream.ReadLine() + Environment.NewLine;
                }

                MessageBox.Show(ex.Status.ToString() + Environment.NewLine + response, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WebBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            if (e.Url != new Uri("about:blank"))
            {
                NameValueCollection response = HttpUtility.ParseQueryString(e.Url.Query);

                if (response.Get("error") == null && response.Get("code") != null)
                {
                    form.Close();

                    code = response.Get("code");

                    client = new MyWebClient();
                    client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    client.Headers[HttpRequestHeader.Authorization] = "Basic YjYwNTg3MzdjOWEwNDhlYTg4YTcyODJiMjUzNzRjMmM6NzIyMDMzZDA4YTk0NGU2MmI1ODI2ZjFhOTkzNzczNTg=";

                    NameValueCollection data = HttpUtility.ParseQueryString(string.Empty);

                    data.Add("grant_type", "authorization_code");
                    data.Add("code", Uri.EscapeDataString(code));
                    data.Add("redirect_uri", Uri.EscapeUriString(server.Address + ":" + server.Port));
                    
                    try
                    {
                        byte[] result = client.UploadValues("https://accounts.spotify.com/api/token", "POST", data);

                        auth = JsonConvert.DeserializeObject<JSON.Authorization>(Encoding.UTF8.GetString(result));
                        
                        client.Headers[HttpRequestHeader.Authorization] = auth.Token_type + " " + auth.Access_token;
                        
                        form = new Form3(client);
                        form.Show();

                        ((Form3)form).GetINFO();
                    }
                    catch (WebException ex)
                    {
                        string message = "";

                        StreamReader stream = new StreamReader(ex.Response.GetResponseStream());

                        while (stream.Peek() != -1)
                            message += stream.ReadLine() + Environment.NewLine;

                        MessageBox.Show(message, "Error - " + ex.Status.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
