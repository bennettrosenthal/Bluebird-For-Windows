using System;
using System.Diagnostics;
using System.Windows.Controls;

public class adbCommand
{
    public void adbCommands(string folderPath, string gameName, string gameID, string apkName, string obbName, string txtFileName, TextBlock pogbox)
    {
        string adbLocation = AppDomain.CurrentDomain.BaseDirectory + "\\adb.exe";
        Process process = new Process();

        process = System.Diagnostics.Process.Start(adbLocation, "devices");
        process.WaitForExit();
        pogbox.Text = "device found";

        process = System.Diagnostics.Process.Start(adbLocation, "uninstall " + gameID);
        process.WaitForExit();
        pogbox.Text = gameName + " uninstalled! Installing APK...";

        process = System.Diagnostics.Process.Start(adbLocation, "install \"" + folderPath + "\\" + gameName + "\\" + apkName + "\"");
        process.WaitForExit();
        pogbox.Text = "APK Installed! Setting permissions...";

        process = System.Diagnostics.Process.Start(adbLocation, "-d shell pm grant " + gameID + " android.permission.RECORD_AUDIO");
        process.WaitForExit();

        process = System.Diagnostics.Process.Start(adbLocation, "-d shell pm grant " + gameID + " android.permission.READ_EXTERNAL_STORAGE");
        process.WaitForExit();

        process = System.Diagnostics.Process.Start(adbLocation, "-d shell pm grant " + gameID + " android.permission.WRITE_EXTERNAL_STORAGE");
        process.WaitForExit();
        pogbox.Text = "Permissions set! Pushing OBB...";

        process = System.Diagnostics.Process.Start(adbLocation, "-d push \"" + folderPath + "\\" + gameName + "\\" + obbName + "\" /sdcard/Android/obb/" + gameID);
        process.WaitForExit();
        pogbox.Text = "Setting name...";

        process = System.Diagnostics.Process.Start(adbLocation, "-d push \"" + folderPath + "\\" + "name.txt\"" + " /sdcard/" + txtFileName);
        pogbox.Text = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        pogbox.Text = gameName + " installed!";
        foreach (var bitch in Process.GetProcessesByName("adb"))
        {
            bitch.Kill();
        }
    }
}
