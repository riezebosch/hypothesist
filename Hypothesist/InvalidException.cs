using System;
using System.Collections.Generic;

namespace Hypothesist
{
    public class InvalidException<T> : Exception
    {
        public InvalidException(IEnumerable<T> matched, IEnumerable<T> unmatched) => 
            (Matched, Unmatched) = (matched, unmatched);
        
        public IEnumerable<T> Matched { get; }
        public IEnumerable<T> Unmatched { get; }
    }
}