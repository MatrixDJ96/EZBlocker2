using System.Windows.Forms;

namespace EZBlocker2
{
    class CustomToolStripRenderer : ToolStripProfessionalRenderer
    {
        public CustomToolStripRenderer() : base(new CustomProfessionalColorTable()) { }
    }
}
