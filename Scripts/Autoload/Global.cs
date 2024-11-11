using Godot;
using System;

public partial class Global : Node
{
	// Singleton vars
	public static Global Instance { get; private set; }

	// Player vars
	public int itemEquipedIndex;
	public string itemEquipedName;

	// Ressources vars
	public int plantSelected = 1; // 1 for carrot, 2 for lettuce
	public int numOfCarrots = 0;
	public int numOfOnions = 0;
	public int numOfRedPotatoe = 0;

	// Time vars
	public enum TimePeriod {DAY, NIGHT};
	public TimePeriod timePeriod;
	
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
