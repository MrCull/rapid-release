using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TosDeployReleaseClassLibary
{
    public class DeployPhasePocosFromDirectorySequences
    {
        private string sequenceName;
        private string directorySearchPath;
        private string additionalPermissions;        
        private List<DatabaseObjectToDeploy> databaseObjectsToDeploy = new List<DatabaseObjectToDeploy>();
        private bool isStarted = false;
        private bool isFinished = false;

        public string SequenceName
        {
            get { return sequenceName; }
            set { sequenceName = value; }
        }
        public bool IsStarted
        {
            get { return isStarted; }
            set { isStarted = value; }
        }
        public bool IsFinished
        {
            get { return isFinished; }
            set { isFinished = value; }
        }

        public List<DatabaseObjectToDeploy> DatabaseOjbectsToDeploy
        {
            get { return databaseObjectsToDeploy; }
            set { databaseObjectsToDeploy = value; }
        }

        public string DirectorySearchPath
        {
            get { return directorySearchPath; }
            set { directorySearchPath = value; }
        }

        public string AdditionalPermissions
        {
            get { return additionalPermissions; }
            set { additionalPermissions = value; }
        }

        public DeployPhasePocosFromDirectorySequences(DeployRelease deployRelease, string deployDirectorySearchPath, string includeFilesPattern, string excludeFilesPattern, string pocoAdditionalPermissions)
        {
            additionalPermissions = pocoAdditionalPermissions;

            // Process the list of files found in the directory and create an obejct of each in databaseObjToDeploy.
            string[] fileEntries = Directory.GetFiles(deployDirectorySearchPath, includeFilesPattern);
            foreach (string fileName in fileEntries)
            {

                if (excludeFilesPattern == null || fileName.IndexOf(excludeFilesPattern) == -1)
                {
                    if (!deployRelease.AllDatabaseObjects.ContainsKey(fileName))
                    {
                        DatabaseObjectToDeploy databaseObjToDeploy = new DatabaseObjectToDeploy(fileName);
                        deployRelease.AllDatabaseObjects[fileName] = databaseObjToDeploy;
                    }
                    databaseObjectsToDeploy.Add(deployRelease.AllDatabaseObjects[fileName]);
                }
            }
        }
    }
}
