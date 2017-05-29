using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Threading;

namespace DeployReleaseClassLibary
{
    public class JobExecuteNoneQuery : Job
    {
        public override event EventHandler<EventArgsJobProgress> ThrowEventJobProgress = delegate { };
        public override event EventHandler<EventArgsJobError> ThrowEventJobError = delegate { };

        private List<String> sqlPrintOutput;
        private bool logErrorsInThisPhase;
        private string noneQueryCommand;

        public JobExecuteNoneQuery(DeployRelease deployRelease, string noneQueryCommand, bool logErrorsInThisPhase)
            : base(deployRelease)
        {
            this.logErrorsInThisPhase = logErrorsInThisPhase;
            this.noneQueryCommand = noneQueryCommand;
        }

        public override void JobExecute()
        {
            var errors = new List<String>();
            sqlPrintOutput = new List<String>();

            var eventAgrsJobProgress = new EventArgsJobProgress();
            eventAgrsJobProgress.JobAdditionalInfo = noneQueryCommand;
            eventAgrsJobProgress.JobOutputMessage = "Starting";
            ThrowEventJobProgress(this, eventAgrsJobProgress);

            using (SqlConnection con = new SqlConnection(DeployReleaseObject.GetDatabaseConnectionString))
            {
                con.InfoMessage += new SqlInfoMessageEventHandler(myConnection_InfoMessage); // This is needed to get back the results of the "print"
                con.Open();

                using (SqlCommand cmd = new SqlCommand(noneQueryCommand, con))
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        if (logErrorsInThisPhase)
                        {
                            errors.Add(ex.Message);

                            var eventAgrsError = new EventArgsJobError();
                            eventAgrsError.JobError = ex.Message;
                            eventAgrsError.JobReference = noneQueryCommand;
                            ThrowEventJobError(this, eventAgrsError);
                        }
                    }
                }
            }

            eventAgrsJobProgress.JobAdditionalInfo = noneQueryCommand;
            eventAgrsJobProgress.JobOutputMessage = "Finished";
            ThrowEventJobProgress(this, eventAgrsJobProgress);
        } // JobExecute

        void myConnection_InfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            sqlPrintOutput.Add(e.Message);

            var eventAgrsUploaded = new EventArgsJobProgress();
            eventAgrsUploaded.JobAdditionalInfo = noneQueryCommand;
            eventAgrsUploaded.JobOutputMessage = e.Message;
            ThrowEventJobProgress(this, eventAgrsUploaded);
        }
    }
}