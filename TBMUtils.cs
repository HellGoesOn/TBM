using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.ModLoader;

namespace TBM
{
    public static class TBMUtils
    {
        public static Vector2 MuzzleOffsets(Vector2 position, float speedX, float speedY, float offset)
        {
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * offset;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                return position + muzzleOffset;
            }
            return position;
        }
        public static Vector2 DistanceToMouse(Player player, float speed)
        {
            return (Main.MouseWorld - player.Center).SafeNormalize(-Vector2.UnitY) * speed;
        }
        public static Vector2 DistanceToMouse(Vector2 position, float speed)
        {
            return (Main.MouseWorld - position).SafeNormalize(-Vector2.UnitY) * speed;
        }
        public static void CombatTexts(string text, Vector2 position, Color color, int w, int h, bool dramatic = false)
        {
            CombatText.NewText(new Rectangle((int)position.X, (int)position.Y, w, h), color, text, dramatic, false);
        }
        /// <summary>
        /// Splices text
        /// </summary>
        /// <param name="text"></param>
        /// <param name="lineLength">Character count after which it splices the text</param>
        /// <returns></returns>
        public static string SpliceText(string text, int lineLength)
        {
            return Regex.Replace(text, "(.{" + lineLength + "})" + ' ', "$1" + Environment.NewLine);
        }
        /// <summary>
        /// Used for chasing projectile AI
        /// </summary>
        /// <param name="projectile"></param>
        /// <param name="distance"></param>
        public static void ChaseAI(Projectile projectile, float distance = 600f)
        {
            if (projectile.localAI[0] == 0f)
            {
                AdjustMagnitude(ref projectile.velocity);
                projectile.localAI[0] = 1f;
            }
            Vector2 move = Vector2.Zero;
            bool target = false;
            for (int k = 0; k < 200; k++)
            {
                if (Main.npc[k].CanBeChasedBy(projectile, false))
                {
                    Vector2 newMove = Main.npc[k].Center - projectile.Center;
                    float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                    if (distanceTo < distance)
                    {
                        move = newMove;
                        distance = distanceTo;
                        target = true;
                    }
                }
            }
            if (target)
            {
                AdjustMagnitude(ref move);
                projectile.velocity = (3 * projectile.velocity + move);
                AdjustMagnitude(ref projectile.velocity);
            }
        }
        private static void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 6f)
            {
                vector *= 12f / magnitude;

            }
        }
        public static void CircleDust(Vector2 pos, Vector2 vel, int dustID)
        {
            float count = 25.0f;
            for (int k = 0; (double)k < (double)count; k++)
            {
                Vector2 vector2 = (Vector2.UnitX * 0.0f + -Vector2.UnitY.RotatedBy((double)k * (6.22 / (double)count), new Vector2()) * new Vector2(2.0f, 8.0f)).RotatedBy((double)vel.ToRotation(), new Vector2());
                int dust = Dust.NewDust(pos - new Vector2(0.0f, 4.0f), 1, 1, dustID, 0f, 0f, 200, Scale: 1.55f);
                Main.dust[dust].scale = 1.25f;
                Main.dust[dust].noGravity = true;
                Main.dust[dust].position = pos + vector2;
                Main.dust[dust].velocity = vel * 0.0f + vector2.SafeNormalize(Vector2.UnitY) * 1.0f;
            }
        }
        public static void CircleDust(Vector2 pos, Vector2 vel, int dustID, float width = 2, float height = 8, float scale = 1.55f, float count = 25.0f)
        {
            for (int k = 0; (double)k < (double)count; k++)
            {
                Vector2 vector2 = (Vector2.UnitX * 0.0f + -Vector2.UnitY.RotatedBy((double)k * (6.22 / (double)count), new Vector2()) * new Vector2(width, height)).RotatedBy((double)vel.ToRotation(), new Vector2());
                int dust = Dust.NewDust(pos - new Vector2(0f, 4f), 1, 1, dustID, 0f, 0f, 200, Scale: scale);
                Main.dust[dust].scale = scale;
                Main.dust[dust].noGravity = true;
                Main.dust[dust].position = pos + vector2;
                Main.dust[dust].velocity = vel * 0.0f + vector2.SafeNormalize(Vector2.UnitY) * 1.0f;
            }
        }
        public static void Explode(Vector2 position, int explosionRadius = 5)
        {

            int minTileX = (int)(position.X / 16f - (float)explosionRadius);
            int maxTileX = (int)(position.X / 16f + (float)explosionRadius);
            int minTileY = (int)(position.Y / 16f - (float)explosionRadius);
            int maxTileY = (int)(position.Y / 16f + (float)explosionRadius);
            if (minTileX < 0)
            {
                minTileX = 0;
            }
            if (maxTileX > Main.maxTilesX)
            {
                maxTileX = Main.maxTilesX;
            }
            if (minTileY < 0)
            {
                minTileY = 0;
            }
            if (maxTileY > Main.maxTilesY)
            {
                maxTileY = Main.maxTilesY;
            }
            bool canKillWalls = false;
            for (int x = minTileX; x <= maxTileX; x++)
            {
                for (int y = minTileY; y <= maxTileY; y++)
                {
                    float diffX = Math.Abs((float)x - position.X / 16f);
                    float diffY = Math.Abs((float)y - position.Y / 16f);
                    double distance = Math.Sqrt((double)(diffX * diffX + diffY * diffY));
                    if (distance < (double)explosionRadius && Main.tile[x, y] != null && Main.tile[x, y].wall == 0)
                    {
                        canKillWalls = true;
                        break;
                    }
                }
            }
            AchievementsHelper.CurrentlyMining = true;
            for (int i = minTileX; i <= maxTileX; i++)
            {
                for (int j = minTileY; j <= maxTileY; j++)
                {
                    float diffX = Math.Abs((float)i - position.X / 16f);
                    float diffY = Math.Abs((float)j - position.Y / 16f);
                    double distanceToTile = Math.Sqrt((double)(diffX * diffX + diffY * diffY));
                    if (distanceToTile < (double)explosionRadius)
                    {
                        bool canKillTile = true;
                        if (Main.tile[i, j] != null && Main.tile[i, j].active())
                        {
                            canKillTile = true;
                            if (Main.tileDungeon[(int)Main.tile[i, j].type] || Main.tile[i, j].type == 88 || Main.tile[i, j].type == 21 || Main.tile[i, j].type == 26 || Main.tile[i, j].type == 107 || Main.tile[i, j].type == 108 || Main.tile[i, j].type == 111 || Main.tile[i, j].type == 226 || Main.tile[i, j].type == 237 || Main.tile[i, j].type == 221 || Main.tile[i, j].type == 222 || Main.tile[i, j].type == 223 || Main.tile[i, j].type == 211 || Main.tile[i, j].type == 404)
                            {
                                canKillTile = false;
                            }
                            if (!Main.hardMode && Main.tile[i, j].type == 58)
                            {
                                canKillTile = false;
                            }
                            if (!TileLoader.CanExplode(i, j))
                            {
                                canKillTile = false;
                            }
                            if (canKillTile)
                            {
                                WorldGen.KillTile(i, j, false, false, false);
                                if (!Main.tile[i, j].active() && Main.netMode != 0)
                                {
                                    NetMessage.SendData(17, -1, -1, null, 0, (float)i, (float)j, 0f, 0, 0, 0);
                                }
                            }
                        }
                        if (canKillTile)
                        {
                            for (int x = i - 1; x <= i + 1; x++)
                            {
                                for (int y = j - 1; y <= j + 1; y++)
                                {
                                    if (Main.tile[x, y] != null && Main.tile[x, y].wall > 0 && canKillWalls && WallLoader.CanExplode(x, y, Main.tile[x, y].wall))
                                    {
                                        WorldGen.KillWall(x, y, false);
                                        if (Main.tile[x, y].wall == 0 && Main.netMode != 0)
                                        {
                                            NetMessage.SendData(17, -1, -1, null, 2, (float)x, (float)y, 0f, 0, 0, 0);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            AchievementsHelper.CurrentlyMining = false;
        }
        public static void ChanneledProjectile(Player p, Projectile projectile)
        {
            int dir = projectile.direction;
            p.ChangeDir(dir);
            p.itemAnimation = 2;
            p.itemTime = 2;
            p.itemRotation = (float)Math.Atan2(projectile.velocity.Y * dir, projectile.velocity.X * dir);
            p.heldProj = projectile.whoAmI;
        }
        /// <summary>
        /// Used for making projectile stay near player
        /// </summary>
        /// <param name="projectile"></param>
        /// <param name="p"></param>
        /// <param name="vel"></param>
        public static void StayNearPlayer(Projectile projectile, Player p, float vel)
        {
            projectile.velocity = TBMUtils.DistanceToMouse(p, vel);
            projectile.position = p.position + TBMUtils.DistanceToMouse(p, vel);
            projectile.Center = p.MountedCenter + TBMUtils.DistanceToMouse(p, vel);
        }
        public static void StayNearPosition(Projectile proj, Vector2 pos)
        {
            proj.position = pos;
            proj.Center = pos;
        }

        public static void PanCamera(Vector2 Position)
        {
            Vector2 ScreenCenter = new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
            Main.screenPosition = Position - ScreenCenter;
        }

        /// <summary>
        /// Allows to create circular movement
        /// </summary>
        /// <param name="centre">Center of the circle</param>
        /// <param name="angle">Angle(must be incremented to move)</param>
        /// <param name="radius">Radius of the circle</param>
        /// <returns></returns>
        public static Vector2 CircleMovement(Vector2 centre, float angle, float radius)
        {
            float x = (float)(centre.X + radius * Math.Sin(angle));
            float y = (float)(centre.Y + radius * Math.Cos(angle));
            return new Vector2(x, y);
        }
        public static Vector2 PivotPoint(Vector2 position, float offset_x, float offset_y, float angle)
        {
            float new_x = position.X + (float)Math.Cos(angle) * offset_x + (float)Math.Cos(angle + (float)Math.PI / 2) * offset_y;
            float new_y = position.Y + (float)Math.Sin(angle) * offset_x + (float)Math.Sin(angle + (float)Math.PI / 2) * offset_y;
            return new Vector2(new_x, new_y);
        }
    }
}
