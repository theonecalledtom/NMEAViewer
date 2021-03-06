﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace NMEAViewer
{
    [JsonObject(MemberSerialization.OptOut)]
    public class ApplicationSettings
    {
        public string ProjectName;
        public string PolarDataName;
        public System.Drawing.Size MainWindowSize;
        public System.Drawing.Point MainWindowLocation;
        public string MainWindowState;
        public string LastPortConnected;
        public string IPText;
        public string IPPortText;
        public bool ComportUsed;

        static string AppSettingsLocation = "nmeaviewerappsettings.json";
        public static ApplicationSettings Load()
        {
            AppSettingsLocation = Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "\\NMEAViewer\\" + "nmeaviewerappsettings.json";
            string DirectoryName = System.IO.Path.GetDirectoryName(AppSettingsLocation);
            if (!System.IO.Directory.Exists(DirectoryName))
            {
                System.IO.Directory.CreateDirectory(DirectoryName);
            }
            
            if (System.IO.File.Exists(AppSettingsLocation))
            {
                string jsonData = System.IO.File.ReadAllText(AppSettingsLocation);
                if (jsonData != null)
                {
                    return JsonConvert.DeserializeObject<ApplicationSettings>(jsonData, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    }
                    );
                }
            }
            return null;
        }

        public void Save()
        {
            if (AppSettingsLocation == null)
            {
                return;
            }

            string output = JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            }
                            );
            System.IO.StreamWriter sOut = System.IO.File.CreateText(AppSettingsLocation);
            if (sOut != null)
            {
                sOut.Write(output);
            }
            sOut.Close();
        }
    }
}
