using Godot;
using System;

public partial class RedPotatoe : Object
{
	public override bool stackable
	{
		get { return true; }
	}

    public override int stackSize
	{
		get { return 10; }
	}

    [Export]public override Texture2D texture
	{
		get { return _texture; }
		set { _texture = value; }
	}
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		GD.Print(texture.ResourcePath);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
