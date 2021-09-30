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
    
    public partial class SelectOS : Form
    {

        public class Output
        {
            public string Brand;
            public string Name;
            public string File;
            public int Index;
            public bool UseUEFI;
        }

        private Timer timer;

        Xml2CSharp.Brands Data = Xml2CSharp.Brands.GetData();

        public SelectOS()
        {
            InitializeComponent();
            FormClosing += SelectOS_FormClosing;
            Start:
            listView1.Scrollable = true;
            listView1.View = View.Details;
            listView1.Font = new Font(FontFamily.GenericSansSerif, 10f);

            listView2.Scrollable = true;
            listView2.View = View.Details;
            listView2.Font = new Font(FontFamily.GenericSansSerif, 10f);

            foreach (var item in Data.Brand)
            {
                listView2.Items.Add(new ListViewItem()
                {
                    Text = item.Name,Selected=item.Default
                });    
            }
            listView2.SelectedIndexChanged += ListView2_SelectedIndexChanged;


            /*this.FormBorderStyle = FormBorderStyle.None;
            string Dir = Core.Settings.instance.OsRoot;
            Core.Settings.instance.Save();
            Dictionary<string,List<string>> OS = new Dictionary<string, List<string>>();

            var DefaultBrand = System.IO.File.ReadAllText(Dir + "default.txt");
            foreach (var BrandDir in System.IO.Directory.GetDirectories(Dir))
            {
                var Brand = BrandDir.Substring(Dir.Length);
                ListViewGroup group = new ListViewGroup(Brand);
                listView1.Groups.Add(group);
                var DefaultOS = System.IO.File.ReadAllText(BrandDir + "\\default.txt");

                foreach (var item in System.IO.Directory.GetFiles(BrandDir,"*.wim"))
                {
                    var a = item.Substring((BrandDir + "\\").Length);
                    var Os = a.Substring(0,a.Length-4);
                    listView1.Items.Add(new ListViewItem(Os, group) { Selected = (Os == DefaultOS && Brand == DefaultBrand) });
                }

            }
            listView1.ShowGroups = true;*/

            if (listView2.Items.Count > 0)
            {
               // listView1.Items[0].Selected = listView1.Items.Count == 1;
                button1.Enabled = (listView1.SelectedItems.Count != 0);
#if DEBUG

#else
                timer = new Timer(5000, 1000, TimerOnComplate, this, Tick);

                listView1.MouseDown += new MouseEventHandler(timer.Form1_KeyDown);
#endif
            }
            else
            {
                button1.Enabled = (listView1.SelectedItems.Count != 0);

                if (MessageBox.Show("Error: No Operating Systems Found", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Cancel)
                {

                }
                else
                {
                    goto Start;
                }
            }
        }

        private void ListView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listView2.SelectedItems.Count == 0)
            {
                listView1.Items.Clear();
                return;
            }
            foreach (var item in Data.Brand)
            {
                if (item.Name == listView2.SelectedItems[0].Text)
                {
                    foreach (var os in item.Os)
                    {
                        var a = new ListViewItem()
                        {
                            Text = os.Name,
                            Selected = os.Default
                        };
                        a.SubItems.Add(os.Index.ToString());
                        a.SubItems.Add(os.File);
                        listView1.Items.Add(a);
                    }
                }
            }

        }

        private void SelectOS_FormClosing(object sender, FormClosingEventArgs e)
        {
            source.Cancel();
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

        private void button1_Click(object sender, EventArgs e)
        {
            if(timer != null)
            timer.Form1_KeyDown(null, null);
            if (listView1.SelectedItems.Count > 0)
            {
                b.Brand = listView2.SelectedItems[0].Text;
                b.Name = listView1.SelectedItems[0].Text;
                b.File = listView1.SelectedItems[0].SubItems[2].Text;
                b.Index = int.Parse(listView1.SelectedItems[0].SubItems[1].Text);
                b.UseUEFI = checkBox1.Checked;
                SelectedOS = true;
            }
        }
        Output b = new Output();
        bool SelectedOS = false;
        System.Threading.CancellationTokenSource source;


        internal static Task<Output> SelectOSTask(Rectangle bounds, SelectOS form)
        {
            form.source = new System.Threading.CancellationTokenSource();
            form.Show();
            form.Bounds = bounds;
            System.Threading.Tasks.Task<Output> task = new Task<Output>(form.Get, form, form.source.Token);
            task.Start();
            return task;
        }

        private Output Get(object a)
        {
            var form = (SelectOS)a;
            while (!SelectedOS) { System.Threading.Thread.Sleep(1); }

            /*
            foreach (var item in Data.Brand)
            {
                if (item.Name  == b.Brand)
                {
                    foreach (var os in item.Os)
                    {
                        if (b.OS == os.Name)
                        {
                            b.Index = os.Index;
                        }
                    }
                }
            }
            */
            
            return b;
        }

        private void SelectOS_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer.Form1_KeyDown(null,null);
            SelectedOS = true;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = (listView1.SelectedItems.Count != 0);

        }
    }
}
