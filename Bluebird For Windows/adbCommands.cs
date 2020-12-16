using System;
using System.Diagnostics;
using System.Windows.Controls;

public class adbCommands
{   
    public string adbLocation = AppDomain.CurrentDomain.BaseDirectory + "\\adb.exe";

    public void uninstall(string gameID)
    {
        startADB();
        Process processayo = new Process();
        processayo.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        processayo.StartInfo.CreateNoWindow = true;
        processayo.StartInfo.FileName = adbLocation;
        processayo.StartInfo.Arguments = "uninstall " + gameID;
        processayo.Start();
        processayo.WaitForExit();
    }

    public void installAPK(string folderPath, string gameName, string apkName)
    {
        startADB();
        Process processda = new Process();
        processda.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        processda.StartInfo.CreateNoWindow = true;
        processda.StartInfo.FileName = adbLocation;
        processda.StartInfo.Arguments = "install \"" + folderPath + "\\" + gameName + "\\" + apkName + "\"";
        processda.Start();
        processda.WaitForExit();
    }

    public void grantPermissions(string gameID)
    {
        startADB();
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
    }

    public void pushOBB(string folderPath, string gameName, string obbName, string gameID)
    {
        startADB();
        Process processmonkaS = new Process();
        processmonkaS.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        processmonkaS.StartInfo.CreateNoWindow = true;
        processmonkaS.StartInfo.FileName = adbLocation;
        processmonkaS.StartInfo.Arguments = "-d push \"" + folderPath + "\\" + gameName + "\\" + obbName + "\" /sdcard/Android/obb/" + gameID + "/" + obbName;
        processmonkaS.Start();
        processmonkaS.WaitForExit();
    }

    public void pushName(string folderPath, string txtFileName)
    {
        startADB();
        Process processfinal = new Process();
        processfinal.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        processfinal.StartInfo.CreateNoWindow = true;
        processfinal.StartInfo.FileName = adbLocation;
        processfinal.StartInfo.Arguments = "push \"" + folderPath + "\\" + "name.txt\"" + " /sdcard/" + txtFileName;
        processfinal.Start();
        processfinal.WaitForExit();
    }

    public void killADB()
    {
        foreach (var process in Process.GetProcessesByName("adb"))
        {
            process.Kill();
        }
    }

    public void startADB()
    {
        Process process = new Process();
        process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.FileName = adbLocation;
        process.StartInfo.Arguments = "devices";
        process.Start();
        process.WaitForExit();
    }

    public void pushMap(string mapName, string mapDir)
    {
        startADB();
        Process process = new Process();
        process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.FileName = adbLocation;
        process.StartInfo.Arguments = "push " + mapDir + " /sdcard/pavlov/maps/";
        process.Start();
        process.WaitForExit();
    }

    public void installLoneAPK(string pathToAPK)
    {
        startADB();
        Process process = new Process();
        process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.FileName = adbLocation;
        process.StartInfo.Arguments = "install " + pathToAPK;
        process.Start();
        process.WaitForExit();
    }
}
