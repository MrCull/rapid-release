using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TosDeployReleaseClassLibary
{
    public abstract class DeployJob
    {
        protected DatabaseObjectToDeploy tosDatabaseObjectToDeploy;
        protected DeployRelease tosDeployReleaseObject;

        public DatabaseObjectToDeploy TosDatabaseObjectToDeploy
        {
            get
            {
                return tosDatabaseObjectToDeploy;
            }

            set
            {
                tosDatabaseObjectToDeploy = value;
            }
        }

        public DeployRelease TosDeployRelease
        {
            get
            {
                return tosDeployReleaseObject;
            }

            set
            {
                tosDeployReleaseObject = value;
            }
        }
        public abstract void Execute();
    }

}