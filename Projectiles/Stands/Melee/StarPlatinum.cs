using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TBM.Enums;
using TBM.Projectiles.Misc;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TBM.Projectiles.Stands.Melee
{
    public class StarPlatinum : FistStand
    {
        public override void SetSafeDefaults()
        {
            SpecialLengths = new[] { 80, 0, 0, 0 };
            SpecialCosts = new[] { 100, 0, 0, 0 };
            FistRushVelocity = new Vector2(0, 0);
            StandHeight = 96;
            FrameCap = 4;
        }
        public override void PostAI()
        {
            if (State == StandState.RushAttackShort || State == StandState.RushAttackLong)
                FrameOffset = 4;
            else
                FrameOffset = 0;
            if (TBMWorld.Get().TimeStopDuration > 0)
            {
                SpecialCosts[0] = 0;
            }
            else
            {
                SpecialCosts[0] = 50;
            }
        }
        public override Texture2D StandTexture
        {
            get
            {
                return TBMTextures.StarPlatinum;
            }
        }
        public override void SpecialMove0(int length)
        {
            int duration = 420;
            if (mod.GetModWorld<TBMWorld>().TimeStopDuration <= 0)
            {
                if (_attackCounter == 0)
                {
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/" + GetType().Name + "_ZaWarudo"));
                }
                if (_attackCounter == 10)
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/" + GetType().Name + "_ZaWarudoSFX"));
                if (++_attackCounter >= length)
                {
                    _attackCounter = 0;
                    State = StandState.Idle;
                }
                if (_attackCounter == 35)
                {
                    Projectile.NewProjectile(projectile.Center, Vector2.Zero, mod.ProjectileType<TheWorldVFX>(), 0, 0, projectile.owner, 0.01f);
                }
                if (_attackCounter == 55)
                {
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        ModPacket packet = mod.GetPacket();
                        packet.Write((byte)MessageType.TimeStopDuration);
                        packet.Write((int)projectile.owner);
                        packet.Write((int)duration);
                        packet.Send();
                    }
                    else
                    {
                        mod.GetModWorld<TBMWorld>().TimeStopDuration = duration;
                        mod.GetModWorld<TBMWorld>().TimeStopOwner = Owner.whoAmI;
                    }
                }
            }
            else
            {
                _attackCounter = 0;
                State = StandState.Idle;
            }
        }
    }
}
