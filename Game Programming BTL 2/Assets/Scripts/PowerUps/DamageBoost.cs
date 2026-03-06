using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerUps/DamageBoost")]
public class DamageBoost : PowerUps
{
    public override void Activate(Player player)
    {
        float multiplier = 2f;
        float duration = 5f;
        player.SetMultiplier(multiplier, duration);
    }
}
