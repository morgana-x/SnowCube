using MCGalaxy;
using MCGalaxy.Events.PlayerEvents;
using MCGalaxy.Maths;
using SnowCube.Modules.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowCube.Modules.Players
{
    internal class Ammo
    {
        public static void Load()
        {
            OnPlayerClickEvent.Register(OnPlayerClick, MCGalaxy.Priority.Normal);
        }

        public static void Unload()
        {
            MCGalaxy.Events.PlayerEvents.OnPlayerClickEvent.Unregister(OnPlayerClick);
        }


        public const int MaxAmmo = 5;
        public static int GetAmmo(MCGalaxy.Player p)
        {
            if (Util.IsNoAmmoLevel(p.level)) return 5;
           return p.Extras.GetInt("snowballs", 0); ;
        }
        public static void SetAmmo(MCGalaxy.Player p, int value)
        {
            p.Extras["snowballs"] = value;
            Hud.HUD_Ammo(p);
        }

        public static void AddAmmo(MCGalaxy.Player p, int value)
        {
            var newvalue = GetAmmo(p) + value;
            if (newvalue > MaxAmmo)
                newvalue = MaxAmmo;
            if (newvalue < 0)
                newvalue = 0;
            SetAmmo(p, newvalue);
        }
        private static void OnPlayerClick(Player p, MouseButton button, MouseAction action, ushort yaw, ushort pitch, byte entity, ushort x, ushort y, ushort z, TargetBlockFace face)
        {
            if (button != MouseButton.Right || action == MouseAction.Released) return;
            if (!Util.IsPVPLevel(p.level)) return;
            if (Block.ToRaw(p.GetHeldBlock()) != (ushort)SnowCube.ItemID.Snowball) return;
            if (Util.IsNoAmmoLevel(p.level)) return;

            if (Ammo.GetAmmo(p) >= Ammo.MaxAmmo) return;

            var b = p.level.GetBlock(x, y, z);
            if (b == 53 || b == 36)
            {
                Ammo.AddAmmo(p, 1);
                Effect.EmitEffect(p.level, Effect.Effects.Snowball_Trail, new Vec3F32(x, y + 1, z));
            }
            
        }


    }
}
