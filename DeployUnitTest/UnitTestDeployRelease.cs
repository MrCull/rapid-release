using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TosDeployReleaseClassLibary;
using System.Collections.Generic;
using System.IO;

namespace TosDeployUnitTest
{
    [TestClass]
    public class UnitTestDeployRelease
    {
        [TestMethod]
        public void Test_TosDeployRelease_Add100Jobs()
        {
            string directory = "c:\\temp\\Add100Jobs";

            Directory.CreateDirectory(directory);
            // prepare some data/files
            for (int i = 0; i < 1; i++)
            {
                var fileText = "if exists (select * from sysobjects where name = 'mac" + i.ToString() + "') begin drop procedure mac" + i.ToString() + " end\r\ngo\r\n create procedure mac" + i.ToString() + " as begin select " + i.ToString() + ", 'KEY1' end";
                var aFilePath = directory + "\\mac" + i.ToString() + ".sql";
                var jobLoadFile = new JobLoadFile();
                using (StreamWriter file = new StreamWriter(aFilePath))
                {
                    file.Write(fileText);
                    file.Close();
                }
            }

            var tosDeployRelease = new DeployRelease();
            tosDeployRelease.DatabaseServerName = "LENOVO\\SQLEXPRESS";
            tosDeployRelease.DatabaseName = "macroDB";
            tosDeployRelease.MacroQueryString = "select rtrim([key]) as [key], rtrim([value]) as [value] from macros";

            List<DeployPhase> deployPhasesList = new List<DeployPhase>();
            List<int> phases = new List<int>();

            DeployPhasePocosFromDirectory deployPhase = new DeployPhasePocosFromDirectory("FirstPhase", true);
            DeployPhasePocosFromDirectorySequences sequences = new DeployPhasePocosFromDirectorySequences(tosDeployRelease, directory + "\\", "*.sql", null, "grant execute on [poco] on public");
            deployPhase.SequencesInPhase.Add(sequences);

            DeployPhasePocosFromDirectorySequences sequences2 = new DeployPhasePocosFromDirectorySequences(tosDeployRelease, directory + "\\", "*.sql", null, "grant execute on [poco] on public");
            deployPhase.SequencesInPhase.Add(sequences2);

            deployPhasesList.Add(deployPhase);


            tosDeployRelease.SratrDeployment(deployPhasesList);
        }



        [TestMethod]
        public void Test_TosDeployRelease_First_Real_Database()
        {
            var tosDeployRelease = new DeployRelease();
            tosDeployRelease.DatabaseServerName = "LENOVO\\SQLEXPRESS";
            tosDeployRelease.DatabaseName = "Rapid_Release_Dev_CMS";
            tosDeployRelease.MacroQueryString = "select * from (select rtrim([cdegrp]) + '_' + rtrim([cdenam]) as [key], rtrim([cdenum]) as [value] from reftxt where cdenum is not NULL union select rtrim([cdegrp]) + '_' + rtrim([cdenam]) + '_STR' as [key], rtrim([cdeval]) as [value] from reftxt where cdeval is not NULL union select 'FALSE' as [key], '0' as [value] union select 'TRUE' as [key], '1' as [value] union select 'SUCCESS' as [key], '0' as [value] union select 'FAILURE' as [key], '-1' as [value] union select 'NOT_FOUND' as [key], '100' as [value] union select 'WARNING' as [key], '50' as [value]) sub order by len([key]) desc";

            List<DeployPhase> deployPhasesList = new List<DeployPhase>();
            List<int> phases = new List<int>();

            String directory = @"C:\Users\user\Desktop\buildstuff\env\Database\cms\";

            DeployPhasePocosFromDirectory deployPhase = new DeployPhasePocosFromDirectory("FirstPhase", false);

            DeployPhasePocosFromDirectorySequences sequenceUFs = new DeployPhasePocosFromDirectorySequences(tosDeployRelease, directory + "Programmability\\Functions\\", "uf*.sql", null, "grant execute on [poco] on public");
            deployPhase.SequencesInPhase.Add(sequenceUFs);

            DeployPhasePocosFromDirectorySequences sequenceViews = new DeployPhasePocosFromDirectorySequences(tosDeployRelease, directory + "Views\\", "vw_*.sql", "vw_user", "grant execute on [poco] on public");
            deployPhase.SequencesInPhase.Add(sequenceViews);

            DeployPhasePocosFromDirectorySequences sequenceUserViews = new DeployPhasePocosFromDirectorySequences(tosDeployRelease, directory + "Views\\", "vw_user*.sql", null, "grant execute on [poco] on public");
            deployPhase.SequencesInPhase.Add(sequenceUserViews);

            DeployPhasePocosFromDirectorySequences sequenceSPs = new DeployPhasePocosFromDirectorySequences(tosDeployRelease, directory + "Programmability\\Stored Procedures\\", "sp_*.sql", null, "grant execute on [poco] on public");
            deployPhase.SequencesInPhase.Add(sequenceSPs);

            DeployPhasePocosFromDirectorySequences sequenceTriggers = new DeployPhasePocosFromDirectorySequences(tosDeployRelease, directory + "Programmability\\Triggers\\", "tr_*.sql", null, "grant execute on [poco] on public");
            deployPhase.SequencesInPhase.Add(sequenceTriggers);

            deployPhasesList.Add(deployPhase);


            //DeployPhaseExecuteNoneQuery deployPhaseExeNonQuery1 = new DeployPhaseExecuteNoneQuery("fixrax", "exec sp_fixrak", true);
            //deployPhasesList.Add(deployPhaseExeNonQuery1);


            DeployPhasePocosFromDirectory deployPhase2 = new DeployPhasePocosFromDirectory("SecondPhase", true);

            DeployPhasePocosFromDirectorySequences sequenceUFs2 = new DeployPhasePocosFromDirectorySequences(tosDeployRelease, directory + "Programmability\\Functions\\", "uf*.sql", null, "grant execute on [poco] on public");
            deployPhase2.SequencesInPhase.Add(sequenceUFs2);

            DeployPhasePocosFromDirectorySequences sequenceViews2 = new DeployPhasePocosFromDirectorySequences(tosDeployRelease, directory + "Views\\", "vw_*.sql", "vw_user", "grant execute on [poco] on public");
            deployPhase.SequencesInPhase.Add(sequenceViews2);

            DeployPhasePocosFromDirectorySequences sequenceUserViews2 = new DeployPhasePocosFromDirectorySequences(tosDeployRelease, directory + "Views\\", "vw_user*.sql", null, "grant execute on [poco] on public");
            deployPhase.SequencesInPhase.Add(sequenceUserViews2);

            DeployPhasePocosFromDirectorySequences sequenceSPs2 = new DeployPhasePocosFromDirectorySequences(tosDeployRelease, directory + "Programmability\\Stored Procedures\\", "sp_*.sql", null, "grant execute on [poco] on public");
            deployPhase.SequencesInPhase.Add(sequenceSPs2);

            DeployPhasePocosFromDirectorySequences sequenceTriggers2 = new DeployPhasePocosFromDirectorySequences(tosDeployRelease, directory + "Programmability\\Triggers\\", "tr_*.sql", null, "grant execute on [poco] on public");
            deployPhase2.SequencesInPhase.Add(sequenceTriggers);

            DeployPhasePocosFromDirectorySequences sequenceIndexes2 = new DeployPhasePocosFromDirectorySequences(tosDeployRelease, directory + "Tables\\", "ix_*.sql", null, "grant execute on [poco] on public");
            deployPhase2.SequencesInPhase.Add(sequenceIndexes2);


            deployPhasesList.Add(deployPhase2);


            tosDeployRelease.SratrDeployment(deployPhasesList);

            //Assert.AreEqual(0, tosDeployRelease.AllDatabaseObjects.Count(s => s.Value.ErrorDatabaseUpdatePass1 != null));
        }
    }
}
