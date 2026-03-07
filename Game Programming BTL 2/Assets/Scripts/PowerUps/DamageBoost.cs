using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerUps/DamageBoost")]
public class DamageBoost : PowerUps
{
    [SerializeField] private float multiplier = 2f;
    [SerializeField] private float duration = 5f;

    public override void Activate(Player player)
    {
        player.StartCoroutine(Coroutine(player));
    }

    private IEnumerator Coroutine(Player player)
    {
        
        AudioManager.Instance.PlayPowerUpSound();
        HUD.Instance.UpdatePlayerPowerUp(player.GetPlayerNumber(), "Damage boost");
        player.SetMultiplier(multiplier, duration);
        yield return new WaitForSeconds(duration);
        AudioManager.Instance.PlayPowerDownSound();
        HUD.Instance.UpdatePlayerPowerUp(player.GetPlayerNumber(), "");
    }
}
