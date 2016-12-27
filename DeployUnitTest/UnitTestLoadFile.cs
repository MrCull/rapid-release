using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TosDeployReleaseClassLibary;
using System.IO;

namespace TosDeployUnitTest
{
    [TestClass]
    public class UnitTestLoadFile
    {
        [TestMethod]
        public void Test_TosDeployRelease_DoJob_LoadFileWhichExists()
        {
            var fileText = "aStringToWriteToAFile";
            var aFilePath = "c:\\temp\\Test_TosDeployRelease_DoJob_LoadFileWhichExists.txt";
            var jobLoadFile = new JobLoadFile();
            var tosDatabaseObjectToDeploy = new DatabaseObjectToDeploy(aFilePath);
            jobLoadFile.TosDatabaseObjectToDeploy = tosDatabaseObjectToDeploy;

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
            var jobLoadFile = new JobLoadFile();
            var tosDatabaseObjectToDeploy = new DatabaseObjectToDeploy(aFilePath);
            jobLoadFile.TosDatabaseObjectToDeploy = tosDatabaseObjectToDeploy;

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
