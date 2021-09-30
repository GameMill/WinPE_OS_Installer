using Functions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OsInstaller.InstallOS
{
    public partial class SelectOsDrive : Form
    {
        private Timer timer = null;
        bool Canceled = false;
        System.Threading.CancellationTokenSource source;

        public static Task<System.IO.DriveInfo> SelectHD(Rectangle bounds, SelectOsDrive form)
        {
            form.FormClosing += form.Form_FormClosing;
            form.source = new System.Threading.CancellationTokenSource();
            form.Show();
            form.Bounds = bounds;

            System.Threading.Tasks.Task<System.IO.DriveInfo> task = new Task<System.IO.DriveInfo>(form.Get, form, form.source.Token);
            task.Start();
            return task;
        }

        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            source.Cancel();
        }

        private System.IO.DriveInfo Get(object state)
        {
            while (Selected == "" && Canceled == false) { System.Threading.Thread.Sleep(1); }
            foreach (var Drive in System.IO.DriveInfo.GetDrives())
            {
                if (Selected == Drive.Name)
                {
                    return Drive;
                }
            }
            return null;
        }

        public SelectOsDrive()
        {
            InitializeComponent();
            foreach (var Drive in System.IO.DriveInfo.GetDrives())
            {
                if (Drive.DriveType == System.IO.DriveType.Fixed)
                {
                    listView1.Items.Add(new ListViewItem(new string[] { Drive.Name, ((int)(((float)Drive.TotalSize) / 1024 / 1024 / 1024)).ToString() + " GB", ((int)(((float)Drive.TotalFreeSpace) / 1024 / 1024 / 1024)).ToString() + " GB", Drive.VolumeLabel }));
                }
            }
        }




        private void HDs_Load(object sender, EventArgs e)
        {

        }
        public string Selected = "";



        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = (listView1.SelectedItems.Count != 0);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            Selected = listView1.SelectedItems[0].Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(timer != null)
            timer.Form1_KeyDown(null, null);
            Selected = null;
            Canceled = true;
        }
    }
}
