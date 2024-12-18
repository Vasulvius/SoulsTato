using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class ObjectDataBase : Node
{
	// Singleton vars
	public static ObjectDataBase Instance { get; private set; }

	// Data vars
	[Export] private Godot.Collections.Array<Texture2D> textures;
	private Dictionary<string, Dictionary> items;
	public Dictionary<string, string> itemsID {get; private set;}
	private const string ITEMS_JSON_PATH = "res://Data/Items.json";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Create this as a singleton
		Instance = this;
		// Get the items data
		items = ReadItemsData();
		// Create a quick dictionary about ID and item's path
		itemsID = CreateItemsID_Dict();
	}

	private Dictionary<string, Dictionary> ReadItemsData()
	{
		FileAccess files = FileAccess.Open(ITEMS_JSON_PATH, FileAccess.ModeFlags.Read);
		string text = files.GetAsText();
		Dictionary<string, Dictionary> jsonFile = (Dictionary<string, Dictionary>)Json.ParseString(text);
		files.Close();

		return jsonFile;
	}

	private Dictionary<string, string> CreateItemsID_Dict()
	{
		Dictionary<string, string> res = new Dictionary<string, string>();
		foreach(string k in items.Keys)
		{
			res.Add(k, (string)items[k]["path"]);
		}
		return res;
	}

	public string FromItemPathGiveID(string path)
	{
		// From the path of the item give the corresponding ID
		var temp = itemsID.ToDictionary(x => x.Value, x => x.Key);
		return temp[path];
	}

	public int GetStackSize(string ID)
	{
		return (int)items[ID]["stacksize"];
	}

	public string GetIconPath(string ID)
	{
		return (string)items[ID]["icon"];
	}

	public Texture2D GetIcon(string ID)
	{
		string iconPath = (string)items[ID]["icon"];
		foreach(Texture2D texture in textures)
		{
			if(texture.ResourcePath == iconPath){ return texture; }
		}

		return null;
	}
}
