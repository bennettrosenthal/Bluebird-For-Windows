using System;
using System.Diagnostics;
using System.Windows.Controls;

public class adbCommand
{
    public void adbCommands(string folderPath, string gameName, string gameID, string apkName, string obbName, string txtFileName, TextBlock pogbox)
    {
        string adbLocation = AppDomain.CurrentDomain.BaseDirectory + "\\adb.exe";
        Process process = new Process();
        process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.FileName = adbLocation;
        process.StartInfo.Arguments = "devices";
        process.Start();
        process.WaitForExit();

        Process processayo = new Process();
        processayo.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        processayo.StartInfo.CreateNoWindow = true;
        processayo.StartInfo.FileName = adbLocation;
        processayo.StartInfo.Arguments = "uninstall " + gameID;
        processayo.Start();
        processayo.WaitForExit();

        Process processda = new Process();
        processda.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        processda.StartInfo.CreateNoWindow = true;
        processda.StartInfo.FileName = adbLocation;
        processda.StartInfo.Arguments = "install \"" + folderPath + "\\" + gameName + "\\" + apkName + "\"";
        processda.Start();
        processda.WaitForExit();

        Process processpizza = new Process();
        processpizza.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        processpizza.StartInfo.CreateNoWindow = true;
        processpizza.StartInfo.FileName = adbLocation;
        processpizza.StartInfo.Arguments = "-d shell pm grant " + gameID + " android.permission.RECORD_AUDIO";
        processpizza.Start();
        processpizza.WaitForExit();

        Process processhere = new Process();
        processhere.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        processhere.StartInfo.CreateNoWindow = true;
        processhere.StartInfo.FileName = adbLocation;
        processhere.StartInfo.Arguments = "-d shell pm grant " + gameID + " android.permission.READ_EXTERNAL_STORAGE";
        processhere.Start();
        processhere.WaitForExit();

        Process processpog = new Process();
        processpog.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        processpog.StartInfo.CreateNoWindow = true;
        processpog.StartInfo.FileName = adbLocation;
        processpog.StartInfo.Arguments = "-d shell pm grant " + gameID + " android.permission.WRITE_EXTERNAL_STORAGE";
        processpog.Start();
        processpog.WaitForExit();

        Process processmonkaS = new Process();
        processmonkaS.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        processmonkaS.StartInfo.CreateNoWindow = true;
        processmonkaS.StartInfo.FileName = adbLocation;
        processmonkaS.StartInfo.Arguments = "-d push \"" + folderPath + "\\" + gameName + "\\" + obbName + "\" /sdcard/Android/obb/" + gameID + "/" + obbName;
        processmonkaS.Start();
        processmonkaS.WaitForExit();

        Process processfinal = new Process();
        processfinal.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        processfinal.StartInfo.CreateNoWindow = true;
        processfinal.StartInfo.FileName = adbLocation;
        processfinal.StartInfo.Arguments = "-d push \"" + folderPath + "\\" + "name.txt\"" + " /sdcard/" + txtFileName;
        processfinal.Start();
        processfinal.WaitForExit();
        foreach (var bitch in Process.GetProcessesByName("adb"))
        {
            bitch.Kill();
        }
    }

    public void uninstall(string gameID)
    {
        string adbLocation = AppDomain.CurrentDomain.BaseDirectory + "\\adb.exe";
        Process processayo = new Process();
        processayo.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        processayo.StartInfo.CreateNoWindow = true;
        processayo.StartInfo.FileName = adbLocation;
        processayo.StartInfo.Arguments = "uninstall " + gameID;
        processayo.Start();
        processayo.WaitForExit();
    }
}
