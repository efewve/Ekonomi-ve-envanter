using UnityEngine;

public class ManualUpgrade : MonoBehaviour
{
    [Header("Player Reference")]
    public PlayerStats player;

    [Header("Upgrade Costs")]
    public int attackCost = 50;
    public int defenseCost = 40;
    public int healthCost = 60;

    [Header("Upgrade Values")]
    public int attackIncrease = 2;
    public int defenseIncrease = 1;
    public int healthIncrease = 20;

    [Header("Cost Multiplier")]
    public float costMultiplier = 1.5f;

    public void UpgradeAttack()
    {
        if (TrySpendGold(attackCost))
        {
            player.attack += attackIncrease;
            attackCost = IncreaseCost(attackCost);

            Debug.Log("Saldırı geliştirildi! Yeni saldırı: " + player.attack);
        }
    }

    public void UpgradeDefense()
    {
        if (TrySpendGold(defenseCost))
        {
            player.defense += defenseIncrease;
            defenseCost = IncreaseCost(defenseCost);

            Debug.Log("Savunma geliştirildi! Yeni savunma: " + player.defense);
        }
    }

    public void UpgradeHealth()
    {
        if (TrySpendGold(healthCost))
        {
            player.maxHealth += healthIncrease;
            player.health += healthIncrease;

            healthCost = IncreaseCost(healthCost);

            Debug.Log("Maksimum can geliştirildi! Yeni maksimum can: "
                      + player.maxHealth);
        }
    }

    private bool TrySpendGold(int amount)
    {
        if (player == null)
        {
            Debug.LogError("Player bağlantısı yapılmadı!");
            return false;
        }

        if (player.gold < amount)
        {
            Debug.Log("Yeterli altın yok!");
            return false;
        }

        player.gold -= amount;
        return true;
    }

    private int IncreaseCost(int currentCost)
    {
        return Mathf.CeilToInt(currentCost * costMultiplier);
    }
}
