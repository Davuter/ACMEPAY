using System;
using System.Collections.Generic;
using System.Text;

namespace Payment.Core.Application.Exceptions
{
    public class NotValidDataException : Exception
    {
        public NotValidDataException(string name, object[] key)
          : base($"Entity \"{name}\" ({string.Join("-",key)}) was not valid")
        {

        }
    }
}
