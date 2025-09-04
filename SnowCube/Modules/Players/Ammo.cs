namespace SnowCube.Modules.Players
{
    internal class Ammo
    {
        public static void Load()
        {
         
        }

        public static void Unload()
        {
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

    }
}
