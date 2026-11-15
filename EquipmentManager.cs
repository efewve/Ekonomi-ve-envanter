using UnityEngine;
using System;

public class EquipmentManager : MonoBehaviour
{
    [Header("Karakter Stat Referansı")]
    public CharacterStats playerStats; // Yansıtılacak karakter statları

    [Header("Aktif Giyilen Ekipmanlar")]
    public EquipmentItem equippedWeapon;
    public EquipmentItem equippedArmor;
    public EquipmentItem equippedHelmet;

    // UI'ın ekipman değişimini algılaması için olay (Event)
    public static event Action OnEquipmentChanged;

    // --- MANUEL EKİPMAN TAKMA FONKSİYONU ---
    public void EquipItem(EquipmentItem newItem)
    {
        if (newItem == null) return;

        // Eşyanın türüne göre ilgili slotu kontrol et
        switch (newItem.equipmentType)
        {
            case EquipmentType.Weapon:
                // Eğer o slotta zaten bir eşya varsa, önce eski bonusu geri al
                if (equippedWeapon != null)
                {
                    RemoveStatBonuses(equippedWeapon);
                }
                equippedWeapon = newItem;
                ApplyStatBonuses(equippedWeapon);
                Debug.Log(newItem.itemName + " silaha takıldı!");
                break;

            case EquipmentType.Armor:
                if (equippedArmor != null)
                {
                    RemoveStatBonuses(equippedArmor);
                }
                equippedArmor = newItem;
                ApplyStatBonuses(equippedArmor);
                Debug.Log(newItem.itemName + " zırha giyildi!");
                break;

            case EquipmentType.Helmet:
                if (equippedHelmet != null)
                {
                    RemoveStatBonuses(equippedHelmet);
                }
                equippedHelmet = newItem;
                ApplyStatBonuses(equippedHelmet);
                Debug.Log(newItem.itemName + " kaska takıldı!");
                break;
        }

        // Arayüzü güncellemek için olayı tetikle
        OnEquipmentChanged?.Invoke();
    }

    // --- MANUEL EKİPMAN ÇIKARMA FONKSİYONU ---
    public void UnequipItem(EquipmentType type)
    {
        switch (type)
        {
            case EquipmentType.Weapon:
                if (equippedWeapon != null)
                {
                    RemoveStatBonuses(equippedWeapon);
                    Debug.Log(equippedWeapon.itemName + " çıkarıldı.");
                    equippedWeapon = null;
                }
                break;

            case EquipmentType.Armor:
                if (equippedArmor != null)
                {
                    RemoveStatBonuses(equippedArmor);
                    Debug.Log(equippedArmor.itemName + " çıkarıldı.");
                    equippedArmor = null;
                }
                break;

            case EquipmentType.Helmet:
                if (equippedHelmet != null)
                {
                    RemoveStatBonuses(equippedHelmet);
                    Debug.Log(equippedHelmet.itemName + " çıkarıldı.");
                    equippedHelmet = null;
                }
                break;
        }

        OnEquipmentChanged?.Invoke();
    }

    // Statları karaktere ekleme
    void ApplyStatBonuses(EquipmentItem item)
    {
        if (playerStats == null) return;

        playerStats.attackDamage += item.attackBonus;
        playerStats.maxHealth += item.healthBonus;
        playerStats.currentHealth += item.healthBonus; // Can artınca mevcut canı da orantılı artır
    }

    // Statları karakterden düşme (Çıkarıldığında)
    void RemoveStatBonuses(EquipmentItem item)
    {
        if (playerStats == null) return;

        playerStats.attackDamage -= item.attackBonus;
        playerStats.maxHealth -= item.healthBonus;

        // Can düşerse maksimum canı aşmasın
        if (playerStats.currentHealth > playerStats.maxHealth)
        {
            playerStats.currentHealth = playerStats.maxHealth;
        }
    }
}