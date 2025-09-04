using System;
using MCGalaxy;
using MCGalaxy.Events.PlayerEvents;
using SnowCube.Events;
using SnowCube.Modules.Players;
using SnowCube.Modules.World;
using MCGalaxy.Maths;

namespace SnowCube.Modules.Item
{
    public class Snowball : Item
    {
        public override ushort BlockID => (ushort)ItemID.Snowball;

        public override BlockDefinition BlockDefinition => new BlockDefinition() { RawID = (ushort)ItemID.Snowball, MaxX = 12, MaxY = 12, MaxZ = 12, MinX = 6, MinY = 6, MinZ = 6, BackTex = 50, BottomTex = 50, FrontTex = 50, TopTex = 50, LeftTex = 50, RightTex = 50, FallBack = 79, Name = "Snowball", BlockDraw = 1, Shape = 16 };

        public override void OnLeftClick(Player p, MouseAction action, ushort yaw, ushort pitch, byte entityid, ushort bx, ushort by, ushort bz, TargetBlockFace face)
        {
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

        public override void OnRightClick(Player p, MouseAction action, ushort yaw, ushort pitch, byte entityid, ushort bx, ushort by, ushort bz, TargetBlockFace face)
        {
            if (action == MouseAction.Released) return;
            if (!Util.IsPVPLevel(p.level)) return;
            if (Util.IsNoAmmoLevel(p.level)) return;

            if (Ammo.GetAmmo(p) >= Ammo.MaxAmmo) return;

            var b = p.level.GetBlock(bx, by, bz);
            if (!Util.IsSnowblock(b)) return;

            Ammo.AddAmmo(p, 1);
            Effect.EmitEffect(p.level, Effect.Effects.Snowball_Trail, new Vec3F32(bx, by + 1, bz));
            Sound.EmitBlockSound(p, 0, MCGalaxy.Blocks.SoundType.Cloth, 50, 100);
        }

        public override void OnMiddleClick(Player p, MouseAction action, ushort yaw, ushort pitch, byte entityid, ushort bx, ushort by, ushort bz, TargetBlockFace face)
        {

        }
    }
}
