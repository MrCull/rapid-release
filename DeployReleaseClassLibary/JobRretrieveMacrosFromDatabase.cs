using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace TosDeployReleaseClassLibary
{
    public class JobRretrieveMacrosFromDatabase : DeployJob
    {
        public JobRretrieveMacrosFromDatabase(DeployRelease deployRelease, DatabaseObjectToDeploy dbObjectToDeploy)
            : base(deployRelease, dbObjectToDeploy)
        { }

        public JobRretrieveMacrosFromDatabase()
        { }

        public override void JobExecute()
        {
            using (SqlConnection con = new SqlConnection(tosDeployReleaseObject.GetDatabaseConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(tosDeployReleaseObject.MacroQueryString, con))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tosDeployReleaseObject.MacroTable[(string)reader["Key"]] = (string)reader["Value"];
                        }
                    }
                }
            }
        }
    }
}