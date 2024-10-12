using Godot;
using System;
using System.Linq;

public partial class Inventory : Node
{
	// Singleton vars
	public static Inventory Instance { get; private set; }

	// Data vars
	[Export] private Godot.Collections.Array<Node2D> brutInventory;
	// [Export] private Node2D[] brutInventory = {};

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Create this as a singleton
		Instance = this;
	}

	public void AddItem(Node2D item)
	{
		brutInventory.Add(item);
	}
}
