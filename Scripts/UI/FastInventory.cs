using Godot;
using System;

public partial class FastInventory : Control
{
	[Export] private Sprite2D[] equipements;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		displayEquiped(Global.Instance.itemEquipedIndex);
	}

	private void _input(InputEvent ev)
	{
		if(Input.IsActionJustPressed("ItemLeft"))
		{
			ChangeItemEquipedIndex(-1);
		}
		if(Input.IsActionJustPressed("ItemRight"))
		{
			ChangeItemEquipedIndex(1);
		}

	}

	private void displayEquiped(int itemIndex)
	{
		for(int i = 0; i < equipements.Length; i++)
		{
			if(i == itemIndex)
			{
				equipements[i].Visible = true;
			}
			else
			{
				equipements[i].Visible = false;
			}
		}
	}

	private void ChangeItemEquipedIndex(int offset)
	{
		// Offset should be +1 or -1 depends on if you go left or right
		if(offset > 0)
		{
			Global.Instance.itemEquipedIndex = (Global.Instance.itemEquipedIndex + offset)%equipements.Length;
		}
		else if(offset < 0)
		{
			Global.Instance.itemEquipedIndex = (Global.Instance.itemEquipedIndex + equipements.Length + offset)%equipements.Length;
		}
		
		displayEquiped(Global.Instance.itemEquipedIndex);
	}
}
