using Godot;
using System;
using System.Collections.Generic;

public partial class InventorySlot : Node
{
	public string itemID { get; private set; }
	public int qty { get; private set; }
	
	// Methods
	public void AddItem(string _itemID, int _qty = 1)
	{
		if(itemID != null && _itemID != itemID){ throw new ArgumentException("_itemID is supposed to be same as this.itemID."); }
		if(_qty < 1){ throw new ArgumentException("_qty cannont be < 1."); }
		itemID = _itemID;
		qty += _qty;
	}

	public void RemoveQty(int _qty)
	{
		if(_qty > qty){ throw new ArgumentException("Cannot remove more than slot have."); }
		qty -= _qty;
		if(qty == 0){ itemID = null; }
	}

	public Dictionary<string, int> RemoveItem()
	{
		Dictionary<string, int> res = new Dictionary<string, int>(){{itemID, qty}};
		itemID = null;
		qty = 0;
		return res;
	}
}
