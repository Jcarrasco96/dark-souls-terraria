using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraSouls.Projectiles;

// ReSharper disable once ClassNeverInstantiated.Global
public class BloodstainProjectile : ModProjectile
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        Projectile.width = 26;
        Projectile.height = 50;
        Projectile.timeLeft = int.MaxValue;
        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;
        Projectile.penetrate = -1;
    }

    public override void AI()
    {
        base.AI();
        
        Projectile.timeLeft = int.MaxValue;
        ++Projectile.frameCounter;
        if (Projectile.frameCounter >= 6)
        {
            Projectile.frameCounter = 0;
            Projectile.frame++;
        }
        if (Projectile.frame > 7)
        {
            Projectile.frame = 0;
        }

        var pulse = 0.2f + 0.2f * (float)Math.Sin(Main.GlobalTimeWrappedHourly * 4f);

        Projectile.alpha = (int)(130 * pulse);

        Lighting.AddLight(Projectile.Center, new Vector3(0f, 1f, 0f) * pulse);

        if (!Main.rand.NextBool(6))
        {
            return;
        }

        var dustId = Dust.NewDust(Projectile.Center, 0, 0, DustID.GreenFairy, 0f, 0f, 100, default, 1.2f);
        var dust = Main.dust[dustId];
        dust.velocity = Main.rand.NextFloat(MathHelper.TwoPi).ToRotationVector2() * Main.rand.NextFloat(2.5f, 4.8f);
        dust.noGravity = true;
        dust.fadeIn = 0.4f;
        dust.scale = 0.9f;
        dust.alpha = 160;
    }

    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();
        Main.projFrames[Projectile.type] = 8;
    }
}