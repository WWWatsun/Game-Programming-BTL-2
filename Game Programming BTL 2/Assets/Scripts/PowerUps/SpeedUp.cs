using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "PowerUps/SpeedUp")]
public class SpeedUp : PowerUps
{
    public override void Activate(Player player)
    {
        float multiplier = 1.5f;
        float duration = 5f;
        player.SetSpeedUp(multiplier, duration);
    }
}
