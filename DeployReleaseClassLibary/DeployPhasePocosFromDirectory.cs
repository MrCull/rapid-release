using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TosDeployReleaseClassLibary
{
    public class DeployPhasePocosFromDirectory : DeployPhase
    {
        private List<DeployPhasePocosFromDirectorySequences> sequencesInPhase = new List<DeployPhasePocosFromDirectorySequences>();

        public DeployPhasePocosFromDirectory(String deployPhaseName, bool logErrorsInThisPhase) 
            : base(deployPhaseName, logErrorsInThisPhase)
        {
        }

        public List<DeployPhasePocosFromDirectorySequences> SequencesInPhase
        {
            get { return sequencesInPhase; }
            set { sequencesInPhase = value; }
        }
    }
}