using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OsInstaller
{
    public partial class Form1 : Form
    {
        delegate void delUpdateText(int i);
        private System.Windows.Forms.Label label3;

        Timer timer;
        public Form1()
        {
            InitializeComponent();
            TopMost = true;
#if DEBUG
            // 
            // label6
            // 
            this.label3 = new System.Windows.Forms.Label();

            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 50F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label6";
            this.label3.Size = new System.Drawing.Size(234, 76);
            this.label3.TabIndex = 10;
            this.label3.Text = "Debug";
            this.label3.Visible = true;
            this.Controls.Add(this.label3);

            //this.button1.Enabled = false;
            this.button2.Enabled = false;
            this.Update();

#endif

            label4.Text = System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();

            
            this.FormBorderStyle = FormBorderStyle.None;
#if DEBUG

#else
            timer = new Timer(5000, 1000, TimerOnComplate,this,Tick);
#endif
            this.FormClosing += Form1_FormClosing;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
#if DEBUG

#else
            timer.Form1_KeyDown(null, null);
            timer = null;
#endif

        }

        private void Tick()
        {
            label2.Text = (timer.HowLong / timer.timer.Interval).ToString();
        }

        private void TimerOnComplate(bool Complate)
        {
            if (Complate)
            {
                InstallOS_ClickAsync(null,null);
            }
            else
            {
                label2.Text = "Canceled";
            }
            
        }




        private async void InstallOS_ClickAsync(object sender, EventArgs e)
        {
            if (!button1.Enabled)
                return;
#if DEBUG

#else
            timer.Form1_KeyDown(null, null);
#endif
            Hide();
            Form form = new InstallOS.SelectHDs();
            var hDD = await InstallOS.SelectHDs.SelectHD(this.Bounds, (InstallOS.SelectHDs)form);
            form.Close(); form = null;
            if (hDD == null)
            {
                Show();
                return;
            }
            form = new InstallOS.SelectOS();
            var Data = await InstallOS.SelectOS.SelectOSTask(Bounds, (InstallOS.SelectOS)form);
            form.Close(); form = null;

            if (Data.Brand == "")
            {
                hDD = null;
                Show();
                return;
            }
#if DEBUG

#else

            form = new InstallOS.Installer() { hDD = hDD , Data = Data };

            if (await InstallOS.Installer.InstallOSTask(Bounds, (InstallOS.Installer)form))
            {
                Shutdown();
            }
            else
                Show();
#endif
            //Application.Exit();
        }

        public static void Shutdown()
        {
            ManagementBaseObject mboShutdown = null;
            ManagementClass mcWin32 = new ManagementClass("Win32_OperatingSystem");
            mcWin32.Get();

            // You can't shutdown without security privileges
            mcWin32.Scope.Options.EnablePrivileges = true;
            ManagementBaseObject mboShutdownParams =
                     mcWin32.GetMethodParameters("Win32Shutdown");

            // Flag 1 means we want to shut down the system. Use "2" to reboot.
            mboShutdownParams["Flags"] = "2";
            mboShutdownParams["Reserved"] = "0";
            foreach (ManagementObject manObj in mcWin32.GetInstances())
            {
                mboShutdown = manObj.InvokeMethod("Win32Shutdown", mboShutdownParams, null);
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (!button2.Enabled)
                return;
#if DEBUG

#else
            timer.Form1_KeyDown(null, null);
#endif

            Hide();
            Form form = new InstallOS.SelectHDs();
            var Drive = await InstallOS.SelectHDs.SelectHD(this.Bounds, (InstallOS.SelectHDs)form);
            form.Close(); form = null;
            if (Drive == null)
            {
                Show();
                return;
            }

            form = new ShowDrive(Drive,this);
            form.Show();
            form.Bounds = this.Bounds;

        }

        private async void CreateRecoveryDrive_Click(object sender, EventArgs e)
        {
            if (!button3.Enabled)
                return;
#if DEBUG

#else
            timer.Form1_KeyDown(null, null);
#endif
            Hide();
            Form form = new InstallOS.SelectOsDrive();
            var DriveInfo = await InstallOS.SelectOsDrive.SelectHD(this.Bounds, (InstallOS.SelectOsDrive)form);
            form.Close(); form = null;
            if (DriveInfo == null)
            {
                Show();
                return;
            }
            form = new InstallOS.CreateImage();
            bool done = await InstallOS.CreateImage.CreateImageFile(this.Bounds, (InstallOS.CreateImage)form,DriveInfo);
            form.Show();


        }

        private async Task GetInstalledData_ClickAsync(object sender, EventArgs e)
        {
            if (!button5.Enabled)
                return;
#if DEBUG

#else
            timer.Form1_KeyDown(null, null);
#endif
            Hide();
            System.IO.DriveInfo drive = null;
            Hide();
            foreach (var item in System.IO.DriveInfo.GetDrives())
            {
                if (System.IO.File.Exists(item.Name+ @"Windows\Setup\Scripts\ID.txt"))
                {
                    drive = item;
                    break;
                }
            }

            if (drive != null)
            {
                Form form = new Core.Get_Drive_Info(SystemInfo.FromFile(drive.Name + @"Windows\Setup\Scripts\ID.txt"));
                var DriveInfo = await Core.Get_Drive_Info.ShowInfo(this.Bounds, (Core.Get_Drive_Info)form);
                form.Close(); form = null;
#pragma warning disable CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
                if (DriveInfo == null)
#pragma warning restore CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
                {
                    Show();
                    return;
                }
            }
            else
            {
                MessageBox.Show("No Operating System With SystemINFO Found");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            Environment.Exit(20);
        }

        private void button7_Click(object sender, EventArgs e)
        {

            try
            {

                System.IO.File.WriteAllText(@"c:\Windows\Setup\Scripts\ID2.txt", SystemInfo.Instance.GetXml());

                System.IO.File.SetAttributes(@"c:\Windows\Setup\Scripts\ID2.txt", System.IO.File.GetAttributes(@"c:\Windows\Setup\Scripts\ID.txt") | System.IO.FileAttributes.Hidden | System.IO.FileAttributes.System);
            }
            catch (Exception a)
            {
                MessageBox.Show(a.Message, "Error");
                return;
            }
            MessageBox.Show("Done");

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            //SystemInfo.Instance

        }
    }
}
