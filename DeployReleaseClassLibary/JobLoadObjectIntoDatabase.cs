using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace TosDeployReleaseClassLibary
{
    public class JobLoadObjectIntoDatabase : DeployJob
    {
        public override void Execute()
        {
            using (SqlConnection con = new SqlConnection(tosDeployReleaseObject.GetDatabaseConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(tosDatabaseObjectToDeploy.ObjectText, con))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}