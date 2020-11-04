using System;
using System.IO.Compression;

class Unzip
{
    async void unzip(string gameName, string gameZip, string folderPath)
    {
        await Task.Run(() => ZipFile.ExtractToDirectory(folderPath + "\\" + gameName + "\\" + gameZip, folderPath + "\\" + gameName));
    }
}


