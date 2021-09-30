using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Runtime.Serialization;
using System.Text;
using System.Net;
using System.IO;

public class SystemInfo
{
    [System.Xml.Serialization.XmlIgnore]
    public static SystemInfo Instance { get; set; } = new SystemInfo();
    public List<Memory> memory = new List<Memory>();
    public Processor processor = null;
    public System2 system = null;
    public ComputerSystem Computer = null;
    public Motherboard motherboard = null;
    public Bios bios = null;

    [System.Xml.Serialization.XmlIgnore]
    public List<Functions.HDD> hds = Functions.HDD.GetHDDs();

    [System.Xml.Serialization.XmlArray("HDs")]
    [System.Xml.Serialization.XmlArrayItem("HD")]
    public List<string> hdForXml { 
        get 
        {
            List<string> r = new List<string>();
            foreach (var item in hds)
            {
                r.Add(item.DriveIndex + " | " + item.IsOK + " | " + item.IsOKCustom + " | " + item.Model + " | " + item.Serial + " | " + item.Size + "GB | " + item.SmartSupported);
            }
            return r;
        } set { } }
    public List<PCI> PCIs = new List<PCI>();


    public List<string> LanMacs = new List<string>();
    public SystemInfo()
    {
        Console.WriteLine("Loading System Info...");

        foreach (var item in (
        from nic in System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()
        where nic.OperationalStatus == System.Net.NetworkInformation.OperationalStatus.Up
        select nic.GetPhysicalAddress().ToString()
        ))
        {
            if (item != "")
            {
                LanMacs.Add(item);
            }
        }

        ObjectQuery RAMquery = new ObjectQuery("SELECT * FROM Win32_PhysicalMemory");
        ManagementObjectSearcher RAMsearcher = new ManagementObjectSearcher(RAMquery);
        foreach (ManagementObject RAMqueryobj in RAMsearcher.Get())
        {
            memory.Add(new Memory(RAMqueryobj));
        }

        RAMquery = new ObjectQuery("SELECT * FROM Win32_SystemEnclosure");
        RAMsearcher = new ManagementObjectSearcher(RAMquery);
        foreach (ManagementObject RAMqueryobj in RAMsearcher.Get())
        {
            system = new System2(RAMqueryobj);
            break;
        }

        RAMquery = new ObjectQuery("SELECT * FROM Win32_ComputerSystem");
        RAMsearcher = new ManagementObjectSearcher(RAMquery);
        foreach (ManagementObject RAMqueryobj in RAMsearcher.Get())
        {
            Computer = new ComputerSystem(RAMqueryobj);
            break;
        }

        RAMquery = new ObjectQuery("SELECT * FROM Win32_BaseBoard");
        RAMsearcher = new ManagementObjectSearcher(RAMquery);
        foreach (ManagementObject RAMqueryobj in RAMsearcher.Get())
        {
            motherboard = new Motherboard(RAMqueryobj);
            break;
        }
        RAMquery = new ObjectQuery("SELECT * FROM Win32_Processor");
        RAMsearcher = new ManagementObjectSearcher(RAMquery);
        foreach (ManagementObject RAMqueryobj in RAMsearcher.Get())
        {
            processor = new SystemInfo.Processor(RAMqueryobj);
            break;
        }
        RAMquery = new ObjectQuery("SELECT * FROM Win32_BIOS");
        RAMsearcher = new ManagementObjectSearcher(RAMquery);
        foreach (ManagementObject RAMqueryobj in RAMsearcher.Get())
        {
            bios = new Bios(RAMqueryobj);
            break;
        }
        RAMquery = new ObjectQuery("SELECT * FROM win32_pnpentity where deviceid like '%PCI%'");
        RAMsearcher = new ManagementObjectSearcher(RAMquery);
        foreach (ManagementObject RAMqueryobj in RAMsearcher.Get())
        {
            PCIs.Add(new PCI(RAMqueryobj));
        }
    }

    [DataContract]
    public class Processor : SupportError
    {
        [System.Xml.Serialization.XmlAttribute]
        public string Name { get; set; }
        [System.Xml.Serialization.XmlAttribute]
        public string MaxClockSpeed { get; set; }
        [System.Xml.Serialization.XmlAttribute]
        public string Manufacturer { get; set; }
        public Processor() { }
        public Processor(ManagementObject queryobj)
        {
            try
            {
                Manufacturer = queryobj["Manufacturer"].ToString().Trim();
            }
            catch (Exception e)
            {
                Error(new Exception("Unable To Get Processor Manufacturer", e));
            }
            try
            {
                MaxClockSpeed = queryobj["MaxClockSpeed"].ToString().Trim();
            }
            catch (Exception e)
            {
                Error(new Exception("Unable To Get Processor MaxClockSpeed", e));
            }
            try
            {
                Name = queryobj["Name"].ToString().Trim();
            }
            catch (Exception e)
            {
                Error(new Exception("Unable To Get Processor Name", e));
            }
        }
    }

    public class Bios : SupportError
    {
        [System.Xml.Serialization.XmlAttribute]
        public string SMBIOSBIOSVersion { get; set; }
        [System.Xml.Serialization.XmlAttribute]
        public string Manufacturer { get; set; }
        [System.Xml.Serialization.XmlAttribute]
        public string Name { get; set; }
        [System.Xml.Serialization.XmlAttribute]
        public string SerialNumber { get; set; }
        [System.Xml.Serialization.XmlAttribute]
        public string Version { get; set; }
        public Bios() { }
        public Bios(ManagementObject queryobj)
        {
            try
            {
                Manufacturer = queryobj["Manufacturer"].ToString().Trim();
            }
            catch (Exception e)
            {
                Error(new Exception("Unable To Get Bios Manufacturer", e));
            }
            try
            {
                SerialNumber = queryobj["SerialNumber"].ToString().Trim();
            }
            catch (Exception e)
            {
                Error(new Exception("Unable To Get Bios SerialNumber", e));
            }
            try
            {
                SMBIOSBIOSVersion = queryobj["SMBIOSBIOSVersion"].ToString().Trim();
            }
            catch (Exception e)
            {
                Error(new Exception("Unable To Get Bios SMBIOSBIOSVersion", e));
            }
            try
            {
                Name = queryobj["Name"].ToString().Trim();
            }
            catch (Exception e)
            {
                Error(new Exception("Unable To Get Bios Name", e));
            }
            try
            {
                Version = queryobj["Version"].ToString().Trim();
            }
            catch (Exception e)
            {
                Error(new Exception("Unable To Get Bios Version", e));
            }
        }
    }

    public class Motherboard : SupportError
    {
        [System.Xml.Serialization.XmlAttribute]
        public string Manufacturer { get; set; }
        [System.Xml.Serialization.XmlAttribute]
        public string Model { get; set; }
        [System.Xml.Serialization.XmlAttribute]
        public string SerialNumber { get; set; }
        [System.Xml.Serialization.XmlAttribute]
        public string Product { get; set; }
        public Motherboard() { }
        public Motherboard(ManagementObject queryobj)
        {
            try
            {
                Manufacturer = queryobj["Manufacturer"].ToString().Trim();
            }
            catch (Exception e)
            {
                Error(new Exception("Unable To Get Motherboard Manufacturer", e));
            }
            try
            {
                Model = queryobj["Model"].ToString().Trim();
            }
            catch (Exception)
            {
                Model = "To Be Filled By O.E.M.";
            }
            try
            {
                SerialNumber = queryobj["SerialNumber"].ToString().Trim();
            }
            catch (Exception e)
            {
                Error(new Exception("Unable To Get Motherboard Manufacturer", e));
            }
            try
            {
                Product = queryobj["Product"].ToString().Trim();
            }
            catch (Exception e)
            {
                Error(new Exception("Unable To Get Motherboard Product", e));
            }
        }
    }

    public class PCI : SupportError
    {
        [System.Xml.Serialization.XmlAttribute]
        public string DeviceID { get; set; }
        [System.Xml.Serialization.XmlAttribute]
        public string Name { get; set; }
        public PCI() { }

        public PCI(ManagementObject RAMqueryobj)
        {
            try
            {
                DeviceID = RAMqueryobj["DeviceID"].ToString();
            }
            catch (Exception e)
            {
                Error(new Exception("Unable To Get PCI DeviceID", e));
            }
            try
            {
                Name = RAMqueryobj["Name"].ToString();
            }
            catch (Exception e)
            {
                Error(new Exception("Unable To Get PCI Name", e));
            }
        }
    }

    [DataContract]
    public class ComputerSystem : SupportError
    {
        [DataMember]
        public string Manufacturer { get; set; }
        [DataMember]
        public string Model { get; set; }
        public ComputerSystem() { }
        public ComputerSystem(ManagementObject queryobj)
        {
            try
            {
                Manufacturer = queryobj["Manufacturer"].ToString().Trim();
            }
            catch (Exception e)
            {
                Error(new Exception("Unable To Get Computer System Manufacturer", e));
            }
            try
            {
                Model = queryobj["Model"].ToString().Trim();
            }
            catch (Exception)
            {
                Model = "To Be Filled By O.E.M.";
            }
        }
    }
    [DataContract]
    public class System2 : SupportError
    {
        [System.Xml.Serialization.XmlAttribute]
        public string Manufacturer { get; set; }
        [System.Xml.Serialization.XmlAttribute]
        public string Model { get; set; }
        [System.Xml.Serialization.XmlAttribute]
        public string SerialNumber { get; set; }
        [System.Xml.Serialization.XmlAttribute]
        public string SMBIOSAssetTag { get; set; }

        public System2() { }
        public System2(ManagementObject queryobj)
        {
            try
            {
                Manufacturer = queryobj["Manufacturer"].ToString().Trim();
            }
            catch (Exception e)
            {
                Error(new Exception("Unable To Get System Manufacturer", e));
            }
            try
            {
                Model = queryobj["Model"].ToString().Trim();
            }
            catch (Exception)
            {
                Model = "To Be Filled By O.E.M.";
            }
            try
            {
                Manufacturer = queryobj["Manufacturer"].ToString().Trim();
            }
            catch (Exception e)
            {
                Error(new Exception("Unable To Get System SerialNumber", e));
            }
            try
            {
                SerialNumber = queryobj["SerialNumber"].ToString().Trim();
            }
            catch (Exception e)
            {
                Error(new Exception("Unable To Get System SerialNumber", e));
            }
            try
            {
                SMBIOSAssetTag = queryobj["SMBIOSAssetTag"].ToString().Trim();
            }
            catch (Exception e)
            {
                Error(new Exception("Unable To Get System SMBIOSAssetTag", e));
            }

        }
    }

    public class Memory : SupportError
    {
        [System.Xml.Serialization.XmlAttribute]
        public string BankLabel { get; set; }
        [System.Xml.Serialization.XmlAttribute]
        public double Capacity { get; set; }

        [System.Xml.Serialization.XmlIgnore]
        public double CapacityInKB { get { return Capacity / 1024; } }
        [System.Xml.Serialization.XmlIgnore]
        public double CapacityInMB { get { return CapacityInKB / 1024; } }
        [System.Xml.Serialization.XmlAttribute]
        public int CapacityInGB { get { return Convert.ToInt32(CapacityInMB / 1024); } }
        [System.Xml.Serialization.XmlAttribute]
        public string PartNumber { get; set; }
        [System.Xml.Serialization.XmlAttribute]
        public string SerialNumber { get; set; }
        [System.Xml.Serialization.XmlAttribute]
        public int Speed { get; set; }
        [System.Xml.Serialization.XmlAttribute]
        public string Tag { get; set; }
        public Memory() { }
        public Memory(ManagementObject RAMqueryobj)
        {
            try
            {
                Capacity = (Convert.ToDouble(RAMqueryobj["Capacity"]));
            }
            catch (Exception e)
            {
                Error(new Exception("Unable To Get Memory Capacity", e));
            }
            try
            {
                BankLabel = RAMqueryobj["BankLabel"].ToString().Trim();
            }
            catch (Exception e)
            {
                Error(new Exception("Unable To Get Memory BankLabel", e));
            }
            try
            {
                PartNumber = RAMqueryobj["PartNumber"].ToString().Trim();
            }
            catch (Exception e)
            {
                Error(new Exception("Unable To Get Memory PartNumber", e));
            }
            try
            {
                Speed = Convert.ToInt32(RAMqueryobj["Speed"]);
            }
            catch (Exception e)
            {
                Error(new Exception("Unable To Get Memory Speed", e));
            }
            try
            {
                Tag = RAMqueryobj["Tag"].ToString().Trim();
            }
            catch (Exception e)
            {
                Error(new Exception("Unable To Get Memory Tag", e));
            }
        }
    }

    internal string GetXml()
    {
        using (System.IO.StringWriter stream1 = new System.IO.StringWriter())
        {
            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(this.GetType());
            ser.Serialize(stream1, this);
            return stream1.ToString();
        }
    }
    internal static SystemInfo FromFile(string FilePath)
    {
        if (System.IO.File.Exists(FilePath))
        {
            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(SystemInfo));
            using (FileStream stream = new FileStream(FilePath,FileMode.Open))
            {
                return (SystemInfo)ser.Deserialize(stream);
            }
        }
        return null;
    }

    [DataContract]
    public class SupportError
    {
        public void Error(Exception e)
        {
            Console.WriteLine(e.Message);
            Console.ReadLine();
        }
    }
}

