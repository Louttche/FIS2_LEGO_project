using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lego.Ev3.Core;
using Lego.Ev3.Desktop;

namespace WindowsFormsApplication2
{
    public partial class mainForm : Form
    {
        string idle = "IDLE";
        string pickup = "Im PickingUP";
        Brick _brick;
        int _forward = 50;
        int _backward = -50;
        uint _time = 300;
        //bool hello = false;

        public mainForm()
        {
            InitializeComponent();
        }

        private async void mainForm_Load(object sender, EventArgs e)
        {
            _brick = new Brick(new BluetoothCommunication("COM4"));
            _brick.BrickChanged += _brick_BrickChanged;
            await _brick.ConnectAsync();
            await _brick.DirectCommand.PlayToneAsync(100, 1000, 300);
            await _brick.DirectCommand.SetMotorPolarity(OutputPort.B | OutputPort.C, Polarity.Backward);
            await _brick.DirectCommand.DrawTextAsync(Lego.Ev3.Core.Color.Background, 12, 32, "IM IDlE");
            await _brick.DirectCommand.StopMotorAsync(OutputPort.All, false);
            await _brick.DirectCommand.ClearChanges(InputPort.Three);
            _brick.Ports[InputPort.Three].SetMode(TouchMode.Bumps);
        }
        
        public void _brick_BrickChanged(object sender, BrickChangedEventArgs e)
        {
            lb_Coloursensor.Text = e.Ports[InputPort.One].SIValue.ToString();
            lb_WallEsensor.Text = e.Ports[InputPort.Three].SIValue.ToString();
            lb_status.Text = idle;
        }

        private async void btnForward_Click(object sender, EventArgs e)
        {
                await _brick.DirectCommand.TurnMotorAtPowerForTimeAsync(OutputPort.B | OutputPort.C, _forward, _time, false);
                lb_status.Text = "on the RUN";
        }

        private async void btnBack_Click(object sender, EventArgs e)
        {
                await _brick.DirectCommand.TurnMotorAtPowerForTimeAsync(OutputPort.B | OutputPort.C, _backward, _time, false);
                lb_status.Text = "on the RUN";
        }

        private async void btnLeft_Click(object sender, EventArgs e)
        {
            _brick.BatchCommand.TurnMotorAtPowerForTime(OutputPort.C, _backward, _time, false);
            _brick.BatchCommand.TurnMotorAtPowerForTime(OutputPort.B, _forward, _time, false);
             await _brick.BatchCommand.SendCommandAsync();
            lb_status.Text = "on the RUN";
        }

        private async void btnRight_Click(object sender, EventArgs e)
        {
            _brick.BatchCommand.TurnMotorAtPowerForTime(OutputPort.B, _backward, _time, false);
            _brick.BatchCommand.TurnMotorAtPowerForTime(OutputPort.C, _forward, _time, false);
            await _brick.BatchCommand.SendCommandAsync();
            lb_status.Text = "on the RUN";
        }

        private async void btnUp_Click(object sender, EventArgs e)
        {
                await _brick.DirectCommand.TurnMotorAtPowerForTimeAsync(OutputPort.A, _forward, _time, false);
                await _brick.DirectCommand.DrawTextAsync(Lego.Ev3.Core.Color.Background, 50, 50, "im in Pick UP MODE!!");
                lb_status.Text = pickup;
        }

        private async void btnDown_Click(object sender, EventArgs e)
        {
                await _brick.DirectCommand.TurnMotorAtPowerForTimeAsync(OutputPort.A, _backward, _time, false);
                await _brick.DirectCommand.DrawTextAsync(Lego.Ev3.Core.Color.Background, 50, 50, "I'm in Pick UP MODE!!");
                lb_status.Text = pickup;
        }
    }
}
