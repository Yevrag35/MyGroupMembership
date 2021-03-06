﻿using MG.Membership.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MG.Membership
{
    public class MyGroup
    {
        public string GroupName { get; }
        public GroupType Type { get; }
        public string SID { get; }
        public string[] Attributes { get; }

        private MyGroup(string name, GroupType type, string sid, string[] atts)
        {
            this.GroupName = name;
            this.Type = type;
            this.SID = sid;
            this.Attributes = atts;
        }

        public static MyGroup CreateFromLines(string[] lines, IReadOnlyDictionary<string, GroupType> groupTypes)
        {
            if (lines.Length != 4)
                throw new ArgumentException("Not enough lines to build an object.");

            string name = null;
            GroupType type = default;
            string sid = null;
            string[] atts = null;

            foreach ((string, string) tup in StringParser.ParseLines(lines))
            {
                switch (tup.Item1)
                {
                    case "Group Name":
                        name = tup.Item2;
                        break;

                    case "Type":
                        type = groupTypes[tup.Item2];
                        break;
                    case "SID":
                        sid = tup.Item2;
                        break;
                    default:
                        atts = StringParser.ParseAttributes(tup);
                        break;
                }
            }
            return new MyGroup(name, type, sid, atts);
        }

        public static MyGroup[] CreateFromLines(string[][] linesList, IReadOnlyDictionary<string, GroupType> groupTypes)
        {
            MyGroup[] groupArr = new MyGroup[linesList.Length];
            for (int i = 0; i < linesList.Length; i++)
            {
                groupArr[i] = CreateFromLines(linesList[i], groupTypes);
            }
            return groupArr;
        }
    }
}
