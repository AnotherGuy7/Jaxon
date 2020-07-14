using Godot;

public class Coin : Area2D
{
	private void OnBodyEntered(object body)
	{
		if (body == Player.player)
		{
			GameData.playerMoney += 5;
			SoundManager.pickupCoin.Play();
			QueueFree();
		}
	}
}
