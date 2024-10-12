using Godot;
using System;

public partial class ObjectDataBase : Node
{
	// Singleton vars
	public static ObjectDataBase Instance { get; private set; }

	// Data vars
	[Export] public Node2D[] objects;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Create this as a singleton
		Instance = this;
	}
}
