using System;
using System.Runtime.Serialization;

namespace Services
{
    public class DuplicateUserException : Exception
    {
        public DuplicateUserException()
        {
        }
    }
}
