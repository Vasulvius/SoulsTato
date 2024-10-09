using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export] private float speed = 100.0f;
	[Export] private AnimatedSprite2D animatedSprite2D;
	[Export] public int playerHeight = 16; // Height of the player sprite in pixel
	public enum lookingDirection {Up, Down, Left, Right};
	public lookingDirection actualLookingDirection = lookingDirection.Down;

    public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity = direction * speed;
			ManageMovingAnimation(direction);
		}
		else
		{
			Idle();
			velocity.X = Mathf.MoveToward(Velocity.X, 0, speed);
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	private void ManageMovingAnimation(Vector2 direction)
	{
		if (direction == new Vector2(0, 1))
		{
			// down
			animatedSprite2D.Play("down_walk");
			actualLookingDirection = lookingDirection.Down;
		}
		else if (direction == new Vector2(0, -1))
		{
			// up
			animatedSprite2D.Play("up_walk");
			actualLookingDirection = lookingDirection.Up;
		}
		else if (direction == new Vector2(-1, 0))
		{
			// left
			animatedSprite2D.Play("side_walk");
			animatedSprite2D.FlipH = false;
			actualLookingDirection = lookingDirection.Left;
		}
		else if (direction == new Vector2(1, 0))
		{
			// right
			animatedSprite2D.Play("side_walk");
			animatedSprite2D.FlipH = true;
			actualLookingDirection = lookingDirection.Right;
		}
	}

	private void Idle()
	{
		if(actualLookingDirection == lookingDirection.Up)
		{
			animatedSprite2D.Play("idle_up");
		}
		else if(actualLookingDirection == lookingDirection.Down)
		{
			animatedSprite2D.Play("idle");
		}
		else if(actualLookingDirection == lookingDirection.Left)
		{
			animatedSprite2D.FlipH = false;
			animatedSprite2D.Play("idle_left");
		}
		else if(actualLookingDirection == lookingDirection.Right)
		{
			animatedSprite2D.FlipH = true;
			animatedSprite2D.Play("idle_left");
		}
	}
}
