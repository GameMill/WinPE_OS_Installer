using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OsInstaller
{
    [XmlRoot("Setting", IsNullable = false)]
    public abstract class SaveableSettings
    {
        public abstract string FileName();


        public override string ToString()
        {
            var emptyNs = new XmlSerializerNamespaces(new[] { System.Xml.XmlQualifiedName.Empty });
            XmlSerializer ser = new XmlSerializer(this.GetType());
            string Data = ""; 
            using (System.IO.TextWriter writer = new System.IO.StringWriter())
            {
                ser.Serialize(writer, this, emptyNs);
                Data = writer.ToString();
            }

            return StringExtensions.RemoveFirstLines(Data, 1);
        }

        public static T Load<T>(string Filename) where T : SaveableSettings, new()
        {
            try
            {
                if (!System.IO.File.Exists(Filename))
                {
                    T a = new T();
                    a.Save();
                    return a;
                }
                var emptyNs = new XmlSerializerNamespaces(new[] { System.Xml.XmlQualifiedName.Empty });
                XmlSerializer ser = new XmlSerializer(typeof(T));
                using (System.IO.TextReader Reader = new System.IO.StringReader(System.IO.File.ReadAllText(Filename)))
                {
                    return (T)ser.Deserialize(Reader);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool Save()
        {

            try
            {
                using (System.IO.FileStream a = new System.IO.FileStream(FileName(), System.IO.FileMode.Create, System.IO.FileAccess.Write))
                {
                    using (System.IO.StreamWriter b = new System.IO.StreamWriter(a))
                    {
                        b.Write(ToString());
                    }
                }
                return true;
            }
            catch (Exception e)
            {

                throw e;
            }
        }
    }

    public static class StringExtensions
    {
        public static string RemoveFirstLines(string text, int linesCount)
        {
            var lines = System.Text.RegularExpressions.Regex.Split(text, "\r\n|\r|\n").Skip(linesCount);
            return string.Join(Environment.NewLine, lines.ToArray());
        }
    }
}
