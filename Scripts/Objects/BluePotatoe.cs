using Godot;
using System;

public partial class BluePotatoe : Object
{
	public override string path
	{
		get { return "res://Nodes/Objects/BluePotatoe.tscn"; }
	}
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void BodyEntered(Node2D body)
	{
		if(body == Player.Instance)
		{
			Inventory.Instance.AddItem(this);
			QueueFree();
		}
	}
}