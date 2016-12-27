﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TosDeployReleaseClassLibary
{
    public class DatabaseObjectToDeploy
    {
        private string filePath;
        private string objectName;
        private string objectText;
        private string databasePermissionsClause;
        private string databaseObjectText;
        private bool isFileLoaded = false;
        private bool isMacroTranslationCompleted = false;
        private string errorsFileLoad;
        private string errorsDatabaseUploadPass1;
        private string errorsDatabaseUploadPass2;
        private bool haveTriedToLoadFile = false;
        private bool isBeingWorkedOn = false;

        public DatabaseObjectToDeploy()
        {
        }

        public DatabaseObjectToDeploy(string filePath)
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
        }

        public bool IsBeingWorkedOn
        {
            get { return isBeingWorkedOn; }
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
    }
}