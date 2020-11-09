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
using System.DirectoryServices.ActiveDirectory;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Compression;
using System.Windows.Media.Animation;
using System.Net.Mime;

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
            String folderPath = new string(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + "\\ModernEra");
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
        }

        private void pogcheck_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void pogcheck_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Object gameChosen = pogcheck.SelectedItem;
            pogbox.Text = gameChosen.ToString();
        }

        private void pogcheck_Click(object sender, MouseEventArgs e)
        {
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // gets path to data folder
            String folderPath = new string(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + "\\ModernEra");
            // reads into a string, then splits the string into an array, where each item is a portion of the txt, split by the word "END"
            string txtLines = File.ReadAllText(folderPath + "\\upsiopts.txt");
            string[] split = txtLines.Split("END");
            // gets the selected item's index, and because the order of the names will be the same as the one in the txt, 
            // uses it to select the game's array item
            int index = pogcheck.SelectedIndex;
            string[] gameArray = split[index].Split("\n");
            
            // getting the required details for download and install
            string gameURL = "";
            string gameName = "";
            string gameZip = "";
            string gameID = "";
            string apkName = "";
            string obbName = "";
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
            pogbox.Text = "Downloading ADB...";
            Directory.CreateDirectory(folderPath + "\\" + gameName);

            // declare event handler for the DL, as if we did this syncronised the UI would freeze, and w/o the handler it would just move on
                pogbox.Text = "Downloading game...";

                WebClient BBBB = new WebClient();
                BBBB.DownloadFileCompleted += new AsyncCompletedEventHandler(ayo);
                BBBB.DownloadFileAsync(gameDL, folderPath + "\\" + gameName + "\\" + gameZip);

                async void ayo(object sender, AsyncCompletedEventArgs e)
                {
                    pogbox.Text = "Download complete, unzipping " + gameName + "..."; 
                    await Task.Run(() => ZipFile.ExtractToDirectory(folderPath + "\\" + gameName + "\\" + gameZip, folderPath + "\\" + gameName)); 
                    pogbox.Text = "Unzipping complete, testing ADB..."; 
                    adbCommands(folderPath, gameName, gameID, apkName, obbName);
                }
            }

            void adbCommands(string folderPath, string gameName, string gameID, string apkName, string obbName)
            {
                string adbLocation = AppDomain.CurrentDomain.BaseDirectory + "\\adb.exe";
                Process process = new Process();

                process = System.Diagnostics.Process.Start(adbLocation, "devices");
                process.WaitForExit();
                pogbox.Text = "device found";

                process = System.Diagnostics.Process.Start(adbLocation, "uninstall " + gameID);
                process.WaitForExit();
                pogbox.Text = gameName + " uninstalled! Installing APK...";
                
                process = System.Diagnostics.Process.Start(adbLocation, "install " + folderPath + "\\" + gameName + "\\" + apkName);
                process.WaitForExit();
                pogbox.Text = "APK Installed! Setting permissions...";

                process = System.Diagnostics.Process.Start(adbLocation, "-d shell pm grant " + gameID + " android.permission.RECORD_AUDIO");
                process.WaitForExit();

                process = System.Diagnostics.Process.Start(adbLocation, "-d shell pm grant " + gameID + " android.permission.READ_EXTERNAL_STORAGE");
                process.WaitForExit();

                process = System.Diagnostics.Process.Start(adbLocation, "-d shell pm grant " + gameID + " android.permission.WRITE_EXTERNAL_STORAGE");
                process.WaitForExit();
                pogbox.Text = "Permissions set! Pushing OBB...";

                process = System.Diagnostics.Process.Start(adbLocation, "-d push " + folderPath + "\\" + gameName + "\\" + obbName);
                process.WaitForExit();
                pogbox.Text = "Setting name...";

                // ADD NAMES HERE WHEN YOU CAN BE BOTHERED

                pogbox.Text = gameName + " installed!";
                foreach (var bitch in Process.GetProcessesByName("adb"))
                {
                    bitch.Kill();
                }
            }
        }
    }
