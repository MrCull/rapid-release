using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DeployReleaseClassLibary
{
    public class JobRretrieveMacrosFromDatabase : Job
    {
        public override event EventHandler<EventArgsJobProgress> ThrowEventJobProgress = delegate { };
        public override event EventHandler<EventArgsJobError> ThrowEventJobError = delegate { };

        public JobRretrieveMacrosFromDatabase(DeployRelease deployRelease, DatabaseObjectToDeploy dbObjectToDeploy)
            : base(deployRelease, dbObjectToDeploy)
        { }

        public JobRretrieveMacrosFromDatabase()
        { }

        public override void JobExecute()
        {
            using (SqlConnection con = new SqlConnection(DeployReleaseObject.GetMainDatabaseConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(DeployReleaseObject.MacroQueryString, con))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DeployReleaseObject.MacroTable[(string)reader["Key"]] = (string)reader["Value"];
                        }
                    }
                }
            }
        }
    }
}