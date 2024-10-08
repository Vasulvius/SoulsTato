using Godot;
using System;
using System.Linq;

public partial class MapManager : Node2D
{
	// Singleton vars
	public static MapManager Instance { get; private set; }

	// Player vars
	private Player player;

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
		// Go search for the player
		var playerArray = GetTree().GetNodesInGroup("Player");
		if(playerArray.Count() > 0)
		{
			// There is scene with no players eg. Main Menu scene
			player = (Player)playerArray[0];
		}
	}

	private void _input(InputEvent ev)
	{
		// Get mouse position in tile map local coordonate
		// Vector2I tilePosition = terraformableLayer.LocalToMap(GetGlobalMousePosition());
		Vector2I playerPosition = groundLayer.LocalToMap(player.GlobalPosition);
		Vector2I tilePosition = LookedTilePostion(playerPosition);

		if(Input.IsActionJustPressed("PlantAndHarvest"))
		{
			bool plantedOnPlayer = Plant(playerPosition);
			bool plantedBesidePlayer = false;
			if(!plantedOnPlayer)
			{
				plantedBesidePlayer = Plant(tilePosition);
			}

			bool harvestOnPlayer = Harvest(playerPosition);
			if(!harvestOnPlayer && !plantedOnPlayer && !plantedBesidePlayer)
			{
				Harvest(tilePosition);
			}
		}
		if(Input.IsActionJustPressed("UseHue"))
		{
			bool hueOnPlayer = UseHue(playerPosition);
			if(!hueOnPlayer)
			{
				UseHue(tilePosition);
			}
		}
	}

	private async void HandleSeed(Vector2I tilePosition, int level, Vector2I atlasCoord, int finalSeedLevel)
	{
		// Manage the growth

		cultureLayer.SetCell(tilePosition, SOURCE_ID, atlasCoord);
		await ToSignal(GetTree().CreateTimer(GROWTH_TIME), "timeout");

		if(level == finalSeedLevel)
		{
			// Just do nothing
		}
		else
		{
			Vector2I newAtlas = new Vector2I(atlasCoord.X+1, atlasCoord.Y);
			HandleSeed(tilePosition, level+1, newAtlas, finalSeedLevel);
		}

	}

	private Vector2I LookedTilePostion(Vector2I playerPosition)
	{
		Vector2I lookedPosition = new Vector2I(0, 0);
		if(player.actualLookingDirection == Player.lookingDirection.Up)
		{
			lookedPosition = new Vector2I(playerPosition.X, playerPosition.Y - 1);
		}
		else if(player.actualLookingDirection == Player.lookingDirection.Down)
		{
			lookedPosition = new Vector2I(playerPosition.X, playerPosition.Y + 1);
		}
		else if(player.actualLookingDirection == Player.lookingDirection.Left)
		{
			lookedPosition = new Vector2I(playerPosition.X - 1, playerPosition.Y);
		}
		else if(player.actualLookingDirection == Player.lookingDirection.Right)
		{
			lookedPosition = new Vector2I(playerPosition.X + 1, playerPosition.Y);
		}
		return lookedPosition;
	}

	private bool Plant(Vector2I tilePosition)
	{
		/*
		Return false if no seed was planted
		Else return true
		*/
		/*
		First check if I can plant on the given tile
		*/

		// Get the tile data of the terraformable layer
		TileData terraformableTileData = terraformableLayer.GetCellTileData(tilePosition);
		// Get the tile data of the culture layer
		TileData cultureTileData = cultureLayer.GetCellTileData(tilePosition);

		if(terraformableTileData != null)
		{
			// Look for if I can plant a seed
			canPlaceSeed = (bool)terraformableTileData.GetCustomData(CAN_PLACE_SEEDS_PROP_NAME);

			if(cultureTileData != null)
			{
				// Cannot place seeds on planted seeds
				canPlaceSeed = (bool)cultureTileData.GetCustomData(CAN_PLACE_SEEDS_PROP_NAME);
			}
		}
		else
		{
			canPlaceSeed = false;
		}

		/*
		Plant if I can
		*/

		if(canPlaceSeed)
		{
			// Plant a seed in the culture tile map layer
			cultureLayer.SetCell(tilePosition, SOURCE_ID, SEED_TILE);
			HandleSeed(tilePosition, 0, SEED_TILE, 3);
			return true;
		}
		else
		{
			return false;
		}
	}

	private bool Harvest(Vector2I tilePosition)
	{
		/*
		Return false if not harvested
		Else return true
		*/

		/*
		First check if I can harvest on the given tile
		*/

		// Get the tile data of the terraformable layer
		TileData terraformableTileData = terraformableLayer.GetCellTileData(tilePosition);
		// Get the tile data of the culture layer
		TileData cultureTileData = cultureLayer.GetCellTileData(tilePosition);

		if(terraformableTileData != null)
		{
			if(cultureTileData != null)
			{
				// Check if I can harvest
				canHarvest = (bool)cultureTileData.GetCustomData(CAN_PLACE_HARVEST_PROP_NAME);
			}
			else
			{
				canHarvest = false;
			}
		}

		/*
		Harvest if I can
		*/

		if(canHarvest)
		{
			// Remove tile
			// Better than cultureLayer.EraseCell()
			cultureLayer.SetCell(tilePosition, SOURCE_ID, EMPTY_TILE);
			Global.Instance.numOfRedPotatoe ++;
			GD.Print("Number of red potatoes = ", Global.Instance.numOfRedPotatoe);
			return true;
		}
		else
		{
			return false;
		}
	}

	private bool UseHue(Vector2I tilePosition)
	{
		/*
		First check if I can use hue on the given tile
		*/

		// Get the tile data of the ground layer
		TileData groundTileData = groundLayer.GetCellTileData(tilePosition);
		// Get the tile data of the terraformable layer
		TileData terraformableTileData = terraformableLayer.GetCellTileData(tilePosition);

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

		/*
		Use hue if I can
		*/

		if(canPlaceDirt)
		{
			// Place dirt
			dirtTiles.Add(tilePosition);
			terraformableLayer.SetCellsTerrainConnect(dirtTiles, DIRT_TERRAIN_SET, DIRT_TERRAIN);
			return true;
		}
		else
		{
			return false;
		}
	}
}
