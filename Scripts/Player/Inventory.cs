using Godot;
using System;
using System.Linq;

public partial class Inventory : Node
{
	// Singleton vars
	public static Inventory Instance { get; private set; }

	// Data vars
	private Godot.Collections.Array<string> brutInventory = new Godot.Collections.Array<string>();
	// [Export] private Node2D[] brutInventory = {};

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Create this as a singleton
		Instance = this;
		DisplayMe();
	}

	public void AddItem(Object item)
	{
		brutInventory.Add(ObjectDataBase.Instance.FromItemPathGiveID(item.path));
	}

	private async void DisplayMe()
	{
		await ToSignal(GetTree().CreateTimer(1), "timeout");
		GD.Print(brutInventory);
		DisplayMe();
	} 
}
