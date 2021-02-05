using MG.Membership.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MG.Membership
{
    public class MyGroupCollection : IEnumerable<MyGroup>
    {
        private SortedList<(GroupType, string), MyGroup> _list;
        private Func<MyGroup, (GroupType, string)> _getKey;

        #region COMPARER CLASS
        private class MyGroupComparer : IComparer<(GroupType, string)>
        {
            public int Compare((GroupType, string) x, (GroupType, string) y)
            {
                int final = x.Item1.CompareTo(y.Item1);
                if (final == 0)
                {
                    final = StringComparer.CurrentCultureIgnoreCase.Compare(x.Item2, y.Item2);
                }
                return final;
            }
        }

        #endregion

        #region INDEXERS
        public MyGroup this[int index] => _list.Values[index];

        #endregion

        #region PROPERTIES
        public int Count => _list.Count;

        #endregion

        #region CONSTRUCTORS
        internal MyGroupCollection(params MyGroup[] groups)
        {
            _list = new SortedList<(GroupType, string), MyGroup>((groups?.Length).GetValueOrDefault(), new MyGroupComparer());
            _getKey = x => (x.Type, x.GroupName);
            if (null != groups)
            {
                foreach (MyGroup myGroup in groups)
                {
                    this.Add(myGroup);
                }
            }
        }

        #endregion

        #region METHODS
        private void Add(MyGroup group)
        {
            _list.Add(_getKey(group), group);
        }

        public IEnumerable<MyGroup> GroupsOfType(params GroupType[] types)
        {
            if (types == null || types.Length <= 0)
                return null;

            return _list.Values.Where(x => types.Contains(x.Type));
        }

        public IEnumerator<MyGroup> GetEnumerator()
        {
            return _list.Values.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }



        #endregion
    }
}
