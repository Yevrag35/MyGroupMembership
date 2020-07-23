using System;
using System.Collections.Generic;
using System.Linq;

namespace MG.Membership.Internal
{
    internal static class StringParser
    {
        private static (string, string) ParseLine(string single)
        {
            string[] split = single.Split(new string[1] { ":" }, StringSplitOptions.None);
            return (split[0], split[1].Trim());
        }
        internal static IEnumerable<(string, string)> ParseLines(IEnumerable<string> lines)
        {
            foreach (string line in lines)
            {
                yield return ParseLine(line);
            }
        }
        internal static string[] ParseAttributes((string, string) line)
        {
            string[] split = line.Item2.Split(new string[1] { ":" }, StringSplitOptions.None);

            string[] atts = split.LastOrDefault().Split(new string[1] { "," }, StringSplitOptions.RemoveEmptyEntries);

            return atts;

        }

        //internal 
    }
}
