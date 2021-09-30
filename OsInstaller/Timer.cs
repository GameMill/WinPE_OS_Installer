using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OsInstaller
{
    class Timer
    {
        public System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        public int HowLong = -1;
        Action<bool> CallBack;
        Form form;
        Action Tick;
        bool Running = false;

        public Timer(int HowLong,int Interval, Action<bool> CallBack,Form form, Action Tick=null)
        {
            this.Tick = Tick;
            this.form = form;
            this.form.KeyPreview = true;
            this.form.KeyDown += new KeyEventHandler(Form1_KeyDown);
            this.form.MouseDown += new MouseEventHandler(Form1_KeyDown);
            this.CallBack = CallBack;
            this.HowLong = HowLong;
            Running = true;
            timer.Interval = Interval;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        public void Form1_KeyDown(object sender, object e)
        {
            if (Running)
            {
                this.form.KeyDown -= Form1_KeyDown;
                this.form.MouseDown -= Form1_KeyDown;
                this.form.KeyPreview = false;
                timer.Stop();
                timer.Dispose();
                Running = false;

                CallBack(false);
            }

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            HowLong -= timer.Interval;
            if (HowLong <= 0)
            {
                this.form.KeyDown -= Form1_KeyDown;
                this.form.MouseDown -= Form1_KeyDown;

                this.form.KeyPreview = false;
                Running = false;
                timer.Stop();
                timer.Dispose();
                CallBack(true);
            }
            else
                if(Tick != null)
                    Tick();
        }
    }
}
