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

    // Start is called before the first frame update
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
    }



    public void ChoosenBallItem(ItemShops item)
    {
        switch (item)
        {
            case ItemShops.Football:
                GlobalValue.WhichBallChoose(GlobalValue.Football);
                _BallItems[0].GetComponent<Image>().color = _BallItemSelectedColor;
                _BallItems[1].GetComponent<Image>().color = _BallItemUnselectedColor;
                _BallItems[2].GetComponent<Image>().color = _BallItemUnselectedColor;
                _BallItems[3].GetComponent<Image>().color = _BallItemUnselectedColor;
                _BallItems[4].GetComponent<Image>().color = _BallItemUnselectedColor;
                _BallItems[5].GetComponent<Image>().color = _BallItemUnselectedColor;

                break;
            case ItemShops.Basketball:
                GlobalValue.WhichBallChoose(GlobalValue.Basketball);
                _BallItems[1].GetComponent<Image>().color = _BallItemSelectedColor;
                _BallItems[0].GetComponent<Image>().color = _BallItemUnselectedColor;
                _BallItems[2].GetComponent<Image>().color = _BallItemUnselectedColor;
                _BallItems[3].GetComponent<Image>().color = _BallItemUnselectedColor;
                _BallItems[4].GetComponent<Image>().color = _BallItemUnselectedColor;
                _BallItems[5].GetComponent<Image>().color = _BallItemUnselectedColor;


                break;
            case ItemShops.TennisBall:
                GlobalValue.WhichBallChoose(GlobalValue.TennisBall);
                _BallItems[2].GetComponent<Image>().color = _BallItemSelectedColor;
                _BallItems[0].GetComponent<Image>().color = _BallItemUnselectedColor;
                _BallItems[1].GetComponent<Image>().color = _BallItemUnselectedColor;
                _BallItems[3].GetComponent<Image>().color = _BallItemUnselectedColor;
                _BallItems[4].GetComponent<Image>().color = _BallItemUnselectedColor;
                _BallItems[5].GetComponent<Image>().color = _BallItemUnselectedColor;
                break;
                case ItemShops.Football1:
                GlobalValue.WhichBallChoose(GlobalValue.FootBall1);
                _BallItems[3].GetComponent<Image>().color = _BallItemSelectedColor;
                _BallItems[1].GetComponent<Image>().color = _BallItemUnselectedColor;
                _BallItems[2].GetComponent<Image>().color = _BallItemUnselectedColor;
                _BallItems[0].GetComponent<Image>().color = _BallItemUnselectedColor;
                _BallItems[4].GetComponent<Image>().color = _BallItemUnselectedColor;
                _BallItems[5].GetComponent<Image>().color = _BallItemUnselectedColor;
                break;
case ItemShops.Basketball1:
    GlobalValue.WhichBallChoose(GlobalValue.BasketBall1);
    _BallItems[4].GetComponent<Image>().color = _BallItemSelectedColor;
    _BallItems[0].GetComponent<Image>().color = _BallItemUnselectedColor;
    _BallItems[1].GetComponent<Image>().color = _BallItemUnselectedColor;
    _BallItems[2].GetComponent<Image>().color = _BallItemUnselectedColor;
    _BallItems[3].GetComponent<Image>().color = _BallItemUnselectedColor;
    _BallItems[5].GetComponent<Image>().color = _BallItemUnselectedColor;
    break;

            case ItemShops.TennisBall1:
                GlobalValue.WhichBallChoose(GlobalValue.TennisBall1);
                _BallItems[5].GetComponent<Image>().color = _BallItemSelectedColor;
                _BallItems[0].GetComponent<Image>().color = _BallItemUnselectedColor;
                _BallItems[1].GetComponent<Image>().color = _BallItemUnselectedColor;
                _BallItems[3].GetComponent<Image>().color = _BallItemUnselectedColor;
                _BallItems[4].GetComponent<Image>().color = _BallItemUnselectedColor;
                _BallItems[2].GetComponent<Image>().color = _BallItemUnselectedColor;
                break;
        }
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
            case ItemShops.Health1x:
                purchaseResult = await PurchaseManager.Instance.OnPurchase(HEALTH1XSKU);
                if (purchaseResult)
                {
                    GlobalValue.AddItems(GlobalValue.Health, 1);
                }
                break;
            case ItemShops.Health2x:
                purchaseResult = await PurchaseManager.Instance.OnPurchase(HEALTH2XSKU);
                if (purchaseResult)
                {
                    GlobalValue.AddItems(GlobalValue.Health, 2);
                }
                break;
            case ItemShops.Health3x:
                purchaseResult = await PurchaseManager.Instance.OnPurchase(HEALTH3XSKU);
                if (purchaseResult)
                {
                    GlobalValue.AddItems(GlobalValue.Health, 3);
                }
                break;
            case ItemShops.Health4x:
                purchaseResult = await PurchaseManager.Instance.OnPurchase(HEALTH4XSKU);
                if (purchaseResult)
                {
                    GlobalValue.AddItems(GlobalValue.Health, 4);
                }
                break;
            case ItemShops.Health5x:
                purchaseResult = await PurchaseManager.Instance.OnPurchase(HEALTH5XSKU);
                if (purchaseResult)
                {
                    GlobalValue.AddItems(GlobalValue.Health, 5);
                }
                break;
            case ItemShops.Health6x:
                purchaseResult = await PurchaseManager.Instance.OnPurchase(HEALTH6XSKU);
                if (purchaseResult)
                {
                    GlobalValue.AddItems(GlobalValue.Health, 6);
                }
                break;
            case ItemShops.Basketball:
                if (GlobalValue.GetItems(GlobalValue.Basketball))
                {
                    Debug.Log("Basketball already purchased.");
            ChoosenBallItem(ItemShops.Basketball);

                    break;
                }
                purchaseResult = await PurchaseManager.Instance.OnPurchase(BASKETBALLSKU);
                if (purchaseResult)
                {
                    GlobalValue.UnlockItem(GlobalValue.Basketball);
            ChoosenBallItem(ItemShops.Basketball);

                }
                break;
            case ItemShops.Football:
                if (GlobalValue.GetItems(GlobalValue.Football))
                {
                    Debug.Log("Football already purchased.");
            ChoosenBallItem(ItemShops.Football);
                    break;
                }
                purchaseResult = await PurchaseManager.Instance.OnPurchase(FOOTBALLSKU);
                if (purchaseResult)
                {
                    GlobalValue.UnlockItem(GlobalValue.Football);
            ChoosenBallItem(ItemShops.Football);
                }
                break;
            case ItemShops.TennisBall:
                if (GlobalValue.GetItems(GlobalValue.TennisBall))
                {
                    Debug.Log("TennisBall already purchased.");
            ChoosenBallItem(ItemShops.TennisBall);
                    break;
                }
                purchaseResult = await PurchaseManager.Instance.OnPurchase(TENNISBALLSKU);
                if (purchaseResult)
                {
                    GlobalValue.UnlockItem(GlobalValue.TennisBall);
            ChoosenBallItem(ItemShops.TennisBall);
                }
                break;
                 case ItemShops.Basketball1:
                if (GlobalValue.GetItems(GlobalValue.BasketBall1))
                {
                    Debug.Log("Basketball already purchased.");
            ChoosenBallItem(ItemShops.Basketball1);
                    break;
                }
                purchaseResult = await PurchaseManager.Instance.OnPurchase(BASKETBALLSKU1);
                if (purchaseResult)
                {
                    GlobalValue.UnlockItem(GlobalValue.BasketBall1);
            ChoosenBallItem(ItemShops.Basketball1);
                }
                break;
            case ItemShops.Football1:
                if (GlobalValue.GetItems(GlobalValue.FootBall1))
                {
                    Debug.Log("Football already purchased.");
            ChoosenBallItem(ItemShops.Football1);
                    break;
                }
                purchaseResult = await PurchaseManager.Instance.OnPurchase(FOOTBALLSKU1);
                if (purchaseResult)
                {
                    GlobalValue.UnlockItem(GlobalValue.FootBall1);
            ChoosenBallItem(ItemShops.Football1);
                }
                break;
            case ItemShops.TennisBall1:
                if (GlobalValue.GetItems(GlobalValue.TennisBall1))
                {
                    Debug.Log("TennisBall already purchased.");
            ChoosenBallItem(ItemShops.TennisBall1);
                    break;
                }
                purchaseResult = await PurchaseManager.Instance.OnPurchase(TENNISBALLSKU1);
                if (purchaseResult)
                {
                    GlobalValue.UnlockItem(GlobalValue.TennisBall1);
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
