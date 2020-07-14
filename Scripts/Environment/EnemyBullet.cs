using Godot;
using System;
using System.Collections;

public class EnemyBullet : Area2D
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
		if (body == Player.player)
		{
			GameData.HurtPlayer(1);
			QueueFree();
		}
		else
		{
			if (body.GetType().ToString() != "Gunner")
			{
				QueueFree();
			}
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
