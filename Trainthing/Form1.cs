using System.Drawing.Design;

namespace Trainthing
{
    public partial class Form1 : Form
    {
        private int Direction = 0;
        private decimal CurrentAcceleration = 0;
        private decimal Braking = 0;
        private decimal CurrentSpeed = 0;
        private decimal ResultantAcceleration = 0;
        private decimal Notch = 0;
        private const decimal rollingCoefficient = 0.0004m;
        private double Incline = 0;
        private decimal g = 9.8107m;
        private decimal RollingResistance = 0;
        private decimal TransitionSpeed = 40;
        private decimal MaxSpeed = 120;
        public Form1()
        {
            InitializeComponent();
            label1.Text = trackBar1.Value == -9 ? "EB" : (trackBar1.Value < 0 ? "B" : (trackBar1.Value > 0 ? "P" : "N")) + (trackBar1.Value == 0 ? "" : Math.Abs(trackBar1.Value));
            label4.Text = 0 + " km/h";
            numericUpDown1.Value = 3.6m;
            numericUpDown2.Value = 40;
            numericUpDown3.Value = 4.0m;
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            label1.Text = trackBar1.Value == -9 ? "EB" : (trackBar1.Value < 0 ? "B" : (trackBar1.Value > 0 ? "P" : "N")) + (trackBar1.Value == 0 ? "" : Math.Abs(trackBar1.Value));
            Notch = trackBar1.Value == 0 ? 0 : trackBar1.Value < 0 ? trackBar1.Value / 9m : trackBar1.Value / 5m;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            CurrentAcceleration = numericUpDown1.Value;
            TransitionSpeed = numericUpDown2.Value;
            Braking = numericUpDown3.Value;
            RollingResistance = rollingCoefficient * g * (decimal)Math.Cos((Math.PI / 180) * Incline);
            ResultantAcceleration = (Notch < 0 ? Notch * Math.Sign(CurrentSpeed) * Braking : (Notch * Direction * GetAccelerationFromCurve(CurrentSpeed))) - (RollingResistance * Math.Sign(CurrentSpeed)) + (g * (decimal)Math.Sin((Math.PI / 180) * Incline));
            CurrentSpeed += ResultantAcceleration / 100;
            label4.Text = Math.Abs(Math.Round((double)(CurrentSpeed * 10)) / 10) + " km/h";
        }

        private decimal GetAccelerationFromCurve(decimal speed)
        { 
            double abspeed = Math.Abs((double)speed);
            if (abspeed < (double)TransitionSpeed)
                return CurrentAcceleration;
            else
            {
                return (decimal)double.Lerp((double)CurrentAcceleration, 0, (double)((CurrentSpeed - TransitionSpeed) / (MaxSpeed * 1.1m - TransitionSpeed)));
            }

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked) Direction = 1;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked) Direction = 0;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked) Direction = -1;
        }
    }
}
