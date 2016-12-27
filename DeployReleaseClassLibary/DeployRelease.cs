using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TosDeployReleaseClassLibary
{
    public class DeployRelease
    {
        private int workerResourceLockObject;
        private int deployPhase;        
        private string databaseName;
        private string databaseServerName;
        private int maximumWorkerSleepTime;
        private Dictionary<string, string> macroTable;

        //private List<IJob> workQueueFileToLoad;
        //private List<IJob> workQueueFileToTranslatMacros;
        //private List<IJob> workQueueDatabaseObjectsToUpload;
        //private List<IJob> workQueueNoneQueryObjectsToExecute;
        private int numberOfWorkers;

        public DeployRelease()
        {
            macroTable = new Dictionary<string, string>();
        }

        public string DatabaseName
        {
            get
            {
                return databaseName;
            }

            set
            {
                databaseName = value;
            }
        }

        public string DatabaseServerName
        {
            get
            {
                return databaseServerName;
            }

            set
            {
                databaseServerName = value;
            }
        }

        public string GetDatabaseConnectionString
        {
            get
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder["Data Source"] = databaseServerName;
                builder["integrated Security"] = true;
                builder["Initial Catalog"] = databaseName;
                return builder.ConnectionString; 
            }

        }

        public Dictionary<string, string> MacroTable
        {
            get
            {
                return macroTable;
            }

            set
            {
                macroTable = value;
            }
        }

        public void SratrDeployment(List<DeployPhase> tosDeployPhase)
        {
            // For each step in this deployment loop through and create the work
            foreach (DeployPhase tosDeployTask in tosDeployPhase)
            {
                
                if (tosDeployTask.GetType() == typeof(DeployPhasePocosFromDirectory))
                {
                    //Find all files in directory using the 
                    //TosDeployPhasePocosFromDirectory.

                }
                else if (tosDeployTask.GetType() == typeof(DeployPhaseExecuteNoneQuery))
                {

                }
                else
                {
                    throw new System.NotImplementedException();
                }
            }
        }

        public void AddJob(JobLoadFile IJob)
        {
            throw new System.NotImplementedException();
        }

        public void GetJob()
        {
            throw new System.NotImplementedException();
        }

        public void AreJobsRuning()
        {
            throw new System.NotImplementedException();
        }

        public void EnableWorkers()
        {
            throw new System.NotImplementedException();
        }

        public void DisableWorkers()
        {
            throw new System.NotImplementedException();
        }

        public void CreateBuildPhases()
        {
            throw new System.NotImplementedException();
        }

        public void JobCompleted()
        {
            throw new System.NotImplementedException();
        }

        public void InitiateWorkers()
        {
            throw new System.NotImplementedException();
        }

        public void LoadMacros()
        {
            throw new System.NotImplementedException();
        }

        public void StartLoadingFiles()
        {
            throw new System.NotImplementedException();
        }

        public void StartTranslatingMacros()
        {
            throw new System.NotImplementedException();
        }

        public void UploadDatabaseObjectsForPhase()
        {
            throw new System.NotImplementedException();
        }

        public void CoordinatorWorker()
        {
            throw new System.NotImplementedException();
        }

        public void JobWorker()
        {
            throw new System.NotImplementedException();
        }

        public void StartDeployPhases()
        {
            throw new System.NotImplementedException();
        }

        public void InitiateDatabaseConnecitons()
        {
            throw new System.NotImplementedException();
        }
    }
}
