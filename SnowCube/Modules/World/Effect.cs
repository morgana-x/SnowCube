using MCGalaxy;
using MCGalaxy.Maths;
using System.Collections.Generic;

namespace SnowCube.Modules.World
{

    public class Effect
    {
        public class DefinedEffect
        {
            public byte ID;
            public byte U1 = 0;
            public byte V1 = 0;
            public byte U2 = 10;
            public byte V2 = 10;
            public byte R = 255;
            public byte G = 255;
            public byte B = 255;
            public byte FrameCount = 1;
            public byte ParticleCount = 20;
            public byte Size = 7;
            public float SizeVariation = 1f;
            public float Spread = 0.6f;
            public float Speed = 0.5f;
            public float Gravity = 1f;
            public float BaseLifetime = 4f;
            public float LifetimeVariation = 1f;
            public bool ExpireOnTouchGround = false;
            public bool CollidesSolid = true;
            public bool CollidesLiquid = true;
            public bool CollidesLeaves = false;
            public bool FullBright = false;

            public DefinedEffect(byte id, byte[] colour)
            {
                ID = id;
                R = colour[0];
                G = colour[1];
                B = colour[2];
            }
            public DefinedEffect(byte id, byte r, byte g, byte b, float gravity, byte framecount=1, byte particlecount = 5, byte size=7, byte uv_x1 = 0, byte uv_y1 = 0, byte uv_x2 = 10, byte uv_y2 = 10)
            {
                ID = id;
                R = r;
                G = g;
                B = b;
                Gravity = gravity;
                U1 = uv_x1;
                V1 = uv_y1;
                U2 = uv_x2;
                V2 = uv_y2;
                FrameCount = framecount;
                Size = size;
                ParticleCount = particlecount;
            }
            public DefinedEffect(byte id)
            {
                ID = id;
            }
            public DefinedEffect()
            {

            }
        }
        public static Dictionary<byte, DefinedEffect> ParticleDefs = new Dictionary<byte, DefinedEffect>();

        public enum Effects
        {
            Snowball_Trail = 10,
            Snowball_Ball = 11,
            Snowball_Hit = 12,
            Snow_Place= 13,
        }

        public static void Load()
        {
            MCGalaxy.Events.PlayerEvents.OnSentMapEvent.Register(EventPlayerSentMap, Priority.Normal);

            AddEffect(new DefinedEffect((byte)Effects.Snowball_Trail, 255, 255, 255, 1f, 1, 3, 15, 0, 0, 10, 10) { BaseLifetime = 2f, Spread = 0.25f });
            AddEffect(new DefinedEffect((byte)Effects.Snowball_Ball, 255, 255, 255, 0f, 1, 1, 50, 5, 5, 10, 10) { Spread = 0f, BaseLifetime = 0.125f, LifetimeVariation=0f, SizeVariation=0f});
            AddEffect(new DefinedEffect((byte)Effects.Snowball_Hit){ R=255, G=255, ParticleCount=10, SizeVariation=0.2f, Size=25, Spread = 0.4f, FrameCount=1, Gravity=4f, Speed=3f, BaseLifetime = 4f, LifetimeVariation = 0.5f });
            AddEffect(new DefinedEffect((byte)Effects.Snow_Place) { R = 255, G = 255, ParticleCount = 10, SizeVariation = 0.2f, Size = 50, Spread = 0.4f, FrameCount = 1, Gravity = 4f, Speed = 1f, CollidesSolid=false, BaseLifetime = 4f, LifetimeVariation = 0.5f });

        }

        public static void Unload()
        {
            MCGalaxy.Events.PlayerEvents.OnSentMapEvent.Unregister(EventPlayerSentMap);
        }
        static void EventPlayerSentMap(Player p, Level prevLevel, Level level)
        {
            SendDefineEffectAll(p);
        }

        public static void EmitEffect(Level level, byte id, float x, float y, float z)
        {
            var packet = MCGalaxy.Network.Packet.SpawnEffect(id, x, y, z, x, y, z);
            foreach (var pl in PlayerInfo.Online.Items)
            {
                if (pl.level != level) continue;
                if (!pl.Session.hasCpe) continue;
                pl.Send(packet);
            }
        }
        public static void EmitEffect(Level level, Effects id, float x, float y, float z)
        {
            EmitEffect(level, (byte)id, x, y, z);
        }
        public static void EmitEffect(Level level, Effects id, Vec3F32 pos)
        {
            EmitEffect(level, (byte)id, pos.X, pos.Y, pos.Z);
        }
        public static void EmitEffect(Player p, byte id)
        {
            var posy = p.Model != "sit" ? p.Pos.Y : p.Pos.Y - 15;
            EmitEffect(p.level, id, p.Pos.X / 32f, posy / 32f, p.Pos.Z / 32f);
        }

        public static void EmitEffect(Player p, Effects id)
        {
            EmitEffect(p, (byte)id);
        }
        public static void AddEffect(byte Id, byte r, byte g, byte b, int gravity)
        {
            AddEffect(new DefinedEffect(Id, r, g, b, gravity));

        }
        public static void AddEffect(DefinedEffect effect)
        {

            if (!ParticleDefs.ContainsKey(effect.ID))
                ParticleDefs.Add(effect.ID, effect);
            else
                ParticleDefs[effect.ID] = effect;

            foreach (var pl in PlayerInfo.Online.Items)
                SendDefineEffect(pl, effect);
        }

        private static void SendDefineEffect(Player p, byte[] effect)
        {
            if (!p.Session.hasCpe) return;
            p.Send(effect);
        }
        public static void SendDefineEffect(Player p, DefinedEffect effect)
        {
            if (!p.Session.hasCpe) return;
            SendDefineEffect(p, EffectPacket(effect));
        }

        public static void SendDefineEffectAll(Player p)
        {
            if (!p.Session.hasCpe) return;
            foreach (var particle in ParticleDefs)
                SendDefineEffect(p, particle.Value);
        }
        public static byte[] EffectPacket(DefinedEffect effect)
        {
            return MCGalaxy.Network.Packet.DefineEffect(effect.ID, effect.U1, effect.V1, effect.U2, effect.V2,
                effect.R, effect.G, effect.B, effect.FrameCount, effect.ParticleCount, effect.Size, effect.SizeVariation,
                effect.Spread, effect.Speed, effect.Gravity, effect.BaseLifetime, effect.LifetimeVariation,
                effect.ExpireOnTouchGround, effect.CollidesSolid, effect.CollidesLiquid, effect.CollidesLeaves, effect.FullBright);
        }
    }
}