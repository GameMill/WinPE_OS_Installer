using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace OsInstaller
{
    public partial class Error : Form
    {
        Exception e;
        public Error(Exception exceptionObject)
        {
            e = exceptionObject;
            InitializeComponent();
            textBox1.Text = exceptionObject.GetType().ToString();
            richTextBox1.Text = exceptionObject.Message;
            textBox2.Text = exceptionObject.Source;
            richTextBox2.Text = exceptionObject.StackTrace;
            using (var stream = File.Open(@"\\192.168.0.3\Project\Errors\Osinstaller\" + DateTime.Now.ToString("dd MMM HH mm ss") + ".txt", FileMode.Create))
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (var writer = System.Runtime.Serialization.Json.JsonReaderWriterFactory.CreateJsonWriter(memoryStream, Encoding.UTF8, true, true, "  "))
                    {
                        var serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(this.e.GetType(), Settings);
                        serializer.WriteObject(writer, this.e);
                        writer.Flush();
                    }
                    try
                    {
                        string text = Encoding.UTF8.GetString(memoryStream.ToArray());
                        text = System.Text.RegularExpressions.Regex.Replace(text, "\"WatsonBuckets\": \\[[^\\]]*(],)", "");
                        text = System.Text.RegularExpressions.Regex.Replace(text, "\"WatsonBuckets\": \\[[^\\]]*(])", "");
                        byte[] a = Encoding.ASCII.GetBytes(text);
                        stream.Write(a, 0, a.Length);
                    }
                    catch (Exception)
                    {

                    }
                    
                }
                
            }
        }
        public readonly System.Runtime.Serialization.Json.DataContractJsonSerializerSettings Settings = new System.Runtime.Serialization.Json.DataContractJsonSerializerSettings { UseSimpleDictionaryFormat = true };


        private void Save_Click(object sender, EventArgs e)
        {

            Application.Exit();

        }
    }
}
