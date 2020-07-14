using Godot;
using System;

public class Spike : Area2D
{
	private void OnSpikeEntered(object body)
	{
		if (body == Player.player)
		{
			GameData.HurtPlayer(1);
		}
	}

}
