using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Helmet
}

[CreateAssetMenu(fileName = "NewEquipment", menuName = "RPG/Equipment Item")]
public class EquipmentItem : ScriptableObject
{
    public string itemName = "Demir Kılıç";
    public EquipmentType equipmentType;
    public Sprite itemIcon;

    [Header("Stat Bonusları")]
    public float attackBonus = 10f; // Silahın vereceği ekstra saldırı
    public float healthBonus = 0f;  // Zırhın vereceği ekstra can
}