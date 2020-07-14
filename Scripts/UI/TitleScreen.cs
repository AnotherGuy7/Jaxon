using Godot;
using System;

public class TitleScreen : Node2D
{
	[Export]
	public PackedScene map1;

	private Panel controlsPanel;

	public override void _Ready()
	{
		controlsPanel = GetNode<Panel>("ControlsPanel");
	}

	public override void _Process(float delta)
	{
		if (!SoundManager.levelMusic.Playing)
		{
			SoundManager.levelMusic.Play();
		}
		if (SoundManager.bossMusic.Playing)
		{
			SoundManager.bossMusic.Stop();
		}
	}

	private void OnPlayButtonPressed()
	{
		SoundManager.uiSelect.Play();
		GetTree().ChangeSceneTo(map1);
		GameData.ResetVariables();
}

	private void OnControlsButtonPressed()
	{
		controlsPanel.Visible = true;
		SoundManager.uiSelect.Play();
	}

	private void OnQuitButtonPressed()
	{
		GetTree().Quit();
		SoundManager.uiSelect.Play();
	}

	private void OnCancelButtonPressed()
	{
		controlsPanel.Visible = false;
		SoundManager.uiSelect.Play();
	}
}
