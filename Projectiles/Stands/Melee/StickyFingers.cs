using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TBM.Enums;
using Terraria;
using Terraria.ModLoader;

namespace TBM.Projectiles.Stands.Melee
{
    public class StickyFingers : FistStand
    {
        public override void SetSafeDefaults()
        {
            FistRushVelocity = new Vector2(0, 0);
            SpecialLengths = new[]
            {
                40,
                60,
                60,
                60
            };
            SpecialCosts = new[] { 0, 0, 0, 0 };
            StandHeight = 104;
            DefaultScale = 0.65f;
            FrameCap = 1;
            FrameOffset = 0;
        }
        public override string ShoutID
        {
            get
            {
                return "Sounds/Ari1";
            }
        }
        public override string ShortShoutID
        {
            get
            {
                return "Sounds/AriShort";
            }
        }
        public override void SpecialMove0(int length)
        {
            if (Owner.whoAmI == Main.myPlayer && FistRushVelocity == Vector2.Zero)
            {
                //Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, ShortShoutID).WithVolume(1f));
                FistRushVelocity = TBMUtils.DistanceToMouse(projectile.Center, 10f);
                if (Main.MouseWorld.X < Owner.Center.X)
                    Owner.direction = -1;
                else
                    Owner.direction = 1;
                TBMPlayer.Get(Owner).PreRushDirection = Owner.direction;
            }
            if (_attackCounter < 1)
            {
                Projectile.NewProjectile(projectile.Center + new Vector2(34 * Main.player[projectile.owner].direction, -14), FistRushVelocity, mod.ProjectileType<UnzippedStickyFist>(), 60, 2.5f, projectile.owner);
            }
            if (++_attackCounter < length)
            {
                FrameOffset = 5;
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
        public override void PostAI()
        {
            if (State == StandState.Idle)
            {
                FrameOffset = 0;
                FrameCap = 1;
            }
            if (State == StandState.RushAttackShort || State == StandState.RushAttackLong)
            {
                FrameCap = 4;
                FrameOffset = 1;
            }
        }
        public override Texture2D StandTexture
        {
            get
            {
                return TBMTextures.StickyFingers;
            }
        }
        public override int FistID
        {
            get
            {
                return mod.ProjectileType<StickyFist>();
            }
        }
    }
}