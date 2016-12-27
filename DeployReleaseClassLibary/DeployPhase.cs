using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TosDeployReleaseClassLibary
{
    public abstract class DeployPhase
    {
        private string phaseName;
        private bool isStarted;
        private bool isFinished;

        public DeployPhase(String deployPhaseName)
        {
            isStarted = false;
            isFinished = false;
            phaseName = deployPhaseName;
        }
    }
}