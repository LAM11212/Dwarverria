using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Terraria.GameContent.Generation;
using Terraria.IO;
using rail;
using Mono.Cecil;
using Microsoft.Xna.Framework;

namespace Dwarverria.Systems
{
    public class UnderGroundWorldSystem : ModSystem
    {
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            int microBiomeIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Micro Biomes"));
            int iceBiomeIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Surface Caves"));
            int tunnelsIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Tunnels"));

            if (tunnelsIndex != -1)
            {
                tasks.RemoveAt(tunnelsIndex);
            }

            if (iceBiomeIndex != -1)
            {
                tasks[iceBiomeIndex] = new DwarverriaOverworld();
            }
            
            if (microBiomeIndex != -1)
            {
                tasks.Insert(microBiomeIndex + 1, new UndergroundWorldGenPass());
            }
        }

        

        private class UndergroundWorldGenPass : GenPass
        {
            public UndergroundWorldGenPass() : base("Dwarverria Underground", 0.2f) { }

            protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
            {
                progress.Message = "Forging The World";

                int topBorder = (int)(Main.worldSurface - 30);
                int spawnY = (int)(Main.worldSurface + 50);
                int spawnX = (int)(Main.maxTilesX / 2);

                Main.spawnTileX = spawnX;
                Main.spawnTileY = spawnY;

                for (int x = -5; x <= 5; x++)
                {
                    for (int y = -5; y <= 5; y++)
                    {
                        int worldX = spawnX + x;
                        int worldY = spawnY + y;

                        WorldGen.KillTile(worldX, worldY, false, false, true);
                        WorldGen.KillWall(worldX, worldY, false);
                    }
                }

                for (int x = 10; x < Main.maxTilesY - 70; x++)
                {
                    int y = topBorder;
                    WorldGen.PlaceTile(x, y, TileID.ObsidianBrick, forced: true, mute: true);

                    Tile tile = Framing.GetTileSafely(x, y);
                    tile.HasTile = true;
                    tile.TileType = TileID.ObsidianBrick;
                    tile.IsActuated = false;
                }

                int chestX = spawnX + 2;
                int chestY = spawnY;
                for(int i = 0; i < 2; i++)
                {
                    WorldGen.PlaceTile(chestX + i, chestY + 1, TileID.ObsidianBrick, forced: true, mute: true);
                }
                int chestIndex = WorldGen.PlaceChest(chestX, chestY, style: 0);
                if(chestIndex >= 0)
                {
                    Chest chest = Main.chest[chestIndex];

                    chest.item[0].SetDefaults(ItemID.IronPickaxe);
                    chest.item[0].stack = 1;

                    chest.item[1].SetDefaults(ItemID.Torch);
                    chest.item[1].stack = 999;

                    chest.item[2].SetDefaults(ItemID.HealingPotion);
                    chest.item[2].stack = 20;
                }

                WorldGen.PlaceTile(spawnX, spawnY + 1, TileID.Stone, forced: true, mute: true);
                WorldGen.PlaceTile(spawnX, spawnY, TileID.Torches, forced: true, mute: true);

            }
        }
        
        private class DwarverriaOverworld : GenPass
        {
            public DwarverriaOverworld() : base("Dwarverria Overworld", 0.1f) { }

            protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
            {
                progress.Message = "Forging The Overworld";

                int spaceStartY = (int)(Main.maxTilesY * 0.07);
                int surfaceStartY = (int)(Main.worldSurface);

                for (int x = 0; x < Main.maxTilesX; x++)
                {
                    for(int y = spaceStartY; y < surfaceStartY; y++)
                    {
                        Tile tile = Framing.GetTileSafely(x, y);
                        tile.HasTile = true;
                        tile.TileType = TileID.Stone;
                        tile.IsActuated = false;
                    }
                }

                int numberOfDiamonds = 250;
                int numberOfRifts = 120;
                /*
                for(int i = 0; i < numberOfDiamonds; i++)
                {
                    int x = WorldGen.genRand.Next(100, Main.maxTilesX - 100);
                    int y = WorldGen.genRand.Next(spaceStartY, surfaceStartY);

                    int xDirection = WorldGen.genRand.NextBool() ? 1 : -1;
                    int yDirection = WorldGen.genRand.NextBool() ? 1 : -1;

                    WorldGen.digTunnel(x, y, x + xDirection * 500, y + yDirection * 100, WorldGen.genRand.Next(4, 7), WorldGen.genRand.Next(16, 32), false);
                }
                */
                for(int i = 0; i < numberOfRifts; i++)
                {

                    float radius = WorldGen.genRand.NextFloat(80, 160);
                    float angle = WorldGen.genRand.NextFloat(0, MathHelper.TwoPi);

                    Microsoft.Xna.Framework.Vector2 pos = new Vector2(
                        WorldGen.genRand.Next(100, Main.maxTilesX - 100),
                        WorldGen.genRand.Next(spaceStartY, surfaceStartY)
                        );

                    for (int step = 0; step < 100; step++)
                    {
                        angle += WorldGen.genRand.NextFloat(-0.2f, 0.2f);

                        pos.X += (float)Math.Cos(angle) * 2f;
                        pos.Y += (float)Math.Sin(angle) * 2f;

                        int x = Math.Clamp((int)pos.X, 10, Main.maxTilesX - 10);
                        int y = Math.Clamp((int)pos.Y, spaceStartY + 10, surfaceStartY - 10);

                        WorldGen.TileRunner(
                            x, y,                             // Location
                            WorldGen.genRand.Next(3, 6),      // Strength (radius)
                            WorldGen.genRand.Next(8, 16),     // Steps (size of the carved area)
                            -1,                               // Type (idk what this does tbh)
                            false,                            // add Tile 
                            0, 0,                             // speed (x/y dont need it here)
                            true                              // noYChange = false (set to true for no Y change confusing but thats how it works)
                            );
                    }
                }
            }
        }
        
    }
}
