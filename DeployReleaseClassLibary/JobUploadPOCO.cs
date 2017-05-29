using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Threading;

namespace DeployReleaseClassLibary
{
    public class JobUploadPOCO : Job
    {
        public override event EventHandler<EventArgsJobProgress> ThrowEventJobProgress = delegate { };
        public override event EventHandler<EventArgsJobError> ThrowEventJobError = delegate { };

        private List<String> sqlPrintOutput;
        private bool logErrorsInThisPhase;

        public JobUploadPOCO(DeployRelease deployRelease, DatabaseObjectToDeploy dbObjectToDeploy, bool logErrorsInThisPhase)
            : base(deployRelease, dbObjectToDeploy)
        {
            this.logErrorsInThisPhase = logErrorsInThisPhase;
        }

        public override void JobExecute()
        {
            var errors = new List<String>();
            sqlPrintOutput = new List<String>();

            using (SqlConnection con = new SqlConnection(DeployReleaseObject.GetDatabaseConnectionString))
            {

                // First check if we need to wait for a previous task translate macros in this string
                while (!databaseObjectToDeploy.IsMacroTranslationCompleted)
                {
                    Thread.Sleep(100);
                }

                con.InfoMessage += new SqlInfoMessageEventHandler(myConnection_InfoMessage); // This is needed to get back the results of the "print"
                con.Open();

                //string permissions = databaseObjectToDeploy.DatabasePermissionClause + " " + databaseObjectToDeploy.ObjectName;
                //string pocoPlusPermissions = databaseObjectToDeploy.ObjectText + " " + permissions;

                var pocoFileText = databaseObjectToDeploy.ObjectText;

                // If the file end wi a "GO" then remote it as we dont need it
                if (pocoFileText.EndsWith("\r\nGO"))
                {
                    pocoFileText = pocoFileText.Remove(pocoFileText.Length - 4);
                }
                if (pocoFileText.EndsWith("\nGO"))
                {
                    pocoFileText = pocoFileText.Remove(pocoFileText.Length - 3);
                }

                databaseObjectToDeploy.ObjectText = pocoFileText.Replace("\nGO\n", "\r\nGO\r\n"); // If any lines dont have correct new line chars then fix now
                databaseObjectToDeploy.ObjectText = pocoFileText.Replace("\r\nGO ", "\r\nGO"); // If any GOs have a slank space after them then get rid now
                
                //split the script on "GO" commands as these cannot be executed in one batch
                string splitter = "\r\nGO\r\n";
                string[] commandTexts = Regex.Split(pocoFileText, splitter, RegexOptions.IgnoreCase);

                foreach (string commandText in commandTexts)
                {
                    if (commandText.Trim() != String.Empty)
                    {
                        using (SqlCommand cmd = new SqlCommand(commandText, con))
                        {
                            var retryCount = 0;
                            var escapeFlag = false;
                            databaseObjectToDeploy.NumberDeadlocks = 0;

                            while (retryCount < 10 && !escapeFlag) // 10 deadlock/retries
                            {
                                try
                                {
                                    escapeFlag = true;
                                    cmd.ExecuteNonQuery();
                                }
                                catch (System.Data.SqlClient.SqlException ex)
                                {
                                    if (ex.Number == 1205) // Deadlock
                                    {
                                        databaseObjectToDeploy.NumberDeadlocks++;
                                        escapeFlag = false;
                                        retryCount++;
                                        Thread.Sleep(10 * retryCount); // sleep 100th of a second mutipled by number of times retried.
                                    }
                                    else if (
                                               ex.Message.Contains("The referenced entity") // dirty hack incase a dependent object is also uploaded at the same time
                                             ||
                                               ex.Message.Contains("Invalid object name") // dirty hack incase a dependent object has been dropped and not yet re-uploaded.
                                             ||
                                               ex.Message.Contains("Cannot drop the") // dirty hack incase this object has been dropped (how/why?!) and not yet re-uploaded.
                                             ||
                                               ex.Message.Contains("Cannot find either") // dirty hack incase dependent object has been dropped and not yet re-uploaded.
                                            )
                                    {
                                        databaseObjectToDeploy.NumberReferencedEntityErrors++;
                                        escapeFlag = false;
                                        retryCount++;
                                        Thread.Sleep(10 * retryCount); // sleep 100th of a second mutipled by number of times retried.
                                    }
                                    //else if (ex.Message.Contains("Incorrect syntax"))
                                    //{
                                    //    var bad = "bad";

                                    //}
                                    else
                                    {
                                        if (logErrorsInThisPhase)
                                        {
                                            errors.Add(ex.Message);

                                            var eventAgrsError = new EventArgsJobError();
                                            eventAgrsError.JobError = ex.Message;
                                            eventAgrsError.JobReference = databaseObjectToDeploy.ObjectName;
                                            ThrowEventJobError(this, eventAgrsError);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            databaseObjectToDeploy.Errors.Add(errors);
            databaseObjectToDeploy.SqlPrintOutput.Add(sqlPrintOutput);
            databaseObjectToDeploy.NumberOfTimesLoaded++;



        } // JobExecute

        void myConnection_InfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            sqlPrintOutput.Add(e.Message);

            var eventAgrsJobProgress = new EventArgsJobProgress();
            eventAgrsJobProgress.JobReference = databaseObjectToDeploy.ObjectName;
            eventAgrsJobProgress.JobOutputMessage = e.Message;
            ThrowEventJobProgress(this, eventAgrsJobProgress);
        }
    }
}