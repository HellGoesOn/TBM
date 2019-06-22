using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBM.Enums;
using Terraria;
using Terraria.ModLoader;

namespace TBM.Projectiles
{
    public abstract class FistStand : StandBase
    {
        public Vector2 FistRushVelocity;
        public int AttackLength = 60;
        public int FrameOffset = 0;
        public float DefaultScale = 0.7f;
        public int[] SpecialLengths;
        public int[] SpecialCosts;
        public bool PlayedShout = false;
        public override void SetSafeDefaults()
        { 
            SpecialLengths = new[] { 0, 0, 0, 0 };
            SpecialCosts = new[] { 0, 0, 0, 0 };
        }
        public virtual int FistID
        {
            get { return mod.ProjectileType<StarFist>(); }
        }
        public virtual string ShortShoutID
        {
            get { return "Sounds/OraShort"; }
        }
        public virtual string ShoutID
        {
            get { return "Sounds/Ora"; }
        }
        public override bool PreAI()
        {
            base.PreAI();
            projectile.netUpdate = true;
            projectile.netUpdate2 = true;
            HandleMeleeStandPreAI();
            if (Owner.whoAmI == Main.myPlayer)
            {
                HandleInputs();
            }
            switch (State)
            {
                case StandState.SpecialMove0:
                    {
                        SpecialMove0(SpecialLengths[0]);
                        break;
                    }
                case StandState.SpecialMove1:
                    {
                        SpecialMove1(SpecialLengths[1]);
                        break;
                    }
                case StandState.SpecialMove2:
                    {
                        SpecialMove2(SpecialLengths[2]);
                        break;
                    }
                case StandState.SpecialMove3:
                    {
                        SpecialMove3(SpecialLengths[3]);
                        break;
                    }
                case StandState.RushAttackShort:
                    {
                        RushAttackShort(60);
                        break;
                    }
                case StandState.RushAttackLong:
                    {
                        RushAttackLong(180);
                        break;
                    }
            }
            if (State == StandState.RushAttackShort && !PlayedShout)
            {
                PlayedShout = true;
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, ShortShoutID).WithVolume(1f));
            }
            if (State == StandState.RushAttackLong && !PlayedShout)
            {
                PlayedShout = true;
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, ShoutID).WithVolume(1f));
            }
            if(_attackCounter == 0)
            {
                PlayedShout = false;
            }
            _frameYOffset = FrameOffset;
            return true;
        }

        private void RushAttackShort(int length)
        {
            FrameSpeed = 1;
            if (Owner.whoAmI == Main.myPlayer && FistRushVelocity == Vector2.Zero)
            {
                FistRushVelocity = TBMUtils.DistanceToMouse(projectile.Center, 10f);
                if (Main.MouseWorld.X < Owner.Center.X)
                    Owner.direction = -1;
                else
                    Owner.direction = 1;
                TBMPlayer.Get(Owner).PreRushDirection = Owner.direction;
            }
            if (++_attackCounter < length)
            {
                TBMPlayer.Get(Owner).InRush = true;
                _positionOffsetX = 25;
                Projectile.NewProjectile(projectile.Center, FistRushVelocity, FistID, 60, 3.5f, projectile.owner);
            }
            else
            {
                FistRushVelocity = Vector2.Zero;
                _positionOffsetX = -20;
                State = StandState.Idle;
                _attackCounter = 0;
            }
        }
        private void RushAttackLong(int length)
        {
            FrameSpeed = 1;
            if (Owner.whoAmI == Main.myPlayer && FistRushVelocity == Vector2.Zero)
            {
                FistRushVelocity = TBMUtils.DistanceToMouse(projectile.Center, 10f);
                if (Main.MouseWorld.X < Owner.Center.X)
                    Owner.direction = -1;
                else
                    Owner.direction = 1;
                TBMPlayer.Get(Owner).PreRushDirection = Owner.direction;
            }
            if (++_attackCounter < length)
            {
                TBMPlayer.Get(Owner).InRush = true;
                _positionOffsetX = 25;
                Projectile.NewProjectile(projectile.Center, FistRushVelocity, FistID, 60, 3.5f, projectile.owner);
            }
            else
            {
                FistRushVelocity = Vector2.Zero;
                _positionOffsetX = -20;
                State = StandState.Idle;
                _attackCounter = 0;
            }
        }
        public void HandleInputs()
        {
            if (Owner.controlUseItem)
            {
                if (State == StandState.Idle)
                {
                    State = StandState.RushAttackShort;
                    if (Owner.controlDown && CheckCost(0))
                        State = StandState.SpecialMove0;
                    if (Owner.controlUp && CheckCost(1))
                        State = StandState.SpecialMove1;

                }
            }
            if (Owner.controlUseTile)
            {
                if (State == StandState.Idle)
                {
                    if (Owner.controlDown && CheckCost(2))
                        State = StandState.SpecialMove2;
                    if (Owner.controlUp && CheckCost(3))
                        State = StandState.SpecialMove3;
                }
            }
        }
        public bool CheckCost(int specialIndex)
        {
            if (TBMPlayer.Get(Owner).Stamina >= SpecialCosts[specialIndex])
            {
                TBMPlayer.Get(Owner).Stamina -= SpecialCosts[specialIndex];
                return true;
            }
            return false;
        }
        public virtual void SpecialMove0(int length)
        {
            State = StandState.Idle;
        }
        public virtual void SpecialMove1(int length)
        {
            State = StandState.Idle;
        }
        public virtual void SpecialMove2(int length)
        {
            State = StandState.Idle;
        }
        public virtual void SpecialMove3(int length)
        {
            State = StandState.Idle;
        }
        private void HandleMeleeStandPreAI()
        {
            FrameSpeed = 3;
            switch (State)
            {
                case StandState.Spawning:
                    if(_positionOffsetX >= 0)
                    {
                        Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/" + GetType().Name + "_Deploy"));
                    }
                    if (_positionOffsetX > -20)
                    {
                        _positionOffsetX -= 2;
                    }
                    if (_positionOffsetX > -18)
                    {
                        Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/" + GetType().Name + "_Deploy2"));
                    }
                    if (_positionOffsetY > -20)
                    {
                        _positionOffsetY -= 2;
                    }
                    if (_alpha < 1.3) _alpha += 0.1f;
                    else
                        State = StandState.Idle;
                    break;
                case StandState.Despawning:
                    if (_positionOffsetX < 0)
                    {
                        _positionOffsetX++;
                    }
                    if (_positionOffsetY < 0)
                    {
                        _positionOffsetY++;
                    }
                    if (_alpha > 0) _alpha -= 0.1f;
                    else
                        projectile.Kill();
                    break;
            }
            projectile.position.X = State == StandState.RushAttackShort || State == StandState.RushAttackLong ?
                Owner.Center.X + _positionOffsetX * Owner.direction :
                Vector2.Lerp(projectile.Center, Owner.Center + new Vector2(_positionOffsetX * Owner.direction, 0), 0.1f + Owner.velocity.X / 32f * Owner.direction).X;
            projectile.position.Y = Owner.Center.Y + _positionOffsetY;
            _standSpriteEffects = Owner.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 drawOrigin = new Vector2(StandTexture.Width / 2, StandTexture.Height / TotalFrameCount / 2);
            Rectangle CurrentFrame = new Rectangle(0, StandHeight * _frameY, StandTexture.Width, StandHeight);
            if (TBMPlayer.Get(Main.player[Main.myPlayer]).IsStandUser)
            {
                spriteBatch.Draw(StandTexture, new Vector2((int)projectile.Center.X, (int)projectile.Center.Y) - Main.screenPosition, CurrentFrame, Color.White * _alpha, 0f, drawOrigin, DefaultScale, _standSpriteEffects, 1f);
            }
        }
    }
}
