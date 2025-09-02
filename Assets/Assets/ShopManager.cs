using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum ShopPanelType
{
    Coins,
    Balls
}

public class ShopManager : MonoBehaviour
{
    [Header("Shop Change Panel Settings")]
    [SerializeField] GameObject _CoinsPanel;
    [SerializeField] GameObject _BallsPanel;
    [SerializeField] Button _CoinsButton;
    [SerializeField] Button _BallsButton;
    private ShopPanelType _currentPanelType = ShopPanelType.Balls;

    [Header("Constraints Settings"), HideInInspector]
    public static string HEALTH1XSKU = "HEALTH1X";
    public static string HEALTH2XSKU = "HEALTH2X";
    public static string HEALTH3XSKU = "HEALTH3X";
    public static string HEALTH4XSKU = "HEALTH4X";
    public static string HEALTH5XSKU = "HEALTH5X";
    public static string HEALTH6XSKU = "HEALTH6X";
    public static string BASKETBALLSKU = "BASKETBALL";
    public static string FOOTBALLSKU = "FOOTBALL";
    public static string TENNISBALLSKU = "TENNISBALL";
    public static string BASKETBALLSKU1 = "BASKETBALL1";
    public static string FOOTBALLSKU1 = "FOOTBALL1";
    public static string TENNISBALLSKU1 = "TENNISBALL1";

    [Header("Ball Panel")]
    [SerializeField] GameObject[] _BallItems;
    [SerializeField] Color _BallItemSelectedColor;
    [SerializeField] Color _BallItemUnselectedColor;

    [Header("Health Buttons")]
    [SerializeField] Button _Health1xButton;
    [SerializeField] Button _Health2xButton;
    [SerializeField] Button _Health3xButton;
    [SerializeField] Button _Health4xButton;
    [SerializeField] Button _Health5xButton;
    [SerializeField] Button _Health6xButton;

    [Header("Ball Buttons")]
    [SerializeField] Button _FootballButton;
    [SerializeField] Button _BasketballButton;
    [SerializeField] Button _TennisBallButton;
    [SerializeField] Button _FootballButton1;
    [SerializeField] Button _BasketballButton1;
    [SerializeField] Button _TennisBallButton1;

    [Header("Gameplay Hook (optional)")]
    [SerializeField] BallController _ballCtrl;
    [SerializeField] bool _isGameplay;

    // ---------------- NEW: auto-detect BallController if not assigned ----------------
    void Awake()
    {
        if (_ballCtrl == null) _ballCtrl = FindObjectOfType<BallController>();
        if (!_isGameplay) _isGameplay = (_ballCtrl != null);
    }
    // ----------------------------------------------------------------------------------

    void Start()
    {
        ChangePanel(_currentPanelType);

        _CoinsButton.onClick.AddListener(() => ChangePanel(ShopPanelType.Coins));
        _BallsButton.onClick.AddListener(() => ChangePanel(ShopPanelType.Balls));

        _Health1xButton.onClick.AddListener(() => OnClickPurchase(ItemShops.Health1x));
        _Health2xButton.onClick.AddListener(() => OnClickPurchase(ItemShops.Health2x));
        _Health3xButton.onClick.AddListener(() => OnClickPurchase(ItemShops.Health3x));
        _Health4xButton.onClick.AddListener(() => OnClickPurchase(ItemShops.Health4x));
        _Health5xButton.onClick.AddListener(() => OnClickPurchase(ItemShops.Health5x));
        _Health6xButton.onClick.AddListener(() => OnClickPurchase(ItemShops.Health6x));

        _FootballButton.onClick.AddListener(() => OnClickPurchase(ItemShops.Football));
        _BasketballButton.onClick.AddListener(() => OnClickPurchase(ItemShops.Basketball));
        _TennisBallButton.onClick.AddListener(() => OnClickPurchase(ItemShops.TennisBall));
        _FootballButton1.onClick.AddListener(() => OnClickPurchase(ItemShops.Football1));
        _BasketballButton1.onClick.AddListener(() => OnClickPurchase(ItemShops.Basketball1));
        _TennisBallButton1.onClick.AddListener(() => OnClickPurchase(ItemShops.TennisBall1));

        RefreshAllBallTiles();
    }

    // ---------------- NEW: simulate purchase in Editor / non-Android ----------------
    private async Task<bool> DoPurchase(string sku)
    {
#if UNITY_EDITOR || !UNITY_ANDROID
        await Task.Yield(); // keep async signature clean
        Debug.Log($"[ShopManager] Simulating purchase for SKU: {sku}");
        return true;
#else
        return await PurchaseManager.Instance.OnPurchase(sku);
#endif
    }
    // ---------------------------------------------------------------------------------

    int IndexOf(ItemShops item) => item switch
    {
        ItemShops.Football    => 0,
        ItemShops.Basketball  => 1,
        ItemShops.TennisBall  => 2,
        ItemShops.Football1   => 3,
        ItemShops.Basketball1 => 4,
        ItemShops.TennisBall1 => 5,
        _ => -1
    };

    string GVKeyOf(ItemShops item) => item switch
    {
        ItemShops.Football    => GlobalValue.Football,
        ItemShops.Basketball  => GlobalValue.Basketball,
        ItemShops.TennisBall  => GlobalValue.TennisBall,
        ItemShops.Football1   => GlobalValue.FootBall1,
        ItemShops.Basketball1 => GlobalValue.BasketBall1,
        ItemShops.TennisBall1 => GlobalValue.TennisBall1,
        _ => string.Empty
    };

    void SetPriceChildActive(ItemShops item, bool active)
    {
        int i = IndexOf(item);
        if (i >= 0 && i < _BallItems.Length)
        {
            Transform t = _BallItems[i].transform;
            // توجه: طبق ساختار فعلی خودت child=2 قیمت است
            if (t.childCount > 2)
                t.GetChild(2).gameObject.SetActive(active);
        }
    }

    void RefreshAllBallTiles()
    {
        ItemShops[] balls =
        {
            ItemShops.Football, ItemShops.Basketball, ItemShops.TennisBall,
            ItemShops.Football1, ItemShops.Basketball1, ItemShops.TennisBall1
        };

        foreach (var b in balls)
        {
            bool owned = GlobalValue.GetItems(GVKeyOf(b));
            SetPriceChildActive(b, !owned);
        }
    }

    void ApplySelectedBallToGameplay(ItemShops item)
    {
        if (!_isGameplay || _ballCtrl == null) return;

        int i = IndexOf(item);
        if (i < 0 || i >= _BallItems.Length) return;

        // آیکنِ خود آیتم داخل UI (child=0) را به‌عنوان sprite منبع می‌گیریم
        var img = _BallItems[i].transform.GetChild(0).GetComponent<Image>();
        if (img == null || img.sprite == null) return;
        print("Oomdam");
        _ballCtrl.ballRenderer.sprite = img.sprite;
        _ballCtrl.image.sprite = img.sprite;
        _ballCtrl.TrySetBallSprite(img.sprite);
    }

    public void ChoosenBallItem(ItemShops item)
    {
        // فقط انتخاب را در GlobalValue ذخیره می‌کنیم (بدون تغییر رنگ‌ها)
        switch (item)
        {
            case ItemShops.Football:    GlobalValue.WhichBallChoose(GlobalValue.Football);    break;
            case ItemShops.Basketball:  GlobalValue.WhichBallChoose(GlobalValue.Basketball);  break;
            case ItemShops.TennisBall:  GlobalValue.WhichBallChoose(GlobalValue.TennisBall);  break;
            case ItemShops.Football1:   GlobalValue.WhichBallChoose(GlobalValue.FootBall1);   break;
            case ItemShops.Basketball1: GlobalValue.WhichBallChoose(GlobalValue.BasketBall1); break;
            case ItemShops.TennisBall1: GlobalValue.WhichBallChoose(GlobalValue.TennisBall1); break;
        }

        // --- REMOVED: رنگ‌دهی به بک‌گراند‌ها (طبق درخواستت حذف شد) ---
        // for (int k = 0; k < _BallItems.Length; k++) { ... }
        // ---------------------------------------------------------------

        // اگر داخل بازی هستیم، همون لحظه اسکین توپ را اعمال کن
        ApplySelectedBallToGameplay(item);
    }

    private void ChangePanel(ShopPanelType currentPanelType)
    {
        if (currentPanelType == ShopPanelType.Balls && _currentPanelType == ShopPanelType.Coins)
        {
            _currentPanelType = ShopPanelType.Balls;
            _CoinsPanel.SetActive(false);
            _BallsPanel.SetActive(true);
        }
        else if (currentPanelType == ShopPanelType.Coins && _currentPanelType == ShopPanelType.Balls)
        {
            _currentPanelType = ShopPanelType.Coins;
            _CoinsPanel.SetActive(true);
            _BallsPanel.SetActive(false);
        }
    }

    public async void OnClickPurchase(ItemShops itemName)
    {
        bool purchaseResult = false;

        switch (itemName)
        {
            // ---------------- Health ----------------
            case ItemShops.Health1x:
                purchaseResult = await DoPurchase(HEALTH1XSKU);
                if (purchaseResult)
                {
                    GlobalValue.AddItems(GlobalValue.Health, 1);
                    if (_isGameplay && _ballCtrl != null)
                    {
                        _ballCtrl.AddHealthAndPersist(1);
                        _ballCtrl.TryContinueAfterShopOrRestart();
                    }
                }
                break;

            case ItemShops.Health2x:
                purchaseResult = await DoPurchase(HEALTH2XSKU);
                if (purchaseResult)
                {
                    GlobalValue.AddItems(GlobalValue.Health, 2);
                    if (_isGameplay && _ballCtrl != null)
                    {
                        _ballCtrl.AddHealthAndPersist(2);
                        _ballCtrl.TryContinueAfterShopOrRestart();
                    }
                }
                break;

            case ItemShops.Health3x:
                purchaseResult = await DoPurchase(HEALTH3XSKU);
                if (purchaseResult)
                {
                    GlobalValue.AddItems(GlobalValue.Health, 3);
                    if (_isGameplay && _ballCtrl != null)
                    {
                        _ballCtrl.AddHealthAndPersist(3);
                        _ballCtrl.TryContinueAfterShopOrRestart();
                    }
                }
                break;

            case ItemShops.Health4x:
                purchaseResult = await DoPurchase(HEALTH4XSKU);
                if (purchaseResult)
                {
                    GlobalValue.AddItems(GlobalValue.Health, 4);
                    if (_isGameplay && _ballCtrl != null)
                    {
                        _ballCtrl.AddHealthAndPersist(4);
                        _ballCtrl.TryContinueAfterShopOrRestart();
                    }
                }
                break;

            case ItemShops.Health5x:
                purchaseResult = await DoPurchase(HEALTH5XSKU);
                if (purchaseResult)
                {
                    GlobalValue.AddItems(GlobalValue.Health, 5);
                    if (_isGameplay && _ballCtrl != null)
                    {
                        _ballCtrl.AddHealthAndPersist(5);
                        _ballCtrl.TryContinueAfterShopOrRestart();
                    }
                }
                break;

            case ItemShops.Health6x:
                purchaseResult = await DoPurchase(HEALTH6XSKU);
                if (purchaseResult)
                {
                    GlobalValue.AddItems(GlobalValue.Health, 6);
                    if (_isGameplay && _ballCtrl != null)
                    {
                        _ballCtrl.AddHealthAndPersist(6);
                        _ballCtrl.TryContinueAfterShopOrRestart();
                    }
                }
                break;

            // ---------------- Balls ----------------
            case ItemShops.Basketball:
                if (GlobalValue.GetItems(GlobalValue.Basketball))
                {
                    SetPriceChildActive(ItemShops.Basketball, false);
                    ChoosenBallItem(ItemShops.Basketball);
                    break;
                }
                purchaseResult = await DoPurchase(BASKETBALLSKU);
                if (purchaseResult)
                {
                    GlobalValue.UnlockItem(GlobalValue.Basketball);
                    SetPriceChildActive(ItemShops.Basketball, false);
                    ChoosenBallItem(ItemShops.Basketball);
                }
                break;

            case ItemShops.Football:
                if (GlobalValue.GetItems(GlobalValue.Football))
                {
                    SetPriceChildActive(ItemShops.Football, false);
                    ChoosenBallItem(ItemShops.Football);
                    break;
                }
                purchaseResult = await DoPurchase(FOOTBALLSKU);
                if (purchaseResult)
                {
                    GlobalValue.UnlockItem(GlobalValue.Football);
                    SetPriceChildActive(ItemShops.Football, false);
                    ChoosenBallItem(ItemShops.Football);
                }
                break;

            case ItemShops.TennisBall:
                if (GlobalValue.GetItems(GlobalValue.TennisBall))
                {
                    SetPriceChildActive(ItemShops.TennisBall, false);
                    ChoosenBallItem(ItemShops.TennisBall);
                    break;
                }
                purchaseResult = await DoPurchase(TENNISBALLSKU);
                if (purchaseResult)
                {
                    GlobalValue.UnlockItem(GlobalValue.TennisBall);
                    SetPriceChildActive(ItemShops.TennisBall, false);
                    ChoosenBallItem(ItemShops.TennisBall);
                }
                break;

            case ItemShops.Basketball1:
                if (GlobalValue.GetItems(GlobalValue.BasketBall1))
                {
                    SetPriceChildActive(ItemShops.Basketball1, false);
                    ChoosenBallItem(ItemShops.Basketball1);
                    break;
                }
                purchaseResult = await DoPurchase(BASKETBALLSKU1);
                if (purchaseResult)
                {
                    GlobalValue.UnlockItem(GlobalValue.BasketBall1);
                    SetPriceChildActive(ItemShops.Basketball1, false);
                    ChoosenBallItem(ItemShops.Basketball1);
                }
                break;

            case ItemShops.Football1:
                if (GlobalValue.GetItems(GlobalValue.FootBall1))
                {
                    SetPriceChildActive(ItemShops.Football1, false);
                    ChoosenBallItem(ItemShops.Football1);
                    break;
                }
                purchaseResult = await DoPurchase(FOOTBALLSKU1);
                if (purchaseResult)
                {
                    GlobalValue.UnlockItem(GlobalValue.FootBall1);
                    SetPriceChildActive(ItemShops.Football1, false);
                    ChoosenBallItem(ItemShops.Football1);
                }
                break;

            case ItemShops.TennisBall1:
                if (GlobalValue.GetItems(GlobalValue.TennisBall1))
                {
                    SetPriceChildActive(ItemShops.TennisBall1, false);
                    ChoosenBallItem(ItemShops.TennisBall1);
                    break;
                }
                purchaseResult = await DoPurchase(TENNISBALLSKU1);
                if (purchaseResult)
                {
                    GlobalValue.UnlockItem(GlobalValue.TennisBall1);
                    SetPriceChildActive(ItemShops.TennisBall1, false);
                    ChoosenBallItem(ItemShops.TennisBall1);
                }
                break;
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public string GameSceneName = "GameScene";
    public void StartGame()
    {
        SceneManager.LoadScene(GameSceneName);
    }
}

[System.Serializable]
public enum ItemShops
{
    Health1x,
    Health2x,
    Health3x,
    Health4x,
    Health5x,
    Health6x,
    Football,
    Basketball,
    TennisBall,
    Football1,
    Basketball1,
    TennisBall1,
}
