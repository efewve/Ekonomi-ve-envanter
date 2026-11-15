using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [Header("Eşya Bilgileri")]
    public string itemName = "Altın Kesesi";
    public int goldAmount = 25; // Bu eşya alındığında vereceği altın miktarı

    [Header("Toplama Efekti (Opsiyonel)")]
    public GameObject pickupEffectPrefab; // Alındığında çıkacak patlama/parlama efekti

    // 2D oyunlar için Trigger (Tetikleyici) algılama fonksiyonu
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Çarpan nesnenin etiketi (Tag) "Player" ise
        if (collision.CompareTag("Player"))
        {
            CollectItem();
        }
    }

    void CollectItem()
    {
        // Daha önce yazdığımız PlayerCurrencyAndXP sistemine altın ekle
        if (PlayerCurrencyAndXP.Instance != null)
        {
            PlayerCurrencyAndXP.Instance.AddGold(goldAmount);
            Debug.Log(itemName + " toplandı! +" + goldAmount + " altın kazandın.");
        }

        // Eğer toplama efekti varsa oynat
        if (pickupEffectPrefab != null)
        {
            Instantiate(pickupEffectPrefab, transform.position, Quaternion.identity);
        }

        // Eşyayı sahneden yok et (toplandı)
        Destroy(gameObject);
    }
}