using Godot;
using System;
using System.ComponentModel;

public partial class CarrotSeedPack : StaticBody2D
{
	[Export] AnimatedSprite2D animatedSprite2D;
	private bool selected = false;
	private int seedType = 1; // carrot

	public override void _Ready()
	{
		animatedSprite2D.Play("default");
	}

	public override void _PhysicsProcess(double delta)
    {
        if(selected)
		{
			GlobalPosition = GlobalPosition.Lerp(GetGlobalMousePosition(), 25*(float)delta);
		}
    }

	private void OnArea2dInputEvent(Node viewport, InputEvent ev, int shape_idx)
	{
		if(Input.IsActionJustPressed("click"))
		{
			Global.Instance.plantSelected = seedType;
			selected = true;
		}
	}

	private void _input(InputEvent ev)
	{
		if(ev is InputEventMouseButton)
		{
			InputEventMouseButton myEv = (InputEventMouseButton)ev;
			if(myEv.ButtonIndex == MouseButton.Left && !myEv.Pressed)
			{
				// Stop moving carrot seed stack
				selected = false;
			}
		}
	}

    
}
