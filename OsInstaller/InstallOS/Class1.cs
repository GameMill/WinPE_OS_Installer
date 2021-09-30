/* 
 Licensed under the Apache License, Version 2.0

 http://www.apache.org/licenses/LICENSE-2.0
 */
using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using OsInstaller.Core;
using System.Collections;

namespace Xml2CSharp
{
	[XmlRoot(ElementName = "os")]
	public class Os
	{
		[XmlAttribute(AttributeName = "Index")]
		public int Index { get; set; }

		[XmlAttribute(AttributeName = "Default")]
		public bool Default { get; set; }

		[XmlAttribute(AttributeName = "Name")]
		public string Name { get; set; }

		[XmlText]
		public string File { get; set; }
	}

	[XmlRoot(ElementName = "Brand")]
	public class Brand
	{
		[XmlElement(ElementName = "os")]
		public List<Os> Os { get; set; }
		[XmlAttribute(AttributeName = "Name")]
		public string Name { get; set; }
		[XmlAttribute(AttributeName = "Default")]
		public bool Default { get; set; }
	}

	[XmlRoot(ElementName = "Brands")]
	public class Brands
	{
		[XmlElement(ElementName = "Brand")]
		public List<Brand> Brand { get; set; }

		[XmlIgnore]
		private static Brands _data;

		public static Brands GetData()
		{
			if (_data != null)
				return _data;
			XmlSerializer serializer = new XmlSerializer(typeof(Brands));

			using (System.IO.Stream reader = new System.IO.FileStream(Settings.instance.OsRoot + "Data.xml", System.IO.FileMode.Open))
			{
				_data = (Brands)serializer.Deserialize(reader);
			}
			return _data;
		}
	

	}

}