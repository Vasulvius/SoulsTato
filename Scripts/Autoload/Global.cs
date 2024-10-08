using Godot;
using System;

public partial class Global : Node
{
	public int plantSelected = 1; // 1 for carrot, 2 for lettuce
	public int numOfCarrots = 0;
	public int numOfOnions = 0;
	public int numOfRedPotatoe = 0;
	public static Global Instance { get; private set; }
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
