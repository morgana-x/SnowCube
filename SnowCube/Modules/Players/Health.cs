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

        public static void Damage(MCGalaxy.Player player, int amount)
        {
            if (Util.IsNoDamageLevel(player.level)) return;
            AddHealth(player, -amount);

            if (GetHealth(player) <= 0)
                Die(player);
        }

        public static void Die(MCGalaxy.Player player)
        {
            if (Util.IsNoDamageLevel(player.level)) return;
            player.HandleDeath(4, $"@p got snowed", false, false);
        }
        public static bool Dead(MCGalaxy.Player player)
        {
            if (Util.IsNoDamageLevel(player.level)) return false;
            return GetHealth(player) <= 0;
        }

    }
}
