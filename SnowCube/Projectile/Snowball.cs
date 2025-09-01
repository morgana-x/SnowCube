using MCGalaxy;
using MCGalaxy.Maths;
using SnowCube.Modules.Players;
using SnowCube.Modules.World;
using static SnowCube.Events.PlayerEvents;
namespace SnowCube.Projectile
{
    public class Snowball : Projectile
    {

        public Snowball(Level level, Player thrower, Vec3F32 position, Vec3S32 velocity) : base(level, thrower, position, velocity)
        {

        }

        public Snowball()
        {

        }

        public override bool Tick()
        {
            Effect.EmitEffect(Level, Effect.Effects.Snowball_Trail, Pos);
            Effect.EmitEffect(Level, Effect.Effects.Snowball_Ball, Pos);
            return base.Tick();
        }

        public override void OnDestroy()
        {
            Effect.EmitEffect(Level, Effect.Effects.Snowball_Hit, Pos);
            base.OnDestroy();
        }

        public override void OnCollide(ushort block, Player pl)
        {
            if (pl != null)
            {
                bool cancel = false;
                PlayerHitBySnowballEvent.Call(pl, this.Thrower, ref cancel);
                if (!cancel)
                {
                    pl.Send(MCGalaxy.Network.Packet.VelocityControl(Vel.X, 2, Vel.Z, 0, 0, 0));
                    Health.Damage(pl, 1, DamageData.DamageType.Snowball, this.Thrower);
                }
   
            }
            Effect.EmitEffect(Level, Effect.Effects.Snowball_Hit, Pos + new Vec3F32(0, 1, 0));
            base.OnCollide(block, pl);
        }
    }
}
