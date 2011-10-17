using System;

namespace IanFNelson.DataStorage.Tests
{
    /// <summary>
    /// An object which cannot be serialized.
    /// </summary>
    public class NonSerializableObject
    {
        public NonSerializableObject() { }

        public string Ziggurat { get; set; }
    }
}
