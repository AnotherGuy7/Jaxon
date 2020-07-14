using Godot;
using System;

public class Hoverboard : StaticBody2D
{
	[Export]
	public Vector2 velocity;

	private int direction = -1;

	private void OnLimitDetectorAreaEntered(object area)
	{
		Area2D potentialLimiter = area as Area2D;
		if (potentialLimiter.Name.Contains("Limit"))
		{
			direction *= -1;
		}
	}

	public override void _PhysicsProcess(float delta)
	{
		MoveLocalX(velocity.x * direction);		//so that depending on where you set it to go it turns back around
		MoveLocalY(velocity.y * direction);
	}
}
