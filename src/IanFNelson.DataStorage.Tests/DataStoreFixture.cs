namespace IanFNelson.DataStorage.Tests
{
    using System;
    using NUnit.Framework;

    /// <summary>
    /// DataStore unit tests.
    /// </summary>
    [TestFixture]
    public class DataStoreFixture
    {
        public DataStoreFixture() { }

        private IDataStorageService dataStorageService;

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            this.dataStorageService = new DataStorageService();
            this.dataStorageService.Clear();
        }

        [TearDown]
        public void TestCleanup()
        {
            this.dataStorageService.Clear();
        }

        [SetUp]
        public void TestInitialize()
        {
            this.dataStorageService.Clear();
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PersistFail1()
        {
            // Arrange
            SerializableObject1 obj = MakeObject1();

            // Act
            this.dataStorageService.Add(null, "key", obj);
        }
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PersistFail2()
        {
            // Arrange
            SerializableObject1 obj = MakeObject1();

            // Act
            this.dataStorageService.Add("username", null, obj);
        }
        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PersistFail3()
        {
            // Arrange
            SerializableObject1 obj = MakeObject1();

            // Act
            this.dataStorageService.Add("username", "NowisthetimeforallgoodmentocometotheaidofthepeopleThequickbrownfoxjumpsoverthelazydogNowisthetimefora", obj);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PersistFail4()
        {
            // Arrange
            SerializableObject1 obj = MakeObject1();

            // Act
            this.dataStorageService.Add("NowisthetimeforallgoodmentocometotheaidofthepeopleThequickbrownfoxjumpsoverthelazydogNowisthetimefora", "key", obj);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PersistFail5()
        {
            // Arrange
            SerializableObject1 obj = MakeObject1();

            // Act
            this.dataStorageService.Add("", "key", obj);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PersistFail6()
        {
            // Arrange
            SerializableObject1 obj = MakeObject1();

            // Act
            this.dataStorageService.Add("username", String.Empty, obj);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void PersistFail7()
        {
            // Arrange
            NonSerializableObject nso = new NonSerializableObject();
            nso.Ziggurat = "penguin";

            // Act
            this.dataStorageService.Add("username", "key", nso);
        }

        [Test]
        public void PersistAnObject()
        {
            // Arrange
            SerializableObject1 objIn = MakeObject1();

            // Act
            this.dataStorageService.Add("nelsoni", "test1", objIn);
            PersistedObject pObjOut = this.dataStorageService.Get("nelsoni", "test1");

            // Assertions
            Assert.AreEqual(pObjOut.Key, "test1");
            Assert.AreEqual(pObjOut.User, "nelsoni");
            Assert.AreEqual(pObjOut.CreatedDate.Date, DateTime.Today);
            Assert.IsFalse(pObjOut.Locked);
            Assert.AreEqual(pObjOut.UpdatedDate.Date, DateTime.Today);
            Assert.AreEqual(objIn.GetType().FullName, pObjOut.Type);
            Assert.AreEqual(objIn, pObjOut.StoredObject as SerializableObject1);
        }

        [Test]
        [ExpectedException(typeof(ObjectLockedException))]
        public void LockingTest1()
        {
            // Arrange
            SerializableObject1 objIn = MakeObject1();

            // Act
            this.dataStorageService.Add("nelsoni", "test1", objIn, true);
            this.dataStorageService.Add("harrisonsi", "test1", objIn, true);		// Different user, not allowed
        }

        [Test]
        public void LockingTest2()
        {
            // Arrange
            SerializableObject1 objIn = MakeObject1();

            // Act
            this.dataStorageService.Add("nelsoni", "test1", objIn, true);

            objIn.LineValue = objIn.LineValue * 2;
            this.dataStorageService.Add("nelsoni", "test1", objIn, true);	// Same user, therefore allowed

            SerializableObject1 objOut = this.dataStorageService.Get("nelsoni", "test1").StoredObject as SerializableObject1;

            // Assert
            Assert.AreEqual(objIn, objOut);
        }

        [Test]
        public void LockingTest3()
        {
            // Arrange
            SerializableObject1 objIFN = MakeObject1();
            this.dataStorageService.Add("nelsoni", "test1", objIFN);	// no locking required

            SerializableObject1 objSH = MakeObject1();
            objSH.Quantity = objSH.Quantity * 3;
            this.dataStorageService.Add("harrisonsi", "test1", objSH);	// should work as no lock check

            SerializableObject1 objIFNOut = this.dataStorageService.Get("nelsoni", "test1").StoredObject as SerializableObject1;

            Assert.AreEqual(objIFN, objIFNOut);

            SerializableObject1 objSHOut = this.dataStorageService.Get("harrisonsi", "test1").StoredObject as SerializableObject1;

            // Assert
            Assert.AreEqual(objSH, objSHOut);
        }

        [Test]
        public void ClearTest()
        {
            // Arrange
            SerializableObject1 obj = MakeObject1();
            this.dataStorageService.Add("carterm", "apple", obj);
            this.dataStorageService.Add("middletonr", "tigger", obj, true);
            this.dataStorageService.Add("windridgep", "zog", obj, true);

            Assert.AreEqual(this.dataStorageService.PersistedObjects.Count, 3);

            // Act
            this.dataStorageService.Clear();

            // Assert
            Assert.AreEqual(this.dataStorageService.PersistedObjects.Count, 0);
        }

        [Test]
        public void RemoveTest()
        {
            // Arrange
            SerializableObject1 obj = MakeObject1();
            this.dataStorageService.Add("nelsoni", "ringo", obj);
            this.dataStorageService.Add("nelsoni", "george", obj);
            this.dataStorageService.Add("middletonr", "john", obj);
            this.dataStorageService.Add("windridgep", "paul", obj);
            this.dataStorageService.Add("harrisons", "george", obj);

            PersistedObjectCollection col1 = this.dataStorageService.GetByUser("nelsoni");
            Assert.AreEqual(col1.Count, 2);

            // Act
            this.dataStorageService.Remove("nelsoni", "george");

            col1 = this.dataStorageService.GetByUser("nelsoni");
            Assert.AreEqual(col1.Count, 1);

            Assert.AreEqual(col1[0].Key, "ringo");

            PersistedObject obj1 = this.dataStorageService.Get("harrisons", "george");
            Assert.IsNotNull(obj1);

            Assert.AreEqual(this.dataStorageService.PersistedObjects.Count, 4);
        }

        [Test]
        public void RemoveByUserTest()
        {
            // Arrange
            SerializableObject1 obj = MakeObject1();
            this.dataStorageService.Add("nelsoni", "ringo", obj);
            this.dataStorageService.Add("nelsoni", "george", obj);
            this.dataStorageService.Add("middletonr", "john", obj);
            this.dataStorageService.Add("windridgep", "paul", obj);
            this.dataStorageService.Add("harrisons", "george", obj);
            this.dataStorageService.Add("carterm", "george", obj);

            PersistedObjectCollection col1 = this.dataStorageService.GetByUser("nelsoni");
            Assert.AreEqual(col1.Count, 2);

            this.dataStorageService.RemoveByUser("nelsoni");

            col1 = this.dataStorageService.GetByUser("nelsoni");
            Assert.AreEqual(col1.Count, 0);

            Assert.AreEqual(this.dataStorageService.PersistedObjects.Count, 4);
        }

        [Test]
        public void RemoveByKeyTest()
        {
            SerializableObject1 obj = MakeObject1();
            this.dataStorageService.Add("nelsoni", "ringo", obj);
            this.dataStorageService.Add("nelsoni", "george", obj);
            this.dataStorageService.Add("middletonr", "john", obj);
            this.dataStorageService.Add("windridgep", "paul", obj);
            this.dataStorageService.Add("harrisons", "george", obj);
            this.dataStorageService.Add("carterm", "george", obj);

            PersistedObjectCollection col1 = this.dataStorageService.GetByKey("george");
            Assert.AreEqual(col1.Count, 3);

            this.dataStorageService.RemoveByKey("george");

            col1 = this.dataStorageService.GetByKey("george");
            Assert.AreEqual(col1.Count, 0);

            Assert.AreEqual(this.dataStorageService.PersistedObjects.Count, 3);
        }

        [Test]
        public void GetByUserTest()
        {
            SerializableObject1 obj = MakeObject1();
            this.dataStorageService.Add("nelsoni", "ringo", obj);
            this.dataStorageService.Add("nelsoni", "george", obj);
            this.dataStorageService.Add("nelsoni", "john", obj);
            this.dataStorageService.Add("windridgep", "paul", obj);
            this.dataStorageService.Add("harrisons", "george", obj);

            Assert.AreEqual(this.dataStorageService.GetByUser("nelsoni").Count, 3);
            Assert.AreEqual(this.dataStorageService.GetByUser("windridgep").Count, 1);
            Assert.AreEqual(this.dataStorageService.GetByUser("harrisons").Count, 1);

        }

        [Test]
        public void GetByKeyTest()
        {
            SerializableObject1 obj = MakeObject1();
            this.dataStorageService.Add("nelsoni", "ringo", obj);
            this.dataStorageService.Add("nelsoni", "george", obj);
            this.dataStorageService.Add("nelsoni", "john", obj);
            this.dataStorageService.Add("windridgep", "paul", obj);
            this.dataStorageService.Add("harrisons", "george", obj);

            Assert.AreEqual(this.dataStorageService.GetByKey("george").Count, 2);
            Assert.AreEqual(this.dataStorageService.GetByKey("john").Count, 1);
            Assert.AreEqual(this.dataStorageService.GetByKey("paul").Count, 1);
            Assert.AreEqual(this.dataStorageService.GetByKey("ringo").Count, 1);
        }

        [Test]
        public void NullKeyTest()
        {
            SerializableObject1 obj = MakeObject1();
            this.dataStorageService.Add("nelsoni", "ringo", obj);
            this.dataStorageService.Add("windridgep", "paul", obj);
            Assert.AreEqual(this.dataStorageService.GetByKey(null).Count, 2);
            Assert.AreEqual(this.dataStorageService.GetByUser(null).Count, 2);
            Assert.AreEqual(this.dataStorageService.GetByUser("windridgep").Count, 1);
            Assert.AreEqual(this.dataStorageService.GetByKey("ringo").Count, 1);
        }

        private SerializableObject1 MakeObject1()
        {
            SerializableObject1 obj = new SerializableObject1();
            obj.Name = "Ziggurat";
            obj.Quantity = 5146;
            obj.LineValue = 1235.97M;
            obj.CreationDate = DateTime.Now.Date.AddHours(15);
            return obj;
        }
    }
}
