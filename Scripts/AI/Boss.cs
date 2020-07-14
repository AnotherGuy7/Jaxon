using Godot;

public class Boss : RigidBody2D
{
	private AnimatedSprite bossAnim;
	private Area2D flameThrowerArea;
	private Timer attackDurationTimer;

	private bool moving = true;
	private bool attacking = false;
	private int direction = -1;
	private bool scannedForBodies = false;

	public override void _Ready()
	{
		GameData.boss = this;
		bossAnim = GetNode<AnimatedSprite>("BossAnim");
		flameThrowerArea = GetNode<Area2D>("FlamethrowerArea");
		attackDurationTimer = GetNode<Timer>("AttackDurationTimer");
	}

	public override void _Process(float delta)
	{
		bossAnim.FlipH = direction == 1;
		if (attacking && !scannedForBodies)
		{
			foreach (object body in flameThrowerArea.GetOverlappingBodies())     //because if you stand on top of a spike and not move while the spike is active, you're counted as not entering
			{
				if (body == Player.player)
				{
					GameData.HurtPlayer(1);
				}
			}
			scannedForBodies = true;
		}
		if (GameData.bossHealth <= 0)
		{
			QueueFree();
		}
		if (attacking)
		{
			bossAnim.Play("Flamethrower");
		}
		else
		{
			bossAnim.Play("Walking");
		}
		if (bossAnim.Animation == "Flamethrower")
		{
			bossAnim.Offset = new Vector2(42f * direction, 0f);
		}
		else
		{
			bossAnim.Offset = Vector2.Zero;
		}
		if (direction == 1)
		{
			flameThrowerArea.Position = new Vector2(90f, 0f);
		}
		else
		{
			flameThrowerArea.Position = new Vector2(0f, 0f);
		}
	}

	public override void _PhysicsProcess(float delta)
	{
		Vector2 velocity = Vector2.Zero;

		if (moving)
		{
			velocity.x = 1.6f;
		}

		MoveLocalX(velocity.x * direction);
		MoveLocalY(velocity.y);
	}

	private void OnHitboxAreaEntered(object area)
	{
		if (area.GetType().ToString() == "Bullet")
		{
			GameData.bossHealth -= 1f;
		}
		else
		{
			Area2D potentialLimiter = area as Area2D;
			if (potentialLimiter.Name.Contains("Limit"))
			{
				direction *= -1;
				attacking = true;
				moving = false;
				attackDurationTimer.Start();
			}
		}
	}

	private void OnHitboxBodyEntered(object body)
	{
		if (body == Player.player)
		{
			GameData.HurtPlayer(1);
		}
	}

	private void OnAttackDurationTimerOut()
	{
		attacking = false;
		moving = true;
		scannedForBodies = false;
	}

	private void OnFlamethrowerBodyEntered(object body)
	{
		if (attacking && body == Player.player)
		{
			GameData.HurtPlayer(1);
		}
	}
}
