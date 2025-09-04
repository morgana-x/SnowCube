using MCGalaxy;
using MCGalaxy.Events.PlayerEvents;
using MCGalaxy.Maths;
using MCGalaxy.Network;
using SnowCube.Events;
using SnowCube.Modules.World;

namespace SnowCube.Modules.Item
{
    public class Snowblock : Item
    {
        public override ushort BlockID => 36;
        public override BlockDefinition BlockDefinition => null;

        public override void OnLeftClick(Player p, MouseAction action, ushort yaw, ushort pitch, byte entityid, ushort bx, ushort by, ushort bz, TargetBlockFace face)
        {
            this.OnRightClick(p, action, yaw, pitch, entityid, bx, by, bz, face);
        }
        public override void OnRightClick(Player p, MouseAction action, ushort yaw, ushort pitch, byte entityid, ushort bx, ushort by, ushort bz, TargetBlockFace face)
        {
            if (action == MouseAction.Released) return;
            if (!Util.IsPVPLevel(p.level)) return;
            if (face == TargetBlockFace.None) return;
            if (p.Extras.GetInt("snowblock") <= 0) return;

            Util.TargetBlockFaceToOffset(face, out int ox, out int oy, out int oz);

            bx = (ushort)(bx + ox);
            by = (ushort)(by + oy);
            bz = (ushort)(bz + oz);

            if (!p.level.IsValidPos(bx, by, bz)) return;

            var b = p.level.GetBlock(bx, by, bz);
            if (b != 0) return;

            bool cancel = false;
            PlayerEvents.PlayerPlaceSnowEvent.Call(p, bx, by, bz, ref cancel);
            if (cancel) return;

            // Release force hold shovel thing
            p.Send(Packet.HoldThis((ushort)Item.ItemID.Shovel, false, p.Session.hasExtBlocks));

            Effect.EmitEffect(p.level, Effect.Effects.Snow_Place, new Vec3F32(bx, by, bz));
            Sound.EmitBlockSound(p, 0, MCGalaxy.Blocks.SoundType.Cloth, 50, 100);
            p.level.UpdateBlock(p, bx, by, bz, 36);


            p.Extras["snowblock"] = (p.Extras.GetInt("snowblock") - 1);
        }
    }
}
