using UnityEngine;
using UnityEngine.Purchasing;
using System;
using System.Collections;
using System.Collections.Generic;

namespace DarkcupGames
{
    public class MyIAPManager : IStoreListener
    {
        public static string currentBuySKU;
        public Action onProcessSuccess;
        public Dictionary<string, string> prices;
        private IStoreController controller;
        private IExtensionProvider extensions;

        public void Init()
        {
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            builder.AddProduct("100_gold_coins", ProductType.Consumable, new IDs
            {
                {"100_gold_coins_google", GooglePlay.Name},
                {"100_gold_coins_mac", MacAppStore.Name}
            });
            UnityPurchasing.Initialize(this, builder);
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            this.controller = controller;
            this.extensions = extensions;
            Debug.Log("Init IAP finished, congratulation!!");
            var products = controller.products.all;
            prices = new Dictionary<string, string>();
            for (int i = 0; i < products.Length; i++)
            {
                prices.Add(products[i].definition.id, products[i].metadata.localizedPriceString);
            }
            var texts = GameObject.FindObjectsOfType<TextPricingIAP>();
            for (int i = 0; i < texts.Length; i++)
            {
                texts[i].UpdateDisplay();
            }
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.Log(error);
            //if (popupConfirm)
            //    popupConfirm.ShowOK("Init Failed", error.ToString());
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
        {
            Debug.Log("Buy product complete, congratulation!!");
            Debug.Log(e);

            if (onProcessSuccess != null)
            {
                onProcessSuccess();
            }

            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product i, PurchaseFailureReason p)
        {
            //Debug.LogError("Purchase failed at product " + i + " for reason: " + p);
        }

        public void ShowAllProduct()
        {
            foreach (var product in controller.products.all)
            {
                Debug.Log(product.metadata.localizedTitle);
                Debug.Log(product.metadata.localizedDescription);
                Debug.Log(product.metadata.localizedPriceString);
            }
        }

        public void OnPurchaseClicked(string productId, Action onSuccess)
        {
            this.onProcessSuccess = onSuccess;
            controller.InitiatePurchase(productId);
            currentBuySKU = productId;
        }
    }
}