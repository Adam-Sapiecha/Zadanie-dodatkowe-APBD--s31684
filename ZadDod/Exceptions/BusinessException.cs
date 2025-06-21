using System;

namespace ZadDod.Exceptions;

    public class BusinessException : Exception
    {
        public BusinessException(string message) : base(message) { }
    }
