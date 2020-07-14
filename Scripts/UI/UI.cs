using Godot;
using System;

public class UI : Control
{
	[Export]
	public Texture healthBar0;

	[Export]
	public Texture healthBar1;

	[Export]
	public Texture healthBar2;

	[Export]
	public Texture healthBar3;

	[Export]
	public Texture healthBar4;

	[Export]
	public Texture healthBar5;

	private TextureRect healthBar;
	private TextureRect bossHealthBar;
	private TextureRect bossHealthBarOutline;
	private AnimationPlayer deathAnim;
	private Label moneyLabel;

	public override void _Ready()
	{
		healthBar = GetNode<TextureRect>("Layer1/HealthBar");
		bossHealthBar = GetNode<TextureRect>("Layer1/BossHealthBar");
		bossHealthBarOutline = GetNode<TextureRect>("Layer1/BossHealthBarOutline");
		deathAnim = GetNode<AnimationPlayer>("Layer1/DeathAnimation");
		moneyLabel = GetNode<Label>("Layer1/MoneyLabel");
	}

	public override void _Process(float delta)
	{
		switch (GameData.playerHealth)
		{
			case 0:
				healthBar.Texture = healthBar0;
				break;
			case 1:
				healthBar.Texture = healthBar1;
				break;
			case 2:
				healthBar.Texture = healthBar2;
				break;
			case 3:
				healthBar.Texture = healthBar3;
				break;
			case 4:
				healthBar.Texture = healthBar4;
				break;
			case 5:
				healthBar.Texture = healthBar5;
				break;
		}

		bossHealthBar.Visible = bossHealthBarOutline.Visible = GameData.bossHealth > 0;
		if (bossHealthBar.Visible)
		{
			bossHealthBar.RectSize = new Vector2((GameData.bossHealth / GameData.bossMaxHealth) * 86f, 12f);
		}
		if (GameData.dead && !deathAnim.IsPlaying())
		{
			deathAnim.Play("DeathAnim");
		}
		moneyLabel.Text = "MONEY: $" + GameData.playerMoney;
	}

	private void OnDeathAnimationFinished(String anim_name)
	{
		if (anim_name == "DeathAnim")
		{
			GetTree().ChangeSceneTo(PackedScenes.packedSceneClass.titleScreen);
		}
	}
}
