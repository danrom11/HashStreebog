using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace HashStreebog
{
    public partial class FormMain : Form
    {
        private int hashBlock = 256;
        public FormMain()
        {
            InitializeComponent();
            statusStrip1.Visible = false;
            toolStripComboBox1.Text = "256";

            toolStripComboBox1.SelectedIndexChanged += (e, sender) => { hashBlock = Convert.ToInt32(toolStripComboBox1.Text); };
            copyToolStripMenuItem.Click += (e, sender) => {  };
        }

        async private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            Streebog streebog = new Streebog(hashBlock);
            openFile.Multiselect = true;
            if(openFile.ShowDialog() == DialogResult.OK)
            {
                statusStrip1.Visible = true;
                menuStrip1.Enabled = false;
                int ready = 0;
                toolStripProgressBar.Maximum = openFile.FileNames.Length;
                toolStripStatusLabel.Text = ready.ToString() + " | " + openFile.FileNames.Length;
                foreach(string fn in openFile.FileNames)
                {
                    string hesh = "0x00";
                    await Task.Run(() =>
                    {
                        hesh = BitConverter.ToString(streebog.GetHash(System.IO.File.ReadAllBytes(fn)));
                    });
                    ready++;
                    dataGridView.Rows.Add(fn, hesh);
                    toolStripProgressBar.Value++;             
                    toolStripStatusLabel.Text = ready.ToString() + " | " + openFile.FileNames.Length;
                    
                }
                statusStrip1.Visible = false;
                menuStrip1.Enabled = true;
                toolStripProgressBar.Value = 0;
            }
        }

        async private void DirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Streebog streebog = new Streebog(hashBlock);
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowNewFolderButton = false;
            if(folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string[] files = Directory.GetFiles(folderBrowserDialog.SelectedPath);
                statusStrip1.Visible = true;
                menuStrip1.Enabled = false;
                int ready = 0;
                toolStripProgressBar.Maximum = files.Length;
                toolStripStatusLabel.Text = ready.ToString() + " | " + files.Length;
                foreach (string fn in files)
                {
                    string hesh = "0x00";
                    await Task.Run(() =>
                    {
                        hesh = BitConverter.ToString(streebog.GetHash(System.IO.File.ReadAllBytes(fn)));
                    });
                    ready++;
                    dataGridView.Rows.Add(fn, hesh);
                    toolStripProgressBar.Value++;
                    toolStripStatusLabel.Text = ready.ToString() + " | " + files.Length;
                }
                statusStrip1.Visible = false;
                menuStrip1.Enabled = true;
                toolStripProgressBar.Value = 0;
            }
        }
    }
}
