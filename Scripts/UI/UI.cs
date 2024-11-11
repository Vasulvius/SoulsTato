using Godot;
using System;

public partial class UI : CanvasLayer
{
	[Export] Control inventoryView;
    public override void _Input(InputEvent @event)
    {
        if(Input.IsActionJustPressed("Start"))
		{
			inventoryView.Visible = !inventoryView.Visible;
		}
    }
}
