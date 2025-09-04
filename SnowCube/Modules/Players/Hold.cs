using MCGalaxy;
using MCGalaxy.Tasks;
using System;

namespace SnowCube.Modules.Players
{
    internal class Hold
    {
        public static void Load()
        {
           // holdTask = MCGalaxy.Server.MainScheduler.QueueRepeat(TickPlayerHold, null, TimeSpan.FromSeconds(0.45f));
        }
        public static void Unload()
        {
           // MCGalaxy.Server.MainScheduler.Cancel(holdTask);
        }

       // static SchedulerTask holdTask;

        static string GetModel(int holding)
        {
            if (holding == 0)
                return "humanoid";
            return $"hold|1.{holding.ToString("D3")}";
        }

        static DateTime nextTick = DateTime.Now;
        public static void Tick()//SchedulerTask task)
        {
            if (DateTime.Now < nextTick) return;
            nextTick = DateTime.Now.AddSeconds(1);
           // holdTask = task;

            foreach (var player in PlayerInfo.Online.Items)
            {
                if (!Util.IsPVPLevel(player.level)) continue;
                if (player.Model == "sit")
                    continue;

                int holding = player.GetHeldBlock();

                if (player.Extras.GetInt("HeldBlock") == holding)
                    continue;
                player.Extras["HeldBlock"] = holding;

                if (holding >= 66) holding = (holding - 256);

                string model = GetModel(holding);
                if (player.Model == model) continue;
                player.UpdateModel(model);
            }
        }
    }
}
