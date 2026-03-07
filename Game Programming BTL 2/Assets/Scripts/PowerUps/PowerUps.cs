using UnityEngine;

[CreateAssetMenu(fileName = "PowerUps", menuName = "Scriptable Objects/PowerUps")]
public class PowerUps : ScriptableObject
{

    [SerializeField] string powerName;
    [SerializeField] public Sprite icon;
    [SerializeField] public Color color;
    public virtual void Activate(Player player)
    {

    }
    
}
