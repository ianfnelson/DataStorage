namespace IanFNelson.DataStorage
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;

    /// <summary>
    /// This class allows serializable objects to be persisted to and from a SQL-server based repository.
    /// The objects themselves are held in an image datatype.
    /// 
    /// Additional optional functionality allows for the persisted objects to be "locked" by a specific user so that others may not simultaneously store objects with the same key.
    /// </summary>
    public class DataStorageService : IDataStorageService
    {
        private static string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[Config.Settings.ConnectionStringName].ConnectionString;

        private static string _applicationId = Config.Settings.ApplicationId;

        /// <summary>
        /// Persist an object, referenced by user and key strings.
        /// 
        /// If an object already exists in the DataStore with this username and key, it will be replaced.
        /// </summary>
        /// <param name="user">User identifier</param>
        /// <param name="key">Key identifier</param>
        /// <param name="toPersist">The object to be persisted (must be serializable)</param>
        public void Add(string user, string key, object toPersist)
        {
            InternalAdd(user, key, toPersist, false);
        }

        /// <summary>
        /// Persist an object, referenced by user and key strings.
        /// 
        /// If an object already exists in the DataStore with this username and key, it will be replaced.
        /// </summary>
        /// <param name="user">User identifier</param>
        /// <param name="key">Key identifier</param>
        /// <param name="toPersist">The object to be persisted (must be serializable)</param>
        public void Add(int user, string key, object toPersist)
        {
            InternalAdd(user.ToString(CultureInfo.InvariantCulture), key, toPersist, false);
        }

        /// <summary>
        /// Persist an object, referenced by user and key strings.
        /// The option is additionally given to exclusively lock the object, which if successful will prevent other users from similarly locking the same object.  If another user has already locked an object with the same key, an ObjectLockedException is thrown.
        /// 
        /// If an object already exists in the DataStore with this username and key, it will be replaced.
        /// </summary>
        /// <param name="user">User identifier</param>
        /// <param name="key">Key identifier</param>
        /// <param name="toPersist">The object to be persisted (must be serializable)</param>
        /// <param name="lockObject">Whether to attempt to exclusively lock the object</param>
        public void Add(string user, string key, object toPersist, bool lockObject)
        {
            InternalAdd(user, key, toPersist, lockObject);
        }

        /// <summary>
        /// Persist an object, referenced by user and key strings.
        /// The option is additionally given to exclusively lock the object, which if successful will prevent other users from similarly locking the same object.  If another user has already locked an object with the same key, an ObjectLockedException is thrown.
        /// 
        /// If an object already exists in the DataStore with this username and key, it will be replaced.
        /// </summary>
        /// <param name="user">User identifier</param>
        /// <param name="key">Key identifier</param>
        /// <param name="toPersist">The object to be persisted (must be serializable)</param>
        /// <param name="lockObject">Whether to attempt to exclusively lock the object</param>
        public void Add(int user, string key, object toPersist, bool lockObject)
        {
            InternalAdd(user.ToString(CultureInfo.InvariantCulture), key, toPersist, lockObject);
        }

        /// <summary>
        /// Retrieve a persisted object from the DataStore, given a user and key.
        /// </summary>
        /// <param name="user">User identifier</param>
        /// <param name="key">Key identifier</param>
        /// <returns>PersistedObject class</returns>
        public PersistedObject Get(string user, string key)
        {
            //Check for nulls, throw exception
            PersistedObjectCollection objects = InternalGet(user, key);
            return objects.Count == 0 ? null : objects[0];
        }

        /// <summary>
        /// Retrieve a persisted object from the DataStore, given a user and key.
        /// </summary>
        /// <param name="user">User identifier</param>
        /// <param name="key">Key identifier</param>
        /// <returns>PersistedObject class</returns>
        public PersistedObject Get(int user, string key)
        {
            return Get(user.ToString(CultureInfo.InvariantCulture), key);
        }

        /// <summary>
        /// Retrieve a collection of all persisted objects for a given user.
        /// </summary>
        /// <param name="user">User identifier</param>
        /// <returns>PersistedObjectCollection containing all objects currently stored by the given user</returns>
        public PersistedObjectCollection GetByUser(string user)
        {
            return InternalGet(user, string.Empty);
        }

        /// <summary>
        /// Retrieve a collection of all persisted objects for a given user.
        /// </summary>
        /// <param name="user">User identifier</param>
        /// <returns>PersistedObjectCollection containing all objects currently stored by the given user</returns>
        public PersistedObjectCollection GetByUser(int user)
        {
            return InternalGet(user.ToString(CultureInfo.InvariantCulture), string.Empty);
        }

        /// <summary>
        /// Retrive a collection of all persisted objects for a given key.
        /// </summary>
        /// <param name="key">Key identifier</param>
        /// <returns>PersistedObjectCollection containing all objects currently stored for a given key</returns>
        public PersistedObjectCollection GetByKey(string key)
        {
            return InternalGet(string.Empty, key);
        }

        /// <summary>
        /// Collection of all persisted objects
        /// </summary>
        public PersistedObjectCollection PersistedObjects
        {
            get { return InternalGet(string.Empty, string.Empty); }
        }

        /// <summary>
        /// Clear all persisted objects from the datastore.  Use it wisely.
        /// </summary>
        public void Clear()
        {
            InternalRemove(string.Empty, string.Empty);
        }

        /// <summary>
        /// Remove persisted objects for a given user and key.
        /// </summary>
        /// <param name="user">User identifier</param>
        /// <param name="key">Key identifier</param>
        public void Remove(string user, string key)
        {
            InternalRemove(user, key);
        }

        /// <summary>
        /// Remove persisted objects for a given user and key.
        /// </summary>
        /// <param name="user">User identifier</param>
        /// <param name="key">Key identifier</param>
        public void Remove(int user, string key)
        {
            InternalRemove(user.ToString(CultureInfo.InvariantCulture), key);
        }

        /// <summary>
        /// Remove all persisted objects for a given user.
        /// </summary>
        /// <param name="user"></param>
        public void RemoveByUser(string user)
        {
            InternalRemove(user, string.Empty);
        }

        /// <summary>
        /// Remove all persisted objects for a given user.
        /// </summary>
        /// <param name="user"></param>
        public void RemoveByUser(int user)
        {
            InternalRemove(user.ToString(CultureInfo.InvariantCulture), string.Empty);
        }

        /// <summary>
        /// Remove all persisted objects for a given key.
        /// </summary>
        /// <param name="key"></param>
        public void RemoveByKey(string key)
        {
            InternalRemove(string.Empty, key);
        }

        /// <summary>
        /// Internal object creation method
        /// </summary>
        /// <param name="user">User identifier</param>
        /// <param name="key">Key identifier</param>
        /// <param name="objectToPersist">The object to be persisted (must be serializable)</param>
        /// <param name="lockObject">Whether to attempt to exclusively lock the object</param>
        private void InternalAdd(string user, string key, object objectToPersist, bool lockObject)
        {
            #region Check parameters
            // user parameter must not be null
            if (user == null) throw new ArgumentNullException("user");

            // key parameter must not be null
            if (key == null) throw new ArgumentNullException("key");

            // user parameter must be between 1 and 100 characters in length
            if ((user.Length <= 0) || (user.Length > 100)) throw new ArgumentOutOfRangeException("user", user);

            // key parameter must be between 1 and 100 characters in length
            if ((key.Length <= 0) || (key.Length > 100)) throw new ArgumentOutOfRangeException("key", key);

            // objectToPersist must not be null
            if (objectToPersist == null) throw new ArgumentNullException("objectToPersist");

            // objectToPersist must be serializable
            if (!((objectToPersist is ISerializable) |
                ((objectToPersist.GetType().Attributes & TypeAttributes.Serializable) == TypeAttributes.Serializable)))
                throw new ArgumentException("Parameter must be serializable", "objectToPersist");
            #endregion

            #region Check no other user has this object locked (if required)
            // Do we need to check that no other users have this object locked?
            if (lockObject)
            {
                // Check that no other users have this object locked
                PersistedObjectCollection objs = InternalGet(string.Empty, key);
                PersistedObject obj = objs.FirstOrDefault<PersistedObject>(po => po.Locked && (po.User != user));
                if (obj != null) throw new ObjectLockedException(obj);
            }
            #endregion

            #region Persist the object
            // Instantiate a MemoryStream, and a BinaryFormatter
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();

            // Serialize our object into the MemoryStream
            bf.Serialize(ms, objectToPersist);

            // Go to start of memorystream
            ms.Seek(0, 0);

            // Save the object into the database
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = conn;
                    command.CommandText = "IanFNelson_DataStorage_Add";
                    command.Parameters.AddWithValue("@application", _applicationId);
                    command.Parameters.AddWithValue("@user", user);
                    command.Parameters.AddWithValue("@key", key);
                    command.Parameters.AddWithValue("@locked", lockObject);
                    command.Parameters.AddWithValue("@type", objectToPersist.GetType().FullName);
                    command.Parameters.AddWithValue("@object", ms.ToArray());


                    conn.Open();
                    command.ExecuteNonQuery();

                }
            }

            // Close the memory stream
            ms.Close();

            #endregion
        }

        /// <summary>
        /// Internal object retrieval method.
        /// </summary>
        /// <param name="user">User identifier (string.Empty if all users)</param>
        /// <param name="key">Key identifier (string.Empty if all keys)</param>
        /// <returns>PersistedObjectCollection of all selected objects</returns>
        private PersistedObjectCollection InternalGet(string user, string key)
        {
            PersistedObjectCollection objects = new PersistedObjectCollection();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand command = new SqlCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Connection = conn;
                command.CommandText = "IanFNelson_DataStorage_GetObjectList";
                command.Parameters.AddWithValue("@application", _applicationId);
                command.Parameters.AddWithValue("@user", (string.IsNullOrEmpty(user) ? (object)DBNull.Value : (object)user));
                command.Parameters.AddWithValue("@key", (string.IsNullOrEmpty(key) ? (object)DBNull.Value : (object)key));

                conn.Open();

                using (SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        objects.Add(
                            new PersistedObject
                            {
                                Application = dr.GetString(0),
                                User = dr.GetString(1),
                                Key = dr.GetString(2),
                                Locked = dr.GetBoolean(3),
                                CreatedDate = dr.GetDateTime(4),
                                UpdatedDate = dr.GetDateTime(5),
                                Type = dr.GetString(6)
                            });
                    }
                }
            }

            return objects;
        }

        /// <summary>
        /// Internal object deletion method.
        /// </summary>
        /// <param name="user">User identifier (string.Empty if all users)</param>
        /// <param name="key">Key identifier (string.Empty if all keys)</param>
        private void InternalRemove(string user, string key)
        {
            // Save the object into the database
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = conn;
                    command.CommandText = "IanFNelson_DataStorage_Delete";
                    command.Parameters.AddWithValue("@application", _applicationId);
                    command.Parameters.AddWithValue("@user", (string.IsNullOrEmpty(user) ? (object)DBNull.Value : (object)user));
                    command.Parameters.AddWithValue("@key", (string.IsNullOrEmpty(key) ? (object)DBNull.Value : (object)key));
                    conn.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
