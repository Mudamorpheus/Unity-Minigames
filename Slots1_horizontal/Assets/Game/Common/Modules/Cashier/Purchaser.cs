using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using JetBrains.Annotations;
#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID || UNITY_TVOS)
using Prime31;
#endif
using UnityEngine;

//===
using Game.Common.Scripts.DI.Cores.Huds;
using Zenject;
//===

public enum CashierItemType {
    Consumable,
    NonConsumable
}

public class Purchaser : MonoBehaviour {
    public GameObject Spinner;
    public static Purchaser instance;
    private int _initedPurchasesCount = 0;
    private int _retryInitProductCounter = 0;
    public string androidPublicKey;
    public List<Action<int>> successPurchaseCallbacks = new List<Action<int>>();

    private List<CashierItem> cashierItems = new List<CashierItem> {
        new CashierItem(0, "1kslotscoinspurchase", "lucky_0_99", 1000, type: CashierItemType.NonConsumable),
        new CashierItem(1, "2kslotscoinspurchase", "lucky_1_99", 2000, type: CashierItemType.NonConsumable),
        new CashierItem(2, "5kslotscoinspurchase", "lucky_2_99", 5000, type: CashierItemType.NonConsumable),
    };

    public class CashierItem {
        public int Index;
        private string _productionKey;
        private string _testKey;
        public int PurchaseValue;
        public CashierItemType Type;
        public Action SuccessPurchaseCallback;

        public string Key {
            get =>
#if (UNITY_ANDROID && !TEST || UNITY_IOS && !TEST) && !UNITY_EDITOR
             _productionKey;
#elif UNITY_IOS && TEST && !UNITY_EDITOR && !UNITY_ANDROID
             _testKey;
#elif UNITY_EDITOR || (UNITY_ANDROID && TEST)
                _testKey;
#endif
        }

        public CashierItem(int index, string keyProd, string keyTest, int purchaseValue,
            CashierItemType type = CashierItemType.Consumable) {
            this.Index = index;
            this._productionKey = keyProd;
            this._testKey = keyTest;
            this.PurchaseValue = purchaseValue;
            this.Type = type;
            this.SuccessPurchaseCallback = () => instance.PurchaseLogic(PurchaseValue);
        }
    }


    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    void Start() {
        Init();
    }


    private void AndroidInit() {
#if UNITY_ANDROID && !TEST
        StartCoroutine(nameof(AndroidInitIE));
#endif
    }

    public IEnumerator AndroidInitIE() {
        if (!IsNetworkAvailable()) yield break;
#if !UNITY_EDITOR && !UNITY_IOS
        IAP.init(androidPublicKey);
        GoogleIABManager.purchaseFailedEvent += AndroidPurchaseFailed;
        GoogleIABManager.purchaseSucceededEvent += AndroidPurchaseSucceededEvent;
        yield return new WaitForSeconds(.5f);
        InitCashierList();
#endif
    }

    private void iOSInit() {
        if (!IsNetworkAvailable()) return;
#if UNITY_IOS && !UNITY_EDITOR
            StoreKitManager.purchaseFailedEvent += IOSPurchaseFailed;
            StoreKitManager.purchaseCancelledEvent += IOSPurchaseCancel;
            StoreKitManager.purchaseSuccessfulEvent += IOSPurchaseSuccessfulEvent;
            StoreKitManager.purchaseErrorEvent += IOSPurchaseErrorEvent;
            StoreKitManager.restoreTransactionsFailedEvent += IOSRestoreFailed;
            StoreKitManager.restoreTransactionsFinishedEvent += IOSRestoreFinished;
        InitCashierList();
#endif
    }

    private void Init() {
        AndroidInit();
        iOSInit();
    }

    [Inject] private BaseHud sceneHud;
    private void PurchaseLogic(int value) {
        foreach (var callback in successPurchaseCallbacks) {
            callback?.Invoke(value);
        }
        //TODO implement purchase logic or use Action from other place
        Spinner.SetActive(false);
        sceneHud.BuyBalance(value);
    }

    private void InitCashierList() {
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
            Debug.Log($"Request product list => {string.Join(" | ", cashierItems.Select(x => x.Key).ToList())}");
            IAP.requestProductData(cashierItems.Select(x => x.Key).ToArray(), cashierItems.Select(x => x.Key).ToArray(),
                productList => {
                    _initedPurchasesCount = productList.Count;
                    if (productList.Count > 0) {
                        Debug.Log($"Product list received => {productList}, " +
                                  $"Products count => {productList.Count}, " +
                                  $"Items List => {string.Join(", ", productList)}");
#if UNITY_IOS && !UNITY_EDITOR
                            try {
                                foreach (var product in productList) {
                                    StoreKitBinding.fetchStorePromotionVisibilityForProduct(product.productId);
                                }

                                foreach (var product in productList) {
                                    StoreKitBinding.updateStorePromotionVisibilityForProduct(product.productId,
                                        SKProductStorePromotionVisibility.Show);
                                }
                            } catch (Exception e) {
                                // ignored
                            }
#endif
                    } else {
                        Debug.Log("Received list product is empty");
                        _retryInitProductCounter++;
                        // Re-trying load purchase data
                        if (_retryInitProductCounter > 10) return;
                        Debug.Log($"Retry attempt {_retryInitProductCounter} for load purchase data");
                        InitCashierList();
                    }
                });
#endif
    }

    public void PurchaseItem(int index) {
        Debug.Log($"PurchaseItem: index => {index}");
        if (!IsNetworkAvailable()) {
            return;
        }
#if !UNITY_EDITOR && (UNITY_IOS || !TEST && UNITY_ANDROID)
        if (_initedPurchasesCount == 0) {
            InitCashierList();
        }
#endif
        if (cashierItems.Count(x => x.Index == index) == 0) {
            Debug.Log("Wrong item index. Check Cashier Item List");
            return;
        }

        var item = cashierItems.Find(x => x.Index.Equals(index));
#if (UNITY_ANDROID && !TEST || UNITY_IOS) && !UNITY_EDITOR
            Purchase(item);
#elif UNITY_EDITOR || (UNITY_ANDROID && TEST)
        PurchaseLogic(item.PurchaseValue);
#endif
    }

    private void Purchase(CashierItem item) {
        Spinner.SetActive(true);
        Debug.Log(
            $"[Purchaser] Purchase item: Index:[{item.Index}] | Key:[{item.Key}] | Type:[{item.Type}] | PurchaseValue:[{item.PurchaseValue}]");
#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID || UNITY_TVOS)
        switch (item.Type) {
            case CashierItemType.Consumable:
                try {
                    IAP.purchaseConsumableProduct(item.Key, (didSucceed, error) => {
                        Debug.Log($"Purchasing product [{item}] result: [{didSucceed}]");
                        if (didSucceed) {
                            item.SuccessPurchaseCallback.Invoke();
                        } else {
                            Debug.Log(error);
                        }
                    });
                } catch (Exception e) {
                    Debug.Log($"===>>> Error purchaseConsumableProduct. Exception: {e.Message}");
                }
                break;
            case CashierItemType.NonConsumable:
                try {
                    IAP.purchaseNonconsumableProduct(item.Key, (didSucceed, error) => {
                        Debug.Log($"Purchasing product [{item}] result: [{didSucceed}]");
                        if (didSucceed) {
                            item.SuccessPurchaseCallback.Invoke();
                        } else {
                            Debug.Log(error);
                        }
                    });
                } catch (Exception e) {
                    Debug.Log($"===>>> Error purchaseNonConsumableProduct. Exception: {e.Message}");
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

#endif
    }

    public void RestorePurchases() {
        Spinner.SetActive(true);
#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID || UNITY_TVOS)
        IAP.restoreCompletedTransactions(Restore);
#endif
    }

    private void Restore(String key) {
        PurchaseLogic(cashierItems.First(x => x.Key == key).Index);
    }


    private static bool IsNetworkAvailable() {
#if UNITY_ANDROID && TEST
      return true;
#endif
        return Application.internetReachability != NetworkReachability.NotReachable;
    }

    private void AndroidPurchaseFailed(string error, int response) {
        Spinner.SetActive(false);
    }

#if UNITY_ANDROID && !UNITY_EDITOR
    private void AndroidPurchaseSucceededEvent(GooglePurchase purchase) {
        Spinner.SetActive(false);
    }
#endif


    private void IOSPurchaseFailed(string error) {
        Spinner.SetActive(false);
    }

    private void IOSPurchaseCancel(string error) {
        Spinner.SetActive(false);
    }


    private void IOSRestoreFinished() {
        Spinner.SetActive(false);
    }

    private void IOSRestoreFailed(string error) {
        Spinner.SetActive(false);
    }

#if UNITY_IOS && !UNITY_EDITOR
    private void IOSPurchaseSuccessfulEvent(StoreKitTransaction transaction) {
        Spinner.SetActive(false);
    }

    private void IOSPurchaseErrorEvent(P31Error error) {
        Spinner.SetActive(false);
    }
#endif


    private void OnDestroy() {
#if UNITY_IOS && !UNITY_EDITOR
            StoreKitManager.purchaseFailedEvent -= IOSPurchaseFailed;
            StoreKitManager.purchaseCancelledEvent -= IOSPurchaseCancel;
            StoreKitManager.purchaseSuccessfulEvent -= IOSPurchaseSuccessfulEvent;
            StoreKitManager.purchaseErrorEvent -= IOSPurchaseErrorEvent;
            StoreKitManager.restoreTransactionsFailedEvent -= IOSRestoreFailed;
            StoreKitManager.restoreTransactionsFinishedEvent -= IOSRestoreFinished;
#elif UNITY_ANDROID && !UNITY_EDITOR
            GoogleIABManager.purchaseFailedEvent -= AndroidPurchaseFailed;
            GoogleIABManager.purchaseSucceededEvent -= AndroidPurchaseSucceededEvent;
#endif
    }
}