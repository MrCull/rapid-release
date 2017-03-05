using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TosDeployReleaseClassLibary;

namespace TosDeployUnitTest
{
    [TestClass]
    public class UnitTestJobRetriveMacrosFromDatabase
    {
        [TestMethod]
        public void Test_TosDeployRelease_DoJob_RetriveMacrosFromDatabase()
        {            
            var tosDatabaseObjectToDeploy = new DatabaseObjectToDeploy();
            var jobRretrieveMacrosFromDatabase = new JobRretrieveMacrosFromDatabase();

            jobRretrieveMacrosFromDatabase.TosDeployRelease = new DeployRelease();

            jobRretrieveMacrosFromDatabase.TosDatabaseObjectToDeploy = tosDatabaseObjectToDeploy;

            jobRretrieveMacrosFromDatabase.TosDeployRelease.DatabaseServerName = "LENOVO\\SQLEXPRESS";
            jobRretrieveMacrosFromDatabase.TosDeployRelease.DatabaseName = "macroDB";
            jobRretrieveMacrosFromDatabase.TosDeployRelease.MacroQueryString = "select rtrim([key]) as [key], rtrim([value]) as [value] from macros";

            jobRretrieveMacrosFromDatabase.Execute();

            Assert.AreEqual(true, jobRretrieveMacrosFromDatabase.TosDeployRelease.MacroTable.Count > 0);
        }
    }
}
