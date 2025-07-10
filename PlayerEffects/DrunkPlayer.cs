using Dwarverria.Content.Items.Potions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Dwarverria.PlayerEffects
{
    public class DrunkPlayer : ModPlayer
    {
        public bool drunkEffect = false;

        public override void ModifyScreenPosition()
        {
            if(drunkEffect && Main.myPlayer == Player.whoAmI)
            {
                Microsoft.Xna.Framework.Vector2 offset = new Microsoft.Xna.Framework.Vector2(
                    (float)Math.Sin(Main.GameUpdateCount * 0.1f) * 10f,
                    (float)Math.Cos(Main.GameUpdateCount * 0.1f) * 10f
                );

                Main.screenPosition += offset;
            }
        }

        public override void ResetEffects()
        {
            drunkEffect = false;
        }

    }
}
