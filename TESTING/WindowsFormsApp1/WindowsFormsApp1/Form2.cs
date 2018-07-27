using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();

            textBox1.KeyDown += ChangeURL;
            webBrowser1.Navigated += SetTextURL;
        }
        
        private void SetTextURL(object sender, WebBrowserNavigatedEventArgs e)
        {
            WebBrowser webBrowser = sender as WebBrowser;

            textBox1.Text = GetURL(webBrowser1);
        }

        private void ChangeURL(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (e.KeyCode == Keys.Enter && GetURL(webBrowser1) != textBox.Text)
                webBrowser1.Navigate(textBox.Text);
        }

        public static string GetURL(WebBrowser webBrowser)
        {
            return webBrowser.Url != null ? webBrowser.Url.ToString() : string.Empty;
        }
    }
}
