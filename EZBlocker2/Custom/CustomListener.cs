namespace EZBlocker2
{
    internal class CustomListener
    {
        private MainForm mainForm;

        public CustomListener(MainForm mainForm) => this.mainForm = mainForm;

        public void StatusHandler(SpotilocalStatus status)
        {
            mainForm.Main_Status(status);
        }
    }
}
