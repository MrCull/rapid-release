using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DeployReleaseClassLibary
{
    public class JobLoadFile : Job
    {
        public override event EventHandler<EventArgsJobProgress> ThrowEventJobProgress = delegate { };
        public override event EventHandler<EventArgsJobError> ThrowEventJobError = delegate { };

        public JobLoadFile(DeployRelease deployRelease, DatabaseObjectToDeploy dbObjectToDeploy)
            : base(deployRelease, dbObjectToDeploy)
        { }

        public JobLoadFile()
        { }

        public override void JobExecute()
        {
            using (StreamReader file = new StreamReader(databaseObjectToDeploy.FilePath))
            {
                databaseObjectToDeploy.ObjectText = file.ReadToEnd();
                databaseObjectToDeploy.IsFileLoaded = true;
            }

            databaseObjectToDeploy.HaveTriedToLoadFile = true;
        }
    }
}