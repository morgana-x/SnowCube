using System.Collections.Generic;
using MCGalaxy;
using MCGalaxy.Events.PlayerEvents;

namespace SnowCube.Modules.Item
{
    public class Item
    {
        public static Dictionary<ushort, Item> Items = new Dictionary<ushort, Item>();
        public enum ItemID // Temp its 3 am
        {
            Snowball = 465,
            Shovel = 466,
            Snowblock = 36,
        }
        public static void Load()
        {
            OnPlayerClickEvent.Register(OnPlayerClick, MCGalaxy.Priority.Normal);

            RegisterItem(new Snowball());
            RegisterItem(new Shovel());
            RegisterItem(new Snowblock());
        }
        public static void Unload()
        {
            OnPlayerClickEvent.Unregister(OnPlayerClick);
            Items.Clear();
        }
        public static void RegisterItem(Item item)
        {
            if (item.BlockDefinition != null)
            {
                item.BlockDefinition.RawID = item.BlockID;
                BlockDefinition.Add(item.BlockDefinition, BlockDefinition.GlobalDefs, null);
            }
            Items.Add(item.BlockID, item);
        }

        public static void OnPlayerClick(Player p, MouseButton button, MouseAction action, ushort yaw, ushort pitch, byte entity, ushort x, ushort y, ushort z, TargetBlockFace face)
        {
            if (!Util.IsPVPLevel(p.level)) return;

            var bid = Block.ToRaw(p.GetHeldBlock());
            if (!Items.ContainsKey(bid)) return;

            switch (button)
            {
                case MouseButton.Left:
                    Items[bid].OnLeftClick(p, action, yaw, pitch, entity, x, y, z, face);
                    break;
                case MouseButton.Right:
                    Items[bid].OnRightClick(p, action, yaw, pitch, entity, x, y, z, face);
                    break;
                case MouseButton.Middle:
                    Items[bid].OnMiddleClick(p, action, yaw, pitch, entity, x, y, z, face);
                    break;
            }

        }

       
        public virtual ushort BlockID { get; }
        public virtual BlockDefinition BlockDefinition { get; set; }

        public virtual void OnLeftClick(Player p, MouseAction action, ushort yaw, ushort pitch, byte entityid, ushort bx, ushort by, ushort bz, TargetBlockFace face)
        {

        }

        public virtual void OnRightClick(Player p, MouseAction action, ushort yaw, ushort pitch, byte entityid, ushort bx, ushort by, ushort bz, TargetBlockFace face)
        {

        }
        public virtual void OnMiddleClick(Player p, MouseAction action, ushort yaw, ushort pitch, byte entityid, ushort bx, ushort by, ushort bz, TargetBlockFace face)
        {

        }
    }
}
