namespace EZBlocker2.Properties
{
    [System.Configuration.SettingsProvider(typeof(CustomSettingsProvider))]
    internal sealed partial class Settings
    {
        public Settings()
        {
            // this.SettingChanging += this.SettingChangingEventHandler;
            // this.SettingsSaving += this.SettingsSavingEventHandler;
        }

        private void SettingChangingEventHandler(object sender, System.Configuration.SettingChangingEventArgs e)
        { }

        private void SettingsSavingEventHandler(object sender, System.ComponentModel.CancelEventArgs e)
        { }
    }
}
