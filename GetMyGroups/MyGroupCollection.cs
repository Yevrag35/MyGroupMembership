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
        public MyGroup this[int index]
        {
            get
            {
                if (index >= 0)
                    return _list.Values[index];

                else
                {
                    int goHere = _list.Count + index;
                    return goHere >= 0 ? _list.Values[goHere] : default;
                }
            }
        }
        public MyGroup this[string groupName]
        {
            get
            {
                return _list.Values.FirstOrDefault(x => x.GroupName.Equals(groupName, StringComparison.CurrentCultureIgnoreCase));
            }
        }

        #endregion

        #region PROPERTIES
        public int Count => _list.Count;

        #endregion

        #region CONSTRUCTORS
        public MyGroupCollection()
            : this(0)
        {
        }
        public MyGroupCollection(int capacity)
        {
            _list = new SortedList<(GroupType, string), MyGroup>(capacity, new MyGroupComparer());
            _getKey = x => (x.Type, x.GroupName);
        }
        internal MyGroupCollection(MyGroup[] groups)
            : this((groups?.Length).GetValueOrDefault())
        {
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
        internal void Add(MyGroup group)
        {
            _list.Add(_getKey(group), group);
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
