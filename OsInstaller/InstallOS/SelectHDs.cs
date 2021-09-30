using Functions;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OsInstaller.InstallOS
{
    public partial class SelectHDs : Form
    {
        private Timer timer;
        bool Canceled = false;
        System.Threading.CancellationTokenSource source;

        public static Task<Functions.HDD> SelectHD(Rectangle bounds, SelectHDs form)
        {
            form.FormClosing += form.Form_FormClosing;
            form.source = new System.Threading.CancellationTokenSource();
            form.Show();
            form.Bounds = bounds;

            System.Threading.Tasks.Task<Functions.HDD> task = new Task<Functions.HDD>(form.Get, form, form.source.Token);
            task.Start();
            return task;
        }

        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            source.Cancel();
        }

        private HDD Get(object state)
        {
            while (Selected == null && Canceled == false) { System.Threading.Thread.Sleep(1); }
            return Selected;
        }

        public SelectHDs()
        {
            InitializeComponent();
            Start:
            listView1.Scrollable = true;
            listView1.View = View.Details;
            listView1.Font = new Font(FontFamily.GenericSansSerif, 10f);

            this.FormBorderStyle = FormBorderStyle.None;

            bool HasSmartFailed = false;

            foreach (var item in Functions.HDD.GetHDDs())
            {
                if (item.Type == "IDE" || item.Type == "SCSI")
                {
                    Color Used = Color.Black;
                    if (item.IsOKCustom == false)
                    {
                        HasSmartFailed = true;
                        Used = Color.Red;
                    }

                    listView1.Items.Add(
                        new ListViewItem(
                            new string[6] { item.DriveIndex.ToString(), item.Model, item.Serial.ToString(), item.IsOKCustom.ToString(), (item.Size).ToString() + " GB", (item.Size < 2000).ToString() },
                            null,
                            Used,
                            Color.White,null
                            ));
                }
            }
            if (listView1.Items.Count > 0)
            {
                listView1.Items[0].Selected = listView1.Items.Count == 1;
                button1.Enabled = (listView1.SelectedItems.Count != 0);
                if (listView1.Items.Count == 1)
                {
                    timer = new Timer(5000, 1000, TimerOnComplate, this, Tick);
                    listView1.MouseDown += new MouseEventHandler(timer.Form1_KeyDown);
                    
                        
                }
                if (HasSmartFailed)
                    timer.Form1_KeyDown(null, null);
            }
            else
            {
                button1.Enabled = (listView1.SelectedItems.Count != 0);

                if (MessageBox.Show("Error: No Harddrives Found", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Cancel)
                {

                }
                else
                {
                    goto Start;
                }
            }
        }



        private void Tick()
        {
            label2.Text = (timer.HowLong / timer.timer.Interval).ToString();
        }


        private void TimerOnComplate(bool Complate)
        {
            listView1.MouseDown -= timer.Form1_KeyDown;
            if (Complate)
            {
                button1_Click(null, null);
            }
            else
            {
                label2.Text = "Canceled";
            }

        }


        private void HDs_Load(object sender, EventArgs e)
        {

        }
        public HDD Selected = null;

        private void button1_Click(object sender, EventArgs e)
        {
            if(timer != null)
                timer.Form1_KeyDown(null, null);
            if (listView1.SelectedItems.Count == 0)
                return;
            var Index = listView1.SelectedItems[0].Text;
            foreach (var item in Functions.HDD.GetHDDs())
            {
                if (item.Type == "IDE" || item.Type == "SCSI") 
                {

                    if (item.DriveIndex.ToString() == Index)
                    {
                        Selected = item;
                        break;
                    }
                }
                else
                {

                }
            }

            if(Selected == null)
            {

            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = (listView1.SelectedItems.Count != 0);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (timer != null)
                timer.Form1_KeyDown(null, null);
            Selected = null;
            Canceled = true;
        }
    }
}
