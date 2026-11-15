using UnityEngine;
using System;

public class PlayerCurrencyAndXP : MonoBehaviour
{
    // Oyunun her yerinden kolayca erişilebilmesi için Singleton
    public static PlayerCurrencyAndXP Instance { get; private set; }

    [Header("Oyuncu İstatistikleri")]
    public long currentGold = 0;
    public long currentXP = 0;
    public int currentLevel = 1;
    public long xpToNextLevel = 100; // Sonraki seviye için gereken XP

    // Arayüzün (UI) güncellenmesini sağlayacak olaylar (Events)
    public static event Action<long> OnGoldChanged;
    public static event Action<long, long> OnXPChanged; // (Mevcut XP, Gereken XP)
    public static event Action<int> OnLevelUp;

    // Kayıt anahtarları (PlayerPrefs için)
    private const string GoldKey = "PlayerGold";
    private const string XPKey = "PlayerXP";
    private const string LevelKey = "PlayerLevel";
    private const string MaxXPKey = "PlayerMaxXP";

    void Awake()
    {
        // Singleton yapısı
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadData(); // Oyuna başlarken kayıtlı verileri yükle
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Başlangıçta UI'ın güncel verileri alması için tetikle
        NotifyUI();
    }

    // --- ALTIN İŞLEMLERİ ---
    public void AddGold(long amount)
    {
        currentGold += amount;
        SaveData();
        OnGoldChanged?.Invoke(currentGold);
        Debug.Log("Altın eklendi: +" + amount + " | Toplam: " + currentGold);
    }

    public bool SpendGold(long amount)
    {
        if (currentGold >= amount)
        {
            currentGold -= amount;
            SaveData();
            OnGoldChanged?.Invoke(currentGold);
            return true; // Harcama başarılı
        }
        Debug.Log("Yetersiz altın!");
        return false; // Yetersiz bakiye
    }

    // --- XP VE SEVİYE İŞLEMLERİ ---
    public void AddXP(long amount)
    {
        currentXP += amount;

        // Oyuncu yeterli XP'ye ulaştıysa (birden fazla level atlayabilir)
        while (currentXP >= xpToNextLevel)
        {
            currentXP -= xpToNextLevel;
            LevelUp();
        }

        SaveData();
        OnXPChanged?.Invoke(currentXP, xpToNextLevel);
    }

    void LevelUp()
    {
        currentLevel++;
        // Her seviye atlandığında sonraki seviye gereksinimi %20 artar (Dengeli bir RPG eğrisi için)
        xpToNextLevel = (long)(xpToNextLevel * 1.2f); 

        Debug.Log("SEVİYE ATLANDIN! Yeni Seviye: " + currentLevel);
        OnLevelUp?.Invoke(currentLevel);
    }

    // --- KAYIT SİSTEMİ (SAVE / LOAD) ---
    void SaveData()
    {
        PlayerPrefs.SetString(GoldKey, currentGold.ToString());
        PlayerPrefs.SetString(XPKey, currentXP.ToString());
        PlayerPrefs.SetInt(LevelKey, currentLevel);
        PlayerPrefs.SetString(MaxXPKey, xpToNextLevel.ToString());
        PlayerPrefs.Save();
    }

    void LoadData()
    {
        currentGold = long.Parse(PlayerPrefs.GetString(GoldKey, "0"));
        currentXP = long.Parse(PlayerPrefs.GetString(XPKey, "0"));
        currentLevel = PlayerPrefs.GetInt(LevelKey, 1);
        xpToNextLevel = long.Parse(PlayerPrefs.GetString(MaxXPKey, "100"));
    }

    void NotifyUI()
    {
        OnGoldChanged?.Invoke(currentGold);
        OnXPChanged?.Invoke(currentXP, xpToNextLevel);
        OnLevelUp?.Invoke(currentLevel);
    }
}