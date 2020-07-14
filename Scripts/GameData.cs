using Godot;
using System;

public class GameData : Node2D
{
	public static Node2D currentContainer;
	public static RigidBody2D boss;

	public static int bossMaxHealth = 0;
	public static float bossHealth = 0;
	public static float playerHealth = 5;
	public static int playerMoney = 60;
	public static bool dead = false;
	public static float invincibilityTimer = 0;

	public static void HurtPlayer(int damage)
	{
		if (invincibilityTimer <= 0f)
		{
			playerHealth -= damage;
			invincibilityTimer += 120;
			SoundManager.hurt.Play();
		}
	}

	public static void ResetVariables()
	{
		bossMaxHealth = 0;
		bossHealth = 0;
		playerHealth = 5;
		playerMoney = 60;
		invincibilityTimer = 0;
		dead = false;
	}

	public override void _Process(float delta)
	{
		if (invincibilityTimer > 0f)
		{
			invincibilityTimer -= 1f + delta;		//should increment at a constant rate
		}
		if (playerHealth > 5)
		{
			playerHealth = 5;
		}
		if (playerHealth <= 0)
		{
			dead = true;
		}
	}
}
