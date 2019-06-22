using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TBM.Enums;
using TBM.Projectiles;

namespace TBM
{
    public static class StandManager
    {
        public static List<StandBase> AllStands;
        public static List<StandBase> CloseRangeStands;
        public static List<StandBase> RangedStands;
        public static void Initalize()
        {
            AllStands = new List<StandBase>();
            CloseRangeStands = new List<StandBase>();
            RangedStands = new List<StandBase>();
            Assembly myAssembly = Assembly.GetAssembly(typeof(StandBase));
            foreach (Type type in myAssembly.GetTypes()
                .Where(myType => myType.IsClass
                && !myType.IsAbstract
                && myType.IsSubclassOf(typeof(StandBase))))
            {
                StandBase stand = (StandBase)Activator.CreateInstance(type);
                AllStands.Add(stand);
                switch(stand.Class)
                {
                    case StandClass.Melee:
                        CloseRangeStands.Add(stand);
                        break;
                    case StandClass.Ranged:
                        RangedStands.Add(stand);
                        break;
                    default:
                        CloseRangeStands.Add(stand);
                        break;
                }
            }
        }
        public static void Unload()
        {
            AllStands.Clear();
            CloseRangeStands.Clear();
        }
    }
}
