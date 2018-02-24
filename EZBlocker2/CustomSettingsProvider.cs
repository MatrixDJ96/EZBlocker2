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

        private string GetValue(SettingsProperty property)
        {
            string line = null;
            if (File.Exists(ezBlockerSettings))
            {
                string propertyName = property.Name.ToLower();
                List<string> lines = new List<string>(File.ReadAllLines(ezBlockerSettings));
                line = lines.Find(x => x.ToLower().Contains(propertyName));
                if (line != null)
                {
                    line = line.Replace(propertyName, "").Replace("=", "").Trim(new[] { ' ', '\t' }).ToLower();
                    if (line != "false" && line != "true")
                        line = null;
                }
            }
            return line ?? property.DefaultValue.ToString();
        }

        private void SetValue(SettingsPropertyValue value)
        {
            string valueName = value.Name.ToLower();
            string newValue = value.SerializedValue.ToString().ToLower();

            string[] currentLines = null;
            if (!File.Exists(ezBlockerSettings))
                currentLines = new string[0];
            else
                currentLines = File.ReadAllLines(ezBlockerSettings);

            for (int i = 0; i < currentLines.Length; i++)
                currentLines[i] = currentLines[i].ToLower();

            List<string> newLines = new List<string>(currentLines);
            bool found = false;

            for (int i = 0; i < currentLines.Length; i++)
            {
                if (currentLines[i].Contains(valueName))
                {
                    if (!currentLines[i].Replace(valueName, "").Replace("=", "").Trim(new[] { ' ', '\t' }).Equals(newValue))
                        newLines[i] = valueName + "=" + newValue;
                    found = true;
                }
            }

            if (found)
            {
                for (int i = 0; i < newLines.Count; i++)
                {
                    if (!newLines[i].Equals(currentLines[i]))
                    {
                        File.WriteAllLines(ezBlockerSettings, newLines);
                        break;
                    }
                }
            }
            else
            {
                newLines.Add(valueName + "=" + newValue);
                File.WriteAllLines(ezBlockerSettings, newLines);
            }
        }

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

            foreach (SettingsProperty property in collection)
            {
                SettingsPropertyValue value = new SettingsPropertyValue(property)
                {
                    SerializedValue = GetValue(property)
                };
                settingsPropertyValueCollection.Add(value);
            }

            return settingsPropertyValueCollection;
        }

        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
        {
            SettingsPropertyValue workingValue = null;
            try
            {
                foreach (SettingsPropertyValue value in collection)
                {
                    workingValue = value;
                    SetValue(workingValue);
                }
            }
            catch
            {
                MessageBox.Show("Error while saving settings...", "EZBlocker 2", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new Exception();
            }
        }
    }
}
