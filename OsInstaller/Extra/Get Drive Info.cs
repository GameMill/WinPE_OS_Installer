using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OsInstaller.Core
{
    public partial class Get_Drive_Info : Form
    {
#pragma warning disable IDE0044 // Add readonly modifier
        System.Threading.CancellationTokenSource source = null;
#pragma warning restore IDE0044 // Add readonly modifier

        public Get_Drive_Info(SystemInfo info)
        {
            InitializeComponent();


        }

        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            source.Cancel();
        }

        internal static Task<bool> ShowInfo(Rectangle bounds, Core.Get_Drive_Info form)
        {
            // form.source = new System.Threading.CancellationTokenSource();
            // form.Show();
            // form.Bounds = bounds;

            // System.Threading.Tasks.Task<bool> task = new Task<bool>(form.Get, form, form.source.Token);
            //  task.Start();
            // return task;
            return null;
        }

        private bool Get()
        {
            throw new NotImplementedException();
        }
    }
}
