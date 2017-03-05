using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TosDeployReleaseClassLibary
{
    public class DeployRelease
    {
        private int workerResourceLockObject;
        private int maximumWorkerSleepTime;
        private string databaseName;
        private string databaseServerName;
        private string macroQuerySting;
        Dictionary<string, string> macroTable = new Dictionary<string, string>();
        Dictionary<string, DatabaseObjectToDeploy> allDatabaseObjects = new Dictionary<string, DatabaseObjectToDeploy>();
        private List<DeployPhase> allDeployPhases;

        public DeployRelease()
        {
        }

        public void SratrDeployment(List<DeployPhase> givenDeployPhases)
        {
            int highestNumberThreads = 0;
            List<Task> taskList;
            taskList = new List<Task>();

            allDeployPhases = givenDeployPhases;
            foreach(DeployPhase deployPhase in allDeployPhases)
            {
                deployPhase.IsStarted = true;
                if (deployPhase is DeployPhaseExecuteNoneQuery)
                {
                    DeployPhaseExecuteNoneQuery deployPhaseNoneQuery = (DeployPhaseExecuteNoneQuery)deployPhase;
                    DeployJob jobExecNonQuery = new JobExecuteNoneQuery(this, deployPhaseNoneQuery.NoneQueryCommand, deployPhase.LogErrorsInThisPhase);
                    jobExecNonQuery.Execute();
                    //job.TosDatabaseObjectToDeploy
                }
                else if (deployPhase is DeployPhasePocosFromDirectory)
                {
                    DeployPhasePocosFromDirectory deployPhasePocos = (DeployPhasePocosFromDirectory)deployPhase;
                    foreach (DeployPhasePocosFromDirectorySequences phaseSequences in deployPhasePocos.SequencesInPhase)
                    {
                        phaseSequences.IsStarted = true;
                        // Load Macros from database (if not done already)
                        if (!AreMacrosLoaded)
                        {
                            
                            DeployJob jobRetriveMacros = new JobRretrieveMacrosFromDatabase(this, null);
                            taskList.Add(Task.Factory.StartNew((Object obj) => { jobRetriveMacros.Execute(); }, TaskCreationOptions.LongRunning));
                            //Task.WaitAll(taskList.ToArray());
                        }

                        // Load in text from all files (if not done already)
                        foreach (DatabaseObjectToDeploy dbPoco in phaseSequences.DatabaseOjbectsToDeploy)
                        {
                            if (!dbPoco.HaveTriedToLoadFile)
                            {
                                DeployJob jobLoadFile = new JobLoadFile(this, dbPoco);
                                //taskList.Add(Task.Run(() => jobLoadFile.Execute()));
                                taskList.Add(Task.Factory.StartNew((Object obj) => { jobLoadFile.Execute(); }, TaskCreationOptions.LongRunning));
                            }
                        }
                        //Task.WaitAll(taskList.ToArray());
                        if (highestNumberThreads < Process.GetCurrentProcess().Threads.Count) { highestNumberThreads = Process.GetCurrentProcess().Threads.Count; }

                        // Translate Macros in all Pocos (if not done already)
                        //taskList = new List<Task>();
                        foreach (DatabaseObjectToDeploy dbPoco in phaseSequences.DatabaseOjbectsToDeploy)
                        {
                            if (!dbPoco.IsMacroTranslationCompleted)
                            {
                                DeployJob jobTranslateMacros = new JobTranslateMacros(this, dbPoco);
                                taskList.Add(Task.Run(() => jobTranslateMacros.Execute()));
                            }
                        }
                        //Task.WaitAll(taskList.ToArray());
                        if (highestNumberThreads < Process.GetCurrentProcess().Threads.Count) { highestNumberThreads = Process.GetCurrentProcess().Threads.Count; }

                        // Load in all Pocos into the database
                        //taskList = new List<Task>();
                        foreach (DatabaseObjectToDeploy dbPoco in phaseSequences.DatabaseOjbectsToDeploy)
                        {
                            dbPoco.DatabasePermissionClause = phaseSequences.AdditionalPermissions;
                            DeployJob jobExecNonQuery = new JobUploadPOCO(this, dbPoco, deployPhase.LogErrorsInThisPhase);
                            //taskList.Add(Task.Run(() => jobExecNonQuery.Execute()));
                            taskList.Add(Task.Factory.StartNew((Object obj) => { jobExecNonQuery.Execute(); }, TaskCreationOptions.LongRunning));
                        }
                        if (highestNumberThreads < Process.GetCurrentProcess().Threads.Count) { highestNumberThreads = Process.GetCurrentProcess().Threads.Count; }
                        Task.WaitAll(taskList.ToArray());

                        phaseSequences.IsFinished = true;
                    }
                }

                deployPhase.IsFinishedd = true;
            }

            Task.WaitAll(taskList.ToArray());

//            int filseWithErrors = allDatabaseObjects.Count(s => s.Value.Errors.Max().Count() > 0);
//            int sqlPitintOutput = allDatabaseObjects.Count(s => s.Value.SqlPrintOutput.Max().Count() > 0);
//            int fileWithDeadlocks = allDatabaseObjects.Count(s => s.Value.NumberDeadlocks > 0);

        }

        public void startRelease()
        {

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

        public string MacroQueryString
        {
            get
            {
                return macroQuerySting;
            }

            set
            {
                macroQuerySting = value;
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

        public bool AreMacrosLoaded
        {
            get
            {
                return macroTable.Count > 0;
            }
        }

        public Dictionary<String, DatabaseObjectToDeploy> AllDatabaseObjects
        {
            get { return allDatabaseObjects; }
            set { allDatabaseObjects = value; }
        }

    }
}
