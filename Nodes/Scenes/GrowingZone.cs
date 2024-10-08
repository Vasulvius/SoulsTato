using Godot;
using System;

public partial class GrowingZone : StaticBody2D
{
	[Export] private Timer carrotTimer;
	[Export] private AnimatedSprite2D animatedSpritePlant;
	private int plant;
	private bool plantGrowing = false;
	private bool plantGrown = false;
	private bool isHighLithed = false;
	private bool playerIn = false;

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
		if(playerIn)
		{
			if(Input.IsActionJustPressed("interact"))
			{
				PlantSeed();
				Harvest();
			}
		}
    }

	// private void OnArea2dEntered(Area2D area)
	// {
	// 	if(!plantGrowing)
	// 	{
	// 		plantGrowing = true;
	// 		if(plant == 1)
	// 		{
	// 			carrotTimer.Start();
	// 			animatedSpritePlant.Play("carrot");
	// 		}
	// 		if(plant == 2)
	// 		{
	// 			carrotTimer.Start(); // choose lettuce time not carrot one
	// 			animatedSpritePlant.Play("lettuce");
	// 		}
	// 	}
	// 	else
	// 	{
	// 		GD.Print("Plant is already growing here !");
	// 	}
	// }

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

	// private void OnArea2dInputEvent(Node viewport, InputEvent ev, int shape_idx)
	// {
	// 	if(Input.IsActionJustPressed("click"))
	// 	{
	// 		Harvest();
	// 	}
	// }

	private void BodyEntered(Node2D body)
	{
		// On player entering
		if(body.IsInGroup("Player"))
		{
			ToggleHighlight();
			playerIn = true;
			// Collect if it can
			// Harvest();
		}
	}

	private void BodyExited(Node2D body)
	{
		// On player entering
		if(body.IsInGroup("Player"))
		{
			ToggleHighlight();
			playerIn = false;
		}
	}

	private void PlantSeed()
	{
		// By default plant carrot seed
		if(!plantGrowing)
		{
			plantGrowing = true;
			carrotTimer.Start();
			animatedSpritePlant.Play("carrot");
		}
	}

	private void Harvest()
	{
		if(plantGrown)
		{
			if(plant == 1)
			{
				Global.Instance.numOfCarrots ++;
			}
			if(plant == 2)
			{
				Global.Instance.numOfOnions ++;
			}
			plantGrowing = false;
			plantGrown = false;
			animatedSpritePlant.Play("none");
			GD.Print("Number of carrots : " + Global.Instance.numOfCarrots);
		}
	}

	private void ToggleHighlight()
	{
		if(isHighLithed)
		{
			isHighLithed = false;
			this.Modulate = new Color(1, 1, 1, 1);
		}
		else
		{
			isHighLithed = true;
			this.Modulate = new Color(1, 0, 1, 1);
		}
	}
}
