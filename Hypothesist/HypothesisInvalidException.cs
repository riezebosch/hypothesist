using System.Text;

namespace Hypothesist;

public class HypothesisInvalidException<T>(string message, ICollection<T> matched, ICollection<T> unmatched)
    : Exception(Format(message, matched, unmatched))
{
    public IEnumerable<T> Matched { get; } = matched;
    public IEnumerable<T> Unmatched { get; } = unmatched;

    private static string Format(string message, ICollection<T> matched, ICollection<T> unmatched)
    {
        var sb = new StringBuilder(message)
            .AppendLine();

        Section(sb, "Matched:", matched);
        Section(sb, "Unmatched:", unmatched);
        
        return sb.ToString();
    }

    private static void Section(StringBuilder sb, string label, ICollection<T> items)
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