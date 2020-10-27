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
            pogcheck.Items.Insert(0,"Please select a game.");
            pogcheck.SelectedIndex = 0;
            
            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + "\\ModernEra");
            String folderPath = new string(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + "\\ModernEra");
            WebClient dl = new WebClient();
            Uri txtURL = new Uri("https://thesideloader.co.uk/upsiopts.txt");
            if (File.Exists(folderPath + "\\upsiopts.txt"))
            {
                File.Delete(folderPath + "\\upsiopts.txt");
            }
            dl.DownloadFile(txtURL, folderPath + "\\upsiopts.txt");
            
            string[] txtLines = File.ReadAllLines(folderPath + "\\upsiopts.txt");
            string temp;
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

        }

        private void pogcheck_Click(object sender, MouseEventArgs e)
        {
            pogcheck.Items.Remove("Please select a game.");
            pogbox.Text += "Click ";
        }
    }
}
