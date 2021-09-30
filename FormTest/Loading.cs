using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormTest
{
    public partial class Loading : Form
    {
        public Loading()
        {
            InitializeComponent();
            
        }
        public static Type Error;
        private void OnShown(object sender, EventArgs e)
        {
            label2.Invalidate();
            label2.Refresh();
            label3.Invalidate();
            label3.Refresh();
            Application.DoEvents();
            using (System.Net.WebClient myWebClient = new System.Net.WebClient())
            {
                var url = ""
                try
                {
                    url = System.IO.File.ReadAllText("UpdateURL.txt");

                }
                catch
                {
                    MessageBox.Show("You need to create the UpdateURL.txt file with the url of the update server");
                    Application.Exit();
                }
#if DEBUG

                myWebClient.DownloadFile(url + "Debug/OsInstaller.dll", AppDomain.CurrentDomain.BaseDirectory + @"\OsInstaller.dll");
                myWebClient.DownloadFile(url + "Debug/OsInstaller.pdb", AppDomain.CurrentDomain.BaseDirectory + @"\OsInstaller.pdb");
#else
                myWebClient.DownloadFile(url + "OsInstaller.dll", AppDomain.CurrentDomain.BaseDirectory + @"\OsInstaller.dll");
                myWebClient.DownloadFile(url + "OsInstaller.pdb", AppDomain.CurrentDomain.BaseDirectory + @"\OsInstaller.pdb");
#endif
            }
            var DLL = System.Reflection.Assembly.LoadFile(AppDomain.CurrentDomain.BaseDirectory + @"\OsInstaller.dll");

            Functions.HDD.GetHDDs();
            Error = DLL.GetType("OsInstaller.Error");
            var form = (Form)Activator.CreateInstance(DLL.GetType("OsInstaller.Form1"));
            form.Show();
            form.Bounds = this.Bounds;
            Hide();
        }
    }
}
