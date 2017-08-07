using System;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace fs_uae_mediaexportergui
{
    
    public partial class Form1 : Form
    {
        
        public static string TargetPath { get; set; }
        public static string FsUaePath { get; set; }
        public static bool IsRadio1Checked;
        public static bool IsRadio2Checked;
      
        


        public string GetFsUaePath()
        {
            return FsUaePath + "\\";
        }

        public string GetTargetPath()
        {
            return TargetPath + "\\"; //test commit
        }

        public string SetFsUaePath(string path)
        {
            FsUaePath = path;
            return path;
        }

        public string SetTargetPath(string path)
        {
            TargetPath = path;
            return path;
        }

        public Form1()
        {
            InitializeComponent();
            

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            /*listBox1.Items.Add("Loaded FS-UAE to Launchbox Media Exporter");
            GetImages myGetImages = new GetImages(listBox1);*/

        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {
            var result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                //
                // The user selected a folder and pressed the OK button.
                // We print the number of files found.
                //
                var files = Directory.GetFiles(folderBrowserDialog1.SelectedPath);
                MessageBox.Show("Files found: " + files.Length, "Message");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            var fbd = new FolderBrowserDialog();
            fbd.Description = "Select your FS-UAE root folder where Launcher.exe is. \n e.g. c:\\programfiles\\fs-uae\\";
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                if (System.IO.File.Exists(fbd.SelectedPath + "\\Launcher.exe"))
                {

                    textBoxFSUAE.Text = fbd.SelectedPath;
                    FsUaePath = textBoxFSUAE.Text;
                    SetFsUaePath(fbd.SelectedPath);
                    listBox1.Items.Add("Found FS-UAE Launcher.exe in " + fbd.SelectedPath);
                }
                else
                {
                    MessageBox.Show("Launcher.exe not found in folder. \nMake sure its the Root folder of FS-UAE.");
                    listBox1.Items.Add("Could not find Launcher.exe - Is it the FS-UAE Root? ");
                }
                
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var fbd = new FolderBrowserDialog();
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;
            string caption = "Error";

            fbd.Description = "Select where your images should be saved to. \n Can be any folder.";
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                if (!IsDirectoryEmpty(fbd.SelectedPath))
                {
                    result =
                        MessageBox.Show(
                            "Directory is not empty. \nAre you sure you want to export to a non-empty folder?", caption, buttons);
                    if (result == System.Windows.Forms.DialogResult.No)
                    {
                        listBox1.Items.Add("Target folder not empty - aborted");
                    }
                    else
                    {
                        listBox1.Items.Add("Target folder is not empty! - set to " + fbd.SelectedPath);
                        textBoxTarget.Text = fbd.SelectedPath;
                        TargetPath = textBoxTarget.Text;
                        SetTargetPath(fbd.SelectedPath);
                    }

                    
                }
                else
                {
                    listBox1.Items.Add("Target path empty. - set to " + fbd.SelectedPath);
                    textBoxTarget.Text = fbd.SelectedPath;
                    TargetPath = textBoxTarget.Text;
                    SetTargetPath(fbd.SelectedPath);
                }
            }
        }



        private void button3_Click(object sender, EventArgs e)
        {
            if (!backgroundWorker1.IsBusy)
            {
                backgroundWorker1.RunWorkerAsync();
            } else
            {
                listBox1.Items.Add("Program is busy");
            }

            
        }
        public bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            IsRadio1Checked = false;
            IsRadio2Checked = false;
            if (radioButton2.Checked)
            {
                listBox1.Items.Add("Image files will be saved as game name e.g: \"Alien Breed 3D.png\" ");
            }
            
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            IsRadio1Checked = false;
            IsRadio2Checked = false;
            if (radioButton1.Checked)
            {
                listBox1.Items.Add("Image files will be saved as UUID e.g.: \"0b0ce6fbaae750757224204d3151834d029b4528.png\"");
            }
            
        }

        private void textBoxTarget_TextChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            
            
        }

        public void Adder(string s)
        {
            if (s.Length < 1)
            {
                MessageBox.Show("String is empty");
            }
            else
            {
                listBox1.Items.Add("KEK");
                
            }
            
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            
            
            if (radioButton1.Checked == true)
            {

                IsRadio1Checked = true;
            }

            if (radioButton2.Checked == true)
            {

                IsRadio2Checked = true;
            }
            if (textBoxFSUAE.Text.Length > 1 && textBoxTarget.Text.Length > 1)
            {
                GetImages myGetImages = new GetImages(this);
                myGetImages.Download();
               
            }
            else
            {

                MessageBox.Show("At least one path is empty!");
                listBox1.Items.Add("Path is empty - aborted");
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {

        }
    }
}