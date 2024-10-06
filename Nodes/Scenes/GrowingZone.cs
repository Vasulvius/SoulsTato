using Godot;
using System;

public partial class GrowingZone : StaticBody2D
{
	[Export] private Timer carrotTimer;
	[Export] private AnimatedSprite2D animatedSpritePlant;
	private int plant;
	private bool plantGrowing = false;
	private bool plantGrown = false;

    public override void _Ready()
    {
        plant = Global.Instance.plantSelected;
    }

    public override void _PhysicsProcess(double delta)
    {
        if(!plantGrowing)
		{
			plant = Global.Instance.plantSelected;
		}
    }

	private void OnArea2dEntered(Area2D area)
	{
		if(!plantGrowing)
		{
			plantGrowing = true;
			if(plant == 1)
			{
				carrotTimer.Start();
				animatedSpritePlant.Play("carrot");
			}
			if(plant == 2)
			{
				carrotTimer.Start(); // choose lettuce time not carrot one
				animatedSpritePlant.Play("lettuce");
			}
		}
		else
		{
			GD.Print("Plant is already growing here !");
		}
	}

	private void CarrotTimerTimout()
	{
		AnimatedSprite2D carrotPlant = animatedSpritePlant;
		if(carrotPlant.Frame == 0)
		{
			carrotPlant.Frame = 1;
			carrotTimer.Start();
		}
		else if(carrotPlant.Frame == 1)
		{
			carrotPlant.Frame = 2;
			plantGrown = true;
		}
	}
}
