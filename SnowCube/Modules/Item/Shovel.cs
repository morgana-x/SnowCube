using MCGalaxy;
using MCGalaxy.Events.PlayerEvents;
using SnowCube.Modules.World;
using MCGalaxy.Maths;
using MCGalaxy.Network;

namespace SnowCube.Modules.Item
{
    public class Shovel : Item
    {
        public override ushort BlockID => (ushort)ItemID.Shovel;

        public override BlockDefinition BlockDefinition => new BlockDefinition() { RawID = (ushort)ItemID.Shovel, MaxX = 0, MaxY = 16, MaxZ = 16, MinX = 0, MinY = 0, MinZ = 5, BackTex = 242, BottomTex = 242, FrontTex = 242, TopTex = 242, LeftTex = 242, RightTex = 242, FallBack = 79, Name = "Shovel", BlockDraw = 1, Shape = 16 };


        // Pick up snow block
        public override void OnLeftClick(Player p, MouseAction action, ushort yaw, ushort pitch, byte entityid, ushort bx, ushort by, ushort bz, TargetBlockFace face)
        {
            if (action == MouseAction.Released) return;
            if (!Util.IsPVPLevel(p.level)) return;

            var b = p.level.GetBlock(bx, by, bz);
            if (!Util.IsSnowblock(b)) return;

            if (p.Extras.GetInt("snowblock") > 0) return;

            
            Effect.EmitEffect(p.level, Effect.Effects.Snow_Place, new Vec3F32(bx, by + 1, bz));
            Sound.EmitBlockSound(p, 0, MCGalaxy.Blocks.SoundType.Cloth, 50, 100);

            p.level.UpdateBlock(p, bx, by, bz, 0);
            p.Extras["snowblock"] = p.Extras.GetInt("snowblock") + 1;

            // Force held shovel thing
            p.Send(Packet.HoldThis((ushort)Item.ItemID.Snowblock, true, p.Session.hasExtBlocks));
        }

        // Place snow block
        public override void OnRightClick(Player p, MouseAction action, ushort yaw, ushort pitch, byte entityid, ushort bx, ushort by, ushort bz, TargetBlockFace face)
        {
            
        }

        public override void OnMiddleClick(Player p, MouseAction action, ushort yaw, ushort pitch, byte entityid, ushort bx, ushort by, ushort bz, TargetBlockFace face)
        {

        }
    }
}
