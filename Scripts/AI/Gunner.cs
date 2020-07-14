using Godot;

public class Gunner : RigidBody2D
{
	[Export]
	public PackedScene bulletToShoot;
	
	private AnimatedSprite gunnerAnim;
	private Sprite gun;
	private Position2D gunBarrel;
	private TextureRect healthBar;
	private Sprite healthBarOutline;
	private Timer individualShotTimer;
	private Timer shootTimer;
	
	private const float maxHealth = 5;		//floats becuase of the way we handle the health bar
	//private const int shootCooldown = 180;
	//private const int individualShotCool = 60;
	private float health = 5;
	//private int shootTimer = 0;
	private int numberOfShots = 3;
	private float healthShowTimer = 0;
	private int direction = -1;
	private bool alerted = false;
	private float rotationSubtraction = 0;
	
	public override void _Ready()
	{
		gunnerAnim = GetNode<AnimatedSprite>("GunnerAnim");
		gun = GetNode<Sprite>("Gun");
		gunBarrel = GetNode<Position2D>("Gun/GunBarrel");
		healthBar = GetNode<TextureRect>("HealthBar");
		healthBarOutline = GetNode<Sprite>("HealthBarOutline");
		individualShotTimer = GetNode<Timer>("IndividualShotTimer");
		shootTimer = GetNode<Timer>("ShootTimer");
	}
	
	public override void _Process(float delta)
	{
		if (healthShowTimer > 0)
		{
			healthShowTimer--;
		}
		healthBar.RectSize = new Vector2((health / maxHealth) * 16f, 6f);
		healthBarOutline.Modulate = healthBar.Modulate = new Color(1f, 1f, 1f, healthShowTimer / 180f);
		if (health <= 0)
		{
			QueueFree();
		}
		if (direction == -1)
		{
			rotationSubtraction = Mathf.Deg2Rad(180f);
		}
		else
		{
			rotationSubtraction = 0;
		}
		if (alerted)
		{
			Vector2 rotation = Player.player.GlobalPosition - GlobalPosition;
			gun.Rotation = rotation.Angle() - rotationSubtraction;
			if (Player.player.GlobalPosition.x - GlobalPosition.x > 0f)
			{
				direction = 1;
			}
			else
			{
				direction = -1;
			}
		}
		gun.FlipH = gunnerAnim.FlipH;
		gun.Position = new Vector2(-7.5f * direction, -4f);
		gunBarrel.Position = new Vector2(-6 * -direction, 3f);

		if (alerted && numberOfShots <= 0 && shootTimer.IsStopped())
		{
			shootTimer.Start();
		}
	}
	
	public override void _PhysicsProcess(float delta)
	{
		Vector2 velocity = Vector2.Zero;		//the 10f is gravity
		if (!alerted)
		{
			velocity.x = 0.6f * direction;
		}
		gunnerAnim.FlipH = direction == 1;
		if (velocity != Vector2.Zero)
		{
			gunnerAnim.Play("Walking");
		}
		else
		{
			gunnerAnim.Play("Idle");
		}
		
		MoveLocalX(velocity.x);
		MoveLocalY(velocity.y);
	}
	
	private void OnDetectionBodyEntered(object body)
	{
		if (body == Player.player)
		{
			alerted = true;
			individualShotTimer.Start();
		}
	}
	
	private void OnDetectionBodyExited(object body)
	{
		if (body == Player.player)
		{
			alerted = false;
		}
	}

	private void OnHitboxAreaEntered(object area)
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

	private void OnHitboxEntered(object body)
	{
		if (body == Player.player)
		{
			GameData.HurtPlayer(1);
		}
	}

	private void OnIndividualShotTimerOut()
	{
		if (alerted && numberOfShots > 0)
		{
			numberOfShots--;
			Vector2 rotation = Player.player.GlobalPosition - GlobalPosition;
			Area2D bullet = (Area2D)bulletToShoot.Instance();
			bullet.GlobalPosition = gunBarrel.GlobalPosition;
			bullet.Rotation = rotation.Angle();
			bullet.Set("velocity", rotation.Normalized() * 7f * direction);
			GameData.currentContainer.AddChild(bullet);
			individualShotTimer.Start();
		}
	}


	private void OnShootTimerOut()
	{
		numberOfShots = 3;
		individualShotTimer.Start();
	}
}
