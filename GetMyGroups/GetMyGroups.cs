using MG.Posh.Extensions.Bound;
using MG.Membership.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using System.Reflection;

namespace MG.Membership
{
    [Cmdlet(VerbsCommon.Get, "MyGroupMembership", ConfirmImpact = ConfirmImpact.None)]
    [Alias("whoami")]
    [OutputType(typeof(MyGroup))]
    public class GetMyGroups : PSCmdlet
    {
        private IReadOnlyDictionary<string, GroupType> GroupTypes { get; } = new GroupDictionary();
        private GroupType[] _specifiedTypes;
        private List<string> _types { get; } = new List<string>();

        [Parameter(Mandatory = false, Position = 0)]
        [Alias("t")]
        public GroupType[] Type
        {
            get => _specifiedTypes;
            set
            {
                _specifiedTypes = value;
                foreach (KeyValuePair<string, GroupType> kvp in this.GroupTypes)
                {
                    foreach (GroupType gt in value)
                    {
                        if (kvp.Value == gt)
                        {
                            _types.Add(kvp.Key);
                        }
                    }
                }
            }
        }

        protected override void BeginProcessing()
        {
        }

        protected override void ProcessRecord()
        {

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

            IEnumerable<MyGroup> allGroups = MyGroup.CreateFromLines(linesList, this.GroupTypes);
            if (this.ContainsParameter(x => x.Type))
            {
                allGroups = allGroups.Where(x => _specifiedTypes.Contains(x.Type));
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
    }
}
