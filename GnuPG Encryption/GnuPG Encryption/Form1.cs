using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Funcional.GnuPG;

namespace GnuPG_Encryption
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileManager gfm = new FileManager(txtPathKey.Text.Trim(), txtEmailKey.Text.Trim());

                //gfm.Source = @"C:\Temp\t.txt";
                //gfm.Destination = @"C:\Temp\t.txt.encriptado";

                gfm.Source = openFileDialog1.FileName;
                gfm.Destination = openFileDialog1.FileName + ".encripted";

                gfm.Encrypt();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtEmailKey.Text = System.Configuration.ConfigurationManager.AppSettings["EmailPublicKeyLoreal"];
            txtPathKey.Text = System.Configuration.ConfigurationManager.AppSettings["PublicKeyLoreal"];
        }
    }
}
