using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DeployReleaseClassLibary
{
    [XmlInclude(typeof(DeployPhaseExecuteNoneQuery))]
    [XmlInclude(typeof(DeployPhasePocosFromDirectory))]
    public abstract class DeployPhase
    {
        private string phaseName;
        private bool isStarted = false;
        private bool isFinished = false;
        private bool logErrorsInThisPhase;
        private DeployRelease deployRelease;
        private List<int> phasesRequiredFor = new List<int>();

        public string PhaseName
        {
            get { return phaseName; }
            set { phaseName = value; }
        }

        [XmlIgnoreAttribute]
        public bool IsStarted
        {
            get { return isStarted; }
            set { isStarted = value; }
        }

        [XmlIgnoreAttribute]
        public bool IsFinishedd
        {
            get { return isFinished; }
            set { isFinished = value; }
        }

        public bool LogErrorsInThisPhase
        {
            get { return logErrorsInThisPhase; }
            set { logErrorsInThisPhase = value; }
        }

        public DeployPhase(String phaseName, bool logErrorsInThisPhase)
        {
            isStarted = false;
            isFinished = false;
            this.phaseName = phaseName;
            this.logErrorsInThisPhase = logErrorsInThisPhase;
        }

        [XmlIgnoreAttribute]
        public DeployRelease DeployRelease
        {
            get
            {
                return deployRelease;
            }

            set
            {
                deployRelease = value;
            }
        }

        public DeployPhase()
        {

        }
 
    }
}