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
            }

            // set up download environment
            WebClient AAAA = new WebClient();
            Uri gameDL = new Uri(gameURL);
            if (Directory.Exists(folderPath + "\\" + gameName))
            {
                Directory.Delete(folderPath + "\\" + gameName, true);
            }
            pogbox.Text = "Downloading game...";
            Directory.CreateDirectory(folderPath + "\\" + gameName);

            // declare event handler for the DL, as if we did this syncronised the UI would freeze, and w/o the handler it would just move on
            AAAA.DownloadFileCompleted += new AsyncCompletedEventHandler(done);
            AAAA.DownloadFileAsync(gameDL, folderPath + "\\" + gameName + "\\" + gameZip);
            
            // now the rest of our code goes in here, as it will start the code after the async download is completed 
            async void done(object sender, AsyncCompletedEventArgs e)
            {
                pogbox.Text = "Download complete, unzipping " + gameName + "...";
                await Task.Run(() => ZipFile.ExtractToDirectory(folderPath + "\\" + gameName + "\\" + gameZip, folderPath + "\\" + gameName));
                pogbox.Text = "Unzipping complete, testing ADB...";
                adbCommands();
                
            }

            void adbCommands()
            {
                string adbLocation = AppDomain.CurrentDomain.BaseDirectory + "\\adb.exe";
                Process process = new Process();
                // hides the console window so people dont freak out
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.StartInfo.CreateNoWindow = true;

                process = System.Diagnostics.Process.Start(adbLocation, "devices");
                process.WaitForExit();
                pogbox.Text = "devices found";
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.StartInfo.CreateNoWindow = true;
                process = System.Diagnostics.Process.Start(adbLocation, "devices");
                process.WaitForExit();
                pogbox.Text = "second thing done";
                foreach (var bitch in Process.GetProcessesByName("adb"))
                {
                    bitch.Kill();
                }
                pogbox.Text = "adb is kil";
            }
        }
    }
}
