using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class ObjectDataBase : Node
{
	// Singleton vars
	public static ObjectDataBase Instance { get; private set; }

	// Data vars
	public Dictionary<string, string> items {get; private set;}
	private const string ITEMS_JSON_PATH = "res://Data/Items.json";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Create this as a singleton
		Instance = this;
		items = ReadItemsData();
	}

	private Dictionary<string, string> ReadItemsData()
	{
		FileAccess files = FileAccess.Open(ITEMS_JSON_PATH, FileAccess.ModeFlags.Read);
		string text = files.GetAsText();
		Dictionary<string, string> jsonFile = (Dictionary<string, string>)Json.ParseString(text);
		files.Close();

		return jsonFile;
	}

	public string FromItemPathGiveID(string path)
	{
		// From the path of the item give the corresponding ID
		var temp = items.ToDictionary(x => x.Value, x => x.Key);
		return temp[path];
	}
}
