namespace IanFNelson.DataStorage
{
    using System;

    public interface IDataStorageService
    {
        /// <summary>
        /// Persist an object, referenced by user and key strings.
        /// 
        /// If an object already exists in the DataStore with this username and key, it will be replaced.
        /// </summary>
        /// <param name="user">User identifier</param>
        /// <param name="key">Key identifier</param>
        /// <param name="toPersist">The object to be persisted (must be serializable)</param>
        void Add(string user, string key, object toPersist);

        /// <summary>
        /// Persist an object, referenced by user and key strings.
        /// 
        /// If an object already exists in the DataStore with this username and key, it will be replaced.
        /// </summary>
        /// <param name="user">User identifier</param>
        /// <param name="key">Key identifier</param>
        /// <param name="toPersist">The object to be persisted (must be serializable)</param>
        void Add(int user, string key, object toPersist);

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
        void Add(string user, string key, object toPersist, bool lockObject);

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
        void Add(int user, string key, object toPersist, bool lockObject);

        /// <summary>
        /// Retrieve a persisted object from the DataStore, given a user and key.
        /// </summary>
        /// <param name="user">User identifier</param>
        /// <param name="key">Key identifier</param>
        /// <returns>PersistedObject class</returns>
        PersistedObject Get(string user, string key);
        
        /// <summary>
        /// Retrieve a persisted object from the DataStore, given a user and key.
        /// </summary>
        /// <param name="user">User identifier</param>
        /// <param name="key">Key identifier</param>
        /// <returns>PersistedObject class</returns>
        PersistedObject Get(int user, string key);
        
        /// <summary>
        /// Retrieve a collection of all persisted objects for a given user.
        /// </summary>
        /// <param name="user">User identifier</param>
        /// <returns>PersistedObjectCollection containing all objects currently stored by the given user</returns>
        PersistedObjectCollection GetByUser(string user);
        
        /// <summary>
        /// Retrieve a collection of all persisted objects for a given user.
        /// </summary>
        /// <param name="user">User identifier</param>
        /// <returns>PersistedObjectCollection containing all objects currently stored by the given user</returns>
        PersistedObjectCollection GetByUser(int user);
        
        /// <summary>
        /// Retrive a collection of all persisted objects for a given key.
        /// </summary>
        /// <param name="key">Key identifier</param>
        /// <returns>PersistedObjectCollection containing all objects currently stored for a given key</returns>
        PersistedObjectCollection GetByKey(string key);
        
        /// <summary>
        /// Collection of all persisted objects
        /// </summary>
        PersistedObjectCollection PersistedObjects { get; }
        
        /// <summary>
        /// Clear all persisted objects from the datastore.  Use it wisely.
        /// </summary>
        void Clear();
        
        /// <summary>
        /// Remove persisted objects for a given user and key.
        /// </summary>
        /// <param name="user">User identifier</param>
        /// <param name="key">Key identifier</param>
        void Remove(string user, string key);
        
        /// <summary>
        /// Remove persisted objects for a given user and key.
        /// </summary>
        /// <param name="user">User identifier</param>
        /// <param name="key">Key identifier</param>
        void Remove(int user, string key);
        
        /// <summary>
        /// Remove all persisted objects for a given user.
        /// </summary>
        /// <param name="user"></param>
        void RemoveByUser(string user);
        
        /// <summary>
        /// Remove all persisted objects for a given user.
        /// </summary>
        /// <param name="user"></param>
        void RemoveByUser(int user);
        
        /// <summary>
        /// Remove all persisted objects for a given key.
        /// </summary>
        /// <param name="key"></param>
        void RemoveByKey(string key);
    }
}
