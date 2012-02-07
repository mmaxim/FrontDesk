using System;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.IO;
using System.Collections;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Diagnostics;

namespace FrontDesk.Common {

	/// <summary>
	/// XML helper stuff
	/// </summary>
	public class XmlWizard {

		public XmlWizard() { }

		public class XSDTest {

			public XSDTest() { }

			public string Name;
			public double Points;
			public int Time;
		}

		private bool m_valid;
		private string m_lastvaliderror="";

		public XPathNavigator GetXPathNavigator(string xml) {
			MemoryStream xmlstr = new MemoryStream(Encoding.ASCII.GetBytes(xml));
			XPathDocument xpathdoc = new XPathDocument(xmlstr);
			XPathNavigator nav = xpathdoc.CreateNavigator();

			return nav;
		}

		public void DisplayDivXslt(string xml, string xslpath, HtmlGenericControl divXml) {
			
			XslTransform xslt = new XslTransform();		
			XPathDocument xsldoc = new XPathDocument(xslpath);
			MemoryStream xmlstr = new MemoryStream(Encoding.ASCII.GetBytes(xml));
			XPathDocument xpathdoc = new XPathDocument(xmlstr);
	
			StringBuilder strb = new StringBuilder();
			xslt.Load(xsldoc, null, null);
			xslt.Transform(xpathdoc, null, new XmlTextWriter(new StringWriter(strb)) , (XmlResolver)null);
		
			divXml.InnerHtml = strb.ToString();
		}

		public ArrayList GetXSDTests(string xml) {
			
			DataSet dsres = new DataSet();
			ArrayList tests = new ArrayList();

			//Read Xml
			try {
				MemoryStream memstream = 
					new MemoryStream(Encoding.ASCII.GetBytes(xml));
				dsres.ReadXml(memstream);
			} catch (Exception) { return tests; }
			
			DataTable suitetbl = dsres.Tables["TestSuite"];
			DataTable testtbl = dsres.Tables["Test"];

			//Check for error
			if (suitetbl != null && suitetbl.Rows[0]["Error"] != null) 
				return tests;

			//Read Tests
			if (testtbl != null)
				foreach (DataRow row in testtbl.Rows) { 
					XSDTest test = new XSDTest();
					test.Name = (string) row["Name"];
					test.Points = Convert.ToDouble(row["Points"]);
					test.Time = Convert.ToInt32(row["Time"]);
					tests.Add(test);
				}

			return tests;
		}

		public string GetLastError() {
			return m_lastvaliderror;
		}

		public bool ValidateXml(string xml, string xsdpath) {
			
			if (xml.Length == 0) 
				return false;

			MemoryStream resstr = 
				new MemoryStream(Encoding.ASCII.GetBytes(xml));
			
			XmlReader reader = new XmlTextReader(resstr);
			XmlValidatingReader vr = new XmlValidatingReader(reader);

			vr.Schemas.Add("urn:frontdesk-result", xsdpath);
			vr.ValidationEventHandler += new ValidationEventHandler(ValidationHandler);
		
			m_valid=true;
			try {
				while (vr.Read()) { 
					if (m_valid == false) break;
				}
			} catch (Exception er) {
				m_lastvaliderror = er.Message;
				m_valid = false;	
			}
			
			return m_valid;
		}

		private void ValidationHandler(object sender, ValidationEventArgs args) {
			m_valid=false;
			m_lastvaliderror = args.Message;
		}
	}
}
