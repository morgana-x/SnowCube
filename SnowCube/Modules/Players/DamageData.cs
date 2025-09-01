using MCGalaxy;
using System;
using System.Collections.Generic;

namespace SnowCube.Modules.Players
{
    public class DamageData
    {
        public class DamageDataDeathMessage
        {
            public List<string> Killed = new List<string>() {  };
            public List<string> Suicide = new List<string>() {  };
            public List<string> Other = new List<string>() {};
            public DamageDataDeathMessage(string[] km, string[] sm, string[] om)
            {
                foreach(var msg in km)
                    Killed.Add(msg);
                foreach(var msg in sm)
                    Suicide.Add(msg);
                foreach (var msg in om)
                    Other.Add(msg);
            }
            public DamageDataDeathMessage(string km, string sm, string om)
            {
                Killed.Add(km);
                Suicide.Add(sm);
                Other.Add(om);
            }
            public DamageDataDeathMessage() { }
            static System.Random rnd = new System.Random();

            public int[] lastIndex = new int[3] { 0, 0, 0 };
            public int GetRandomNonRepeat(int i, int count)
            {
                if (count <= 1) return 0;

                int result = 0;
                while (result == lastIndex[i])
                    result = rnd.Next(0, count);

                lastIndex[i] = result;
                return result;

            }
            public string GetMessage(DamageData data)
            {
                if (data.Victim == data.Attacker)
                    return Suicide[GetRandomNonRepeat(0, Suicide.Count)].Replace("@v", data.Victim.ColoredName);
                if (data.Attacker != null)
                    return Killed[GetRandomNonRepeat(1, Killed.Count)].Replace("@v", data.Victim.ColoredName).Replace("@k", data.Attacker.ColoredName);
                if (data.Victim != null)
                    return Other[GetRandomNonRepeat(2, Other.Count)].Replace("@v", data.Victim.ColoredName);

                return Other[rnd.Next(0, Other.Count)].Replace("@v", "&cNULL");
            }

        }

        public enum DamageType
        {
            None,
            Snowball,
            Fall
        }

        public static Dictionary<DamageType, DamageDataDeathMessage> DeathMessages = new Dictionary<DamageType, DamageDataDeathMessage>()
        {
            [DamageType.None] = new DamageDataDeathMessage("@v &ewas murdered by @k", "@v &egot bored of life", "@v &edied."),
            [DamageType.Snowball] = new DamageDataDeathMessage(new string[] {"@v &egot &bsnowed &eby @k", "@k &egave @v &bbrain freeze", "@v &baccepted snow to the face &efrom @k", "@v &egot &bsmashed with snow &efrom @k"}, new string[] { "@v &cfailed to dodge their own snowball", "@v &efound out about gravity" }, new string[] { "@v &edied of &csnow exposure" }),
            [DamageType.Fall] = new DamageDataDeathMessage(new string[] {"@v&e's legs were shattered by @k", "@v &cwas &c(russian)&e\"&9fell out window&e\" by @k"}, new string[] {"@v &elistened to the voices and jumped"}, new string[] {"@v&e's legs were &cshattered", "@v &ecouldn't break their fall"})
        };


        public Player Victim;
        public Player Attacker = null;
        public DamageType Type;
        public int Damage;
        public bool Cancel = false;
        public bool AnnounceDeath = true;

        public string DeathMessage { get { 
                if (DeathMessages.ContainsKey(Type))
                    return DeathMessages[Type].GetMessage(this); 
                return DeathMessages[DamageType.None].GetMessage(this);
            } }
        public DamageData() { }
        public DamageData(Player victim, DamageType type, int damage)
        {
            Victim = victim;
            Type = type;
            Damage = damage;
        }
        public DamageData(Player victim, Player attacker, DamageType type, int damage)
        {
            Victim = victim;
            Attacker = attacker;
            Type = type;
            Damage = damage;
        }
    }
}
