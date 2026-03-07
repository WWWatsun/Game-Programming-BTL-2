using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "PowerUps/SpeedUp")]
public class SpeedUp : PowerUps
{
    [SerializeField] private float multiplier = 1.5f;
    [SerializeField] private float duration = 5f;

    public override void Activate(Player player)
    {
        player.StartCoroutine(Coroutine(player));
    }

    private IEnumerator Coroutine(Player player)
    {
        AudioManager.Instance.PlayPowerUpSound();
        player.SetSpeedUp(multiplier, duration);
        HUD.Instance.UpdatePlayerPowerUp(player.GetPlayerNumber(), "Speed up");
        yield return new WaitForSeconds(duration);
        AudioManager.Instance.PlayPowerDownSound();
        HUD.Instance.UpdatePlayerPowerUp(player.GetPlayerNumber(), "");
    }
}
