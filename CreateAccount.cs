using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net;
using System.IO;

namespace MagpieLoginTest
{
    public partial class CreateAccount : Form
    {
        private string request = "";
        private string sLine = "";
        private WebRequest wrequest = null;
        private Stream objStream = null;
        private StreamReader objReader = null;

        public CreateAccount()
        {
            InitializeComponent();
            CreateErrorLabel.Hide();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CATextBox.Text = "";
            this.Hide();
            CreateErrorLabel.Hide();
            this.Owner.Focus();
        }

        private void CAExitButton_Click(object sender, EventArgs e)
        {
            this.CATextBox.Text = "";
            this.Hide();
            CreateErrorLabel.Hide();
            this.Owner.Focus();
        }

        private void SignUpButton_Click(object sender, EventArgs e)
        {
            string username = CATextBox.Text;
            request = "http://www.zjohns.com/magpie/login.php?User_ID=" + username;
            wrequest = WebRequest.Create(request);
            objStream = wrequest.GetResponse().GetResponseStream();
            objReader = new StreamReader(objStream);
            sLine = objReader.ReadLine();
            if(sLine != "[]")
            {
                CreateErrorLabel.Show();
            }
            else
            {
                request = "http://www.zjohns.com/magpie/create.php?User_ID=" + username;
                wrequest = WebRequest.Create(request);
                objStream = wrequest.GetResponse().GetResponseStream();
            }

        }
    }
}
