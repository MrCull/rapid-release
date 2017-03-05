using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TosDeployReleaseClassLibary
{
    public abstract class DeployPhase
    {
        private string phaseName;
        private bool isStarted = false;
        private bool isFinished = false;
        private bool logErrorsInThisPhase;
        private List<int> phasesRequiredFor = new List<int>();

        public string PhaseName
        {
            get { return phaseName; }
            set { phaseName = value; }
        }
        public bool IsStarted
        {
            get { return isStarted; }
            set { isStarted = value; }
        }
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
    }
}