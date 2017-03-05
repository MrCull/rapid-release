using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TosDeployReleaseClassLibary
{
    public abstract class DeployJob
    {
        protected DeployRelease tosDeployReleaseObject;
        protected DatabaseObjectToDeploy tosDatabaseObjectToDeploy;
        
        public DeployJob(DeployRelease deployRelease, DatabaseObjectToDeploy dbObjectToDeploy)
        {
            tosDeployReleaseObject = deployRelease;
            tosDatabaseObjectToDeploy = dbObjectToDeploy;
        }

        public DeployJob(DeployRelease deployRelease)
        {
            tosDeployReleaseObject = deployRelease;
        }

        public DeployJob()
        {
        }

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
        public void Execute()
        {
            if (tosDatabaseObjectToDeploy != null)
            {
                tosDatabaseObjectToDeploy.IsBeingWorkedOn = true;
            }

            JobExecute();

            if (tosDatabaseObjectToDeploy != null)
            {
                tosDatabaseObjectToDeploy.IsBeingWorkedOn = false;
            }
        }

        public abstract void JobExecute();
    }

}