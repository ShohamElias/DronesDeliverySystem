using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL.DO
{
    

        [Serializable]
        public class BadIdException : Exception
        {
            public int ID { get; private set; }

            public BadIdException(int id) : base() => ID = id;
            public BadIdException(string message) : base(message) { }
            public BadIdException(int id, string message, Exception inner) : base(message, inner) => ID = id;

         //   protected BadIdException(int idSerializationInfo info, StreamingContext context)
         //: base(info, context) { }
         //   // special constructor for our custom exception
            public BadIdException(int id,  string message) : base(message)
            { this.ID = id; }

            override public string ToString()
            { return "bad id:" + ID + "\n" + Message; }
        }


        [Serializable]
        public class IDExistsExceprion : Exception
        {
            public int ID { get; private set; }

            public IDExistsExceprion(int id) : base() => ID = id;
            public IDExistsExceprion(string message) : base(message) { }
            public IDExistsExceprion(int id, string message, Exception inner) : base(message, inner) => ID = id;

            //   protected BadIdException(int idSerializationInfo info, StreamingContext context)
            //: base(info, context) { }
            //   // special constructor for our custom exception
            public IDExistsExceprion(int id, string message) : base(message)
            { this.ID = id; }

            override public string ToString()
            { return "ID already Exists:" + ID + "\n" + Message; }
        }

}
