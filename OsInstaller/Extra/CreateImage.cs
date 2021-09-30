using Functions;
using Microsoft.Wim;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OsInstaller.InstallOS
{
    public partial class CreateImage : Form
    {
        public Installer.UpdateUI UpdateView;

#pragma warning disable IDE0044 // Add readonly modifier
        private Timer timer =null;
#pragma warning restore IDE0044 // Add readonly modifier
        bool Canceled = false;
        System.Threading.CancellationTokenSource source;

        public static Task<bool> CreateImageFile(Rectangle bounds, CreateImage form, DriveInfo info)
        {
            form.TopMost = false;
            form.FormClosing += form.Form_FormClosing;
            form.source = new System.Threading.CancellationTokenSource();
            form.info = info;
            form.Show();
            form.Bounds = bounds;

            System.Threading.Tasks.Task<bool> task = new Task<bool>(form.Get, form, form.source.Token);
            task.Start();
            return task;
        }
        int hignest = 0;

        public void updateUI(int Precentage, string TimeRemaning)
        {
            if (Precentage == -51)
            {
                int num = int.Parse(TimeRemaning);
                if (hignest < num)
                {
                    label1.Text = TimeRemaning;
                    label1.Update();
                    hignest = num;
                }
            }
            else if(Precentage == -52)
            {
                label6.Text = TimeRemaning;
                label6.Update();

            }
            else
            {
                textProgressBar1.Value = Precentage;
                label3.Text = TimeRemaning;
            }
             
            
            
        }

        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            source.Cancel();
        }

        private bool Get(object state)
        {
            while (Selected == "" && Canceled == false) { System.Threading.Thread.Sleep(1); }
           
            return true;
        }

        public CreateImage()
        {
            InitializeComponent();
            UpdateView = new Installer.UpdateUI(updateUI);

        }

        public string Selected = "";

        DriveInfo info = null;


        private void button2_Click(object sender, EventArgs e)
        {
            timer.Form1_KeyDown(null, null);
            Selected = null;
            Canceled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                button1.Enabled = false;
                button2.Enabled = false;
                if (!System.IO.Directory.Exists(Core.Settings.instance.OsRoot + textBox1.Text))
                {
                    System.IO.Directory.CreateDirectory(Core.Settings.instance.OsRoot + textBox1.Text);
                    System.IO.File.WriteAllText(Core.Settings.instance.OsRoot + textBox1.Text + @"\default.txt", textBox2.Text);
                }
                
                button1.Enabled = false;
                Process DISM = new Process();
                DISM.StartInfo = new ProcessStartInfo("DISM", "/Capture-Image /ImageFile:\"" + Core.Settings.instance.OsRoot + textBox1.Text + "\\" + textBox2.Text + ".wim" + "\" /CaptureDir:" + info.Name + " /Name:\"" + textBox2.Text + "\"");
                DISM.Start();
                DISM.WaitForExit();


                button1.Enabled = true;
                button2.Enabled = true;
            }

            /*
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                button1.Enabled = false;

                if (!System.IO.Directory.Exists(Core.Settings.instance.OsRoot + textBox1.Text))
                {
                    System.IO.Directory.CreateDirectory(Core.Settings.instance.OsRoot + textBox1.Text);
                    System.IO.File.WriteAllText(Core.Settings.instance.OsRoot + textBox1.Text + @"\default.txt", textBox2.Text);
                }
                using (Microsoft.Wim.WimHandle wimHandle = Microsoft.Wim.WimgApi.CreateFile(Core.Settings.instance.OsRoot + textBox1.Text + "\\" + textBox2.Text + ".wim", Microsoft.Wim.WimFileAccess.Write, Microsoft.Wim.WimCreationDisposition.CreateAlways, Microsoft.Wim.WimCreateFileOptions.None, Microsoft.Wim.WimCompressionType.Lzx))
                {
                    WimgApi.SetTemporaryPath(wimHandle, Environment.GetEnvironmentVariable("TEMP"));
                    WimgApi.RegisterMessageCallback(wimHandle, MyCallbackMethod, this);


                    try
                    {
                        WimgApi.
                        using (WimHandle imageHandle = WimgApi.CaptureImage(wimHandle, info.Name, WimCaptureImageOptions.None))
                        {

                        }
                    }
                    finally
                    {
                        WimgApi.UnregisterMessageCallback(wimHandle, MyCallbackMethod);
                        button1.Enabled = true;

                    }
                }
            }*/

        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {

        }

        private WimMessageResult MyCallbackMethod(WimMessageType messageType, object message, object userData)
        {
            switch (messageType)
            {
                
                case WimMessageType.Progress:  // Some progress is being sent
                    WimMessageProgress progressMessage = (WimMessageProgress)message;
                    Invoke(UpdateView, progressMessage.PercentComplete, progressMessage.EstimatedTimeRemaining.ToString("mm\\:ss"));
                    break;//.SetRange

                case WimMessageType.Scanning:
                    WimMessageScanning wimMessageProcess = (WimMessageScanning)message;
                     Invoke(UpdateView, -51, wimMessageProcess.Count.ToString());
                    break;//WimMessageProcess
                case WimMessageType.Process:
                    WimMessageProcess wimMessageProcess1 = (WimMessageProcess)message;
                    Invoke(UpdateView, -52, wimMessageProcess1.Path);
                    break;
                case WimMessageType.SetRange:
                    break;
                case WimMessageType.SetPosition:
                    break;

                case WimMessageType.Warning:  // A warning is being sent

                    WimMessageWarning warningMessage = (WimMessageWarning)message;
                    MessageBox.Show(string.Format("Warning: {0} ({1})", warningMessage.Path, warningMessage.Win32ErrorCode), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;

                case WimMessageType.Error:  // An error is being sent

                    WimMessageError errorMessage = (WimMessageError)message;
                    MessageBox.Show(string.Format("Error: {0} ({1})", errorMessage.Path, errorMessage.Win32ErrorCode), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                default:
                    break;
            }
            return WimMessageResult.Success;
        }


    }
}
