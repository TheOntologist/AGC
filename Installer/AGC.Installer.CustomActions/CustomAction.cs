using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Deployment.WindowsInstaller;
using System.IO;

namespace AGC.Installer.CustomActions
{
    public class CustomActions
    {
        private static string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        [CustomAction]
        public static ActionResult CopySoundFiles(Session session)
        {
            session.Log("Begin CopySoundFiles");

            try
            {
                // Check that files were installed
                if (Directory.Exists(session["INSTALLLOCATION"] + @"\Sounds"))
                {
                    // Get source folder path
                    DirectoryInfo source = new DirectoryInfo(session["INSTALLLOCATION"] + @"\Sounds");

                    // Set destination folder to AGC folder in AppData
                    string destination = appdata + @"\AGC\Sounds";

                    // Create AGC folder in AppData if it doesn't exist yet
                    Directory.CreateDirectory(destination);

                    // Copy sound files to AGC folder in AppData
                    foreach (FileInfo file in source.GetFiles())
                    {
                        file.CopyTo(destination + @"\" + file.Name, true);
                    }
                }
                session.Log("End CopySoundFiles");
                return ActionResult.Success;
            }
            catch (Exception ex)
            {
                session.Log("CopySoundFiles custom action failed with exception. Exception message: " + ex.Message);
                return ActionResult.Failure;
            }
        }
    }
}
