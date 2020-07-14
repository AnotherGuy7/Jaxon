using Godot;
using System;

public class BossMap : Node2D
{
	private ParallaxLayer nightSky;
	private ParallaxLayer outerBuildings;
	private ParallaxLayer innerBuildings;
	private AnimationPlayer animationP;
	private Sprite darkening;

	private readonly Color cyan = new Color(0.42f, 1f, 0.99f, 1f);
	private readonly Color green = new Color(0.49f, 1, 0.42f, 1f);
	private readonly Color pink = new Color(0.97f, 0.42f, 1f, 1f);

	private Random rand = new Random();

	public override void _Ready()
	{
		GameData.bossHealth = 40;
		GameData.bossMaxHealth = 40;
		GameData.currentContainer = GetNode<Node2D>("Environment");
		nightSky = GetNode<ParallaxLayer>("Parallax/NightSky");
		outerBuildings = GetNode<ParallaxLayer>("Parallax/OuterBuildings");
		innerBuildings = GetNode<ParallaxLayer>("Parallax/InnerBuildings");
		animationP = GetNode<AnimationPlayer>("AnimationP");
		darkening = GetNode<Sprite>("AnimationP/Darkening");
		switch (rand.Next(1, 4))
		{
			case 1:
				nightSky.Modulate = cyan;
				break;
			case 2:
				nightSky.Modulate = green;
				break;
			case 3:
				nightSky.Modulate = pink;
				break;
		}
		switch (rand.Next(1, 4))
		{
			case 1:
				outerBuildings.Modulate = cyan;
				break;
			case 2:
				outerBuildings.Modulate = green;
				break;
			case 3:
				outerBuildings.Modulate = pink;
				break;
		}
		switch (rand.Next(1, 4))
		{
			case 1:
				innerBuildings.Modulate = cyan;
				break;
			case 2:
				innerBuildings.Modulate = green;
				break;
			case 3:
				innerBuildings.Modulate = pink;
				break;
		}
	}

	public override void _Process(float delta)
	{
		if (SoundManager.levelMusic.Playing)
		{
			SoundManager.levelMusic.Stop();
		}
		if (!SoundManager.bossMusic.Playing)
		{
			SoundManager.bossMusic.Play();
		}
		if (GameData.bossHealth <= 0)
		{
			animationP.Play("EndScene");
		}
		darkening.GlobalPosition = Player.player.GlobalPosition;
	}

	private void OnColorChangeTimerOut()
	{
		switch (rand.Next(1, 4))
		{
			case 1:
				nightSky.Modulate = cyan;
				break;
			case 2:
				nightSky.Modulate = green;
				break;
			case 3:
				nightSky.Modulate = pink;
				break;
		}
		switch (rand.Next(1, 4))
		{
			case 1:
				outerBuildings.Modulate = cyan;
				break;
			case 2:
				outerBuildings.Modulate = green;
				break;
			case 3:
				outerBuildings.Modulate = pink;
				break;
		}
		switch (rand.Next(1, 4))
		{
			case 1:
				innerBuildings.Modulate = cyan;
				break;
			case 2:
				innerBuildings.Modulate = green;
				break;
			case 3:
				innerBuildings.Modulate = pink;
				break;
		}
	}

	private void OnAnimationFinished(String anim_name)
	{
		if (anim_name == "EndScene")
		{
			GetTree().ChangeSceneTo(PackedScenes.packedSceneClass.titleScreen);
		}
	}
}
