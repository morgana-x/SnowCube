using MCGalaxy.Maths;
using MCGalaxy;
using System;
using MCGalaxy.Network;
using System.Collections.Generic;
using SnowCube.Modules.Players;
namespace SnowCube
{
    public class Util
    {
        public static bool IsPVPLevel(Level level)
        {
            return level.Config.MOTD.Contains("+snowball") || level.Extras.GetBoolean("snowball");
        }

        public static bool IsNoAmmoLevel(Level level)
        {
            return level.Config.MOTD.Contains("-ammo") || level.Extras.GetBoolean("-ammo");
        }

        public static bool IsNoDamageLevel(Level level)
        {
            return level.Config.MOTD.Contains("-damage") || level.Extras.GetBoolean("-damage");
        }
        public static bool CanBreakBlocks(Level level)
        {
            return level.CanDelete;// && (!IsPVPLevel(level));
        }

        public static Vec3U16 Round(Vec3F32 v)
        {
            unchecked { return new Vec3U16((ushort)Math.Round(v.X), (ushort)Math.Round(v.Y), (ushort)Math.Round(v.Z)); }
        }


        public static List<Player> PlayersAt(Level level, Vec3F32 pos, float radius)
        {
            List<Player> result = new List<Player>();
            foreach (MCGalaxy.Player pl in level.players)
            {
                if (IsPVPLevel(level) && Health.Dead(pl)) continue;

                if ((pl.Pos.ToVec3F32() - pos).LengthSquared <= radius)
                {
                    result.Add(pl);
                    continue;
                }
                if (((pl.Pos.ToVec3F32() - new Vec3F32(0, 0.7f, 0)) - pos).LengthSquared <= radius)
                    result.Add(pl);
            }
            return result;
        }

        public static Player PlayerAt(Level level, Vec3F32 pos, float radius)
        {
            foreach (MCGalaxy.Player pl in level.players)
            {
                if (IsPVPLevel(level) && Health.Dead(pl)) continue;

                if ((pl.Pos.ToVec3F32() - pos).LengthSquared <= radius)
                    return pl;
                if (((pl.Pos.ToVec3F32() - new Vec3F32(0, 0.7f,0)) - pos).LengthSquared <= radius)
                    return pl;
            }
            return null;
        }

        public static void SetHotbar(Player p, byte slot, ushort block)
        {
            p.Send(Packet.SetHotbar(block, slot, p.Session.hasExtBlocks));

        }

        public static void ClearHotbar(Player p)
        {
            for (byte i = 0; i < 9; i++)
                SetHotbar(p, i, 0);
        }

        public static void SetSpectator(MCGalaxy.Player p)
        {
            p.Send(Packet.HackControl(true, true, true, true, true, -1));
            Entities.GlobalDespawn(p, false);

            if (p.Extras.Contains("spectator"))
                return;
            p.Extras["spectator"] = true;
            p.Message("%eYou are %cdead! %fSpectate the game to your liking!");
        }
        public static void UnsetSpectator(MCGalaxy.Player p)
        {
            p.Send(Packet.HackControl(false, false, false, false, true, -1));

            Entities.GlobalSpawn(p, false);

            if (p.Extras.Contains("spectator"))
                p.Extras.Remove("spectator");
        }


    }
}
