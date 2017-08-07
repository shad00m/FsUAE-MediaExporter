using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace fs_uae_mediaexportergui
{


    
    public class GetImages : Form1
    {

        private Form1 mMainForm;
        public GetImages(Form1 mainForm)
        {
            mMainForm = mainForm;
        }
        //ListBox listBox;
        public static WebClient Client = new WebClient();
        public static string RomName;
        public static string FrontImage;
        public static string TitleImage;
        public static string Screen1Image;
        public static string Screen2Image;
        public static string Screen3Image;
        public static string Screen4Image;
        public static string Screen5Image;
        public static string GameUUID;
        public static string VariantGameUUID;
        public static string VariantUUID;
        public static int Have;
        public static bool saveAsUuid;
        public static string initials;
        public static string imageName;
        public static string sourcePathToImage;
        public static string sourcePathToImageFull;
        public static string destImageFull;
        public static SQLiteConnection m_dbConnection;
        public static string sourcefileSuffix;
        public static string downloadSuffix;
        public static string targetPath;
        public static string Suffix;

        /*public GetImages(ListBox listBox)
        {
            this.listBox = listBox;
        }*/

        /*public void AddToListBox(string s)
        {
            listBox.Items.Add(s);
        }*/
        
        public void GetFrontImage(string image, string type, string suffix, bool saveAsUuid)
        {

            
            Suffix = suffix;
            if (type.Equals("front"))
            {
                targetPath = GetTargetPath() + "Box - Front" + "\\"; //
                sourcefileSuffix = "_106x134_lbcover.png";
                downloadSuffix = "?s=512&f=png";


            } else if (type.Equals("title"))
            {
                targetPath = GetTargetPath() + "Screenshot - Game Title" + "\\"; //
                sourcefileSuffix = "_1x.png";
                downloadSuffix = "?f=png&s=2x";
 

            } else
            {
                targetPath = GetTargetPath() + "Screenshot - Gameplay" + "\\"; //
                sourcefileSuffix = "_1x.png";
                downloadSuffix = "?f=png&s=2x";
                
            }
                

            if (image.Length >= 2 && Have == 4 && !saveAsUuid)
            {
                initials = image.Substring(5, 2); // xx
                imageName = image.Substring(5, 40) + sourcefileSuffix; // x^40_106x134_lbcover.png
                sourcePathToImage = GetFsUaePath() + "\\Data\\Images\\" + initials + "\\"; // c:\fsuae\data\Images\xx\
                sourcePathToImageFull = System.IO.Path.Combine(sourcePathToImage, imageName);
                destImageFull = System.IO.Path.Combine(targetPath, RomName + suffix); // c:\fsuae\moved\Game 01\Game 01.png

                if (!System.IO.File.Exists(sourcePathToImageFull)) // if the image doesnt exist in fs-uae download it from server
                {
                    Client.BaseAddress = @"http://oagd.net/image/";
                    Client.DownloadFile(image.Substring(5, 40) + downloadSuffix, sourcePathToImage + imageName);
                    //AddToListBox("Downloaded to fs-uae " + RomName);
                    mMainForm.listBox1.AddItemThreadSafe("Downloaded to fs-uae " + RomName);
                }
                if (!System.IO.Directory.Exists(sourcePathToImage)) // create hex image folder in fs-uae if doesnt exist
                {
                    System.IO.Directory.CreateDirectory(sourcePathToImage);
                    //AddToListBox("Created folder " + sourcePathToImage);
                    mMainForm.listBox1.AddItemThreadSafe("Created folder " + sourcePathToImage);
                }

                if (!System.IO.Directory.Exists(targetPath)) // create Launchbox image folders
                {
                    System.IO.Directory.CreateDirectory(targetPath);
                    //AddToListBox("Created folder " + targetPath);
                    mMainForm.listBox1.AddItemThreadSafe("Created folder " + targetPath);
                }

                if (Form1.IsRadio2Checked)
                {
                    if (!System.IO.File.Exists(destImageFull) && System.IO.File.Exists(sourcePathToImageFull))
                    {
                        System.IO.File.Copy(sourcePathToImageFull, destImageFull, true);
                        //AddToListBox("Copied image " + destImageFull);
                        mMainForm.listBox1.AddItemThreadSafe("Copied image " + destImageFull);
                    }
                }
                
                else
                {
                    SaveUuid();
                }
                //if image doesnt exist in target already but exists in fs-uae folder copy it to target
                
            }
            
            return;
        }
        public void Download()
        {
            string dataSource = GetFsUaePath() + "Data\\Databases\\Launcher.sqlite";
            m_dbConnection = new SQLiteConnection("Data Source=" + dataSource);
            m_dbConnection.Open();
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM game ", m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                RomName =
                    Convert.ToString(reader["name"])
                        .Replace(":", "")
                        .Replace("'", "_")
                        .Replace("/", "")
                        .Replace("\"", "_")
                        .Replace("!", "")
                        .Replace("?", "_");
                
                FrontImage = Convert.ToString(reader["front_image"]);
                TitleImage = Convert.ToString(reader["title_image"]);
                Screen1Image = Convert.ToString(reader["screen1_image"]);
                Screen2Image = Convert.ToString(reader["screen2_image"]);
                Screen3Image = Convert.ToString(reader["screen3_image"]);
                Screen4Image = Convert.ToString(reader["screen4_image"]);
                Screen5Image = Convert.ToString(reader["screen5_image"]);
                Have = Convert.ToInt16(reader["Have"]);
                GameUUID = Convert.ToString(reader["uuid"]); // 146c7d69-79ed-5a8c-af26-b117cd9a4edd

                GetFrontImage(FrontImage, "front", ".png", false);
                GetFrontImage(TitleImage, "title", ".png", false);
                GetFrontImage(Screen1Image, "screen", "-01.png", false);
                GetFrontImage(Screen2Image, "screen", "-02.png", false);
                GetFrontImage(Screen3Image, "screen", "-03.png", false);
                GetFrontImage(Screen4Image, "screen", "-04.png", false);
                GetFrontImage(Screen5Image, "screen", "-05.png", false);
            }
            m_dbConnection.Close();
            MessageBox.Show("Finished Successfully");
        }

        public void SaveUuid()
        {
            
            SQLiteCommand commandTest = new SQLiteCommand("Select * From Game_Variant WHERE game_uuid='" + GameUUID + "' and have='4';", m_dbConnection);
            SQLiteDataReader readerTest = commandTest.ExecuteReader();
            string uuidPath;
            while (readerTest.Read())
            {
                VariantUUID = Convert.ToString(readerTest["uuid"]);
                uuidPath = System.IO.Path.Combine(targetPath, VariantUUID + Suffix);
                if (System.IO.File.Exists(sourcePathToImageFull) && !System.IO.File.Exists(uuidPath))
                {
                    System.IO.File.Copy(sourcePathToImageFull, uuidPath, true);
                    //AddToListBox("Copied image to " + uuidPath);
                    //mMainForm.listBox1.AddItemThreadSafe("Copied image to " + uuidPath);
                    mMainForm.listBox1.AddItemThreadSafe("Copied image for " + RomName);
                }

            }


        }



    }

}
