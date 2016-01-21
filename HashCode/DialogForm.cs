using System;
using System.Drawing;
using System.Windows.Forms;

namespace HashCode
{
    public partial class DialogForm : Form
    {
        public DialogForm()
        {
            InitializeComponent();
        }

        public void Show(string Message)
        {
            this.message.Text = Message;
            this.message.Location = new Point((this.Size.Width - this.message.Size.Width) / 2, this.message.Location.Y);
            this.ShowDialog();
        }

        private void OK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
