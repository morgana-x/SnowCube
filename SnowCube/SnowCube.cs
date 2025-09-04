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
            Modules.Item.Item.Load();
            Modules.World.Effect.Load();
            Modules.World.Sound.Load();
            Modules.Players.Ammo.Load();
            Modules.Players.Build.Load();
            Modules.Players.Loadout.Load();
            Modules.Players.Hold.Load();
            Modules.Players.Health.Load();
            Modules.Players.Hud.Load();
            task = Server.MainScheduler.QueueRepeat(Tick, null, System.TimeSpan.FromMilliseconds(50));
        }

        public override void Unload(bool auto)
        {
            Server.MainScheduler.Cancel(task);

            Modules.Item.Item.Unload();
            Modules.World.Effect.Unload();
            Modules.World.Sound.Unload();
            Modules.Players.Loadout.Unload();
            Modules.Players.Build.Unload();
            Modules.Players.Ammo.Unload();
            Modules.Players.Hold.Unload();
            Modules.Players.Health.Unload();
            Modules.Players.Hud.Unload();
        }

        public static void Tick(SchedulerTask task)
        {
            Modules.Players.Hold.Tick();
            Modules.Projectile.Projectile.TickAll(0.50f);
        }
    }
}
