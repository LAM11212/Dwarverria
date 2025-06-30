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

namespace Dwarverria.Systems
{
    public class UnderGroundWorldSystem : ModSystem
    {

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            int genIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Surface Caves"));

            if(genIndex != -1)
            {
                tasks[genIndex] = new UndergroundWorldGenPass();
            }
        }

        private class UndergroundWorldGenPass : GenPass
        {
            public UndergroundWorldGenPass() : base("Dwarverria Underground", 0.2f) { }

            protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
            {
                progress.Message = "Digging Deep...";

                for (int x = 50; x < Main.maxTilesX - 50; x++)
                {
                    progress.Set(x / (float)Main.maxTilesX);

                    int surfaceStart = 0;
                    int surfaceEnd = Main.maxTilesY;

                    for (int y = surfaceStart + 50; y < surfaceEnd - 50; y++)
                    {
                        Tile tile = Framing.GetTileSafely(x, y);
                        tile.ClearEverything();

                        if (WorldGen.genRand.NextFloat() < 0.1f)
                        {
                            continue;
                        }
                        tile.TileType = TileID.Stone;
                        tile.HasTile = true;
                        WorldGen.SquareTileFrame(x, y);
                    }
                }
            }
        }
    }
}
