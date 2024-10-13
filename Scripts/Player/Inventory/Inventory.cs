using Godot;
using System;
using System.Linq;

public partial class Inventory : Node
{
	// Singleton vars
	public static Inventory Instance { get; private set; }

	// Data vars
	[Export] private int slotNumber = 10;
	private Godot.Collections.Array<string> brutInventory = new Godot.Collections.Array<string>();
	private Godot.Collections.Array<InventorySlot> slots = new Godot.Collections.Array<InventorySlot>();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Create this as a singleton
		Instance = this;
		// Create the array of slots
		for(int i = 0; i < slotNumber; i++)
		{
			slots.Add(new InventorySlot());
		}
	}

    public override void _Input(InputEvent @event)
    {
        if(Input.IsActionJustPressed("Test"))
		{
			GD.Print("Brut :", brutInventory);
			PutInventroyInSlots();
			GD.Print("Brut :", brutInventory);
			// slots[0].AddItem("0");
			int k = 0;
			foreach(InventorySlot slot in slots)
			{
				GD.Print("Slot " + k + " : ", slot.itemID, " | ", slot.qty);
				k ++;
			}
		}
    }

    public void AddItem(Object item)
	{
		brutInventory.Add(ObjectDataBase.Instance.FromItemPathGiveID(item.path));
	}

	private void PutInventroyInSlots()
	{
		foreach(string ID in brutInventory)
		{
			int stackSize = ObjectDataBase.Instance.GetStackSize(ID);
			foreach(InventorySlot slot in slots)
			{
				if((slot.itemID == ID || slot.itemID == null) && slot.qty < stackSize)
				{
					// Put item then quit for loop
					slot.AddItem(ID);
					break;
				}
			}
			// Ajouter quelque chose pour gérer le fait que l'objet n'a pas été mis dans un slot
		}
		brutInventory.Clear();
	}
}
