using Godot;
using System;

public class PackedScenes : Node2D		//I didn't fully finish implementation of this class
{
	[Export]
	public PackedScene titleScreen;

	[Export]
	public PackedScene Map1;

	[Export]
	public PackedScene Map2;

	[Export]
	public PackedScene Map3;

	public static PackedScenes packedSceneClass;

	public override void _Ready()
	{
		packedSceneClass = this;
	}
}
