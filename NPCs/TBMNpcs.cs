using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TBM.NPCs
{
    public class TBMNpcs : GlobalNPC
    {
        private Vector2 _preTimeStopPosition;
        private int _me = -99;
        public bool IsStopped
        {
            get
            {
                return !TimeStopManagement.TimeStopImmuneNPCs.Contains(_me) && mod.GetModWorld<TBMWorld>().TimeStopDuration > 0;
            }
        }
        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }
        public override bool PreAI(NPC npc)
        {
            if(_me == -99)
            {
                _me = npc.type;
            }
            if (!IsStopped)
            {
                _preTimeStopPosition = npc.position;
            }
            if (IsStopped)
            {
                if (_preTimeStopPosition != Vector2.Zero)
                    npc.position = _preTimeStopPosition;
                if (npc.type == NPCID.QueenBee || npc.type == NPCID.GolemFistLeft || npc.type == NPCID.GolemFistRight)
                {
                    for (int i = 0; i < npc.ai.Length; i++)
                    {
                        npc.ai[i] = 0;
                    }
                }
                npc.frameCounter = 0;
                npc.velocity *= 0f;
            }
            return !IsStopped;
        }
        public override void ResetEffects(NPC npc)
        {
            if (IsStopped)
                npc.velocity *= 0;
        }
    }
}
