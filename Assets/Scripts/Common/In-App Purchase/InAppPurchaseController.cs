using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace Scripts.Common.InAppPurchase
{
    public class InAppPurchaseController : Singleton<InAppPurchaseController>, IDetailedStoreListener
    {
        private Action _onPurchaseSucceed;
        private Action _onPurchaseFailed;

        private IStoreController _storeController;
        private IExtensionProvider _extensionProvider;

        private void Start()
        {
            HandleProduct();
        }

        private void HandleProduct()
        {

        }

        public void BuyProduct(string productID)
        {
            if (!IsInitialized())
            {
                _onPurchaseFailed?.Invoke();
                _onPurchaseFailed = null;

                Debug.LogError("Store is not initialized!");
                return;
            }

            Product product = _storeController.products.WithID(productID);

            if(product == null)
            {
                _onPurchaseFailed?.Invoke();
                _onPurchaseFailed = null;

                Debug.LogError("Product is null!");
                return;
            }

            if (!product.availableToPurchase)
            {
                _onPurchaseFailed?.Invoke();
                _onPurchaseFailed = null;

                Debug.LogError("Product is not available!");
                return;
            }

            _storeController.InitiatePurchase(product);
        }

        public void SetPurchaseAction(Action onSuccess = null, Action onFailure = null)
        {
            _onPurchaseSucceed = onSuccess;
            _onPurchaseFailed = onFailure;
        }

        public void RestorePurchase()
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)
            {
                IAppleExtensions apple = _extensionProvider.GetExtension<IAppleExtensions>();
                apple.RestoreTransactions((result, message) =>
                {
                    Debug.LogError($"RestorePurchases continuing: {result}. Detailed: {message}.");
                });
            }
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            _storeController = controller;
            _extensionProvider = extensions;            
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.LogError($"Initialize failed! Reason to fail: {error}");
            return;
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            Debug.LogError($"Initialize failed! Reason to fail: {error}. Detail: {message}");
            return;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
            _onPurchaseFailed?.Invoke();
            _onPurchaseFailed = null;

            Debug.LogError($"Purchase product {product.definition.id} failed because of: {failureDescription.reason}. Detail: {failureDescription.message}");
            return;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            _onPurchaseFailed?.Invoke();
            _onPurchaseFailed = null;

            Debug.LogError($"Purchase product {product.definition.id} failed because of: {failureReason}");
            return;
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            string log = $"Successfully purchased {purchaseEvent.purchasedProduct.definition.id}";
            Debug.LogError(log);

            _onPurchaseSucceed?.Invoke();
            _onPurchaseSucceed = null;
            _onPurchaseFailed = null;
            
            return PurchaseProcessingResult.Complete;
        }

        public bool IsInitialized()
        {
            return _storeController != null && _extensionProvider != null;
        }
    }
}
