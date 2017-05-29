using System;

namespace DeployReleaseClassLibary
{
    public class EventArgsJobProgress : EventArgs 
    {
        private string jobReference;
        private string jobOutputMessage;
        private string jobAdditionalInfo;

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

        public string JobOutputMessage
        {
            get
            {
                return jobOutputMessage;
            }

            set
            {
                jobOutputMessage = value;
            }
        }

        public string JobAdditionalInfo
        {
            get
            {
                return jobAdditionalInfo;
            }

            set
            {
                jobAdditionalInfo = value;
            }
        }
    }
}
