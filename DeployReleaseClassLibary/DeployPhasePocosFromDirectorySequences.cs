using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DeployReleaseClassLibary
{
    public class DeployPhasePocosFromDirectorySequences
    {
        private DeployRelease deployRelease;
        private string sequenceName;
        private string subDirectory;
        private string additionalPermissions;        
        private List<DatabaseObjectToDeploy> databaseObjectsToDeploy = new List<DatabaseObjectToDeploy>();
        private bool isStarted = false;
        private bool isFinished = false;
        private string filePrefix;
        private string filePrefixExclude;
        private string filePostfix;
        private bool prepared = false;

        public string SequenceName
        {
            get { return sequenceName; }
            set { sequenceName = value; }
        }

        [XmlIgnoreAttribute]
        public bool IsStarted
        {
            get { return isStarted; }
            set { isStarted = value; }
        }

        [XmlIgnoreAttribute]
        public bool IsFinished
        {
            get { return isFinished; }
            set { isFinished = value; }
        }

        public string SubDirectory
        {
            get { return subDirectory; }
            set { subDirectory = value; }
        }

        public string FilePrefix
        {
            get
            {
                return filePrefix;
            }

            set
            {
                filePrefix = value;
            }
        }

        public string FilePostfix
        {
            get
            {
                return filePostfix;
            }

            set
            {
                filePostfix = value;
            }
        }

        public string FilePrefixExclude
        {
            get
            {
                return filePrefixExclude;
            }

            set
            {
                filePrefixExclude = value;
            }
        }

        public string AdditionalPermissions
        {
            get { return additionalPermissions; }
            set { additionalPermissions = value; }
        }

        [XmlIgnoreAttribute]
        public List<DatabaseObjectToDeploy> DatabaseOjbectsToDeploy
        {
            get { return databaseObjectsToDeploy; }
            set { databaseObjectsToDeploy = value; }
        }

        public DeployRelease DeployRelease
        {
            get
            {
                return deployRelease;
            }

            set
            {
                deployRelease = value;
            }
        }

        public DeployPhasePocosFromDirectorySequences()
        {
        }

        public DeployPhasePocosFromDirectorySequences(String sequenceName, DeployRelease deployRealese, string deployDirectorySearchPath, string includeFilenamesPattern, string excludeFilenamesPattern, string pocoAdditionalPermissions)
        {
            SequenceName = sequenceName;
            DeployRelease = deployRealese;
            SubDirectory = deployDirectorySearchPath;
            FilePrefix = includeFilenamesPattern;
            FilePrefixExclude = excludeFilenamesPattern;
            AdditionalPermissions = pocoAdditionalPermissions;
        }

        public DeployPhasePocosFromDirectorySequences(String sequenceName, string deployDirectorySearchPath, string includeFilenamesPattern, string excludeFilenamesPattern, string pocoAdditionalPermissions)
        {
            SequenceName = sequenceName;
            SubDirectory = deployDirectorySearchPath;
            FilePrefix = includeFilenamesPattern;
            FilePrefixExclude = excludeFilenamesPattern;
            AdditionalPermissions = pocoAdditionalPermissions;
        }

        public void PrepareDeployPhasePocosFromDirectorySequences()
        {
            if (!prepared) // only need to prepare the firectory files once.
            {
                string directoryPath = deployRelease.DirectoryOfEnviromentDatabaseCodeFolder + subDirectory;
                // Process the list of files found in the directory and create an obejct of each in databaseObjToDeploy.
                var fileEntries = Directory.GetFiles(directoryPath, FilePrefix)
                                   .Where(s => s.EndsWith(FilePostfix)); // this is needed to stop things like ".sql~"

                foreach (string fileName in fileEntries)
                {
                    // Check to make sure this file should not be ignored
                    if (FilePrefixExclude == null || fileName.IndexOf(FilePrefixExclude) == -1)
                    {
                        // If this file does not exists in the main dictionary then add it
                        if (!deployRelease.AllDatabaseObjects.ContainsKey(fileName))
                        {
                            DatabaseObjectToDeploy databaseObjToDeploy = new DatabaseObjectToDeploy(fileName);
                            deployRelease.AllDatabaseObjects[fileName] = databaseObjToDeploy;
                        }
                        // Add this file to the collection that will be deployed in this phase
                        databaseObjectsToDeploy.Add(deployRelease.AllDatabaseObjects[fileName]);
                    }
                }

                prepared = true; // these have now been preapred so set this flag so that this will not be done again.
            }
        }
    }
}
