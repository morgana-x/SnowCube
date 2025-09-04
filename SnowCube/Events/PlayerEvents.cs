using MCGalaxy.Events;
using MCGalaxy;
using System;
using MCGalaxy.Maths;
using SnowCube.Modules.Players;

namespace SnowCube.Events
{
    public class PlayerEvents
    {
        public delegate void PlayerThrowingSnowball(Player pl, Modules.Projectile.Projectile pr, ref bool cancel);

        public sealed class PlayerThrowingSnowballEvent : IEvent<PlayerThrowingSnowball>
        {
            public static void Call(Player p, Modules.Projectile.Projectile pr, ref bool cancel)
            {
                IEvent<PlayerThrowingSnowball>[] items = handlers.Items;
                for (int i = 0; i < items.Length; i++)
                {
                    try { items[i].method(p, pr, ref cancel); }
                    catch (Exception ex) { LogHandlerException(ex, items[i]); }
                }
            }
        }


        public delegate void PlayerPickingUpSnow(Player pl, Vec3U16 pos, ref bool cancel);

        public sealed class PlayerPickingUpSnowEvent : IEvent<PlayerPickingUpSnow>
        {
            public static void Call(Player p, Vec3U16 pos, ref bool cancel)
            {
                IEvent<PlayerPickingUpSnow>[] items = handlers.Items;
                for (int i = 0; i < items.Length; i++)
                {
                    try { items[i].method(p, pos, ref cancel); }
                    catch (Exception ex) { LogHandlerException(ex, items[i]); }
                }
            }
        }

        public delegate void PlayerHitBySnowball(Player pl, Player thrower, ref bool cancel);
        public sealed class PlayerHitBySnowballEvent : IEvent<PlayerHitBySnowball>
        {
            public static void Call(Player p, Player thrower, ref bool cancel)
            {
                IEvent<PlayerHitBySnowball>[] items = handlers.Items;
                for (int i = 0; i < items.Length; i++)
                {
                    try { items[i].method(p, thrower, ref cancel); }
                    catch (Exception ex) { LogHandlerException(ex, items[i]); }
                }
            }
        }

        public delegate void PlayerDamaging(Player pl, ref DamageData damagedata);

        public sealed class PlayerDamagingEvent : IEvent<PlayerDamaging>
        {
            public static void Call(Player p, ref DamageData damagedata)
            {
                IEvent<PlayerDamaging>[] items = handlers.Items;
                for (int i = 0; i < items.Length; i++)
                {
                    try { items[i].method(p, ref damagedata); }
                    catch (Exception ex) { LogHandlerException(ex, items[i]); }
                }
            }
        }


        public delegate void PlayerKilled(Player player, ref DamageData damagedata);

        public sealed class PlayerKilledEvent : IEvent<PlayerKilled>
        {
            public static void Call(Player p, ref DamageData damagedata)
            {
                IEvent<PlayerKilled>[] items = handlers.Items;
                for (int i = 0; i < items.Length; i++)
                {
                    try { items[i].method(p, ref damagedata); }
                    catch (Exception ex) { LogHandlerException(ex, items[i]); }
                }
            }
        }
    }
}
