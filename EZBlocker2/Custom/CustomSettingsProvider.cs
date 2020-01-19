using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Windows.Forms;
using static EZBlocker2.Program;

namespace EZBlocker2
{
    public class CustomSettingsProvider : SettingsProvider
    {
        private readonly string ezBlockerSettings = ezBlockerFullExe.Substring(0, ezBlockerFullExe.Length - 4) + ".ini";

        public override void Initialize(string name, NameValueCollection config) => base.Initialize(ApplicationName, config);

        public override string ApplicationName
        {
            get => Application.ProductName;
            set { }
        }

        public override string Name => "CustomSettingsProvider";

        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection collection)
        {
            SettingsPropertyValueCollection settingsPropertyValueCollection = new SettingsPropertyValueCollection();

            try
            {
                List<string> lines = null;
                if (File.Exists(ezBlockerSettings))
                    lines = new List<string>(File.ReadAllLines(ezBlockerSettings));

                foreach (SettingsProperty property in collection)
                {
                    string propertyName = property.Name.ToLower();
                    object value = null;

                    if (lines != null)
                    {
                        string line = lines.Find(x => x.ToLower().Contains(propertyName));
                        if (line != null)
                            value = line.Replace(propertyName, "").Replace("=", "").Trim(new[] { ' ', '\t' }).ToLower();
                    }

                    SettingsPropertyValue propertyValue = new SettingsPropertyValue(property)
                    {
                        SerializedValue = value ?? property.DefaultValue ?? ""
                    };

                    settingsPropertyValueCollection.Add(propertyValue);
                }

            }
            catch
            {
                MessageBox.Show("Error while reading settings...", ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new Exception();
            }

            return settingsPropertyValueCollection;
        }

        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
        {
            try
            {
                List<string> currentLines = null;
                if (File.Exists(ezBlockerSettings))
                    currentLines = new List<string>(File.ReadAllLines(ezBlockerSettings));

                List<string> newLines = new List<string>();

                bool write = false;

                foreach (SettingsPropertyValue value in collection)
                {
                    string valueName = value.Name.ToLower();
                    string newValue = value.SerializedValue.ToString().ToLower();

                    string line = null;
                    if (currentLines != null)
                    {
                        line = currentLines.Find(x => x.ToLower().Contains(valueName));
                        if (line != null)
                            line = line.Replace(valueName, "").Replace("=", "").Trim(new[] { ' ', '\t' }).ToLower();
                    }

                    if (line == null || line != newValue)
                        write = true;

                    newLines.Add(valueName + "=" + newValue);
                }

                if (write)
                {
                    newLines.Sort();
                    File.WriteAllLines(ezBlockerSettings, newLines);
                }
            }
            catch
            {
                MessageBox.Show("Error while saving settings...", ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new Exception();
            }
        }
    }
}
