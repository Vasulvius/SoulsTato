using Godot;
using System;

public partial class Inventory : Node
{
	// Singleton vars
	public static Inventory Instance { get; private set; }

	// Data vars
	private Node2D[] brutInventory;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Create this as a singleton
		Instance = this;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
