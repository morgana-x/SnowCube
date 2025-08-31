using MCGalaxy;
using MCGalaxy.Maths;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SnowCube.Projectile
{
    public class Projectile
    {
        public static Dictionary<Level, List<Projectile>> Projectiles = new Dictionary<Level, List<Projectile>>();

        public static void TickAll()
        {
            int i = 0;
            while (i < Projectiles.Count)
            {
                var pair = Projectiles.ElementAt(i);

                if (pair.Key.players.Count == 0)
                {
                    Projectiles.Remove(pair.Key);
                    continue;
                };

                int j = 0;
                while (j < pair.Value.Count)
                {
                    try
                    {
                        if (pair.Value[j].Tick())
                        {
                            pair.Value[j].OnDestroy();
                            pair.Value.Remove(pair.Value[j]);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                        pair.Value[j].OnDestroy();
                        pair.Value.Remove(pair.Value[j]);
                    }
                    j++;
                }

                i++;
            }
        }

        public static void Spawn(Level level, Projectile projectile)
        {
            if (!Projectiles.ContainsKey(level))
                Projectiles.Add(level, new List<Projectile>());

            projectile.Level = level;
            projectile.Expiration = DateTime.Now.AddSeconds(projectile.LifeTime);

            Projectiles[level].Add(projectile);
        }

        public static void Throw(Level level, Projectile projectile, Vec3F32 position, Vec3F32 dir, Player thrower = null)
        {
            projectile.Level = level;
            projectile.Pos = position;
            projectile.Vel = dir;
            projectile.Thrower = thrower;
            Spawn(level, projectile);
        }

        public static void Throw(Player p, Projectile projectile, ushort yaw, ushort pitch, float power = 2.5f)
        {
            var dir = DirUtils.GetDirVectorExt(yaw, pitch);
            Throw(p.level, projectile, p.Pos.ToVec3F32() + dir * 1f + new Vec3F32(0, 0.5f, 0), dir * power, p);
        }

        public void Throw(Player p, ushort yaw, ushort pitch, float power = 2.5f)
        {
            Throw(p, this, yaw, pitch, power);
        }
        public Projectile()
        {
        }
        public Projectile(Level level, Player thrower, Vec3F32 position, Vec3F32 velocity)
        {
            Level = level;
            Thrower = thrower;
            Pos = position;
            Vel = velocity;
        }

        public void Spawn()
        {
            Spawn(Level, this);
        }

        public Player Thrower = null;
        public Level Level;

        public Vec3F32 Pos;
        public Vec3F32 Vel;
        public Vec3U16 BlockPos { get { return Util.Round(Pos); } set { Pos.X = value.X << 5; Pos.Y = value.Y << 5; Pos.Z = value.Z << 5; } }

        public float Drag = 0.95f;
        public float Gravity = 1.5f;

        public float Radius = 4f;

        public bool destroy = false;

        public float LifeTime = 100;

        public DateTime Expiration;

        private ushort CollidedBlock()
        {
            var bp = BlockPos;
            return Level.GetBlock(bp.X, bp.Y, bp.Z);
        }

        private Player CollidedPlayer()
        {
            return Util.PlayerAt(Level, Pos, Radius);
        }
        private List<Player> CollidedPlayers()
        {
            return Util.PlayersAt(Level, Pos, Radius);
        }
        public virtual bool Tick()
        {
            if (destroy)
                return true;
            if (DateTime.Now > Expiration)
                return true;

            Vel *= Drag;
            if (Vel.Y > -Gravity)
                Vel.Y -= 0.25f;

            Pos += Vel;

            if (Pos.Y < 0 || Pos.X < 0 || Pos.Z < 0)
                return true;
            if (Pos.X >= Level.Width || Pos.Z >= Level.Length)
                return true;

            var block = CollidedBlock();
            var pl = CollidedPlayer();

            if (block != 0 || pl != null)
            {
                if (pl != null)
                    foreach (var p in CollidedPlayers())
                        OnCollide(block, p);
                else
                    OnCollide(block, null);
                return true;
            }

            return false;
        }


        public void Destroy()
        {
            destroy = true;
        }

        public virtual void OnDestroy()
        {

        }

        public virtual void OnCollide(ushort block, Player pl)
        {

        }
    }
}
