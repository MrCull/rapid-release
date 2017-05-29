using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace DeployReleaseClassLibary
{
    public class JobTranslateMacros : Job
    {
        public override event EventHandler<EventArgsJobProgress> ThrowEventJobProgress = delegate { };
        public override event EventHandler<EventArgsJobError> ThrowEventJobError = delegate { };

        public JobTranslateMacros(DeployRelease deployRelease, DatabaseObjectToDeploy dbObjectToDeploy)
            : base(deployRelease, dbObjectToDeploy)
        { }

        public JobTranslateMacros()
        { }

        public override void JobExecute()
        {
            // First check if we need to wait for a previous task to load this file into memory
            while(!databaseObjectToDeploy.HaveTriedToLoadFile)
            {
                Thread.Sleep(10);
            }

            foreach (KeyValuePair<string, string> entry in DeployReleaseObject.MacroTable)
            {
                databaseObjectToDeploy.ObjectText = databaseObjectToDeploy.ObjectText.Replace(entry.Key, entry.Value);
            }

            // Nasty hack to fix when ANSI_WARNINGS has been translated into ANSI_50S by above.
            databaseObjectToDeploy.ObjectText = databaseObjectToDeploy.ObjectText.Replace("ANSI_50S", "ANSI_WARNINGS");

            databaseObjectToDeploy.IsMacroTranslationCompleted = true;
        }
    }
}