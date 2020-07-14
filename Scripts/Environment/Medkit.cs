using Godot;

public class Medkit : Area2D
{
	private void OnBodyEntered(object body)
	{
		if (body == Player.player)
		{
			GameData.playerHealth = 5;
			SoundManager.pickupMedkit.Play();
			QueueFree();
		}
	}
}
