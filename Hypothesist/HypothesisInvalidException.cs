using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hypothesist;

public class HypothesisInvalidException<T> : Exception
{
    public HypothesisInvalidException(string message, IEnumerable<T> matched, IEnumerable<T> unmatched) 
        : base(Format(message, matched, unmatched))
    {
        Matched = matched;
        Unmatched = unmatched;
    }

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

    private static void Section(StringBuilder sb, string label, IEnumerable<T> items)
    {
        sb.AppendLine(label);
        if (items.Any())
            List(sb, items);
        else
            None(sb);
    }

    private static void None(StringBuilder sb) => 
        sb.AppendLine("   <none>");

    private static void List(StringBuilder sb, IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            sb.Append("* ");
            sb.AppendLine(item?.ToString() ?? "null");
        }
    }
}