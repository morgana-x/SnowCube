using MCGalaxy;
using MCGalaxy.Events.PlayerEvents;
using MCGalaxy.Network;
using System.Collections.Generic;

namespace SnowCube.Modules.Players
{
    public class Build
    {

        public static void Load()
        {

            OnBlockChangingEvent.Register(OnBlockPlace, MCGalaxy.Priority.Normal);
            OnSentMapEvent.Register(OnMapSent, MCGalaxy.Priority.Normal);
        }

        public static void Unload()
        {
            MCGalaxy.Events.PlayerEvents.OnBlockChangingEvent.Unregister(OnBlockPlace);
            MCGalaxy.Events.PlayerEvents.OnSentMapEvent.Unregister(OnMapSent);
        }
        private static void SendBlockPerms(MCGalaxy.Player player)
        {
            if (!player.Session.hasCpe) return;

            if ((player.Game.Referee)) { player.SendCurrentBlockPermissions(); return; }


            List<byte> bulk = new List<byte>();
            // Send Can Break/Place Order
            bulk.Clear();
            for (int i = 0; i < 256; i++)
                bulk.AddRange(Packet.BlockPermission((ushort)i, false, false, player.Session.hasExtBlocks));
            player.Send(bulk.ToArray());
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
                bool has = player.Game.Referee;
                bulk.AddRange(Packet.SetInventoryOrder(block, has ? x : (ushort)0, player.Session.hasExtBlocks));
                if (has) x++;
            }
            player.Send(bulk.ToArray());
        }

        private static void OnMapSent(Player p, Level prevLevel, Level level)
        {
            if (!Util.IsPVPLevel(level)) return;
            SendBlockPerms(p);
            SendBlockOrder(p);
        }
        private static void OnBlockPlace(Player p, ushort x, ushort y, ushort z, ushort block, bool placing, ref bool cancel)
        {
            if (!Util.IsPVPLevel(p.level)) return;
            if (p.Game.Referee) return;
            cancel = true;
            p.RevertBlock(x, y, z);
        }


    }
}
