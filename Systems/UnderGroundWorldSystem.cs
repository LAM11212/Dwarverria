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
            }
        }
        
    }
}
