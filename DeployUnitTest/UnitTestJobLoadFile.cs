using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TosDeployReleaseClassLibary;
using System.IO;

namespace TosDeployUnitTest
{
    [TestClass]
    public class UnitTestJobLoadFile
    {
        [TestMethod]
        public void Test_TosDeployRelease_DoJob_LoadFileWhichExists()
        {
            var fileText = "aStringToWriteToAFile";
            var aFilePath = "c:\\temp\\Test_TosDeployRelease_DoJob_LoadFileWhichExists.txt";            
            var tosDatabaseObjectToDeploy = new DatabaseObjectToDeploy(aFilePath);

            var jobLoadFile = new JobLoadFile(null, tosDatabaseObjectToDeploy);

            using (StreamWriter file = new StreamWriter(aFilePath))
            {
                file.Write(fileText);
                file.Close();
            }

            jobLoadFile.Execute();

            Assert.AreEqual(fileText, tosDatabaseObjectToDeploy.ObjectText);
        }

        [TestMethod]
        public void Test_TosDeployRelease_DoJob_LoadFileIsCompleted()
        {
            var fileText = "aStringToWriteToAFile";
            var aFilePath = "c:\\temp\\Test_TosDeployRelease_DoJob_LoadFileWhichExists.txt";
            var tosDatabaseObjectToDeploy = new DatabaseObjectToDeploy(aFilePath);
            var jobLoadFile = new JobLoadFile(null, tosDatabaseObjectToDeploy);

            using (StreamWriter file = new StreamWriter(aFilePath))
            {
                file.Write(fileText);
                file.Close();
            }

            jobLoadFile.Execute();

            Assert.AreEqual(true, tosDatabaseObjectToDeploy.IsFileLoaded);
        }
    }
}
