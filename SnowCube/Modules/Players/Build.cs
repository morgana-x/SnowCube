using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowCube.Modules.Players
{
    public class Build
    {
        public static void Load()
        {
            Events.PlayerEvents.PlayerKilledEvent.Register(PlayerKilled, MCGalaxy.Priority.Normal);
        }

        public static void Unload()
        {
            Events.PlayerEvents.PlayerKilledEvent.Unregister(PlayerKilled);
        }

        public static void PlayerKilled(MCGalaxy.Player p, ref DamageData damageData)
        {
            if (p.Extras.GetInt("snowblock") <= 0) return;
            p.Extras["snowblock"] = 0;
        }
    }
}
