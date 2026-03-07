using UnityEngine;

[CreateAssetMenu(fileName = "PowerUps/Heal")]
public class Heal : PowerUps
{

    [SerializeField] int healAmount = 10;
    public override void Activate(Player player)
    {
        Debug.Log("Healed");
        player.HealPlayer(healAmount);
    }
}