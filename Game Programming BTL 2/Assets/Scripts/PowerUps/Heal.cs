using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerUps/Heal")]
public class Heal : PowerUps
{

    [SerializeField] int healAmount = 10;
    [SerializeField] float powerUpTextDuration = 1f;
    public override void Activate(Player player)
    {
        Debug.Log("Healed");
        player.HealPlayer(healAmount);
        AudioManager.Instance.PlayPowerUpSound();
        player.StartCoroutine(Coroutine(player));
    }

    private IEnumerator Coroutine(Player player)
    {
        HUD.Instance.UpdatePlayerPowerUp(player.GetPlayerNumber(), "Heal");
        yield return new WaitForSeconds(powerUpTextDuration);
        HUD.Instance.UpdatePlayerPowerUp(player.GetPlayerNumber(), "");
    }
}