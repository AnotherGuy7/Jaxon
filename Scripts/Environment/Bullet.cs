using Godot;
using System;

public class Bullet : Area2D
{
	public Vector2 velocity;
	public Sprite bulletSprite;

	public override void _Ready()
	{
		bulletSprite = GetNode<Sprite>("BulletSprite");
	}

	public override void _Process(float delta)
	{
		bulletSprite.Rotation = velocity.Angle();
	}

	private void OnBulletBodyEntered(object body)
	{
		if (body != Player.player)
		{
			QueueFree();
		}
	}

	private void OnTimeLeftOut()
	{
		QueueFree();
	}

	public override void _PhysicsProcess(float delta)
	{
		MoveLocalX(velocity.x);
		MoveLocalY(velocity.y);
	}
}
