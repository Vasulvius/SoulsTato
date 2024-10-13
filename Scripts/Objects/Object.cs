using Godot;
using System;

public abstract partial class Object : Node2D
{
    public abstract string path { get; }

    protected void BodyEntered(Node2D body)
	{
		if(body == Player.Instance)
		{
			bool puttedInInventory = Inventory.Instance.AddItemInSlots(this.path);
			if(puttedInInventory){ QueueFree(); }
		}
	}
}