using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeployReleaseClassLibary
{
    public abstract class Job
    {
        public abstract event EventHandler<EventArgsJobProgress> ThrowEventJobProgress;
        public abstract event EventHandler<EventArgsJobError> ThrowEventJobError;

        protected DeployRelease DeployReleaseObject;
        protected DatabaseObjectToDeploy databaseObjectToDeploy;
        
        public Job(DeployRelease deployRelease, DatabaseObjectToDeploy dbObjectToDeploy)
        {
            DeployReleaseObject = deployRelease;
            databaseObjectToDeploy = dbObjectToDeploy;
        }

        public Job(DeployRelease deployRelease)
        {
            DeployReleaseObject = deployRelease;
        }

        public Job()
        {
        }

        public DatabaseObjectToDeploy DatabaseObjectToDeploy
        {
            get
            {
                return databaseObjectToDeploy;
            }

            set
            {
                databaseObjectToDeploy = value;
            }
        }

        public DeployRelease DeployRelease
        {
            get
            {
                return DeployReleaseObject;
            }

            set
            {
                DeployReleaseObject = value;
            }
        }
        public void Execute()
        {
            if (databaseObjectToDeploy != null)
            {
                databaseObjectToDeploy.IsBeingWorkedOn = true;
            }

            JobExecute();

            if (databaseObjectToDeploy != null)
            {
                databaseObjectToDeploy.IsBeingWorkedOn = false;
            }
        }

        public abstract void JobExecute();
    }

}