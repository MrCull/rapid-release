using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TosDeployReleaseClassLibary;

namespace TosDeployUnitTest
{
    [TestClass]
    public class UnitTestDatabaseObjectToDeploy
    {
        [TestMethod]
        public void Test_TosDatabaseObjectToDeploy_InstantiateObjectAndCheckFilePath()
        {
            string filePath = "notImportant";
            var tosDatabaseObjectToDeploy = new DatabaseObjectToDeploy(filePath);

            Assert.AreEqual(filePath, tosDatabaseObjectToDeploy.FilePath);
        }


        [TestMethod]
        public void Test_TosDatabaseObjectToDeploy_InstantiateObjectAndCheckDidFileLoadSucessfully()
        {
            string filePath = "notImportant";
            var tosDatabaseObjectToDeploy = new DatabaseObjectToDeploy(filePath);

            Assert.AreEqual(false, tosDatabaseObjectToDeploy.HaveTriedToLoadFile);
        }


        [TestMethod]
        public void Test_TosDatabaseObjectToDeploy_InstantiateObjectAndCheckIsBeingWorkedOn()
        {
            string filePath = "notImportant";
            var tosDatabaseObjectToDeploy = new DatabaseObjectToDeploy(filePath);

            Assert.AreEqual(false, tosDatabaseObjectToDeploy.IsBeingWorkedOn);
        }

        [TestMethod]
        public void Test_TosDatabaseObjectToDeploy_InstantiateObjectAndCheckIsFileLoaded()
        {
            string filePath = "notImportant";
            var tosDatabaseObjectToDeploy = new DatabaseObjectToDeploy(filePath);

            Assert.AreEqual(false, tosDatabaseObjectToDeploy.IsFileLoaded);
        }

        [TestMethod]
        public void Test_TosDatabaseObjectToDeploy_InstantiateObjectAndCheckIsMacroTranslationCompleted()
        {
            string filePath = "notImportant";
            var tosDatabaseObjectToDeploy = new DatabaseObjectToDeploy(filePath);

            Assert.AreEqual(false, tosDatabaseObjectToDeploy.IsMacroTranslationCompleted);
        }


        [TestMethod]
        public void Test_TosDatabaseObjectToDeploy_InstantiateObjectAndCheckOjectName()
        {
            string filePath = "c:\\temp\\abc.sql";
            var tosDatabaseObjectToDeploy = new DatabaseObjectToDeploy(filePath);

            Assert.AreEqual("abc", tosDatabaseObjectToDeploy.ObjectName);
        }


        [TestMethod]
        public void Test_TosDatabaseObjectToDeploy_SimulateLoadFile()
        {
            string filePath = "c:\\temp\\abc.sql";
            string fileText = "notImportant";
            var tosDatabaseObjectToDeploy = new DatabaseObjectToDeploy(filePath);

            tosDatabaseObjectToDeploy.ObjectText = fileText;

            Assert.AreEqual(fileText, tosDatabaseObjectToDeploy.ObjectText);
        }

        [TestMethod]
        public void Test_TosDatabaseObjectToDeploy_SimulateLoadIsFileLoaded()
        {
            string filePath = "c:\\temp\\abc.sql";
            string fileText = "notImportant";
            var tosDatabaseObjectToDeploy = new DatabaseObjectToDeploy(filePath);

            tosDatabaseObjectToDeploy.ObjectText = fileText;

            Assert.AreEqual(true, tosDatabaseObjectToDeploy.IsFileLoaded);
        }

        [TestMethod]
        public void Test_TosDatabaseObjectToDeploy_SimulateLoadDidFileLoadSuccessfully()
        {
            string filePath = "c:\\temp\\abc.sql";
            string fileText = "notImportant";
            var tosDatabaseObjectToDeploy = new DatabaseObjectToDeploy(filePath);

            tosDatabaseObjectToDeploy.ObjectText = fileText;

            Assert.AreEqual(true, tosDatabaseObjectToDeploy.HaveTriedToLoadFile);
        }

        [TestMethod]
        public void Test_TosDatabaseObjectToDeploy_SimulateLoadFileErrorText()
        {
            string filePath = "c:\\temp\\abc.sql";
            string fileLoadErrorText = "notImportant";
            var tosDatabaseObjectToDeploy = new DatabaseObjectToDeploy(filePath);

            tosDatabaseObjectToDeploy.ErrorsFileLoad = fileLoadErrorText;

            Assert.AreEqual(fileLoadErrorText, tosDatabaseObjectToDeploy.ErrorsFileLoad);
        }

        [TestMethod]
        public void Test_TosDatabaseObjectToDeploy_SimulateLoadErrorText()
        {
            string filePath = "c:\\temp\\abc.sql";
            string fileLoadErrorText = "notImportant";
            var tosDatabaseObjectToDeploy = new DatabaseObjectToDeploy(filePath);

            tosDatabaseObjectToDeploy.ErrorsFileLoad = fileLoadErrorText;

            Assert.AreEqual(true, tosDatabaseObjectToDeploy.HaveTriedToLoadFile);
        }
    }
}
