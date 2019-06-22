using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBM
{
    /// <summary>
    /// This class handles miscelanious things related to time stop, such as modded NPCs being immune to it and such
    /// </summary>
    public static class TimeStopManagement
    {
        public static List<int> TimeStopImmuneNPCs;
        public static void Initialize()
        {
            TimeStopImmuneNPCs = new List<int>();
        }
    }
}
