using MCGalaxy;
using MCGalaxy.Network;
using System;
using System.Collections.Generic;

namespace SnowCube.Modules.Players
{
    public class Loadout
    {
        public static void Load()
        {
            MCGalaxy.Events.PlayerEvents.OnPlayerSpawningEvent.Register(OnPlayerSpawn, MCGalaxy.Priority.Normal);
        }


        public static void Unload()
        {
            MCGalaxy.Events.PlayerEvents.OnPlayerSpawningEvent.Unregister(OnPlayerSpawn);
        }
        public static void SendBlockOrder(MCGalaxy.Player player)
        {
            if (!player.Session.hasCpe) return;

            List<byte> bulk = new List<byte>();
            ushort x = 1;
            for (ushort i = 0; i < player.level.CustomBlockDefs.Length; i++)
            {
                if (i >= Block.MaxRaw) break;
                var def = player.level.CustomBlockDefs[i];
                if (i > Block.CPE_MAX_BLOCK && (def == null || def.RawID > Block.MaxRaw)) continue;

                var block = Block.ToRaw(i);
                bool has = player.Game.Referee || block == (ushort)SnowCube.ItemID.Snowball;
                bulk.AddRange(Packet.SetInventoryOrder(block, has ? x : (ushort)0, player.Session.hasExtBlocks));
                if (has) x++;
            }
            player.Send(bulk.ToArray());
        }
        private static void OnPlayerSpawn(Player p, ref Position pos, ref byte yaw, ref byte pitch, bool respawning)
        {
            if (Util.IsPVPLevel(p.level))
            {
                SendBlockOrder(p);
                Util.ClearHotbar(p);
                Util.SetHotbar(p, 4, (ushort)SnowCube.ItemID.Snowball);
            }
        }
    }
}
