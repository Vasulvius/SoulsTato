using Godot;
using System;
using System.Linq;

public partial class MapManager : Node2D
{
	// Singleton vars
	public static MapManager Instance { get; private set; }

	// Global states
	private bool canPlaceSeed = false;
	private bool canPlaceDirt = false;
	private bool canHarvest = false;

	// Tile map management vars
	private TileMapLayer groundLayer;
	private TileMapLayer terraformableLayer;
	private TileMapLayer cultureLayer;
	private const string CAN_PLACE_SEEDS_PROP_NAME = "canPlaceSeeds";
	private const string CAN_PLACE_DIRT_PROP_NAME = "canPlaceDirt";
	private const string CAN_PLACE_HARVEST_PROP_NAME = "canHarvest";
	private const int SOURCE_ID = 0;
	private const int DIRT_TERRAIN_SET = 1;
	private const int DIRT_TERRAIN = 0;
	private Godot.Collections.Array<Vector2I> dirtTiles = new Godot.Collections.Array<Vector2I>{};
	private Vector2I EMPTY_TILE = new Vector2I(14, 0);

	// Growth vars
	private const float GROWTH_TIME = 3f;
	private Vector2I SEED_TILE = new Vector2I(11, 1);

	public override void _Ready()
	{
		// Create this as a singleton
		Instance = this;
		// Go search for the layer to terraform
		groundLayer = (TileMapLayer)GetTree().GetNodesInGroup("Background")[0];
		// Go search for the layer to terraform
		terraformableLayer = (TileMapLayer)GetTree().GetNodesInGroup("Terraformable")[0];
		// Go search for the layer to plant seeds
		cultureLayer = (TileMapLayer)GetTree().GetNodesInGroup("Culture")[0];
	}

	private void _input(InputEvent ev)
	{
		if(Input.IsActionJustPressed("PlantAndHarvest"))
		{
			// On click plant a seed if I can

			// Get mouse position in tile map local coordonate
			Vector2I localMousePosition = terraformableLayer.LocalToMap(GetGlobalMousePosition());
			// Get the tile data of the terraformable layer
			TileData terraformableTileData = terraformableLayer.GetCellTileData(localMousePosition);
			// Get the tile data of the culture layer
			TileData cultureTileData = cultureLayer.GetCellTileData(localMousePosition);

			if(terraformableTileData != null)
			{
				// Look for if I can plant a seed
				canPlaceSeed = (bool)terraformableTileData.GetCustomData(CAN_PLACE_SEEDS_PROP_NAME);

				if(cultureTileData != null)
				{
					// Cannot place seeds on planted seeds
					canPlaceSeed = (bool)cultureTileData.GetCustomData(CAN_PLACE_SEEDS_PROP_NAME);
					// Check if I can harvest
					canHarvest = (bool)cultureTileData.GetCustomData(CAN_PLACE_HARVEST_PROP_NAME);
				}
			}
			else
			{
				canPlaceSeed = false;
			}

			if(canPlaceSeed)
			{
				// Plant a seed in the culture tile map layer
				cultureLayer.SetCell(localMousePosition, SOURCE_ID, SEED_TILE);
				HandleSeed(localMousePosition, 0, SEED_TILE, 3);
			}

			if(canHarvest)
			{
				// Remove tile
				// Better than cultureLayer.EraseCell()
				cultureLayer.SetCell(localMousePosition, SOURCE_ID, EMPTY_TILE);
				Global.Instance.numOfRedPotatoe ++;
				GD.Print("Number of red potatoes = ", Global.Instance.numOfRedPotatoe);
			}
		}
		if(Input.IsActionJustPressed("UseHue"))
		{
			// On right_click place dirt in I can

			// Get mouse position in tile map local coordonate
			Vector2I localMousePosition = terraformableLayer.LocalToMap(GetGlobalMousePosition());
			// Get the tile data of the ground layer
			TileData groundTileData = groundLayer.GetCellTileData(localMousePosition);
			// Get the tile data of the terraformable layer
			TileData terraformableTileData = terraformableLayer.GetCellTileData(localMousePosition);

			if(groundTileData != null)
			{
				// Look for if I can place dirt on ground
				canPlaceDirt = (bool)groundTileData.GetCustomData(CAN_PLACE_DIRT_PROP_NAME);
				if(terraformableTileData != null)
				{
					// Look for if I can place dirt on path
					canPlaceDirt = (bool)terraformableTileData.GetCustomData(CAN_PLACE_DIRT_PROP_NAME);
				}
			}
			else
			{
				canPlaceDirt = false;
			}

			if(canPlaceDirt)
			{
				// Place dirt
				dirtTiles.Add(localMousePosition);
				terraformableLayer.SetCellsTerrainConnect(dirtTiles, DIRT_TERRAIN_SET, DIRT_TERRAIN);
			}

		}
	}

	private async void HandleSeed(Vector2I localMousePosition, int level, Vector2I atlasCoord, int finalSeedLevel)
	{
		// Manage the growth

		cultureLayer.SetCell(localMousePosition, SOURCE_ID, atlasCoord);
		await ToSignal(GetTree().CreateTimer(GROWTH_TIME), "timeout");

		if(level == finalSeedLevel)
		{
			// Just do nothing
		}
		else
		{
			Vector2I newAtlas = new Vector2I(atlasCoord.X+1, atlasCoord.Y);
			HandleSeed(localMousePosition, level+1, newAtlas, finalSeedLevel);
		}

	}
}
