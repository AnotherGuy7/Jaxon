using Godot;
using System;

public class SoundManager : Node2D
{
	public static AudioStreamPlayer levelMusic;
	public static AudioStreamPlayer bossMusic;
	public static AudioStreamPlayer hurt;
	public static AudioStreamPlayer jump;
	public static AudioStreamPlayer pickupCoin;
	public static AudioStreamPlayer pickupMedkit;
	public static AudioStreamPlayer shoot;
	public static AudioStreamPlayer uiSelect;
	public static AudioStreamPlayer flamethrower;

	public override void _Ready()
	{
		levelMusic = GetNode<AudioStreamPlayer>("LevelMusic");
		bossMusic = GetNode<AudioStreamPlayer>("BossMusic");
		hurt = GetNode<AudioStreamPlayer>("Hurt");
		jump = GetNode<AudioStreamPlayer>("Jump");
		pickupCoin = GetNode<AudioStreamPlayer>("PickupCoin");
		pickupMedkit = GetNode<AudioStreamPlayer>("PickupMedkit");
		shoot = GetNode<AudioStreamPlayer>("Shoot");
		uiSelect = GetNode<AudioStreamPlayer>("UISelect");
		flamethrower = GetNode<AudioStreamPlayer>("Flamethrower");
	}
}
