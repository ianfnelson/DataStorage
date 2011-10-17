namespace IanFNelson.DataStorage
{
    using System;
    using System.Runtime.Serialization;
    using System.Security.Permissions;

    /// <summary>
    /// Custom exception thrown when a user attempts to exclusively lock an object which is already locked by another user.
    /// </summary>
    [Serializable()]
    public class ObjectLockedException : Exception
    {
        /// <summary>
        /// Recommended constructor - takes the locked PersistedObject as a property.
        /// </summary>
        /// <param name="lockedObject">PersistedObject which is already locked</param>
        public ObjectLockedException(PersistedObject lockedObject)
            : base()
        {
            LockedObject = lockedObject;
        }

        /// <summary>
        /// Constructor included for standards compliance
        /// </summary>
        public ObjectLockedException()
            : base()
        {
        }

        /// <summary>
        /// Constructor included for standards compliance
        /// </summary>
        /// <param name="message"></param>
        public ObjectLockedException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Constructor included for standards compliance
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public ObjectLockedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Constructor included for standards compliance
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected ObjectLockedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// The PersistedObject which is already locked by another user
        /// </summary>
        public PersistedObject LockedObject { get; private set; }

        /// <summary>
        /// Included for standards compliance
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            if (info == null) throw new ArgumentNullException("info");
            info.AddValue("lockedObject", LockedObject, typeof(PersistedObject));
            base.GetObjectData(info, context);
        }
    }
}
