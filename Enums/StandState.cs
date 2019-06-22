using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBM.Enums
{
    public enum StandState : byte
    {
        Idle,
        RushAttackShort,
        RushAttackLong,
        Spawning,
        Despawning,
        SpecialMove0,
        SpecialMove1,
        SpecialMove2,
        SpecialMove3
    }
}
