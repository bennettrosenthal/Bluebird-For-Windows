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
            // sets the dropdown to display "Please select a game"
            pogcheck.Items.Insert(0,"Please select a game.");
            pogcheck.SelectedIndex = 0;
            
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
            Object gameChosen = pogcheck.SelectedItem;
            string game = gameChosen.ToString();
            String folderPath = new string(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + "\\ModernEra");
            string txtLines = File.ReadAllText(folderPath + "\\upsiopts.txt");
            string[] split = txtLines.Split("END");

            
            foreach (var line in split)
            {
                pogbox.Text += line;
            }
            

        }
    }
}
