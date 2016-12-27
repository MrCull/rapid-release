using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TosDeployReleaseClassLibary
{
    public class JobTranslateMacros : DeployJob
    {
        private int macroTable;
        private int tosBuildDatabaseObject;

        public override void Execute()
        {
            foreach (KeyValuePair<string, string> entry in tosDeployReleaseObject.MacroTable)
            {
                tosDatabaseObjectToDeploy.ObjectText = tosDatabaseObjectToDeploy.ObjectText.Replace(entry.Key, entry.Value);
            }

            tosDatabaseObjectToDeploy.IsMacroTranslationCompleted = true;
        }
    }
}