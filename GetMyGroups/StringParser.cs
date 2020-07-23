using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MG.Membership
{
    public static class StringParser
    {
        public static (string, string) ParseLine(string single)
        {
            string[] split = single.Split(new string[1] { ":" }, StringSplitOptions.None);
            return (split[0], split[1].Trim());
        }
        public static IEnumerable<(string, string)> ParseLines(IEnumerable<string> lines)
        {
            foreach (string line in lines)
            {
                yield return ParseLine(line);
            }
        }
        public static (string, string[]) ParseAttributes(string line)
        {
            string[] split = line.Split(new string[1] { ":" }, StringSplitOptions.None);

            string[] atts = split.LastOrDefault().Split(new string[1] { "," }, StringSplitOptions.RemoveEmptyEntries);

            return (split[0], atts);

        }
    }
}
