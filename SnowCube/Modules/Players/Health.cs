using MCGalaxy;
using System;

namespace SnowCube.Modules.Players
{
    public class Health
    {
        public static void Load()
        {
            MCGalaxy.Events.PlayerEvents.OnPlayerSpawningEvent.Register(OnPlayerSpawning, MCGalaxy.Priority.Normal);
        }

        private static void OnPlayerSpawning(Player p, ref Position pos, ref byte yaw, ref byte pitch, bool respawning)
        {
            SetHealth(p, MaxHealth);
        }

        public static void Unload()
        {
            MCGalaxy.Events.PlayerEvents.OnPlayerSpawningEvent.Unregister(OnPlayerSpawning);
        }


        public const int MaxHealth = 3;
        public static int GetHealth(MCGalaxy.Player player)
        {
            if (Util.IsNoDamageLevel(player.level)) return MaxHealth;
            return player.Extras.GetInt("health", MaxHealth);
        }
        public static void SetHealth(MCGalaxy.Player player, int value)
        {
            player.Extras["health"] = value;
            Hud.HUD_Health(player);
        }

        public static void AddHealth(MCGalaxy.Player player, int value)
        {
            var newvalue = Math.Max(GetHealth(player) + value, 0);
            if (newvalue > MaxHealth) newvalue = MaxHealth;
            SetHealth(player, newvalue);
        }

        public static void Damage(MCGalaxy.Player player, int amount, DamageData.DamageType type, MCGalaxy.Player attacker=null)
        {
            if (Util.IsNoDamageLevel(player.level)) return;
            if (player.Game.Referee) return;
          
            DamageData data = new DamageData(player, attacker, type, amount);
            Events.PlayerEvents.PlayerDamagingEvent.Call(player, ref data);

            if (data.Cancel) return;

            AddHealth(player, -data.Damage);

            if (GetHealth(player) <= 0)
                Die(player, data);
        }

        public static void Die(MCGalaxy.Player player, DamageData damageData)
        {
            if (Util.IsNoDamageLevel(player.level)) return;
            Events.PlayerEvents.PlayerKilledEvent.Call(player, ref damageData);

 
            player.HandleDeath(4, damageData.DeathMessage, false, false);
        }
        public static bool Dead(MCGalaxy.Player player)
        {
            if (Util.IsNoDamageLevel(player.level)) return false;
            return GetHealth(player) <= 0;
        }

    }
}
