using Godot;
using System;

public partial class TimeCycleManager : Node2D
{
	// Singleton vars
	public static TimeCycleManager Instance { get; private set; }

	// Time vars
	[Export] public Timer dayTimer {get; private set;}
	[Export] public Timer nightTimer {get; private set;}

	// Display vars
	[Export] private CanvasLayer nightCanvas;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Create this as a singleton
		Instance = this;
		LaunchTimeCycle();
	}

	private async void LaunchTimeCycle()
	{
		if(Global.Instance.timePeriod == Global.TimePeriod.DAY)
		{
			nightCanvas.Visible = false;
			dayTimer.Start();
			await ToSignal(dayTimer, "timeout");
			Global.Instance.timePeriod = Global.TimePeriod.NIGHT;
			// GD.Print("Night start");
			LaunchTimeCycle();
		}
		else
		{
			nightCanvas.Visible = true;
			nightTimer.Start();
			await ToSignal(nightTimer, "timeout");
			Global.Instance.timePeriod = Global.TimePeriod.DAY;
			// GD.Print("Day start");
			LaunchTimeCycle();
		}
	}
}
