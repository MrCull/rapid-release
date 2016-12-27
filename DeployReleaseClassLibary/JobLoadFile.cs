using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TosDeployReleaseClassLibary
{
    public class JobLoadFile : DeployJob
    {
        public override void Execute()
        {
            using (StreamReader file = new StreamReader(tosDatabaseObjectToDeploy.FilePath))
            {
                tosDatabaseObjectToDeploy.ObjectText = file.ReadToEnd();
                file.Close();
                tosDatabaseObjectToDeploy.IsFileLoaded = true;
            }
        }
    }
}