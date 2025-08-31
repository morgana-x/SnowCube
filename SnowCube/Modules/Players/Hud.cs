using MCGalaxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowCube.Modules.Players
{
    public class Hud
    {
        public static void Load()
        {
            MCGalaxy.Events.PlayerEvents.OnPlayerSpawningEvent.Register(OnPlayerSpawn, MCGalaxy.Priority.Normal);
        }


        public static void Unload()
        {
            MCGalaxy.Events.PlayerEvents.OnPlayerSpawningEvent.Unregister(OnPlayerSpawn);
        }

        private static void OnPlayerSpawn(Player p, ref Position pos, ref byte yaw, ref byte pitch, bool respawning)
        {
            if (Util.IsPVPLevel(p.level))
            {
                HUD_Health(p);
                HUD_Ammo(p);
            }
        }
        public static void HUD_Health(MCGalaxy.Player pl)
        {
            if (Util.IsNoDamageLevel(pl.level)) return;
            var msg = "&0" + new string('♥', Health.MaxHealth - Modules.Players.Health.GetHealth(pl)) + "&c" + new string('♥', Modules.Players.Health.GetHealth(pl));
            pl.SendCpeMessage(MCGalaxy.CpeMessageType.BottomRight2, msg);
        }

        public static void HUD_Ammo(MCGalaxy.Player pl)
        {
            if (Util.IsNoAmmoLevel(pl.level)) return;
            var msg = "&0" + new string('•', Ammo.MaxAmmo - Modules.Players.Ammo.GetAmmo(pl)) + "&b" + new string('•', Modules.Players.Ammo.GetAmmo(pl));
            pl.SendCpeMessage(MCGalaxy.CpeMessageType.BottomRight1, msg);
        }

    }
}
