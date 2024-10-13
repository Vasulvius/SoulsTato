using Godot;
using System;
using System.Linq;

public partial class Inventory : Node
{
	// Singleton vars
	public static Inventory Instance { get; private set; }

	// Data vars
	private const int SLOT_NUMBER = 1;
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

		foreach(InventorySlot slot in slots)
		{
			if((slot.itemID == ID || slot.itemID == null) && slot.qty < stackSize)
			{
				// Put item then quit for loop
				slot.AddItem(ID);
				puttedInSlots = true;
				break;
			}
		}

		PrintSlots();

		return puttedInSlots;
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
