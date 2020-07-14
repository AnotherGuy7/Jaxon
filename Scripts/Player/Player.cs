using Godot;
using System;

public class Player : KinematicBody2D
{
	[Export]
	public PackedScene bulletScene;

	private const float moveSpeed = 6f;
	private const float jumpSpeed = -20f;
	private const float gravity = 0.7f;
	private const float maxFallSpeed = 25f;
	private const float bulletSpeed = 13f;
	private const int shootCooldown = 20;

	private float currentYVelocity = 0f;
	private float rotationSubtraction = 0;
	private Vector2 gunOffset = Vector2.Zero;
	private int shootCooldownTimer = 0;

	private bool doubleJumped = false;

	private AnimatedSprite playerAnim;
	private Sprite gun;
	private Position2D gunBarrelPoint;

	public static Player player;

	public override void _Ready()
	{
		player = this;
		playerAnim = GetNode<AnimatedSprite>("PlayerAnim");
		gun = GetNode<Sprite>("Gun");
		gunBarrelPoint = GetNode<Position2D>("Gun/GunBarrel");
	}

	public override void _PhysicsProcess(float delta)
	{
		Vector2 velocity = Vector2.Zero;

		if (!GameData.dead)
		{
			if (Input.IsActionPressed("move_left"))
			{
				velocity.x -= moveSpeed;
				playerAnim.FlipH = true;
				gun.Offset = new Vector2(-11.5f, 0f);
				gunOffset.x = 10f;
				gun.FlipH = true;
				rotationSubtraction = Mathf.Deg2Rad(180f);
				gunBarrelPoint.Position = new Vector2(-23f, -2f);
			}
			if (Input.IsActionPressed("move_right"))
			{
				velocity.x += moveSpeed;
				playerAnim.FlipH = false;
				gun.Offset = new Vector2(11.5f, 0f);
				gunOffset.x = 0f;
				gun.FlipH = false;
				rotationSubtraction = 0;
				gunBarrelPoint.Position = new Vector2(23f, -2f);
			}
			if (IsOnFloor())
			{
				doubleJumped = false;
				currentYVelocity = 1f;
				if (Input.IsActionJustPressed("jump"))
				{
					currentYVelocity = jumpSpeed;
				}
				if (velocity.x != 0f)
				{
					playerAnim.Play("Walking");
					gunOffset.y = 0;
					if (playerAnim.Frame == 1 || playerAnim.Frame == 3)
					{
						gunOffset.y = -1f;
					}
				}
				else
				{
					playerAnim.Play("Idle");
				}
			}
			else
			{
				if (Input.IsActionJustPressed("jump") && !doubleJumped)
				{
					doubleJumped = true;
					currentYVelocity = jumpSpeed;
				}
				if (currentYVelocity < maxFallSpeed)
				{
					currentYVelocity += gravity;
				}
				if (currentYVelocity > 0f)
				{
					playerAnim.Play("Jumping");
				}
				else
				{
					playerAnim.Play("Falling");
				}
			}
			velocity.y = currentYVelocity;
		}

		if (!GameData.dead)
		{
			MoveAndSlide(velocity * 14f, new Vector2(0, -1));
		}
		else
		{
			playerAnim.Play("Dying");
		}
	}

	public override void _Process(float delta)
	{
		gun.Position = new Vector2(-5 + gunOffset.x, -6 + gunOffset.y);
		Vector2 rotation = GetGlobalMousePosition() - GlobalPosition;
		gun.Rotation = rotation.Angle() - rotationSubtraction;
		gun.Visible = !GameData.dead;

		if (shootCooldownTimer > 0)
		{
			shootCooldownTimer--;
		}
		if (Input.IsActionPressed("shoot") && shootCooldownTimer <= 0 && GameData.playerMoney > 0 && !GameData.dead)
		{
			Area2D bullet = (Area2D)bulletScene.Instance();
			bullet.GlobalPosition = gunBarrelPoint.GlobalPosition;
			//bullet.Rotation = rotation.Angle();
			GameData.currentContainer.AddChild(bullet);
			bullet.Set("velocity", rotation.Normalized() * bulletSpeed);
			GameData.playerMoney -= 5;
			shootCooldownTimer += shootCooldown;
			SoundManager.shoot.Play();
		}
	}
}
