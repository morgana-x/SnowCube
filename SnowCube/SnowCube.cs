using MCGalaxy;
using MCGalaxy.Tasks;


namespace SnowCube
{
    public class SnowCube : Plugin
    {
        public override string name => "snowcube";

        SchedulerTask task;


        public enum ItemID // Temp its 3 am
        {
            Snowball = 466
        }

        public override void Load(bool auto)
        {
            Modules.World.Effect.Load();
            Modules.Players.Fight.Load();
            Modules.Players.Ammo.Load();
           // Modules.Players.Build.Load();
            Modules.Players.Health.Load();
            Modules.Players.Hud.Load();
            task = Server.MainScheduler.QueueRepeat(Tick, null, System.TimeSpan.FromMilliseconds(50));


            var bd = new BlockDefinition() { RawID = (ushort)ItemID.Snowball, MaxX = 12, MaxY = 12, MaxZ = 12, MinX=6, MinY=6, MinZ=6, BackTex=50, BottomTex=50, FrontTex=50, TopTex=50, LeftTex=50, RightTex=50, FallBack=79, Name="Snowball", BlockDraw=1, Shape=16 };
            BlockDefinition.Add(bd, BlockDefinition.GlobalDefs, null);
        }

        public override void Unload(bool auto)
        {
            Server.MainScheduler.Cancel(task);

            Modules.World.Effect.Unload();
            Modules.Players.Fight.Unload();
            Modules.Players.Ammo.Unload();
          //  Modules.Players.Build.Unload();
            Modules.Players.Health.Unload();
            Modules.Players.Hud.Unload();
        }

        public static void Tick(SchedulerTask task)
        {
            Projectile.Projectile.TickAll();
        }
    }
}
