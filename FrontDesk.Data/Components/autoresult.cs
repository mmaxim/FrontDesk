using System;
using System.Data;
using System.IO;
using System.Xml;
using System.Collections;

namespace FrontDesk.Components.Evaluation {

	/// <summary>
	/// Auto Result
	/// </summary>
	public class AutoResult : Result {
		
		public AutoResult() { }

		public const String RESULT_FIELD = "result";
		public const String EVALID_FIELD = "evalID";
		public const String SUCCESS_FIELD = "success";
		public const String COMPETE_FIELD = "compete";

		//Success codes
		public const int CRITICALLYFLAWED=0, FLAWED=1, FLAWLESS=2, DEPFAIL=3;

		protected string m_result;
		protected int m_success, m_evalid;
		protected double m_compete;

		[FieldName(EVALID_FIELD)]
		public int EvalID {
			get { return m_evalid; }
			set { m_evalid = value; }
		}

		[FieldName(SUCCESS_FIELD)]
		public int Success {
			get { return m_success; }
			set { m_success = value; }
		}

		[FieldName(COMPETE_FIELD)]
		public double CompetitionScore {
			get { return m_compete; }
			set { m_compete = value; }
		}

		[FieldName(RESULT_FIELD)]
		public string XmlResult {
			get { return m_result; }
			set { ProcessXmlResult(value); m_result = value; }
		}

		protected void ProcessXmlResult(string xmlresult) {
			
			DataSet dsres = new DataSet();
			double points=0.0;

			//Read Xml
			try {
				MemoryStream memstream = 
					new MemoryStream(System.Text.Encoding.ASCII.GetBytes(xmlresult));
				dsres.ReadXml(memstream);
			} catch (Exception) { return; }
			
			DataTable restbl = dsres.Tables["Result"];
			DataTable errortbl = dsres.Tables["Error"];
			DataTable failtbl = dsres.Tables["Failure"];

			//Add up error points
			if (errortbl != null)
				foreach (DataRow row in errortbl.Rows) 
					points += Convert.ToDouble(row["points"]);
			
			//Add up failure points
			if (failtbl != null)
				foreach (DataRow row in failtbl.Rows)
					points += Convert.ToDouble(row["points"]);

			//Set fields
			if (restbl.Columns.Contains("Comp"))
				CompetitionScore = Convert.ToDouble(restbl.Rows[0]["Comp"]);
			else
				CompetitionScore = 0;
			Points = -points;
			string sucstr = (string) restbl.Rows[0]["Success"];
			if (sucstr == "criticallyflawed")
				Success = CRITICALLYFLAWED;
			else if (sucstr == "flawed")
				Success = FLAWED;
			else if (sucstr == "depfail")
				Success = DEPFAIL;
			else if (sucstr == "flawless")
				Success = FLAWLESS;
			else
				Success = CRITICALLYFLAWED;
		}

		public class AutoResultList : ArrayList {

			public AutoResultList() { }

			public new AutoResult this[int index] {
				get { return (AutoResult) base[index]; }
				set { base[index] = value; }
			}
		}
	}
}
