using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MG.Membership
{
    public class MyGroup
    {
        public string GroupName { get; private set; }
        public string Type { get; private set; }
        public string SID { get; private set; }
        public string[] Attributes { get; private set; }

        private MyGroup() { }

        public static MyGroup CreateFromLines(string[] lines)
        {
            if (lines.Length != 4)
                throw new ArgumentException("Not enough lines to build an object.");

            var myGroup = new MyGroup();
            foreach ((string, string) tup in StringParser.ParseLines(lines))
            {
                switch (tup.Item1)
                {
                    case "Group Name":
                        myGroup.GroupName = tup.Item2;
                        break;

                    case "Type":
                        myGroup.Type = tup.Item2;
                        break;
                    case "SID":
                        myGroup.SID = tup.Item2;
                        break;
                    default:
                        myGroup.Attributes = tup.Item2.Split(new string[1] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        break;
                }
            }
            return myGroup;
        }

        public static MyGroup[] CreateFromLines(string[][] linesList)
        {
            MyGroup[] groupArr = new MyGroup[linesList.Length];
            for (int i = 0; i < linesList.Length; i++)
            {
                groupArr[i] = CreateFromLines(linesList[i]);
            }
            return groupArr;

            //foreach (string[] lines in linesList)
            //{
            //    yield return CreateFromLines(lines);
            //}
        }
    }
}
