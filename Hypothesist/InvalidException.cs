using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hypothesist
{
    public class InvalidException<T> : Exception
    {
        public InvalidException(string message, IEnumerable<T> matched,
            IEnumerable<T> unmatched) : base(ToString(message, matched, unmatched)) =>
            (Matched, Unmatched) = (matched, unmatched);

        public IEnumerable<T> Matched { get; }
        public IEnumerable<T> Unmatched { get; }

        private static string ToString(string message, IEnumerable<T> matched, IEnumerable<T> unmatched)
        {
            var sb = new StringBuilder(message);
            sb.AppendLine();

            ToString(sb, matched, "Matched:");
            sb.AppendLine();
            
            ToString(sb, unmatched, "Unmatched:");
            return sb.ToString();
        }

        private static void ToString(StringBuilder sb, IEnumerable<T> matched, string label)
        {
            sb.Append(label);
            foreach (var item in matched)
            {
                sb.AppendLine();
                sb.Append("* ");
                sb.Append(item);
            }
        }
    }
}