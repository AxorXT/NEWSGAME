using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InvestmentUI : MonoBehaviour
{
    [Serializable]
    public class ProductSlot
    {
        public Image productImage;
        public Button minusButton;
        public Button plusButton;
        public TMP_Text quantityText;
    }

    [Header("Sistema del juego")]
    public StoreGameManager store;

    [Header("Los 6 espacios de productos")]
    public ProductSlot[] slots = new ProductSlot[6];

    [Header("Confirmar compra")]
    public Button confirmButton;
    public TMP_Text totalCompraText;

    private void Start()
    {
        if (store == null)
        {
            Debug.LogError("Falta asignar StoreGameManager.");
            return;
        }

        if (confirmButton != null)
            confirmButton.onClick.AddListener(ConfirmarCompra);

        for (int i = 0; i < slots.Length; i++)
        {
            int index = i;

            if (slots[i].plusButton != null)
            {
                slots[i].plusButton.onClick.AddListener(
                    () => CambiarCantidad(index, 1));
            }

            if (slots[i].minusButton != null)
            {
                slots[i].minusButton.onClick.AddListener(
                    () => CambiarCantidad(index, -1));
            }
        }

        ActualizarInterfaz();
    }

    private void CambiarCantidad(int index, int cantidad)
    {
        if (index < 0 || index >= store.products.Count)
            return;

        store.ChangeQuantity(index, cantidad);
        ActualizarInterfaz();
    }

    private void ActualizarInterfaz()
    {
        float total = 0;
        bool hayProductos = false;

        for (int i = 0; i < slots.Length; i++)
        {
            if (i >= store.products.Count)
                break;

            StoreProduct producto = store.products[i];
            ProductSlot slot = slots[i];

            if (slot.quantityText != null)
                slot.quantityText.text =
                    producto.selectedQuantity.ToString();

            total += producto.purchaseCost *
                     producto.selectedQuantity;

            if (producto.selectedQuantity > 0)
                hayProductos = true;
        }

        if (totalCompraText != null)
            totalCompraText.text = $"Total: ${total:N0}";

        if (confirmButton != null)
            confirmButton.gameObject.SetActive(hayProductos);
    }

    private void ConfirmarCompra()
    {
        store.BuySelectedProducts();
        ActualizarInterfaz();
    }
}