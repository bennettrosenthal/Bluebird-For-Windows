using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Net;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Compression;
using System.Windows.Media.Animation;
using System.Net.Mime;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualBasic;
using System.Windows.Automation;

namespace Bluebird_For_Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // creates program files (x86) directory and, if the txt file exists, deletes it and redownloads it
            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + "\\ModernEra");
            String folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + "\\ModernEra";
            WebClient dl = new WebClient();
            Uri txtURL = new Uri("https://thesideloader.co.uk/upsiopts.txt");
            if (File.Exists(folderPath + "\\upsiopts.txt"))
            {
                File.Delete(folderPath + "\\upsiopts.txt");
            }
            dl.DownloadFile(txtURL, folderPath + "\\upsiopts.txt");
            
            // creates an array to read the txt into an array
            string[] txtLines = File.ReadAllLines(folderPath + "\\upsiopts.txt");
            string temp;

            // loops through array, looking for the names of the games then drops them into the dropdown
            foreach (string line in txtLines)
            {
                if (line.StartsWith("NAME="))
                {
                    temp = line.Substring(line.IndexOf("NAME=")).Replace("NAME=", "");
                    pogcheck.Items.Add(temp);
                }
            }

            // hide progress bar
            pogbar.Visibility = Visibility.Hidden;
        }

        private void pogcheck_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void pogcheck_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Object gameChosen = pogcheck.SelectedItem;
        }

        private void pogcheck_Click(object sender, MouseEventArgs e)
        {
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // gets path to data folder
            String folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + "\\ModernEra";
            // reads into a string, then splits the string into an array, where each item is a portion of the txt, split by the word "END"
            string txtLines = File.ReadAllText(folderPath + "\\upsiopts.txt");
            string[] split = txtLines.Split(new string[] { "END" }, StringSplitOptions.None);
            // gets the selected item's index, and because the order of the names will be the same as the one in the txt, 
            // uses it to select the game's array item
            int index = pogcheck.SelectedIndex;
            string[] gameArray = split[index].Split(new string[] { "\n" }, StringSplitOptions.None);
            
            // getting the required details for download and install
            string gameURL = "";
            string gameName = "";
            string gameZip = "";
            string gameID = "";
            string apkName = "";
            string obbName = "";
            string txtFileName = "";
            foreach (string line in gameArray)
            {
                if (line.StartsWith("DOWNLOADFROM="))
                {
                    string temp = line.Substring(line.IndexOf("DOWNLOADFROM=")).Replace("DOWNLOADFROM=", "");
                    string temp2 = temp.Replace("\r", "");
                    gameURL = temp2;
                }

                if (line.StartsWith("NAME="))
                {
                    string temp = line.Substring(line.IndexOf("NAME=")).Replace("NAME=", "");
                    string temp2 = temp.Replace("\r", "");
                    gameName = temp2;
                }

                if (line.StartsWith("ZIPNAME="))
                {
                    string temp = line.Substring(line.IndexOf("ZIPNAME=")).Replace("ZIPNAME=", "");
                    string temp2 = temp.Replace("\r", "");
                    gameZip = temp2;
                }

                if (line.StartsWith("COMOBJECT="))
                {
                    string temp = line.Substring(line.IndexOf("COMOBJECT=")).Replace("COMOBJECT=", "");
                    string temp2 = temp.Replace("\r", "");
                    gameID = temp2;
                }

                if (line.StartsWith("APK="))
                {
                    string temp = line.Substring(line.IndexOf("APK=")).Replace("APK=", "");
                    string temp2 = temp.Replace("\r", "");
                    apkName = temp2;
                }

                if (line.StartsWith("OBB="))
                {
                    string temp = line.Substring(line.IndexOf("OBB=")).Replace("OBB=", "");
                    string temp2 = temp.Replace("\r", "");
                    obbName = temp2;
                }

                if (line.StartsWith("INPUTFILENAME="))
                {
                    string temp = line.Substring(line.IndexOf("INPUTFILENAME=")).Replace("INPUTFILENAME=", "");
                    string temp2 = temp.Replace("\r", "");
                    txtFileName = temp2;
                }
            }

            // set up download environment

            Uri gameDL = new Uri(gameURL);
            if (Directory.Exists(folderPath + "\\" + gameName))
            {
                Directory.Delete(folderPath + "\\" + gameName, true);
            }
            if (Directory.Exists(folderPath + "\\" + "adb"))
            {
                Directory.Delete(folderPath + "\\" + "adb", true);
            }
            if (File.Exists(folderPath + "\\name.txt"))
            {
                File.Delete(folderPath + "\\name.txt");
            }
            Directory.CreateDirectory(folderPath + "\\" + gameName);

            // declare event handler for the DL, as if we did this syncronised the UI would freeze, and w/o the handler it would just move on
            //adbCommand pog = new adbCommand();
            //pog.adbCommands(folderPath, gameName, gameID, apkName, obbName, txtFileName, pogbox);
            pogbox.Text = "Downloading game...";
            pogbar.Visibility = Visibility.Visible;

            WebClient BBBB = new WebClient();
            BBBB.DownloadFileCompleted += new AsyncCompletedEventHandler(ayo);
            BBBB.DownloadProgressChanged += new DownloadProgressChangedEventHandler(yuh);
            BBBB.DownloadFileAsync(gameDL, folderPath + "\\" + gameName + "\\" + gameZip);

                void yuh(object bender, DownloadProgressChangedEventArgs d)
                {
                    pogbar.Value = d.ProgressPercentage;
                }

                async void ayo(object ender, AsyncCompletedEventArgs f)
                {
                    pogbar.Visibility = Visibility.Hidden;
                    pogbox.Text = "Download complete, unzipping " + gameName + "..."; 
                    await Task.Run(() => ZipFile.ExtractToDirectory(folderPath + "\\" + gameName + "\\" + gameZip, folderPath + "\\" + gameName)); 
                    pogbox.Text = "Unzipping complete...";
                    string name = Interaction.InputBox("What should your name be for " + gameName + "?", "Input Name");
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(@Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + "\\ModernEra\\name.txt", true))
                    {
                        file.WriteLine(name);
                    }
                    
                    adbCommands pog = new adbCommands();
                    await Task.Run(() => pog.uninstall(gameID));
                    pogbox.Text = gameName + " uninstalled if present! Installing APK...";
                    await Task.Run(() => pog.installAPK(folderPath, gameName, apkName));
                    pogbox.Text = "APK installed! Setting permissions...";
                    await Task.Run(() => pog.grantPermissions(gameID));
                    pogbox.Text = "Permissions set! Pushing OBB...";
                    await Task.Run(() => pog.pushOBB(folderPath, gameName, obbName, gameID));
                    pogbox.Text = "OBB pushed! Setting name...";
                    await Task.Run(() => pog.pushName(folderPath, txtFileName));
                    pog.killADB();

                    if (Directory.Exists(folderPath + "\\" + gameName))
                    {
                        Directory.Delete(folderPath + "\\" + gameName, true);
                    }
                    pogbox.Text = gameName + " installed!";
            }
        }

        async void uninstallButton_Click(object sender, RoutedEventArgs e)
        {
            // gets path to data folder
            String folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + "\\ModernEra";
            // reads into a string, then splits the string into an array, where each item is a portion of the txt, split by the word "END"
            string txtLines = File.ReadAllText(folderPath + "\\upsiopts.txt");
            string[] split = txtLines.Split(new string[] { "END" }, StringSplitOptions.None);
            // gets the selected item's index, and because the order of the names will be the same as the one in the txt, 
            // uses it to select the game's array item
            int index = pogcheck.SelectedIndex;
            string[] gameArray = split[index].Split(new string[] { "\n" }, StringSplitOptions.None);

            string gameID = "";
            string gameName = "";

            foreach (string line in gameArray)
            {
                if (line.StartsWith("COMOBJECT="))
                {
                    string temp = line.Substring(line.IndexOf("COMOBJECT=")).Replace("COMOBJECT=", "");
                    string temp2 = temp.Replace("\r", "");
                    gameID = temp2;
                }

                if (line.StartsWith("NAME="))
                {
                    string temp = line.Substring(line.IndexOf("NAME=")).Replace("NAME=", "");
                    string temp2 = temp.Replace("\r", "");
                    gameName = temp2;
                }
            }
            pogbox.Text = "Uninstalling " + gameName + "...";
            adbCommands un = new adbCommands();
            await Task.Run(() => un.uninstall(gameID));
            un.killADB();
            pogbox.Text = gameName + " uninstalled!";
        }

        async void permissionsButton_Click(object sender, RoutedEventArgs e)
        {
            // gets path to data folder
            String folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + "\\ModernEra";
            // reads into a string, then splits the string into an array, where each item is a portion of the txt, split by the word "END"
            string txtLines = File.ReadAllText(folderPath + "\\upsiopts.txt");
            string[] split = txtLines.Split(new string[] { "END" }, StringSplitOptions.None);
            // gets the selected item's index, and because the order of the names will be the same as the one in the txt, 
            // uses it to select the game's array item
            int index = pogcheck.SelectedIndex;
            string[] gameArray = split[index].Split(new string[] { "\n" }, StringSplitOptions.None);

            string gameID = "";
            string gameName = "";

            foreach (string line in gameArray)
            {
                if (line.StartsWith("COMOBJECT="))
                {
                    string temp = line.Substring(line.IndexOf("COMOBJECT=")).Replace("COMOBJECT=", "");
                    string temp2 = temp.Replace("\r", "");
                    gameID = temp2;
                }

                if (line.StartsWith("NAME="))
                {
                    string temp = line.Substring(line.IndexOf("NAME=")).Replace("NAME=", "");
                    string temp2 = temp.Replace("\r", "");
                    gameName = temp2;
                }
            }

            pogbox.Text = "Setting permissions for " + gameID + "...";
            adbCommands perms = new adbCommands();
            await Task.Run(() => perms.grantPermissions(gameID));
            perms.killADB();
            pogbox.Text = "Permissions set for " + gameName + "!";
        }

        async void nameButton_Click(object sender, RoutedEventArgs e)
        {
            // gets path to data folder
            String folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + "\\ModernEra";
            // reads into a string, then splits the string into an array, where each item is a portion of the txt, split by the word "END"
            string txtLines = File.ReadAllText(folderPath + "\\upsiopts.txt");
            string[] split = txtLines.Split(new string[] { "END" }, StringSplitOptions.None);
            // gets the selected item's index, and because the order of the names will be the same as the one in the txt, 
            // uses it to select the game's array item
            int index = pogcheck.SelectedIndex;
            string[] gameArray = split[index].Split(new string[] { "\n" }, StringSplitOptions.None);

            string txtFileName = "";
            string gameName = "";

            foreach (string line in gameArray)
            {
                if (line.StartsWith("INPUTFILENAME="))
                {
                    string temp = line.Substring(line.IndexOf("INPUTFILENAME=")).Replace("INPUTFILENAME=", "");
                    string temp2 = temp.Replace("\r", "");
                    txtFileName = temp2;
                }

                if (line.StartsWith("NAME="))
                {
                    string temp = line.Substring(line.IndexOf("NAME=")).Replace("NAME=", "");
                    string temp2 = temp.Replace("\r", "");
                    gameName = temp2;
                }
            }
            
            if (File.Exists(folderPath + "\\name.txt"))
            {
                File.Delete(folderPath + "\\name.txt");
            }
            string name = Interaction.InputBox("What should your name be for " + gameName + "?", "Input Name");
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + "\\ModernEra\\name.txt", true))
            {
                file.WriteLine(name);
            }
            adbCommands nam = new adbCommands();
            await Task.Run(() => nam.pushName(folderPath, txtFileName));
            nam.killADB();
            pogbox.Text = "Name set for " + gameName + "!"; 
        }

        async void mapButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fileDialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = fileDialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                string mapDir = fileDialog.SelectedPath;
                string mapName = System.IO.Path.GetDirectoryName(mapDir);
                mapName = mapDir.Replace(mapName, "");
                mapName = mapName.Replace("\\", "");
                pogbox.Text = "Pushing " + mapName + "...";

                adbCommands map = new adbCommands();
                await Task.Run(() => map.pushMap(mapName, mapDir));
                map.killADB();
                pogbox.Text = mapName + " pushed!";
            }
        }
    }
    }
