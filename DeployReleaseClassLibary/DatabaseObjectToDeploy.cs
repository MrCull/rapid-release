using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DeployReleaseClassLibary
{
    public class DatabaseObjectToDeploy
    {
        private string filePath;
        private string objectName;
        private string objectText;
        private string databasePermissionsClause;
        private bool isFileLoaded = false;
        private bool isMacroTranslationCompleted = false;
        private string errorsFileLoad;
        private string errorsDatabaseUploadPass1;
        private string errorsDatabaseUploadPass2;
        private bool haveTriedToLoadFile = false;
        private bool isBeingWorkedOn = false;
        private int numberDeadlocks = 0;
        private int numberReferencedEntityErrors = 0;
        private int numberOfTimesLoaded = 0;
        private List<List<string>>errors;
        private List<List<string>>sqlPrintOutput;

        public DatabaseObjectToDeploy()
        {
            errors = new List<List<string>>();
            sqlPrintOutput = new List<List<string>>();
        }

        public DatabaseObjectToDeploy(string filePath) : this()
        {
            FilePath = filePath;
        }

        public string FilePath
        {
            get
            {
                return filePath;
            }
            set
            {
                filePath = value;
                objectName = Path.GetFileNameWithoutExtension(filePath);
            }
        }

        public string ObjectName
        {
            get {return objectName;}            
        }


        public bool HaveTriedToLoadFile
        {
            get { return haveTriedToLoadFile; }
            set { haveTriedToLoadFile = value; }
        }

        public bool IsBeingWorkedOn
        {
            get { return isBeingWorkedOn; }
            set { isBeingWorkedOn = value; }
        }

        public bool IsFileLoaded
        {
            get { return isFileLoaded; }
            set { isFileLoaded = value; }
        }

        public bool IsMacroTranslationCompleted
        {
            get { return isMacroTranslationCompleted;}
            set { isMacroTranslationCompleted = value; }
        }

        public string ObjectText
        {
            get
            {
                return objectText;
            }

            set
            {
                objectText = value;
                isFileLoaded = true;
                haveTriedToLoadFile = true;
            }
        }

        public string ErrorsFileLoad
        {
            get
            {
                return errorsFileLoad;
            }

            set
            {
                errorsFileLoad = value;
                haveTriedToLoadFile = true;
            }
        }

        public string ErrorDatabaseUpdatePass1
        {
            get
            {
                return errorsDatabaseUploadPass1;
            }

            set
            {
                errorsDatabaseUploadPass1 = value; 
            }
        }

        public string ErrorDatabaseUpdatePass2
        {
            get
            {
                return errorsDatabaseUploadPass2;
            }

            set
            {
                errorsDatabaseUploadPass2 = value;
            }
        }

        public string DatabasePermissionClause
        {
            get
            {
                return databasePermissionsClause;
            }

            set
            {
                databasePermissionsClause = value;
            }
        }

        public List<List<string>> Errors
        {
            get { return errors; }
            set { errors = value; }
        }

        public List<List<string>> SqlPrintOutput
        {
            get { return sqlPrintOutput; }
            set { sqlPrintOutput = value; }
        }

        public int NumberDeadlocks
        {
            get { return numberDeadlocks; }
            set { numberDeadlocks = value; }
        }

        public int NumberOfTimesLoaded
        {
            get { return numberOfTimesLoaded; }
            set { numberOfTimesLoaded = value; }
        }

        public int NumberReferencedEntityErrors
        {
            get
            {
                return numberReferencedEntityErrors;
            }

            set
            {
                numberReferencedEntityErrors = value;
            }
        }
    }
}