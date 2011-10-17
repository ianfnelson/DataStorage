namespace IanFNelson.DataStorage
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    /// <summary>
    /// This class holds the details of an object which has been persisted using IanFNelson.DataStorage.DataStore class.
    /// The object itself is obtained through the StoredObject property.
    /// </summary>
    public class PersistedObject
    {
        /// <summary>
        /// Internal constructor (PersistedObject should not be instantiated outside of IanFNelson.DataStorage!)
        /// </summary>
        internal PersistedObject() { }

        private object _storedObject;

        private string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[Config.Settings.ConnectionStringName].ConnectionString;

        /// <summary>
        /// Application to which this object is related.
        /// </summary>
        public string Application { get; internal set; }

        /// <summary>
        /// Id of the User who has persisted this object.
        /// </summary>
        public string User { get; internal set; }

        /// <summary>
        /// Key by which the object is referenced.
        /// </summary>
        public string Key { get; internal set; }

        /// <summary>
        /// Indicates whether this persisted object is exclusively locked by this user.
        /// </summary>
        public bool Locked { get; internal set; }

        /// <summary>
        /// The actual stored object.
        /// </summary>
        public object StoredObject
        {
            get
            {
                // Get the object from database once only.
                if (_storedObject != null)
                    return _storedObject;

                // Get the object from the database (stored as "image" type)
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Connection = conn;
                        command.CommandText = "IanFNelson_DataStorage_GetObject";
                        command.Parameters.AddWithValue("@application", Application);
                        command.Parameters.AddWithValue("@user", User);
                        command.Parameters.AddWithValue("@key", Key);

                        conn.Open();

                        using (SqlDataReader dr = command.ExecuteReader())
                        {
                            // If no rows in the database for this user/key combination, it has been removed since this PersistedObject was instantiated.
                            if (dr.HasRows)
                            {
                                // Extract the image property to an array of bytes.
                                dr.Read();
                                byte[] objData = (byte[])dr[0];

                                // Instantiate MemoryStream and BinaryFormatter
                                MemoryStream ms = new MemoryStream();
                                BinaryFormatter bf = new BinaryFormatter();

                                // Copy the binary object into the memory stream
                                ms.Write(objData, 0, objData.Length);
                                ms.Seek(0, 0);

                                // Deserialize the binary object from memory stream into private _storedObject property
                                _storedObject = bf.Deserialize(ms);

                                // Clean up
                                ms.Close();

                            }
                        }
                    }
                }
                return _storedObject;	// might still be null, but unlikely
            }
        }

        /// <summary>
        /// DateTime at which this object was initially persisted.
        /// </summary>
        public DateTime CreatedDate { get; internal set; }

        /// <summary>
        /// DateTime at which this object was last persisted.
        /// </summary>
        public DateTime UpdatedDate { get; internal set; }

        /// <summary>
        /// Fully qualified name of the Type of the persisted object
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
        public string Type { get; internal set; }
    }
}
