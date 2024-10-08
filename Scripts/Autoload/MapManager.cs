using Godot;
using System;

public partial class MapManager : Node2D
{
	public static MapManager Instance { get; private set; }
	private TileMapLayer terraformableLayer;
	private const int SOURCE_ID = 0;
	private Vector2I SEED_TILE = new Vector2I(11, 1);
	public override void _Ready()
	{
		// Create this as a singleton
		Instance = this;
		terraformableLayer = (TileMapLayer)GetTree().GetNodesInGroup("Terraformable")[0];
	}

	public override void _Process(double delta)
	{
	}

	private void _input(InputEvent ev)
	{
		if(Input.IsActionJustPressed("click"))
		{
			Vector2I localMousePosition = terraformableLayer.LocalToMap(GetGlobalMousePosition());
			terraformableLayer.SetCell(localMousePosition, SOURCE_ID, SEED_TILE);
		}
	}
}
