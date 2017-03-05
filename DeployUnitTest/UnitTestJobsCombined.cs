using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TosDeployReleaseClassLibary;
using System.IO;

namespace TosDeployUnitTest
{
    [TestClass]
    public class UnitTestJobsCombined
    {
        [TestMethod]
        public void Test_DeployRelease_DoJobs_LoadFile_TranslateMacros_DeployToDb()
        {
            DeployRelease deployRelease = new DeployRelease();
            DatabaseObjectToDeploy tosDatabaseObjectToDeploy = new DatabaseObjectToDeploy();

            deployRelease.DatabaseServerName = "LENOVO\\SQLEXPRESS";
            deployRelease.DatabaseName = "macroDB";

            // prepare some data/files
            var fileText = "if exists (select * from sysobjects where name = 'mac') begin drop procedure mac end \r\ngo\r\n create procedure mac as begin select 333, 'KEY1' end";
            var aFilePath = "c:\\temp\\Test_DeployRelease_DoJobs_LoadFile_TranslateMacros_DeployToDb.txt";
            var jobLoadFile = new JobLoadFile();
            using (StreamWriter file = new StreamWriter(aFilePath))
            {
                file.Write(fileText);
                file.Close();
            }

            // call job to load files
            tosDatabaseObjectToDeploy = new DatabaseObjectToDeploy(aFilePath);
             jobLoadFile.TosDatabaseObjectToDeploy = tosDatabaseObjectToDeploy;
            jobLoadFile.Execute();

            // retrive macros
            JobRretrieveMacrosFromDatabase jobRretrieveMacrosFromDatabase = new JobRretrieveMacrosFromDatabase();
            jobRretrieveMacrosFromDatabase.TosDeployRelease = deployRelease;

            jobRretrieveMacrosFromDatabase.TosDatabaseObjectToDeploy = tosDatabaseObjectToDeploy;

            jobRretrieveMacrosFromDatabase.TosDeployRelease.MacroQueryString = "select rtrim([key]) as [key], rtrim([value]) as [value] from macros";
            jobRretrieveMacrosFromDatabase.Execute();

            // call job to translate macros
            var jobTranslateMacros = new JobTranslateMacros();
            jobTranslateMacros.TosDeployRelease = deployRelease;

            jobTranslateMacros.TosDatabaseObjectToDeploy = tosDatabaseObjectToDeploy;
            jobTranslateMacros.Execute();

            // call job to load into database
            JobUploadPOCO jobLoadObjectIntoDatabase = new JobUploadPOCO(deployRelease, tosDatabaseObjectToDeploy, true);
            jobLoadObjectIntoDatabase.Execute();

            Assert.IsTrue(true); // dummy temp test just for functional validation
        }
    }
}
