using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MG.Membership
{
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
}
