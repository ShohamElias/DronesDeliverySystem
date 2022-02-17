using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
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
            public BadIdException(int id, string message) : base(message)
            { this.ID = id; }

            override public string ToString()
            { return "bad id:" + ID + "\n" + Message; }
        }

        public class BadInputException : Exception
        {
            public string Input { get; private set; }

            public BadInputException(string message) : base(message) { }
            public BadInputException(string id, string message, Exception inner) : base(message, inner) => Input = id;

            //   protected BadIdException(int idSerializationInfo info, StreamingContext context)
            //: base(info, context) { }
            //   // special constructor for our custom exception
            public BadInputException(string id, string message) : base(message)
            { this.Input = id; }

            override public string ToString()
            { return "bad Input:" + Input + "\n" + Message; }
        }

        [Serializable]
        public class NoMatchException : Exception
        {

            public NoMatchException(string message) : base(message) { }

            //   protected BadIdException(int idSerializationInfo info, StreamingContext context)
            //: base(info, context) { }
            //   // special constructor for our custom exception

            override public string ToString()
            { return "couldnt find any match \n" + Message; }
        }
        [Serializable]
        public class IDExistsException : Exception
        {
            public int ID { get; private set; }
            public IDExistsException() : base() { }
            public IDExistsException(int id) : base() => ID = id;
            public IDExistsException(string message) : base(message) { }
            public IDExistsException(int id, string message, Exception inner) : base(message, inner) => ID = id;

            //   protected BadIdException(int idSerializationInfo info, StreamingContext context)
            //: base(info, context) { }
            //   // special constructor for our custom exception
            public IDExistsException(int id, string message) : base(message)
            { this.ID = id; }

            override public string ToString()
            { return "ID already Exists:" + ID + "\n" + Message; }
        }

        [Serializable]
        public class WrongDroneStatException : Exception
        {
            public int ID { get; private set; }

            public WrongDroneStatException(int id) : base() => ID = id;
            public WrongDroneStatException(string message) : base(message) { }
            public WrongDroneStatException(int id, string message, Exception inner) : base(message, inner) => ID = id;

            //   protected BadIdException(int idSerializationInfo info, StreamingContext context)
            //: base(info, context) { }
            //   // special constructor for our custom exception
            public WrongDroneStatException(int id, string message) : base(message)
            { this.ID = id; }

            override public string ToString()
            { return "wrong status of drone:" + ID + "\n" + Message; }
        }

        [Serializable]
        public class BatteryIssueException : Exception
        {
            //do i need to print the battery to?
            public int ID { get; private set; }

            public BatteryIssueException(int id) : base() => ID = id;
            public BatteryIssueException(string message) : base(message) { }
            public BatteryIssueException(int id, string message, Exception inner) : base(message, inner) => ID = id;

            //   protected BadIdException(int idSerializationInfo info, StreamingContext context)
            //: base(info, context) { }
            //   // special constructor for our custom exception
            public BatteryIssueException(int id, string message) : base(message)
            { this.ID = id; }

            override public string ToString()
            { return "battery problem of drone:" + ID + "\n" + Message; }
        }


        [Serializable]
        public class StationProblemException : Exception
        {
          public int ID { get; private set; }
           public StationProblemException(int id) : base() => ID = id;
           public StationProblemException(string message) : base(message) { }
           public StationProblemException(int id, string message, Exception inner) : base(message, inner) => ID = id;

            public StationProblemException(int id, string message) : base(message)
            { this.ID = id; }

            override public string ToString()
            { return  Message; }
        }
    }
