using MG.Posh.Extensions.Bound;
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
    [OutputType(typeof(MyGroup))]
    public class GetMyGroups : PSCmdlet
    {
        [Parameter(Mandatory = false, Position = 0)]
        [Alias("t")]
        public GroupType[] Type { get; set; }

        protected override void ProcessRecord()
        {
            string[] ts = null;
            if (this.ContainsParameter(x => x.Type))
            {
                ts = new string[Type.Length];
                for (int i1 = 0; i1 < Type.Length; i1++)
                {
                    GroupType t = Type[i1];
                    ts[i1] = ReadEnumValue(t);
                }
            }

            base.ProcessRecord();
            string[] processed = this.ProcessStringOutput();
            string[][] linesList = new string[processed.Length / 4][];
            for (int i = 0; i < (processed.Length / 4); i++)
            {
                linesList[i] = new string[4];
                linesList[i][0] = processed[(i * 4)];
                linesList[i][1] = processed[(i * 4) + 1];
                linesList[i][2] = processed[(i * 4) + 2];
                linesList[i][3] = processed[(i * 4) + 3];
            }

            IEnumerable<MyGroup> allGroups = MyGroup.CreateFromLines(linesList);
            if (this.ContainsParameter(x => x.Type))
            {
                allGroups = allGroups.Where(x => ts.Contains(x.Type));
            }
            base.WriteObject(allGroups, true);
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
            var lines = new List<string>(50);
            proc.Start();
            while (!proc.StandardOutput.EndOfStream)
            {
                string oneLine = proc.StandardOutput.ReadLine();
                if (!string.IsNullOrWhiteSpace(oneLine))
                    lines.Add(oneLine);
            }
            return lines.Skip(2).ToArray();
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
}
