using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DeployReleaseClassLibary
{
    public class DeployRelease
    {        
        public event EventHandler<EventArgsJobProgress> ThrowEventJobProgress = delegate { };
        public event EventHandler<EventArgsJobError> ThrowEventJobError = delegate { };
        private string deployName;
        private string databaseServerName;
        private string databaseName;        
        private String directoryOfEnviromentDatabaseCodeFolder;
        private string macroQuerySting;
        private Dictionary<string, string> macroTable = new Dictionary<string, string>();
        private Dictionary<string, DatabaseObjectToDeploy> allDatabaseObjects = new Dictionary<string, DatabaseObjectToDeploy>();
        private List<DeployPhase> allDeployPhases;
        private DateTime startTime;
        private DateTime endTime;
        private int largestNumberThreadsCount = 0;
        private string databaseForPhase;
        private EventArgsJobProgress eventAgrsJobProgress;
        private EventArgsJobError eventAgrsJobError;

        public DeployRelease()
        {
            allDeployPhases = new List<DeployPhase>();
            eventAgrsJobProgress = new EventArgsJobProgress();
            eventAgrsJobError = new EventArgsJobError();
        }

        public string DeployName
        {
            get
            {
                return deployName;
            }

            set
            {
                deployName = value;
            }
        }

        public void DoDeployment()
        {
            startTime = DateTime.Now;            
            List<Task> taskList;
            taskList = new List<Task>();

            
            foreach(DeployPhase deployPhase in AllDeployPhases)
            {
                deployPhase.IsStarted = true;
                deployPhase.DeployRelease = this;

                eventAgrsJobProgress.JobOutputMessage = String.Format("{0}", deployPhase.PhaseName);
                ThrowEventJobProgress(this, eventAgrsJobProgress);

                if (deployPhase is DeployPhaseExecuteNoneQuery)
                {
                    Task.WaitAll(taskList.ToArray());
                    this.DatabaseForPhase = DatabaseName;
                    DeployPhaseExecuteNoneQuery deployPhaseNoneQuery = (DeployPhaseExecuteNoneQuery)deployPhase;
                    Job jobExecNonQuery = new JobExecuteNoneQuery(this, deployPhaseNoneQuery.NoneQueryCommand, deployPhase.LogErrorsInThisPhase);

                    jobExecNonQuery.ThrowEventJobProgress += (sender, args) => { PocoUploaded(sender, args); };
                    jobExecNonQuery.ThrowEventJobError += (sender, args) => { PocoUploadError(sender, args); };

                    jobExecNonQuery.Execute();
                    //job.DatabaseObjectToDeploy
                }
                else if (deployPhase is DeployPhasePocosFromDirectory)
                {
                    DeployPhasePocosFromDirectory deployPhasePocos = (DeployPhasePocosFromDirectory)deployPhase;

                    if (deployPhasePocos.DatabaseForPhase != null)
                    {
                        if (DatabaseForPhase != deployPhasePocos.DatabaseForPhase)
                        {
                            Task.WaitAll(taskList.ToArray());
                            DatabaseForPhase = deployPhasePocos.DatabaseForPhase;
                        }
                    }
                    else
                    {
                        if (DatabaseForPhase != this.databaseName)
                        {
                            Task.WaitAll(taskList.ToArray());
                            DatabaseForPhase = this.databaseName;
                        }
                    }

                    foreach (DeployPhasePocosFromDirectorySequences phaseSequences in deployPhasePocos.SequencesInPhase)
                    {
                        phaseSequences.IsStarted = true;
                        phaseSequences.DeployRelease = this;

                        eventAgrsJobProgress.JobOutputMessage = String.Format("{0}", phaseSequences.SequenceName);
                        ThrowEventJobProgress(this, eventAgrsJobProgress);

                        phaseSequences.PrepareDeployPhasePocosFromDirectorySequences();

                        // Load Macros from database (if not done already)
                        if (!AreMacrosLoaded)
                        {                            
                            Job jobRetriveMacros = new JobRretrieveMacrosFromDatabase(this, null);
                            taskList.Add(Task.Factory.StartNew((Object obj) => { jobRetriveMacros.Execute(); }, TaskCreationOptions.LongRunning));
                            Task.WaitAll(taskList.ToArray());
                        }

                        // Load in text from all files (if not done already)
                        foreach (DatabaseObjectToDeploy dbPoco in phaseSequences.DatabaseOjbectsToDeploy)
                        {
                            if (!dbPoco.HaveTriedToLoadFile)
                            {
                                Job jobLoadFile = new JobLoadFile(this, dbPoco);
                                //taskList.Add(Task.Run(() => jobLoadFile.Execute()));
                                taskList.Add(Task.Factory.StartNew((Object obj) => { jobLoadFile.Execute(); }, TaskCreationOptions.LongRunning));
                            }
                        //}
                        //Task.WaitAll(taskList.ToArray());
                        if (largestNumberThreadsCount < Process.GetCurrentProcess().Threads.Count) { largestNumberThreadsCount = Process.GetCurrentProcess().Threads.Count; }

                        // Translate Macros in all Pocos (if not done already)
                        //taskList = new List<Task>();
                        //foreach (DatabaseObjectToDeploy dbPoco in phaseSequences.DatabaseOjbectsToDeploy)
                        //{
                            if (!dbPoco.IsMacroTranslationCompleted)
                            {
                                Job jobTranslateMacros = new JobTranslateMacros(this, dbPoco);
                                taskList.Add(Task.Run(() => jobTranslateMacros.Execute()));
                            }
                        //}
                        //Task.WaitAll(taskList.ToArray());
                        if (largestNumberThreadsCount < Process.GetCurrentProcess().Threads.Count) { largestNumberThreadsCount = Process.GetCurrentProcess().Threads.Count; }

                        // Load in all Pocos into the database
                        //taskList = new List<Task>();
                        //foreach (DatabaseObjectToDeploy dbPoco in phaseSequences.DatabaseOjbectsToDeploy)
                        //{
                            dbPoco.DatabasePermissionClause = phaseSequences.AdditionalPermissions;
                            JobUploadPOCO jobUploadPOCO = new JobUploadPOCO(this, dbPoco, deployPhase.LogErrorsInThisPhase);

                            jobUploadPOCO.ThrowEventJobProgress += (sender, args) => { PocoUploaded(sender, args); };
                            jobUploadPOCO.ThrowEventJobError += (sender, args) => { PocoUploadError(sender, args); };
                            //taskList.Add(Task.Run(() => jobExecNonQuery.Execute()));
                            taskList.Add(Task.Factory.StartNew((Object obj) => { jobUploadPOCO.Execute(); }, TaskCreationOptions.LongRunning));
                        }
                        //if (largestNumberThreadsCount < Process.GetCurrentProcess().Threads.Count) { largestNumberThreadsCount = Process.GetCurrentProcess().Threads.Count; }
                        //Task.WaitAll(taskList.ToArray());

                        phaseSequences.IsFinished = true;
                    }
                }

                Task.WaitAll(taskList.ToArray());
                deployPhase.IsFinishedd = true;
            }

            Task.WaitAll(taskList.ToArray());
            endTime = DateTime.Now;

            //            int filseWithErrors = allDatabaseObjects.Count(s => s.Value.Errors.Max().Count() > 0);
            //            int sqlPitintOutput = allDatabaseObjects.Count(s => s.Value.SqlPrintOutput.Max().Count() > 0);
            //            int fileWithDeadlocks = allDatabaseObjects.Count(s => s.Value.NumberDeadlocks > 0);

        }

        public void ThrowErrors()
        {
            eventAgrsJobError.JobReference = "Beginning error dump...";
            ThrowEventJobError(this, eventAgrsJobError);

            foreach (KeyValuePair<String, DatabaseObjectToDeploy> databaseObjectToDeploy in allDatabaseObjects)
            {
                foreach (List<String> errors in databaseObjectToDeploy.Value.Errors)
                {
                    foreach (String error in errors)
                    {
                        eventAgrsJobError.JobReference = databaseObjectToDeploy.Key;
                        eventAgrsJobError.JobError = error;
                        ThrowEventJobError(this, eventAgrsJobError);
                    }
                }
                
            }
        }

        private void PocoUploaded(object sender, EventArgsJobProgress e)
        {
            ThrowEventJobProgress(this, e);
        }

        private void PocoUploadError(object sender, EventArgsJobError e)
        {
            ThrowEventJobError(this, e);
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
                builder["Initial Catalog"] = databaseForPhase;
                return builder.ConnectionString; 
            }
        }

        /// <summary>
        /// Gets the database name for the main database (containing reftxt)
        /// </summary>
        public string GetMainDatabaseConnectionString
        {
            get
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder["Data Source"] = databaseServerName;
                builder["integrated Security"] = true;
                builder["Initial Catalog"] = DatabaseName;
                return builder.ConnectionString;
            }
        }

        [XmlIgnoreAttribute]
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

        public bool AreMacrosLoaded
        {
            get
            {
                return macroTable.Count > 0;
            }
        }

        [XmlIgnoreAttribute]
        public Dictionary<String, DatabaseObjectToDeploy> AllDatabaseObjects
        {
            get { return allDatabaseObjects; }
            set { allDatabaseObjects = value; }
        }

        public string DirectoryOfEnviromentDatabaseCodeFolder
        {
            get
            {
                return directoryOfEnviromentDatabaseCodeFolder;
            }

            set
            {
                directoryOfEnviromentDatabaseCodeFolder = value;
            }
        }

        public List<DeployPhase> AllDeployPhases
        {
            get
            {
                return allDeployPhases;
            }

            set
            {
                allDeployPhases = value;
            }
        }

        public DateTime StartTime
        {
            get
            {
                return startTime;
            }

            set
            {
                startTime = value;
            }
        }

        public DateTime EndTime
        {
            get
            {
                return endTime;
            }

            set
            {
                endTime = value;
            }
        }

        public int LargestNumberThreadsCount
        {
            get
            {
                return largestNumberThreadsCount;
            }

            set
            {
                largestNumberThreadsCount = value;
            }
        }

        public string DatabaseForPhase
        {
            get
            {
                return databaseForPhase;
            }

            set
            {
                var changedDb = false;
                if (databaseForPhase == null || databaseForPhase != value)
                {
                    changedDb = true;
                }
                databaseForPhase = value;

                if (changedDb)
                {
                    eventAgrsJobProgress.JobOutputMessage = String.Format("Server: [{0}], Database: [{1}]", databaseServerName, databaseForPhase);
                    ThrowEventJobProgress(this, eventAgrsJobProgress);
                }
            }
        }

        public TimeSpan GetTotalTime()
        {
            return endTime - startTime;
        }

        public void addDeployPahse(DeployPhase deployPhase)
        {
            AllDeployPhases.Add(deployPhase);
        }

    }
}
