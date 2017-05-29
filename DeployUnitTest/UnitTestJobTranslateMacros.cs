using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DeployReleaseClassLibary;

namespace DeployUnitTest
{
    [TestClass]
    public class UnitTestJobTranslateMacros
    {

        [TestMethod]
        public void Test_DeployRelease_DoJob_IsMarcoTranslationCompleted()
        {
            var jobTranslateMacros = new JobTranslateMacros();
            var databaseObjectToDeploy = new DatabaseObjectToDeploy();

            jobTranslateMacros.DeployRelease = new DeployRelease();

            jobTranslateMacros.DatabaseObjectToDeploy = databaseObjectToDeploy;
            databaseObjectToDeploy.ObjectText = "KEY";
            jobTranslateMacros.DeployRelease.MacroTable.Add("KEY", "VALUE");
            jobTranslateMacros.Execute();

            Assert.AreEqual(true, databaseObjectToDeploy.IsMacroTranslationCompleted);
        }

        [TestMethod]
        public void Test_DeployRelease_DoJob_TransalteOneMacro()
        {
            var jobTranslateMacros = new JobTranslateMacros();
            var databaseObjectToDeploy = new DatabaseObjectToDeploy();

            jobTranslateMacros.DeployRelease = new DeployRelease();

            jobTranslateMacros.DatabaseObjectToDeploy = databaseObjectToDeploy;
            databaseObjectToDeploy.ObjectText = "KEY";
            jobTranslateMacros.DeployRelease.MacroTable.Add("KEY", "VALUE");
            jobTranslateMacros.Execute();

            Assert.AreEqual("VALUE", databaseObjectToDeploy.ObjectText);
        }

        [TestMethod]
        public void Test_DeployRelease_DoJob_TransalteMutipleMacro()
        {
            var jobTranslateMacros = new JobTranslateMacros();
            var databaseObjectToDeploy = new DatabaseObjectToDeploy();

            jobTranslateMacros.DeployRelease = new DeployRelease();

            jobTranslateMacros.DatabaseObjectToDeploy = databaseObjectToDeploy;
            databaseObjectToDeploy.ObjectText = "KEY1ABCKEY2abcKEY1AbC";
            jobTranslateMacros.DeployRelease.MacroTable.Add("KEY1", "VALUE1");
            jobTranslateMacros.DeployRelease.MacroTable.Add("KEY2", "VALUE2");
            jobTranslateMacros.Execute();

            Assert.AreEqual("VALUE1ABCVALUE2abcVALUE1AbC", databaseObjectToDeploy.ObjectText);
        }

    }
}
