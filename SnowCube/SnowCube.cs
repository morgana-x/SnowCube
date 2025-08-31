using MCGalaxy;
using MCGalaxy.Tasks;


namespace SnowCube
{
    public class SnowCube : Plugin
    {
        public override string name => "snowcube";

        SchedulerTask task;
        public override void Load(bool auto)
        {
            Modules.World.Effect.Load();
            Modules.Players.Fight.Load();
            Modules.Players.Ammo.Load();
            Modules.Players.Build.Load();
            Modules.Players.Health.Load();
            Modules.Players.Hud.Load();
            task = Server.MainScheduler.QueueRepeat(Tick, null, System.TimeSpan.FromMilliseconds(50));
        }

        public override void Unload(bool auto)
        {
            Server.MainScheduler.Cancel(task);

            Modules.World.Effect.Unload();
            Modules.Players.Fight.Unload();
            Modules.Players.Ammo.Unload();
            Modules.Players.Build.Unload();
            Modules.Players.Health.Unload();
            Modules.Players.Hud.Unload();
        }

        public static void Tick(SchedulerTask task)
        {
            Projectile.Projectile.TickAll();
        }
    }
}
