using System;
using System.Collections;

namespace FrontDesk.Components.Evaluation {

	/// <summary>
	/// Auto job test
	/// </summary>
	public class AutoJobTest : DataComponent {

		public AutoJobTest() { }

		public const String JOBID_FIELD = "jobID";
		public const String EVALID_FIELD = "evalID";
		public const String SUBID_FIELD = "subID";
		public const String TESTERIP_FIELD = "testerIP";
		public const String TESTERDESC_FIELD = "testerDesc";
		public const String STATUS_FIELD = "status";
		public const String NAME_FIELD = "jobname";
		public const String CREATOR_FIELD = "jobcreator";
		public const String ONSUBMIT_FIELD = "onsubmit";

		public const int QUEUED=0, INPROGRESS=1, DONE=2;

		private bool m_onsubmit;
		private int m_jobid, m_evalid, m_subid, m_status;
		private string m_testerip, m_testerdesc, m_name, m_creator;
		[JoinedEntity] public AutoEvaluation m_eval = new AutoEvaluation();

		[FieldName(JOBID_FIELD)]
		public int JobID {
			get { return m_jobid; }
			set { m_jobid = value; }
		}

		[FieldName(EVALID_FIELD)]
		public int EvalID {
			get { return m_evalid; }
			set { m_evalid = value; }
		}

		[FieldName(NAME_FIELD)]
		public string JobName {
			get { return m_name; }
			set { m_name = value; }
		}

		[FieldName(CREATOR_FIELD)]
		public string JobCreator {
			get { return m_creator; }
			set { m_creator = value; }
		}

		[FieldName(SUBID_FIELD)]
		public int SubmissionID {
			get { return m_subid; }
			set { m_subid = value; }
		}

		[FieldName(ONSUBMIT_FIELD)]
		public bool OnSubmit {
			get { return m_onsubmit; }
			set { m_onsubmit = value; }
		}

		public AutoEvaluation AutoEval {
			get { return m_eval; }
			set { m_eval = value; }
		}

		[FieldName(TESTERIP_FIELD)]
		public string TesterIP {
			get { return m_testerip; }
			set { m_testerip = value; }
		}

		[FieldName(TESTERDESC_FIELD)]
		public string TesterDescription {
			get { return m_testerdesc; }
			set { m_testerdesc = value; }
		}

		[FieldName(STATUS_FIELD)]
		public int Status {
			get { return m_status; }
			set { m_status = value; }
		}

		public class AutoJobTestList : ArrayList {
			public AutoJobTestList() { }

			public new AutoJobTest this[int index] {
				get { return (AutoJobTest) base[index]; }
				set { base[index] = value; }
			}
		}
	}
}
