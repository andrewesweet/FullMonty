using System;

namespace FullMonty.AddIn
{
    public class WrongHandleTypeException : Exception
    {
        public WrongHandleTypeException(string handleName, string expectedType)
            : base($"'{handleName}' does not represent a {expectedType}")
        {
        }
    }
}