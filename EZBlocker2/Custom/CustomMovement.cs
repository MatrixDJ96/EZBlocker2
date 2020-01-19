using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace EZBlocker2
{
    class CustomMovement
    {
        private Form form;

        private bool dragging = false;
        private Size cursorPosition;
        private Point formPosition;

        public event Action NewPosition;

        public CustomMovement(Form form) => this.form = form;

        private List<Type> exclude = new List<Type>();

        public void Exclude(Type control)
        {
            exclude.Add(control);
        }

        public void SetMovement(Control.ControlCollection collection)
        {
            foreach (Control control in collection)
            {
                if (control.HasChildren)
                    SetMovement(control.Controls);

                if (!exclude.Exists(x => x.Name == control.GetType().Name))
                {
                    control.MouseDown += FormMovement_MouseDown;
                    control.MouseLeave += FormMovement_MouseLeave;
                    control.MouseMove += FormMovement_MouseMove;
                    control.MouseUp += FormMovement_MouseUp;
                }
            }
        }

        private void FormMovement_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                dragging = true;
                cursorPosition = new Size(Cursor.Position);
                formPosition = form.Location;
            }
        }

        private void FormMovement_MouseLeave(object sender, EventArgs e) => dragging = false;

        private void FormMovement_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point diff = Point.Subtract(Cursor.Position, cursorPosition);
                form.Location = Point.Add(formPosition, new Size(diff));
            }
        }

        private void FormMovement_MouseUp(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                dragging = false;
                NewPosition();
            }
        }
    }
}
