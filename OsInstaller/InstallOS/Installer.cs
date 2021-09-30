using Microsoft.Wim;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OsInstaller.InstallOS
{
    public partial class Installer : Form
    {
        public Installer()
        {
            InitializeComponent();
            UpdateView = new UpdateUI(updateUI);
            
        }
        public void updateUI(int Precentage, string TimeRemaning)
        {
            textProgressBar1.Value = Precentage;
            label3.Text = TimeRemaning;
        }

        public Functions.HDD hDD { get; set; }
        public InstallOS.SelectOS.Output Data;
        public UpdateUI UpdateView;
        public static Dictionary<string, string> PciConverter = new Dictionary<string, string>()
        {
            { "VEN_10DE&DEV_1E84", "NVIDIA GeForce RTX 2070 SUPER" },
            { "VEN_10DE&DEV_1B06", "NVIDIA GeForce GTX 1080 Ti" },
            { "VEN_10DE&DEV_1B80", "NVIDIA GeForce GTX 1080" },
            { "VEN_10DE&DEV_1BA0", "NVIDIA GeForce GTX 1080 Mobile" },
            { "VEN_10DE&DEV_1BE0", "NVIDIA GeForce GTX 1080 Mobile" },
            { "VEN_10DE&DEV_1B81", "NVIDIA GeForce GTX 1070" },
            { "VEN_10DE&DEV_1B82", "NVIDIA GeForce GTX 1070 Ti" },
            { "VEN_10DE&DEV_1BA1", "NVIDIA GeForce GTX 1070 Mobile" },
            { "VEN_10DE&DEV_1BA2", "NVIDIA GeForce GTX 1070 Mobile" },
            { "VEN_10DE&DEV_1BE1", "NVIDIA GeForce GTX 1070 Mobile" },
            { "VEN_10DE&DEV_1B83", "NVIDIA GeForce GTX 1060 6GB" },
            { "VEN_10DE&DEV_1B84", "NVIDIA GeForce GTX 1060 3GB" },
            { "VEN_10DE&DEV_1C03", "(OLD) NVIDIA GeForce GTX 1060 6GB" },
            { "VEN_10DE&DEV_1C02", "(OLD) NVIDIA GeForce GTX 1060 6GB" },
            { "VEN_10DE&DEV_1C04", "NVIDIA GeForce GTX 1060 5GB" },
            { "VEN_10DE&DEV_1C06", "(OLD) NVIDIA GeForce GTX 1060 6GB" },
            { "VEN_10DE&DEV_1C20", "NVIDIA GeForce GTX 1060 Mobile" },
            { "VEN_10DE&DEV_1C60", "NVIDIA GeForce GTX 1060 6GB Mobile" },
            { "VEN_10DE&DEV_1050", "NVIDIA GeForce GT 520M" },
            { "VEN_10DE&DEV_1C21", "NVIDIA GeForce GTX 1050 Ti Mobile" },
            { "VEN_10DE&DEV_1C22", "NVIDIA GeForce GTX 1050 Mobile" },
            { "VEN_10DE&DEV_1C61", "NVIDIA GeForce GTX 1050 Ti Mobile" },
            { "VEN_10DE&DEV_1C62", "NVIDIA GeForce GTX 1050 Mobile" },
            { "VEN_10DE&DEV_1C81", "NVIDIA GeForce GTX 1050" },
            { "VEN_10DE&DEV_1C82", "NVIDIA GeForce GTX 1050 Ti" },
            { "VEN_10DE&DEV_1C83", "NVIDIA GeForce GTX 1050 3GB" },
            { "VEN_10DE&DEV_1C8c", "NVIDIA GeForce GTX 1050 Ti Mobile" },
            { "VEN_10DE&DEV_1C8d", "NVIDIA GeForce GTX 1050 Mobile" },
            { "VEN_10DE&DEV_1C8F", "NVIDIA GeForce GTX 1050 Ti Max-Q" },
            { "VEN_10DE&DEV_1C92", "NVIDIA GeForce GTX 1050 Mobile" },
            { "VEN_10DE&DEV_1CCC", "NVIDIA GeForce GTX 1050 Ti Mobile" },
            { "VEN_10DE&DEV_1CCD", "NVIDIA GeForce GTX 1050 Mobile" },
            { "VEN_1002&DEV_4150", "AMD Radeon RV350 [Radeon 9550/9600/X1050 Series]" },
            { "VEN_1002&DEV_4152", "AMD Radeon RV360 [Radeon 9600/X1050 Series]" },
            { "VEN_1002&DEV_4170", "AMD Radeon RV350 [Radeon 9550/9600/X1050 Series]" },
            { "VEN_1002&DEV_4172", "AMD Radeon RV350 [Radeon 9600/X1050 Series]" },
            { "VEN_1002&DEV_4E51", "AMD Radeon RV350 [Radeon 9550/9600/X1050 Series]" },
            { "VEN_1002&DEV_5B63", "AMD Radeon RV370 [Radeon X300/X550/X1050 Series]" },
            { "VEN_1002&DEV_5B72", "AMD Radeon RV380 [Radeon X300/X550/X1050 Series]" },
            { "VEN_1002&DEV_5B73", "AMD Radeon RV370 [Radeon X300/X550/X1050 Series" },

        }; 

        public delegate void UpdateUI(int Precentage, string TimeRemaning);

        internal static Task<bool> InstallOSTask(Rectangle bounds, Installer form)
        {
            form.label16.Text = form.hDD.DriveIndex;
            form.label4.Text = form.hDD.Model;
            form.label5.Text = form.hDD.Serial;
            form.label7.Text = form.hDD.IsOK.ToString();
            form.label9.Text = form.hDD.IsOKCustom.ToString();
            form.label11.Text = form.hDD.Size.ToString() + " GB";
            form.label13.Text = form.hDD.SmartSupported.ToString();
            form.label1.Text = form.Data.Brand;
            form.label19.Text = form.Data.Name;

            form.label23.Text = SystemInfo.Instance.motherboard.SerialNumber;
            form.label26.Text = SystemInfo.Instance.motherboard.Model;
            form.label30.Text = SystemInfo.Instance.bios.SerialNumber;
            form.label28.Text = SystemInfo.Instance.Computer.Model;

            foreach (var Pci in SystemInfo.Instance.PCIs)
            {
                foreach (var item in PciConverter)
                {
                    if (Pci.DeviceID.Contains(item.Key))
                    {
                        form.listView2.Items.Add(item.Value);
                    }
                }
                
            }


            form.label21.Text = SystemInfo.Instance.processor.Name + " @ "+SystemInfo.Instance.processor.MaxClockSpeed+"MHz";

            foreach (var Ram in SystemInfo.Instance.memory)
            {
                form.listView1.Items.Add(new ListViewItem(new string[] { Ram.CapacityInGB.ToString()+"GB", Ram.PartNumber, (Ram.SerialNumber == null)?"N/A":Ram.SerialNumber ,Ram.Speed.ToString()+"Mhz", Ram.Tag,Ram.BankLabel }));
            }
            form.Show();
            form.Bounds = bounds;
            System.Threading.Tasks.Task<bool> task = new Task<bool>(form.Install, form);
            task.Start();
            return task;
        }

        public static bool IsUEFI()
        {
            RegistryKey winLogonKey = Registry.LocalMachine.OpenSubKey(@"System\CurrentControlSet\Control", false);
            string currentKey = "1";
            try
            {
                currentKey = winLogonKey.GetValue("PEFirmwareType").ToString();
            }
            catch
            {


            }


            if (currentKey == "2")
                return true;
            return false;
        }
        public static void Create_Uefi(string Drive, bool Data_Partition, int Size, bool Recovery)
        {
            string exec = string.Format(
                "select disk " + Drive + "\r\n" +
                "clean\r\n" +
                "convert gpt\r\n" +
                "create partition primary size=550\r\n" +
                "format quick fs=ntfs label=\"Windows RE tools\"\r\n" +
                "assign letter=\"T\"\r\n" +
                "set id=\"de94bba4-06d1-4d40-a16a-bfd50179d6ac\"\r\n" +
                "gpt attributes=0x8000000000000001\r\n" +
                "create partition efi size=100\r\n" +
                "format quick fs=fat32 label=\"System\"\r\n" +
                "assign letter=\"S\"\r\n" +
                "create partition msr size=128\r\n" +
                "create partition primary\r\n");
            if (Data_Partition && Recovery)
            { exec += string.Format("shrink minimum=" + (20 + Size) + "000\r\n"); }
            else if (Recovery) { exec += string.Format("shrink minimum=20000\r\n"); }
            else if (Data_Partition) { exec += string.Format("shrink minimum=" + Size + "000\r\n"); }

            exec += string.Format(
                "format quick fs=ntfs label=\"Windows\"\r\n" +
                "assign letter=\"w\"\r\n"
                );
            if (Recovery)
            {
                exec += string.Format("create partition primary\r\n");
                if (Data_Partition)
                {
                    exec += string.Format("create partition primary\r\n" + "shrink minimum=" + Size + "000\r\n");
                }
                exec += string.Format(
                "format quick fs=ntfs label=\"Recovery image\"\r\n" +
                "assign letter=\"R\"\r\n" +
                "set id=\"de94bba4-06d1-4d40-a16a-bfd50179d6ac\"\r\n" +
                "gpt attributes=0x8000000000000001"
                );
            }

            if (Data_Partition)
            {
                exec += string.Format("\r\n" +
                    "create partition primary\r\n" +
                    "format quick fs=ntfs label=\"Data\"\r\nassign letter=l\r\n"
                    );
            }
            Functions.CMD.Diskpart(exec, true);

        }

        public static void Create_Bios(string Drive, bool Data_Partition, int Size, bool Recovery)
        {
            string exec = string.Format("select disk " + Drive + "\r\n" +
            "clean\r\n" +
            "create partition primary size=550\r\n" +
            "format quick fs=ntfs label=\"System\"\r\n" +
            "assign letter=\"S\"\r\n" +
            "active\r\n" +
            "create partition primary\r\n");
            if (Data_Partition && Recovery)
            { exec += string.Format("shrink minimum=" + (20 + Size) + "000\r\n"); }
            else if (Recovery) { exec += string.Format("shrink minimum=20000\r\n"); }

            exec += string.Format(
            "format quick fs=ntfs label=\"Windows\"\r\n" +
            "assign letter=\"w\"");
            if (Recovery)
            {
                exec += string.Format(
                   "\r\ncreate partition primary\r\n");
                if (Data_Partition) { exec += string.Format("shrink minimum=" + Size + "000\r\n"); }
                exec += string.Format(
                   "format quick fs=ntfs label=\"Recovery image\"\r\n" +
                   "assign letter=\"R\"\r\n" +
                   "set id=27");
            }

            if (Data_Partition)
            {
                exec += string.Format("\r\n" +
                    "create partition primary\r\n" +
                    "format quick fs=ntfs label=\"Data\"\r\nassign letter=l\r\n"
                    );
            }
            Functions.CMD.Diskpart(exec, true);
        }

        private bool Install(object a)
        {
            
            Installer installer = (Installer)a;

            bool IS_UEFI = IsUEFI();
            if(IS_UEFI==false)
                IS_UEFI = hDD.Size >= 2000;
            //return;
            if (IS_UEFI || Data.UseUEFI)
            {
                Create_Uefi(installer.hDD.DriveIndex, false, 0, false);
            }
            else
            {
                Create_Bios(installer.hDD.DriveIndex, false, 0, false);
            }


            try
            {
                using (WimHandle wimHandle = WimgApi.CreateFile(Core.Settings.instance.OsRoot+ installer.Data.Brand+"\\"+ installer.Data.File, WimFileAccess.Read, WimCreationDisposition.OpenExisting, WimCreateFileOptions.Chunked, WimCompressionType.None))
                {
                    WimgApi.SetTemporaryPath(wimHandle, Environment.GetEnvironmentVariable("TEMP"));
                    WimgApi.RegisterMessageCallback(wimHandle, MyCallbackMethod, this);
                    try
                    {
                        using (WimHandle imageHandle = WimgApi.LoadImage(wimHandle, installer.Data.Index))
                        {
                            WimgApi.ApplyImage(imageHandle, @"w:\", WimApplyImageOptions.None);
                        }
                    }
                    finally
                    {
                        WimgApi.UnregisterMessageCallback(wimHandle, MyCallbackMethod);
                    }
                }
            }
            catch (Exception e)
            {
                if (MessageBox.Show(e.Message,"Error",MessageBoxButtons.OKCancel,MessageBoxIcon.Error) == DialogResult.Cancel)
                {
                    return false;
                }
            }
            


            if (IS_UEFI || Data.UseUEFI)
            { Functions.CMD.exec("bcdboot", "w:\\Windows /s S: /f UEFI"); }
            else
            { Functions.CMD.exec("bcdboot", "w:\\Windows /s S: /f BIOS"); }

            if (checkBox1.Checked)
            {
                if (!System.IO.Directory.Exists(@"w:\Windows\Setup\"))
                {
                    System.IO.Directory.CreateDirectory(@"w:\Windows\Setup\");
                }
                if (!System.IO.Directory.Exists(@"w:\Windows\Setup\Scripts\"))
                {
                    System.IO.Directory.CreateDirectory(@"w:\Windows\Setup\Scripts\");
                }

                System.IO.File.WriteAllText(@"w:\Windows\Setup\Scripts\ID.txt", SystemInfo.Instance.GetXml());
                System.IO.File.SetAttributes(@"w:\Windows\Setup\Scripts\ID.txt", System.IO.File.GetAttributes(@"w:\Windows\Setup\Scripts\ID.txt") | System.IO.FileAttributes.Hidden | System.IO.FileAttributes.System);
            }

            return true;
        }

        private WimMessageResult MyCallbackMethod(WimMessageType messageType, object message, object userData)
        {
            switch(messageType)
            {
                case WimMessageType.Progress:  // Some progress is being sent
                    WimMessageProgress progressMessage = (WimMessageProgress)message;
                    Invoke(UpdateView, progressMessage.PercentComplete, progressMessage.EstimatedTimeRemaining.ToString("mm\\:ss"));
                    break;

                case WimMessageType.Warning:  // A warning is being sent

                    WimMessageWarning warningMessage = (WimMessageWarning)message;
                    MessageBox.Show(string.Format("Warning: {0} ({1})", warningMessage.Path, warningMessage.Win32ErrorCode), "Warning",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    break;

                case WimMessageType.Error:  // An error is being sent

                    WimMessageError errorMessage = (WimMessageError)message;
                    MessageBox.Show(string.Format("Error: {0} ({1})", errorMessage.Path, errorMessage.Win32ErrorCode), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
            }
            return WimMessageResult.Success;
        }

        private void Installer_Load(object sender, EventArgs e)
        {
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
