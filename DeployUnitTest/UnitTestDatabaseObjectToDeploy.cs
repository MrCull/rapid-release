using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DeployReleaseClassLibary;

namespace DeployUnitTest
{
    [TestClass]
    public class UnitTestDatabaseObjectToDeploy
    {
        [TestMethod]
        public void Test_DatabaseObjectToDeploy_InstantiateObjectAndCheckFilePath()
        {
            string filePath = "notImportant";
            var databaseObjectToDeploy = new DatabaseObjectToDeploy(filePath);

            Assert.AreEqual(filePath, databaseObjectToDeploy.FilePath);
        }


        [TestMethod]
        public void Test_DatabaseObjectToDeploy_InstantiateObjectAndCheckDidFileLoadSucessfully()
        {
            string filePath = "notImportant";
            var databaseObjectToDeploy = new DatabaseObjectToDeploy(filePath);

            Assert.AreEqual(false, databaseObjectToDeploy.HaveTriedToLoadFile);
        }


        [TestMethod]
        public void Test_databaseObjectToDeploy_InstantiateObjectAndCheckIsBeingWorkedOn()
        {
            string filePath = "notImportant";
            var databaseObjectToDeploy = new DatabaseObjectToDeploy(filePath);

            Assert.AreEqual(false, databaseObjectToDeploy.IsBeingWorkedOn);
        }

        [TestMethod]
        public void Test_databaseObjectToDeploy_InstantiateObjectAndCheckIsFileLoaded()
        {
            string filePath = "notImportant";
            var databaseObjectToDeploy = new DatabaseObjectToDeploy(filePath);

            Assert.AreEqual(false, databaseObjectToDeploy.IsFileLoaded);
        }

        [TestMethod]
        public void Test_databaseObjectToDeploy_InstantiateObjectAndCheckIsMacroTranslationCompleted()
        {
            string filePath = "notImportant";
            var databaseObjectToDeploy = new DatabaseObjectToDeploy(filePath);

            Assert.AreEqual(false, databaseObjectToDeploy.IsMacroTranslationCompleted);
        }


        [TestMethod]
        public void Test_databaseObjectToDeploy_InstantiateObjectAndCheckOjectName()
        {
            string filePath = "c:\\temp\\abc.sql";
            var databaseObjectToDeploy = new DatabaseObjectToDeploy(filePath);

            Assert.AreEqual("abc", databaseObjectToDeploy.ObjectName);
        }

        [TestMethod]
        public void Test_databaseObjectToDeploy_SimulateLoadIsFileLoaded()
        {
            string filePath = "c:\\temp\\abc.sql";
            string fileText = "notImportant";
            var databaseObjectToDeploy = new DatabaseObjectToDeploy(filePath);

            databaseObjectToDeploy.ObjectText = fileText;

            Assert.AreEqual(true, databaseObjectToDeploy.IsFileLoaded);
        }

        [TestMethod]
        public void Test_databaseObjectToDeploy_SimulateLoadDidFileLoadSuccessfully()
        {
            string filePath = "c:\\temp\\abc.sql";
            string fileText = "notImportant";
            var databaseObjectToDeploy = new DatabaseObjectToDeploy(filePath);

            databaseObjectToDeploy.ObjectText = fileText;

            Assert.AreEqual(true, databaseObjectToDeploy.HaveTriedToLoadFile);
        }

        [TestMethod]
        public void Test_databaseObjectToDeploy_SimulateLoadFileErrorText()
        {
            string filePath = "c:\\temp\\abc.sql";
            string fileLoadErrorText = "notImportant";
            var databaseObjectToDeploy = new DatabaseObjectToDeploy(filePath);

            databaseObjectToDeploy.ErrorsFileLoad = fileLoadErrorText;

            Assert.AreEqual(fileLoadErrorText, databaseObjectToDeploy.ErrorsFileLoad);
        }

        [TestMethod]
        public void Test_databaseObjectToDeploy_SimulateLoadErrorText()
        {
            string filePath = "c:\\temp\\abc.sql";
            string fileLoadErrorText = "notImportant";
            var databaseObjectToDeploy = new DatabaseObjectToDeploy(filePath);

            databaseObjectToDeploy.ErrorsFileLoad = fileLoadErrorText;

            Assert.AreEqual(true, databaseObjectToDeploy.HaveTriedToLoadFile);
        }
    }
}
