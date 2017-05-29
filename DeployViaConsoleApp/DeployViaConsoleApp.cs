using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using DeployReleaseClassLibary;

namespace ConsoleApplicationTest
{
    class DeployViaConsoleApp
    {
        private static object consoleLock;
        static void Main(string[] args)
        {
            var deployRelease = new DeployRelease();
            consoleLock = new object();
            

            XmlSerializer xsSubmit = new XmlSerializer(typeof(DeployRelease));
            using (FileStream fileStream = new FileStream(@"DeployReleaseConfig.xml", FileMode.Open))
            {
                deployRelease = (DeployRelease)xsSubmit.Deserialize(fileStream);
            }

            deployRelease.ThrowEventJobProgress += (sender, e) => { JobProgress(sender, e); };
            deployRelease.ThrowEventJobError += (sender, e) => { JobError(sender, e); };
            deployRelease.DoDeployment();
            deployRelease.ThrowErrors();

            Console.WriteLine("Start {0}", deployRelease.StartTime);
            Console.WriteLine("End {0}", deployRelease.EndTime);
            Console.WriteLine("Total {0}", deployRelease.GetTotalTime());
            Console.WriteLine("Max number of threads {0}", deployRelease.LargestNumberThreadsCount);

            Console.ReadKey(true);
        }

        private static void JobProgress(object sender, EventArgsJobProgress e)
        {
            lock (consoleLock)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("{0}: {1} {2}", DateTime.Now, e.JobOutputMessage, e.JobAdditionalInfo);
            }
        }

        private static void JobError(object sender, EventArgsJobError e)
        {
            lock (consoleLock)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("{0}: {1} - {2}", DateTime.Now, e.JobReference, e.JobError);
            }
        }
    }
}
