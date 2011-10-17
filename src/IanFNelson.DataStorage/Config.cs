using System;
using System.Configuration;

namespace IanFNelson.DataStorage
{
	/// <summary>
	/// Class to provide easy access to configuration properties.
	/// </summary>
    public sealed class Config
    {
        /// <summary>
        /// Private constructor.
        /// </summary>
        private Config() { }

        /// <summary>
        /// Application Settings
        /// </summary>
        public static DataStorageSection Settings
        {
            get { return ConfigurationManager.GetSection( "ianFNelson.dataStorage" ) as DataStorageSection; }
        }
    }
}
