using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OsInstaller
{
    public partial class ShowDrive : Form
    {
        Form1 form1;
        Functions.HDD drive = null;
        public ShowDrive(Functions.HDD drive, Form1 form1)
        {
            this.drive = drive;
            this.form1 = form1;
            InitializeComponent();


            label2.Text = drive.DriveIndex;
            label3.Text = drive.Model;
            label5.Text = drive.Serial;
            label7.Text = drive.IsOK.ToString();
            label9.Text = drive.IsOKCustom.ToString();
            label11.Text = drive.Size.ToString()+ " GB";
            label13.Text = drive.SmartSupported.ToString();
            var Bad = drive.IsOKCustomBadMembers;

            foreach (var item in drive.Attributes)
            {
                Color Used = Color.Black;
                if (Bad.Contains(item.Key))
                {
                    Used = Color.Red;
                }
                listView1.Items.Add(
                        new ListViewItem(new string[]{
                        item.Key.ToString(),
                        item.Value.Attribute,
                        item.Value.Current.ToString(),
                        item.Value.Worst.ToString(),
                        item.Value.Threshold.ToString(),
                        item.Value.Data.ToString()
                        }, item.Key, Used, Color.White, null)
                    );
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
            this.form1.Show();
        }

        private void ShowDrive_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("All Data Will Be Erased. Are You Sure?", "Format Drive", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                FormatDrive();
            }
        }



        public void FormatDrive()
        {
            string exec = string.Format(
                "select disk " + this.drive.DriveIndex + "\r\n" +
                "clean");

            Functions.CMD.Diskpart(exec, true);

        }
        public void PartitionDrive()
        {
            string exec = string.Format(
                "select disk " + this.drive.DriveIndex + "\r\n" +
                "clean\r\n" +
                "convert gpt\r\n" +
                "format quick fs=ntfs\r\n" +
                "assign letter=\"y\"\r\n");


            Functions.CMD.Diskpart(exec, true);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("All Data Will Be Erased. Are You Sure?", "Format Drive", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                PartitionDrive();
            }
        }
    }
}
