using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TosDeployReleaseClassLibary
{
    public class JobTranslateMacros : DeployJob
    {
        public JobTranslateMacros(DeployRelease deployRelease, DatabaseObjectToDeploy dbObjectToDeploy)
            : base(deployRelease, dbObjectToDeploy)
        { }

        public JobTranslateMacros()
        { }

        public override void JobExecute()
        {
            // First check if we need to wait for a previous task to load this file into memory
            while(!tosDatabaseObjectToDeploy.HaveTriedToLoadFile)
            {
                Thread.Sleep(10);
            }

            foreach (KeyValuePair<string, string> entry in tosDeployReleaseObject.MacroTable)
            {
                tosDatabaseObjectToDeploy.ObjectText = tosDatabaseObjectToDeploy.ObjectText.Replace(entry.Key, entry.Value);
            }

            tosDatabaseObjectToDeploy.IsMacroTranslationCompleted = true;
        }
    }
}