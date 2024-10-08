using Godot;
using System;
using System.Linq;

public partial class MapManager : Node2D
{
	public static MapManager Instance { get; private set; }
	private TileMapLayer groundLayer;
	private TileMapLayer terraformableLayer;
	private TileMapLayer cultureLayer;
	private const int SOURCE_ID = 0;
	private Vector2I SEED_TILE = new Vector2I(11, 1);
	private const string CAN_PLACE_SEEDS_PROP_NAME = "canPlaceSeeds";
	private const string CAN_PLACE_DIRT_PROP_NAME = "canPlaceDirt";
	private bool canPlaceSeed = false;
	private bool canPlaceDirt = false;
	private Godot.Collections.Array<Vector2I> dirtTiles = new Godot.Collections.Array<Vector2I>{};
	private const int DIRT_TERRAIN_SET = 1;
	private const int DIRT_TERRAIN = 0;
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

	public override void _Process(double delta)
	{
	}

	private void _input(InputEvent ev)
	{
		if(Input.IsActionJustPressed("click"))
		{
			// Get mouse position in tile map local coordonate
			Vector2I localMousePosition = terraformableLayer.LocalToMap(GetGlobalMousePosition());
			// Get the tile data of the terraformable layer
			TileData terraformableTileData = terraformableLayer.GetCellTileData(localMousePosition);

			if(terraformableTileData != null)
			{
				// Look for if I can plant a seed
				canPlaceSeed = (bool)terraformableTileData.GetCustomData(CAN_PLACE_SEEDS_PROP_NAME);
			}
			else
			{
				canPlaceSeed = false;
			}

			if(canPlaceSeed)
			{
				// Plant a seed in the culture tile map layer
				cultureLayer.SetCell(localMousePosition, SOURCE_ID, SEED_TILE);
			}
		}
		if(Input.IsActionJustPressed("right_click"))
		{
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
}
