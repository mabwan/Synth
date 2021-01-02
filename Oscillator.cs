using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
namespace Synth
{
    public class Oscillator : GroupBox
    {
        public Oscillator()
        {
            this.Controls.Add(new Button()
            {
                Name = "Sine",
                Location = new Point(10, 15),
                Text = "Sine",
            }); 
            this.Controls.Add(new Button()
            {
                Name = "Saw",
                Location = new Point(80, 15),
                Text = "Saw",
            });
            this.Controls.Add(new Button()
            {
                Name = "Square",
                Location = new Point(150, 15),
                Text = "Square",
            });
            this.Controls.Add(new Button()
            {
                Name = "Triangle",
                Location = new Point(10, 55),
                Text = "Triangle",
            });
            this.Controls.Add(new Button()
            {
                Name = "Noise",
                Location = new Point(80, 55),
                Text = "Noise",
            });
            foreach(Control control in this.Controls)
            {
                control.Size = new Size(70, 30);
                control.Font = new Font("Microsoft Sans Serif", 6.75f);
                control.Click += WaveButton_CLick;
            }
        }
        public WaveForm WaveForm { get; private set; }
        private void WaveButton_CLick(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            this.WaveForm = (WaveForm)Enum.Parse(typeof(WaveForm), button.Text);
            foreach(Button otherButtons in this.Controls.OfType<Button>())
            {
                otherButtons.UseVisualStyleBackColor = true;
            }
            button.BackColor = Color.Yellow;
        }
    }
}
