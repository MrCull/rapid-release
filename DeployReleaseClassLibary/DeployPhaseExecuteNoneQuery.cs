using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TosDeployReleaseClassLibary
{
    public class DeployPhaseExecuteNoneQuery : DeployPhase 
    {
        private string phaseName;
        private bool isStarted;
        private bool isFinished;
        private string noneQueryText;

        public DeployPhaseExecuteNoneQuery(String deployPhaseName,  String deployNoneQueryText) 
            : base(deployPhaseName)
        {
            
        }

        public int PhaseName
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }



    }
}