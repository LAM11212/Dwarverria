using Dwarverria.PlayerEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Dwarverria.Content.Items.Buffs
{
    public class BeerBuff : ModBuff
    {
        public bool isDrunk = false;

        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.moveSpeed += 5;
            player.poisoned = true;
            player.dpsDamage += 1;
            player.GetModPlayer<DrunkPlayer>().drunkEffect = true;
        }
    }
}
