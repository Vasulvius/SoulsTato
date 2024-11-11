using Godot;
using System;

public partial class InventoryView : Control
{
	// Singleton vars
	public static InventoryView Instance { get; private set; }

	// Other vars
	[Export] Godot.Collections.Array<TextureRect> slots;
	[Export] Godot.Collections.Array<Label> labels;

	public override void _Ready()
	{
		// Create this as a singleton
		Instance = this;
	}

	public void ChangeSlotTexture(int slotNumber, Texture2D icon)
	{
		slots[slotNumber].Texture = icon;
	}

	public void ChangeLabel(int labelNumber, string text)
	{
		labels[labelNumber].Text = text;
	}
}
