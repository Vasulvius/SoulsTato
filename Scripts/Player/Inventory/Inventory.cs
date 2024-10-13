using Godot;
using System;
using System.Linq;

public partial class Inventory : Node
{
	// Singleton vars
	public static Inventory Instance { get; private set; }

	// Data vars
	private const int SLOT_NUMBER = 10;
	// private Godot.Collections.Array<string> brutInventory = new Godot.Collections.Array<string>();
	private Godot.Collections.Array<InventorySlot> slots = new Godot.Collections.Array<InventorySlot>();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Create this as a singleton
		Instance = this;
		// Create the array of slots
		for(int i = 0; i < SLOT_NUMBER; i++)
		{
			slots.Add(new InventorySlot());
		}
	}

	public bool AddItemInSlots(string path)
	{

		// Return true if the item was putted in inventory else return false

		string ID = ObjectDataBase.Instance.FromItemPathGiveID(path);
		int stackSize = ObjectDataBase.Instance.GetStackSize(ID);
		bool puttedInSlots = false;

		for(int k = 0; k < slots.Count; k++)
		{
			InventorySlot slot = slots[k];
			if((slot.itemID == ID || slot.itemID == null) && slot.qty < stackSize)
			{
				// Put item then quit for loop
				slot.AddItem(ID);
				puttedInSlots = true;
				Texture2D icon = ObjectDataBase.Instance.GetIcon(ID);
				if(icon == null){ throw new ArgumentException("No icon found in ObjectDataBase");}
				InventoryView.Instance.ChangeSlotTexture(k, icon);
				InventoryView.Instance.ChangeLabel(k, IntToString(slot.qty));
				break;
			}
		}

		PrintSlots();

		return puttedInSlots;
	}

	// Utils

	private string IntToString(int num)
	{
		if(num < 10)
		{
			return "0"+num;
		}
		else
		{
			return num.ToString();
		}
	}
	private void PrintSlots()
	{
		int k = 0;
		foreach(InventorySlot slot in slots)
		{
			GD.Print("Slot " + k + " : ", slot.itemID, " | ", slot.qty);
			k ++;
		}
	}
}
