using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using System.Threading;

namespace MG.Membership
{
    [Cmdlet(VerbsCommon.Get, "MyGroupMembership", ConfirmImpact = ConfirmImpact.None)]
    [Alias("whoami")]
    [OutputType(typeof(GroupMembership))]
    public class GetMyGroups : PSCmdlet
    {
        [Parameter(Mandatory = false, Position = 0)]
        [Alias("t")]
        public GroupType[] Type { get; set; }

        protected override void ProcessRecord()
        {
            string[] ts = null;
            if (Type != null)
            {
                ts = new string[Type.Length];
                for (int i1 = 0; i1 < Type.Length; i1++)
                {
                    var t = Type[i1];
                    ts[i1] = ReadEnumValue(t);
                }
            }

            base.ProcessRecord();
            string[] processed = ProcessStringOutput();
            var tempList = new List<GroupMembership>(processed.Length / 4);
            for (int i = 0; i < processed.Length; i+=4)
            {
                var col = new List<SpecialItem>(4);
                var one = processed[i+1];
                var si = new SpecialItem(one);
                if (ts != null && !ts.Contains(si.Value))
                    continue;

                else
                    col.Add(si);

                var two = processed[i];
                col.Add(new SpecialItem(two));
                var three = processed[i + 2];
                col.Add(new SpecialItem(three));
                var four = processed[i + 3];
                col.Add(new SpecialItem(four));

                var group = new GroupMembership(col);
                tempList.Add(group);
            }
            IEnumerable<GroupMembership> ien = tempList.OrderBy(x => x.GroupName);
            WriteObject(ien.OrderBy(x => x.Type), true);
        }

        private protected string[] ProcessStringOutput()
        {
            var proc = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = "whoami.exe",
                    Arguments = "/groups /fo list",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            proc.Start();
            while (!proc.HasExited)
            {
                Thread.Sleep(200);
            }
            var output = proc.StandardOutput.ReadToEnd();

            string[] strs = output.Split(new string[1] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var outArr = new string[strs.Length - 2];
            for (int i = 2; i < strs.Length; i++)
            {
                outArr[i-2] = strs[i];
            }
            return outArr;
        }

        private string ReadEnumValue(GroupType t)
        {
            FieldInfo fi = t.GetType().GetField(t.ToString());
            ValueAttribute att = ((fi.GetCustomAttributes(typeof(ValueAttribute), false)) as ValueAttribute[])[0];
            return att.Value;
        }
    }

    internal class ValueAttribute : Attribute
    {
        private string _val;
        public string Value => _val;

        public ValueAttribute(string val) => _val = val;
    }

    public enum GroupType : int
    {
        [Value("Well-known group")]
        WellKnown = 0,

        [Value("Alias")]
        Alias = 1,

        [Value("Group")]
        Group = 2,

        [Value("Label")]
        Label = 3
    }

    internal class SpecialItem
    {
        public string Key;
        public string Value;
        public SpecialItem(string go)
        {
            string[] split = go.Split(new string[1] { ":" }, StringSplitOptions.None);
            Key = split[0];
            Value = split[1].Trim();
        }
    }

    public sealed class GroupMembership
    {
        private string _n;
        private string _t;
        private string _sid;
        private string[] _atts;

        public string GroupName => _n ?? null;
        public string Type => _t ?? null;
        public string SID => _sid ?? null;
        public string[] Attributes => _atts ?? null;

        internal GroupMembership(List<SpecialItem> items)
        {
            if (items.Count != 4)
                throw new ArgumentException("If you specify any special items, there must be 4 of them!");

            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                switch (item.Key)
                {
                    case "Group Name":
                        _n = item.Value;
                        break;
                    case "Type":
                        _t = item.Value;
                        break;
                    case "SID":
                        _sid = item.Value;
                        break;
                    default:
                        _atts = item.Value.Split(new string[1] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        break;
                }
            }
        }
    }
}
