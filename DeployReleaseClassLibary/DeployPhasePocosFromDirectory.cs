using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TosDeployReleaseClassLibary
{
    public class DeployPhasePocosFromDirectory : DeployPhase
    {
        private string directorySearchPath;
        private string additionalPermissions;

        public DeployPhasePocosFromDirectory(String deployPhaseName, string deployDirectorySearchPath, string pocoAdditionalPermissions) 
            : base(deployPhaseName)
        {
            directorySearchPath = deployDirectorySearchPath;
            additionalPermissions = pocoAdditionalPermissions;
        }

        public string DirectorySearchPath
        {
            get
            {
                return directorySearchPath;
            }

            set
            {
                directorySearchPath = value;
            }
        }
    }
}