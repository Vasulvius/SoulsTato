using Godot;
using System;

public abstract partial class Object : Node2D
{
    public abstract bool stackable { get; }
    public abstract int stackSize { get; }
    public abstract Texture2D texture { get; set;}
    protected Texture2D _texture;
}