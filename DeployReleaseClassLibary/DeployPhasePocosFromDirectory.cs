using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DeployReleaseClassLibary
{
    public class DeployPhasePocosFromDirectory : DeployPhase
    {
        private List<DeployPhasePocosFromDirectorySequences> sequencesInPhase = new List<DeployPhasePocosFromDirectorySequences>();
        private string databaseForPhase;

        public DeployPhasePocosFromDirectory(String deployPhaseName, bool logErrorsInThisPhase) 
            : base(deployPhaseName, logErrorsInThisPhase)
        {
        }

        public DeployPhasePocosFromDirectory()
        : base()
        {

        }

        public List<DeployPhasePocosFromDirectorySequences> SequencesInPhase
        {
            get { return sequencesInPhase; }
            set { sequencesInPhase = value; }
        }

        public string DatabaseForPhase
        {
            get
            {
                return databaseForPhase;
            }

            set
            {
                databaseForPhase = value;
            }
        }
    }
}