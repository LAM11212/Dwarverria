using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using System.Drawing;
using Microsoft.Xna.Framework;


namespace Dwarverria.Content.Items.Tools.Pickaxes
{
    public class MegaPick : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.damage = 35;
            Item.knockBack = 5f;
            Item.useTime = 5;
            Item.useAnimation = 15;
            Item.pick = 200;

            Item.DamageType = DamageClass.Melee;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            //Item.value = do later
            Item.rare = ItemRarityID.Purple;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;

        }
        /*
        public override void MeleeEffects(Player player, Microsoft.Xna.Framework.Rectangle hitbox)
        {
            if (Main.rand.NextBool(5))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.GreenTorch);
            }
        }
        */
        public override bool? UseItem(Player player)
        {

            if (Main.netMode != NetmodeID.MultiplayerClient) // Only run this on the server side
            {
                Microsoft.Xna.Framework.Point mouseTile = Main.MouseWorld.ToTileCoordinates();

                int radius = 3;

                for (int x = 0; x <= radius; x++)
                {
                    for (int y = 0; y <= radius; y++)
                    {
                        int tileX = mouseTile.X + x;
                        int tileY = mouseTile.Y + y;

                        if (tileX >= 0 && tileX < Main.maxTilesX && tileY >= 0 && tileY < Main.maxTilesY)
                        {
                            Tile tile = Framing.GetTileSafely(tileX, tileY);
                            if (tile.HasTile && Main.tileSolid[tile.TileType])
                            {
                                WorldGen.KillTile(tileX, tileY, false, false, false);
                            }
                        }
                    }
                }
            }
            return true;
        }
    }
}
