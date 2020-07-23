using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MG.Membership.Internal
{
    internal class GroupDictionary : IReadOnlyDictionary<string, GroupType>
    {
        private Dictionary<string, GroupType> _dict;

        public GroupType this[string key] => _dict[key];

        public GroupDictionary()
        {
            _dict = new Dictionary<string, GroupType>(4)
            {
                { "Well-known group", GroupType.WellKnownGroup },
                { "Alias", GroupType.Alias },
                { "Group", GroupType.Group },
                { "Label", GroupType.Label }
            };
        }

        public int Count => _dict.Count;
        public IEnumerable<string> Keys => _dict.Keys;
        public IEnumerable<GroupType> Values => _dict.Values;
        

        public bool ContainsKey(string key) => _dict.ContainsKey(key);
        public IEnumerator<KeyValuePair<string, GroupType>> GetEnumerator() => _dict.GetEnumerator();
        public bool TryGetValue(string key, out GroupType value) => _dict.TryGetValue(key, out value);
        IEnumerator IEnumerable.GetEnumerator() => _dict.GetEnumerator();
    }
}
