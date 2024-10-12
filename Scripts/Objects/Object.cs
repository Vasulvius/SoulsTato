using Godot;
using System;

public abstract partial class Object : Node2D
{
    public abstract string path { get; }
    [Export] public bool stackable {get; protected set;}
    [Export] public int stackSize {get; protected set;}
    [Export] public Texture2D texture {get; protected set;}
}