using System;

namespace DeployReleaseClassLibary
{
    public class EventArgsJobError : EventArgs
    {
        private string jobReference;
        private string jobError;
        
        public string JobReference
        {
            get
            {
                return jobReference;
            }

            set
            {
                jobReference = value;
            }
        }

        public string JobError
        {
            get
            {
                return jobError;
            }

            set
            {
                jobError = value;
            }
        }
    }
}