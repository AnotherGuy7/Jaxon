using Godot;
using System;

public class StreetLight : Sprite
{
	private Sprite light;
	
	public override void _Ready()
	{
		light = GetNode<Sprite>("Layer1/Light");
	}
	
	public override void _Process(float delta)
	{
		light.FlipH = FlipH;
		light.GlobalPosition = GlobalPosition;
	}
}
