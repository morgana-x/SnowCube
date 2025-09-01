using MCGalaxy;
using MCGalaxy.Events.PlayerEvents;
using SnowCube.Events;
using SnowCube.Modules.World;
using System;

namespace SnowCube.Modules.Players
{
    public class Fight
    {

        public static void Load()
        {
            OnPlayerClickEvent.Register(OnPlayerClick, MCGalaxy.Priority.Normal);
        }

        public static void Unload()
        {
            MCGalaxy.Events.PlayerEvents.OnPlayerClickEvent.Unregister(OnPlayerClick);
        }


        private static void OnPlayerClick(Player p, MouseButton button, MouseAction action, ushort yaw, ushort pitch, byte entity, ushort x, ushort y, ushort z, TargetBlockFace face)
        {
            if (button != MouseButton.Left) return; // Throw

            if (action == MouseAction.Released) return;

            if (!Util.IsPVPLevel(p.level)) return;

            if ( Block.ToRaw(p.GetHeldBlock()) != (ushort)SnowCube.ItemID.Snowball) return;

            if (!Util.IsNoAmmoLevel(p.level) && Ammo.GetAmmo(p) <= 0) return;

            if (p.Extras.TryGet("snowcooldown", out object expire) && DateTime.Now < (DateTime)expire)
                return;

            var snowball = new Projectile.Snowball();
            bool cancel = false;
            PlayerEvents.PlayerThrowingSnowballEvent.Call(p, snowball, ref cancel);
            if (cancel) return;

            p.Extras["snowcooldown"] = DateTime.Now.AddSeconds(0.25f);


            if (!Util.IsNoAmmoLevel(p.level))
                Ammo.AddAmmo(p, -1);

            Sound.EmitBlockSound(p, 0, MCGalaxy.Blocks.SoundType.Snow, 50, 100);
            snowball.Throw(p, yaw, pitch, 2.5f);
        }



    }
}
