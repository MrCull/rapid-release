using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeployReleaseClassLibary
{
    public class DeployPhaseExecuteNoneQuery : DeployPhase 
    {
        private string noneQueryCommand;

        public string NoneQueryCommand
        {
            get { return noneQueryCommand; }
            set { noneQueryCommand = value;  }
        }


        public DeployPhaseExecuteNoneQuery()
           : base()
        {
        }

        public DeployPhaseExecuteNoneQuery(String deployPhaseName,  String noneQueryCommand, bool logErrorsInThisPhase)
            : base(deployPhaseName, logErrorsInThisPhase)
        {
            this.noneQueryCommand = noneQueryCommand;
        }

    }
}