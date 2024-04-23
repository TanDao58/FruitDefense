using UnityEngine;
using UnityEngine.Purchasing;
using System;

public class MyIAPManager : IStoreListener
{
    public IStoreController controller;
    public IExtensionProvider extensions;
    public static Action doneAction;

    public void Init() {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct("no_ads", ProductType.NonConsumable, new IDs
        {
            {"no_ads", GooglePlay.Name},
            {"no_ads", MacAppStore.Name}
        });
        UnityPurchasing.Initialize(this, builder);
    }

    public void Init(string sku) {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct(sku, ProductType.Consumable);

        UnityPurchasing.Initialize(this, builder);
    }

    public void InitSubscription(string sku)
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct(sku, ProductType.Subscription);

        UnityPurchasing.Initialize(this, builder);
    }

    /// <summary>
    /// Called when Unity IAP is ready to make purchases.
    /// </summary>
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions) {
        this.controller = controller;
        this.extensions = extensions;
        Debug.Log("Init IAP finished, congratulation!!");
    }

    /// <summary>
    /// Called when Unity IAP encounters an unrecoverable initialization error.
    ///
    /// Note that this will not be called if Internet is unavailable; Unity IAP
    /// will attempt initialization until it becomes available.
    /// </summary>
    public void OnInitializeFailed(InitializationFailureReason error) {
        Debug.LogError("Init Failed: " + error.ToString());
    }

    /// <summary>
    /// Called when a purchase completes.
    ///
    /// May be called at any time after OnInitialized().
    /// </summary>
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e) {
        Debug.Log("Buy product complete, congratulation!!");
        Debug.Log(e);
        //popupConfirm.ShowOK("Buy completed", "Buy product complete, congratulation!!");
        doneAction?.Invoke();
        return PurchaseProcessingResult.Complete;
    }

    /// <summary>
    /// Called when a purchase fails.
    /// </summary>
    public void OnPurchaseFailed(Product i, PurchaseFailureReason p) {
        Debug.LogError("Purchase failed at product " + i + " for reason: " + p);
        Debug.LogError("Buy failed Purchase failed at product " + i + " for reason: " + p);
        Debug.LogError("Buy failed Purchase failed at product ");
    }

    public void ShowAllProduct() {
        foreach (var product in controller.products.all) {
            Debug.Log(product.metadata.localizedTitle);
            Debug.Log(product.metadata.localizedDescription);
            Debug.Log(product.metadata.localizedPriceString);
        }
    }

    // Example method called when the user presses a 'buy' button
    // to start the purchase process.
    public void OnPurchaseClicked(string productId, Action doneAction) {
        //popupConfirm.ShowOK("Processing", productId);
        MyIAPManager.doneAction = doneAction;
        controller.InitiatePurchase(productId);
    }
}