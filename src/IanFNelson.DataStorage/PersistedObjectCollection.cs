using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;

namespace IanFNelson.DataStorage
{
    /// <summary>
    /// Collection of persisted objects
    /// </summary>
    public class PersistedObjectCollection : Collection<PersistedObject>
    {
        public PersistedObjectCollection() { }

        public PersistedObjectCollection(IList<PersistedObject> list) : base( list ) { }

        /// <summary>
        /// Filters the collection to return only those objects which have been exclusively locked.
        /// </summary>
        public PersistedObjectCollection LockedObjects
        {
            get
            {
                return new PersistedObjectCollection(
                    this.Where<PersistedObject>( po => po.Locked ).ToList<PersistedObject>() 
                );
            }
        }
    }
}
