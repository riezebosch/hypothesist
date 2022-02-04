using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hypothesist;

public class InvalidException<T> : Exception
{
    public InvalidException(string message, IEnumerable<T> matched,
        IEnumerable<T> unmatched) : base(Format(message, matched, unmatched)) =>
        (Matched, Unmatched) = (matched, unmatched);

    public IEnumerable<T> Matched { get; }
    public IEnumerable<T> Unmatched { get; }

    private static string Format(string message, IEnumerable<T> matched, IEnumerable<T> unmatched)
    {
        var sb = new StringBuilder(message)
            .AppendLine();

        Section(sb, "Matched:", matched);
        Section(sb, "Unmatched:", unmatched);
        
        return sb.ToString();
    }

    private static StringBuilder Section(StringBuilder sb, string label, IEnumerable<T> items)
    {
        sb.AppendLine(label);
        return items.Any() 
            ? List(sb, items)
            : None(sb);
    }

    private static StringBuilder None(StringBuilder sb) => 
        sb.AppendLine("   <none>");

    private static StringBuilder List(StringBuilder sb, IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            sb.Append("* ");
            sb.AppendLine(item.ToString());
        }

        return sb;
    }
}