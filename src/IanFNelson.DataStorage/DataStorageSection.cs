using System;

namespace IanFNelson.DataStorage
{
    public class DataStorageSection : System.Configuration.ConfigurationSection {
        
        [System.Configuration.ConfigurationProperty("connectionStringName", DefaultValue="IanFNelson.DataStorage", IsKey=false, IsRequired=false)]
        public string ConnectionStringName {
            get { return ( string )base["connectionStringName"]; }
            set { base["connectionStringName"] = value; }
        }

        [System.Configuration.ConfigurationProperty("applicationId", IsRequired=true)]
        public string ApplicationId
        {
            get { return ( string )base["applicationId"]; }
            set { base["applicationId"] = value; }
        }
    }
}
