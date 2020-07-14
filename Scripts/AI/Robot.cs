using Godot;
using System;

public class Robot : RigidBody2D
{
	[Export]
	public int direction = 1;

	private AnimatedSprite robotAnim;
	private TextureRect healthBar;
	private Sprite healthBarOutline;

	private const float maxHealth = 3;
	private float health = 3;
	private int healthShowTimer = 0;

	public override void _Ready()
	{
		robotAnim = GetNode<AnimatedSprite>("RobotAnim");
		healthBar = GetNode<TextureRect>("HealthBar");
		healthBarOutline = GetNode<Sprite>("HealthBarOutline");
	}

	private void OnAreaEntered(object area)
	{
		if (area.GetType().ToString() == "Bullet")
		{
			health -= 1f;
			healthShowTimer = 180;
		}
		else
		{
			Area2D potentialLimiter = area as Area2D;
			if (potentialLimiter.Name.Contains("Limit"))
			{
				direction *= -1;
			}
		}
	}


	private void OnBodyEntered(object body)
	{
		if (body == Player.player)
		{
			GameData.HurtPlayer(1);
		}
	}

	public override void _Process(float delta)
	{
		if (healthShowTimer > 0)
		{
			healthShowTimer--;
		}
		healthBar.RectSize = new Vector2((health / maxHealth) * 16f, 6f);
		healthBar.Modulate = new Color(1f, 1f, 1f, healthShowTimer / 180f);
		healthBarOutline.Modulate = new Color(1f, 1f, 1f, healthShowTimer / 180f);
		if (health <= 0)
		{
			QueueFree();
		}
	}

	public override void _PhysicsProcess(float delta)
	{
		Vector2 velocity = new Vector2(0.5f * direction, 0f);

		robotAnim.FlipH = direction == 1;

		MoveLocalX(velocity.x);
		MoveLocalY(velocity.y);
	}
}
