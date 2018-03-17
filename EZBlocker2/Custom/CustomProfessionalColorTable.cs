using System.Drawing;
using System.Windows.Forms;

namespace EZBlocker2
{
    class CustomProfessionalColorTable : ProfessionalColorTable
    {
        private Color selectedColor = SystemColors.MenuHighlight;

        public override Color MenuItemSelected => SelectedColor;

        public override Color MenuItemSelectedGradientBegin => SelectedColor;

        public override Color MenuItemSelectedGradientEnd => SelectedColor;

        public Color SelectedColor { get => selectedColor; set => selectedColor = value; }
    }
}
