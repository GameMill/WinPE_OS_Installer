using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsInstaller.Core
{
    public class Settings : SaveableSettings
    {
        [System.Xml.Serialization.XmlIgnore]
        private static Settings _instance = null;

        [System.Xml.Serialization.XmlIgnore]
        public static Settings instance { 
            get 
            {
#if DEBUG
                if (_instance == null) { _instance = new Settings(); _instance.OsRoot = @"\\192.168.0.3\Shop\OperatingSystems\"; }
                return _instance;
                
# else

                if (_instance == null) { _instance = SaveableSettings.Load<Settings>("Setting.xml"); }  return _instance;
#endif
            }
        }

        public override string FileName()
        {
            return "Setting.xml";
        }

        public string OsRoot { get; set; } = @"y:\";
    }
}
