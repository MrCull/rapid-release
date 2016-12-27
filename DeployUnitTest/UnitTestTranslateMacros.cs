using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TosDeployReleaseClassLibary;

namespace TosDeployUnitTest
{
    [TestClass]
    public class UnitTestTranslateMacros
    {

        [TestMethod]
        public void Test_TosDeployRelease_DoJob_IsMarcoTranslationCompleted()
        {
            var jobTranslateMacros = new JobTranslateMacros();
            var tosDatabaseObjectToDeploy = new DatabaseObjectToDeploy();

            jobTranslateMacros.TosDeployRelease = new DeployRelease();

            jobTranslateMacros.TosDatabaseObjectToDeploy = tosDatabaseObjectToDeploy;
            tosDatabaseObjectToDeploy.ObjectText = "KEY";
            jobTranslateMacros.TosDeployRelease.MacroTable.Add("KEY", "VALUE");
            jobTranslateMacros.Execute();

            Assert.AreEqual(true, tosDatabaseObjectToDeploy.IsMacroTranslationCompleted);
        }

        [TestMethod]
        public void Test_TosDeployRelease_DoJob_TransalteOneMacro()
        {
            var jobTranslateMacros = new JobTranslateMacros();
            var tosDatabaseObjectToDeploy = new DatabaseObjectToDeploy();

            jobTranslateMacros.TosDeployRelease = new DeployRelease();

            jobTranslateMacros.TosDatabaseObjectToDeploy = tosDatabaseObjectToDeploy;
            tosDatabaseObjectToDeploy.ObjectText = "KEY";
            jobTranslateMacros.TosDeployRelease.MacroTable.Add("KEY", "VALUE");
            jobTranslateMacros.Execute();

            Assert.AreEqual("VALUE", tosDatabaseObjectToDeploy.ObjectText);
        }

        [TestMethod]
        public void Test_TosDeployRelease_DoJob_TransalteMutipleMacro()
        {
            var jobTranslateMacros = new JobTranslateMacros();
            var tosDatabaseObjectToDeploy = new DatabaseObjectToDeploy();

            jobTranslateMacros.TosDeployRelease = new DeployRelease();

            jobTranslateMacros.TosDatabaseObjectToDeploy = tosDatabaseObjectToDeploy;
            tosDatabaseObjectToDeploy.ObjectText = "KEY1ABCKEY2abcKEY1AbC";
            jobTranslateMacros.TosDeployRelease.MacroTable.Add("KEY1", "VALUE1");
            jobTranslateMacros.TosDeployRelease.MacroTable.Add("KEY2", "VALUE2");
            jobTranslateMacros.Execute();

            Assert.AreEqual("VALUE1ABCVALUE2abcVALUE1AbC", tosDatabaseObjectToDeploy.ObjectText);
        }

    }
}
