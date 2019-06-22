using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBM;
using TBM.Enums;
using TBM.Projectiles.Stands;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TBM.Projectiles
{
    public abstract class StandBase : ModProjectile
    {
        private Texture2D _standTexture = TBMTextures.EmptyTexture;
        public SpriteEffects _standSpriteEffects = SpriteEffects.None;
        //Sealing off texture so it cannot be modified by derived classes
        public sealed override string Texture
        {
            get
            {
                return TBMTextures.EmptyFull;
            }
        }
        //I did the above because the texture stand will use will be this
        public virtual Texture2D StandTexture
        {
            get { return _standTexture; }
            set { _standTexture = value; }
        }
        public Player Owner
        {
            get { return Main.player[projectile.owner]; }
        }
        public int _attackCounter;
        public sealed override void SetDefaults()
        {
            SetSafeDefaults();
            projectile.width = 1;
            projectile.tileCollide = false;
            projectile.height = 1;
        }
        public virtual void SetSafeDefaults()
        {
        }
        public StandClass Class = StandClass.Melee;
        public StandState State = StandState.Spawning;
        public int _positionOffsetX = 0;
        public int _positionOffsetY = 0;
        public float _alpha = 0;
        public int _frameY = 0;
        public int _frameYOffset = 0;
        private int _frameCap;
        public int FrameSpeed = 3;
        public virtual int FrameCap
        {
            get { return _frameCap; }
            set { _frameCap = value; }
        }
        public int TotalFrameCount
        {
            get { return StandTexture.Height / StandHeight; }
        }
        public int StandHeight = 1;
        public override bool PreAI()
        {
            projectile.netUpdate = true;
            projectile.netUpdate2 = true;
            projectile.timeLeft = 2;
            if (Owner.dead)
                projectile.Kill();

            if (++projectile.frameCounter > FrameSpeed)
            {
                if(++_frameY > FrameCap + _frameYOffset- 1)
                {
                    _frameY = 0 + _frameYOffset;
                }
                projectile.frameCounter = 0;
            }
            if (_frameY < _frameYOffset)
                _frameY = _frameYOffset;

            return base.PreAI();
        }

        public sealed override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write((byte)State);
            writer.Write((byte)_standSpriteEffects);
            writer.Write((int)_positionOffsetX);
            writer.Write((int)_attackCounter);
        }
        public sealed override void ReceiveExtraAI(BinaryReader reader)
        {
            State = (StandState)reader.ReadByte();
            _standSpriteEffects = (SpriteEffects)reader.ReadByte();
            _positionOffsetX = reader.ReadInt32();
            _attackCounter = reader.ReadInt32();
        }
    }
}
