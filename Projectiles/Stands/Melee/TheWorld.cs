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
    public class TheWorld : FistStand
    {
        public override void SetSafeDefaults()
        {
            SpecialLengths = new[] { 80, 30, 0, 0 };
            SpecialCosts = new[] { 50, 10, 0, 0 };
            FistRushVelocity = new Vector2(0, 0);
            StandHeight = 116;
            FrameCap = 1;
        }
        public override int FistID
        {
            get
            {
                return mod.ProjectileType<TheWorldFist>();
            }
        }
        public override string ShortShoutID
        {
            get
            {
                return "Sounds/MudaShort";
            }
        }
        public override string ShoutID
        {
            get
            {
                return "Sounds/MudaShort";
            }
        }
        public override void PostAI()
        {
            if (State == StandState.SpecialMove0 && _attackCounter > 10 && _attackCounter < 75 || State == StandState.SpecialMove1)
            {
                StandTexture = TBMTextures.TheWorldTimeStop;
            }
            else
            {
                StandTexture = TBMTextures.TheWorld;
            }
            if (State == StandState.RushAttackShort || State == StandState.RushAttackLong)
            {
                FrameCap = 4;
                FrameOffset = 1;
            }
            else
            {
                FrameCap = 1;
                FrameOffset = 0;
            }
            if(TBMWorld.Get().TimeStopDuration > 0)
            {
                SpecialCosts[0] = 0;
            }
            else
            {
                SpecialCosts[0] = 50;
            }
        }
        public override void SpecialMove0(int length)
        {
            int duration = 420;
            if (mod.GetModWorld<TBMWorld>().TimeStopDuration <= 0)
            {
                if (_attackCounter == 0)
                {
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/" + GetType().Name + "_Deploy"));
                }
                if (++_attackCounter >= length)
                {
                    _attackCounter = 0;
                    State = StandState.Idle;
                }
                if (_attackCounter == 30)
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/" + GetType().Name + "_ZaWarudoSFX"));
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
        public override void SpecialMove1(int length)
        {
            if (Owner.whoAmI == Main.myPlayer && FistRushVelocity == Vector2.Zero)
            {
                FistRushVelocity = TBMUtils.DistanceToMouse(projectile.Center, 10f);
                if (Main.MouseWorld.X < Owner.Center.X)
                    Owner.direction = -1;
                else
                    Owner.direction = 1;
                TBMPlayer.Get(Owner).PreRushDirection = Owner.direction;
            }
            if (_attackCounter < 1)
            {
                for (int i = 0; i < 8; i++)
                {
                    Projectile.NewProjectile(projectile.Center + new Vector2(Main.rand.Next(-16, 16), Main.rand.Next(-25, 25)).RotatedBy(FistRushVelocity.SafeNormalize(-Vector2.UnitY).ToRotation()), FistRushVelocity, mod.ProjectileType<Knife>(), 22, 0f, projectile.owner);
                }
            }
            if (++_attackCounter < length)
            {
                _positionOffsetX = 25;
            }
            else
            {
                FistRushVelocity = Vector2.Zero;
                _positionOffsetX = -20;
                State = StandState.Idle;
                _attackCounter = 0;
            }
        }
    }
}
