using System;

namespace IanFNelson.DataStorage.Tests
{
    [Serializable()]
    public class SerializableObject1
    {
        public SerializableObject1() { }

        public string Name { get; set; }

        public Decimal LineValue { get; set; }

        public int Quantity { get; set; }

        public DateTime CreationDate { get; set; }

        public override bool Equals(object obj)
        {
            if ( !( obj is SerializableObject1 ) )
            {
                return false;
            }

            SerializableObject1 objCast = obj as SerializableObject1;

            if ( objCast.CreationDate != CreationDate )
                return false;

            if ( objCast.LineValue != LineValue )
                return false;
            if ( objCast.Name != Name )
                return false;
            if ( objCast.Quantity != Quantity )
                return false;

            return true;
        }


    }
}
