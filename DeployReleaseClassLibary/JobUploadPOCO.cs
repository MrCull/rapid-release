using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Threading;

namespace TosDeployReleaseClassLibary
{
    public class JobUploadPOCO : DeployJob
    {
        private List<String> sqlPrintOutput;
        private bool logErrorsInThisPhase;

        public JobUploadPOCO(DeployRelease deployRelease, DatabaseObjectToDeploy dbObjectToDeploy, bool logErrorsInThisPhase)
            : base(deployRelease, dbObjectToDeploy)
        {
            this.logErrorsInThisPhase = logErrorsInThisPhase;
        }

      //  public JobUploadPOCO()
      //  { }

        public override void JobExecute()
        {
            var errors = new List<String>();
            sqlPrintOutput = new List<String>();

            using (SqlConnection con = new SqlConnection(tosDeployReleaseObject.GetDatabaseConnectionString))
            {

                // First check if we need to wait for a previous task translate macros in this string
                while (!tosDatabaseObjectToDeploy.IsMacroTranslationCompleted)
                {
                    Thread.Sleep(100);
                }

                //con.InfoMessage += new SqlInfoMessageEventHandler(myConnection_InfoMessage); // This is needed to get back the results of the "print"
                con.Open();

                // If the file end wi a "GO" then remote it as we dont need it
                if (tosDatabaseObjectToDeploy.ObjectText.EndsWith("\r\nGO"))
                {
                    tosDatabaseObjectToDeploy.ObjectText = tosDatabaseObjectToDeploy.ObjectText.Remove(tosDatabaseObjectToDeploy.ObjectText.Length - 4);
                }

                //string permissions = tosDatabaseObjectToDeploy.DatabasePermissionClause + " " + tosDatabaseObjectToDeploy.ObjectName;
                //string pocoPlusPermissions = tosDatabaseObjectToDeploy.ObjectText + " " + permissions;

                //split the script on "GO" commands as these cannot be executed in one batch
                string splitter = "\r\nGO\r\n";
                string[] commandTexts = Regex.Split(tosDatabaseObjectToDeploy.ObjectText, splitter, RegexOptions.IgnoreCase);

                foreach (string commandText in commandTexts)
                {
                    if (commandText.Trim() != String.Empty)
                    {
                        using (SqlCommand cmd = new SqlCommand(commandText, con))
                        {
                            var retryCount = 0;
                            var escapeFlag = false;
                            tosDatabaseObjectToDeploy.NumberDeadlocks = 0;

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
                                        tosDatabaseObjectToDeploy.NumberDeadlocks++;
                                        escapeFlag = false;
                                        retryCount++;
                                        Thread.Sleep(10 * retryCount); // sleep 100th of a second mutipled by number of times retried.
                                    }
                                    else
                                    {
                                        if (logErrorsInThisPhase)
                                        {
                                            errors.Add(ex.Message);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            tosDatabaseObjectToDeploy.Errors.Add(errors);
            tosDatabaseObjectToDeploy.SqlPrintOutput.Add(sqlPrintOutput);
            tosDatabaseObjectToDeploy.NumberOfTimesLoaded++;
        } // JobExecute

        void myConnection_InfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            sqlPrintOutput.Add(e.Message);
        }
    }
}