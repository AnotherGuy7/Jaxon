using Godot;
using System.Collections;

public class Flyer : StaticBody2D
{
	private const float maxHealth = 2f;
	private float health = 2f;

	private AnimatedSprite flyerAnim;
	private TextureRect healthBar;
	private Sprite healthBarOutline;
	private bool alerted = false;
	private Vector2 directionToPlayer;
	private int healthShowTimer = 0;
	
	public override void _Ready()
	{
		flyerAnim = GetNode<AnimatedSprite>("FlyerAnim");
		healthBar = GetNode<TextureRect>("HealthBar");
		healthBarOutline = GetNode<Sprite>("HealthBarOutline");
	}

	public override void _Process(float delta)
	{
		if (healthShowTimer > 0)
		{
			healthShowTimer--;
		}
		healthBar.RectSize = new Vector2((health / maxHealth) * 16f, 6f);
		healthBar.Modulate = healthBarOutline.Modulate = new Color(1f, 1f, 1f, healthShowTimer / 180f);
		if (health <= 0)
		{
			QueueFree();
		}
	}

	public override void _PhysicsProcess(float delta)
	{
		directionToPlayer *= 0.9f;		//velocity reduction so it slows down
		
		flyerAnim.FlipH = directionToPlayer.x < 0f;
		
		MoveLocalX(directionToPlayer.x);
		MoveLocalY(directionToPlayer.y);

	}	
	
	private void OnBodyEntered(object body)
	{
		if (body == Player.player)
		{
			GameData.HurtPlayer(1);
			directionToPlayer = Vector2.Zero;
		}
	}
	
	private void OnAttackTimerOut()
	{
		if (alerted)
		{
			Vector2 unormalizedDirection = Player.player.GlobalPosition - GlobalPosition;
			directionToPlayer = unormalizedDirection.Normalized() * 6f;
		}
	}

	private void OnDetectionEntered(object body)
	{
		if (body == Player.player)
		{
			alerted = true;
			flyerAnim.Play("Looking");
		}
	}

	private void OnDetectionAreaExited(object body)
	{
		if (body == Player.player)
		{
			alerted = false;
			flyerAnim.Play("Idle");
		}
	}

	private void OnAreaEntered(object area)
	{
		Area2D collider = area as Area2D;
		if (collider.Name.Contains("PBullet"))
		{
			health -= 1f;
			healthShowTimer = 180;
		}
	}
}
