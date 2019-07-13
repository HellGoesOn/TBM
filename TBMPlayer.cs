using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using TBM.Projectiles;
using Microsoft.Xna.Framework.Input;
using Terraria.ModLoader.IO;
using TBM.Projectiles.Stands.Melee;
using TBM.Enums;
using TBM.Projectiles.Stands;
using Terraria.DataStructures;

namespace TBM
{
    public class TBMPlayer : ModPlayer
    {
        public StandBase MyStand;
        public Projectile MyStandProjectile;
        public Vector2 PanTarget;
        public int PreRushDirection;
        public bool InRush;
        private double _preTimeStopTime;
        private bool _playedSound;
        private int _stamina;
        public int Stamina
        {
            get
            {
                if (_stamina > StaminaMax)
                    _stamina = StaminaMax;
                return _stamina;
            }
            set
            {
                _stamina = value < 0 ? 0 : value > StaminaMax ? StaminaMax : value;
            }
        }
        public int StaminaMax;
        public bool IsStandUser
        {
            get { return MyStand != null && !(MyStand is EmptyStand); }
            set { IsStandUser = value; }
        }
        public bool IsStopped
        {
            get
            {
                TBMWorld world = mod.GetModWorld<TBMWorld>();
                return world.TimeStopDuration > 0 && world.TimeStopOwner != player.whoAmI;
            }
        }
        public static TBMPlayer Get(Player player)
        {
            return player.GetModPlayer<TBMPlayer>();
        }
        public override void Initialize()
        {
            Stamina = 0;
            StaminaMax = 100;
            MyStand = new EmptyStand();
        }
        public override TagCompound Save()
        {
            return new TagCompound()
                {
                    {"MyStand", MyStand.GetType().Name}
                };
        }
        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            TBMPlayer tbm = clientPlayer as TBMPlayer;
            if (tbm.MyStandProjectile != null && MyStandProjectile != null)
            {
                if (tbm.MyStandProjectile.whoAmI != MyStandProjectile.whoAmI)
                {
                    ModPacket modPacket = mod.GetPacket();
                    modPacket.Write((byte)MessageType.MyStandID);
                    modPacket.Write((int)player.whoAmI);
                    modPacket.Write((int)MyStandProjectile.whoAmI);
                    modPacket.Send();
                }
                if (tbm.InRush != InRush)
                {
                    ModPacket modPacket = mod.GetPacket();
                    modPacket.Write((byte)MessageType.IsInRush);
                    modPacket.Write((int)player.whoAmI);
                    modPacket.Write((bool)InRush);
                    modPacket.Send();
                }
            }
        }
        public override void Load(TagCompound tag)
        {
            string standName = tag.GetString("MyStand");
            if (standName != "FistStand")
                MyStand = (StandBase)Activator.CreateInstance((StandManager.AllStands.Find(x => x.GetType().Name == standName).GetType()));
            else
                MyStand = new EmptyStand();
        }
        public override void ModifyScreenPosition()
        {
            if (PanTarget != Vector2.Zero && PanTarget != null)
            {
                TBMUtils.PanCamera(PanTarget);
            }
            PanTarget = Vector2.Zero;
        }
        public override void SetControls()
        {
            if (MyStandProjectile != null && MyStandProjectile.active)
            {
                if (InRush)
                {
                    player.controlDown =
                        player.controlJump =
                    player.controlUp = false;
                    player.controlLeft = PreRushDirection == -1;
                    player.controlRight = PreRushDirection == 1;
                }
            }
            if (IsStopped)
            {
                BlockInputs();
            }
        }
        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if(!IsStopped && mod.GetModWorld<TBMWorld>().TimeStopDuration > 0 && mod.GetModWorld<TBMWorld>().TimeStopOwner == player.whoAmI)
            {
                return false;
            }
            return base.PreHurt(pvp, quiet, ref damage, ref hitDirection, ref crit, ref customDamage, ref playSound, ref genGore, ref damageSource);
        }
        public override void PostHurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit)
        {
            if(damage >= 5)
                Stamina += 5;
        }
        public override void ResetEffects()
        {
            if (MyStandProjectile != null && !MyStandProjectile.active)
                MyStandProjectile = null;
            MiscelaniousTimeStopEffects();

            if (InRush)
            {
                player.velocity.X *= 0.6f;
                player.velocity.Y *= 0.1f;
            }
            if (IsStopped)
            {
                player.velocity *= 0;
                player.position = player.oldPosition;
            }
            if(player.name == "Conrad")
            {
                StaminaMax = 300;
                Stamina = 300;
            }
            else
                StaminaMax = 50;
            InRush = false;
        }

        private void MiscelaniousTimeStopEffects()
        {
            if (mod.GetModWorld<TBMWorld>().TimeStopDuration > 0)
            {
                for (int i = 0; i < Main.tileFrameCounter.Length; i++)
                {
                    Main.tileFrameCounter[i] = 0;
                }
                Main.windSpeed = 0;
                Main.time = _preTimeStopTime;
                foreach(Rain rain in Main.rain)
                {
                    rain.velocity *= 0;
                }
                TBMWorld world = mod.GetModWorld<TBMWorld>();
                if (world.TimeStopDuration <= 75)
                {
                    if (!_playedSound)
                    {
                        if (Get(Main.player[world.TimeStopOwner]).MyStand is StarPlatinum)
                        {
                            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/StarPlatinum_ZaWarudoReleaseSFX"));
                        }
                        if (Get(Main.player[world.TimeStopOwner]).MyStand is TheWorld)
                        {
                            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/TheWorld_ZaWarudoReleaseSFX"));
                        }
                        _playedSound = true;
                    }
                }
                else
                    _playedSound = false;
            }
            else
            {
                _preTimeStopTime = Main.time;
            }
        }

        private void BlockInputs()
        {
            player.controlDown = false;
            player.controlUp = false;
            player.controlLeft = false;
            player.controlRight = false;
            player.controlJump = false;
            player.controlUseItem = false;
            player.controlUseTile = false;
            player.controlSmart = false;
            player.controlMount = false;
            player.controlQuickHeal = false;
            player.controlQuickMana = false;
            player.controlHook = false;
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (TBMInput.ActivateStand.JustPressed && MyStand != null)
            {

                if (MyStandProjectile == null)
                {
                    int projectile = Projectile.NewProjectile(player.Center, Vector2.Zero, mod.ProjectileType(MyStand.GetType().Name), 0, 0, Main.myPlayer);
                    MyStandProjectile = Main.projectile[projectile];
                }
                else
                {
                    StandBase stand = MyStandProjectile.modProjectile as StandBase;
                    if (stand.State == StandState.Idle)
                    {
                        stand.State = StandState.Despawning;
                        MyStandProjectile = null;
                    }
                }
            }
        }
    }
}
