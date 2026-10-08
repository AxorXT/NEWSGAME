using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StoreProduct
{
    public string productName;
    public string origin;
    public int purchaseCost = 100;
    public float sellingPrice = 150;
    public int stock = 0;
    public int selectedQuantity = 0;

    [NonSerialized] public float revenue;
    [NonSerialized] public float costOfGoodsSold;
}
public class StoreGameManager : MonoBehaviour
{
    public GameOverUI gameOverUI;
    private bool juegoTerminado = false;

    [Header("Dinero")]
    public float startingMoney = 100000f;
    public float money;
    public float dailyRevenue;
    public float dailyExpenses;
    public float employeePayment = 500f;

    [Header("Precios")]
    public bool preciosConfirmados = false;

    [Header("Ventas")]
    public float tiempoEntreVentas = 3f;
    private float temporizadorVentas = 0f;

    [Header("Estado de la tienda")]
    [Range(0, 100)] public float popularity = 75f;
    public int currentDay = 1;

    [Header("Productos")]
    public List<StoreProduct> products = new List<StoreProduct>();

    public event Action<string> OnNotification;
    public event Action OnStoreUpdated;

    private void Awake()
    {
        money = startingMoney;

        // Productos de ejemplo para comenzar.
        if (products.Count == 0)
        {
            AddProduct("Audífonos", "México", 100, 180);
            AddProduct("Cargador", "México", 60, 120);
            AddProduct("Teclado", "México", 250, 400);
            AddProduct("Mouse", "México", 120, 220);
            AddProduct("Bocina", "México", 180, 300);
            AddProduct("Cable USB", "México", 30, 70);
        }
    }

    private void Update()
    {
        if (juegoTerminado)
            return;

        if (money <= 0)
        {
            money = 0;
            juegoTerminado = true;

            if (gameOverUI != null)
            {
                gameOverUI.MostrarGameOver();
            }

            return;
        }

        if (!preciosConfirmados)
            return;

        temporizadorVentas += Time.deltaTime;

        if (temporizadorVentas >= tiempoEntreVentas)
        {
            temporizadorVentas = 0f;

            IntentarVenta();
        }
    }

    private void AddProduct(string name, string origin, int cost, float price)
    {
        products.Add(new StoreProduct
        {
            productName = name,
            origin = origin,
            purchaseCost = cost,
            sellingPrice = price
        });
    }

    public void ChangeQuantity(int index, int amount)
    {
        if (!ValidIndex(index)) return;

        StoreProduct product = products[index];

        product.selectedQuantity = Mathf.Max(
            0, product.selectedQuantity + amount);

        OnStoreUpdated?.Invoke();
    }

    public void BuySelectedProducts()
    {
        float totalCost = 0f;

        foreach (StoreProduct product in products)
        {
            totalCost += product.purchaseCost *
                         product.selectedQuantity;
        }

        if (totalCost <= 0)
        {
            Notify("Selecciona productos antes de comprar.");
            return;
        }

        if (totalCost > money)
        {
            money = 0;
            juegoTerminado = true;

            if (gameOverUI != null)
                gameOverUI.MostrarGameOver();

            return;
        }

        money -= totalCost;

        foreach (StoreProduct product in products)
        {
            product.stock += product.selectedQuantity;
            product.selectedQuantity = 0;
        }

        Notify($"Compra realizada por ${totalCost:N0}.");
        OnStoreUpdated?.Invoke();
    }

    public void SetSellingPrice(int index, float price)
    {
        if (preciosConfirmados) return;

        if (!ValidIndex(index)) return;

        products[index].sellingPrice = Mathf.Clamp(price, 1, 100);
        OnStoreUpdated?.Invoke();
    }

    public void SellProduct(int index)
    {
        if (!ValidIndex(index)) return;

        StoreProduct product = products[index];

        if (product.stock <= 0)
        {
            Notify($"No quedan unidades de {product.productName}.");
            return;
        }

        if (product.sellingPrice <= 0)
        {
            Notify("Asigna un precio de venta mayor que cero.");
            return;
        }

        product.stock--;

        money += product.sellingPrice;
        dailyRevenue += product.sellingPrice;
        product.revenue += product.sellingPrice;
        product.costOfGoodsSold += product.purchaseCost;

        Notify(
            $"¡Se vendió {product.productName} por " +
            $"${product.sellingPrice:N0}!");

        OnStoreUpdated?.Invoke();
    }

    public void PayEmployees()
    {
        if (employeePayment <= 0)
        {
            Notify("No hay pagos de empleados pendientes.");
            return;
        }

        if (money < employeePayment)
        {
            Notify("No tienes suficiente dinero para pagar a tus empleados.");
            return;
        }

        money -= employeePayment;
        dailyExpenses += employeePayment;

        Notify($"Pagaste ${employeePayment:N0} a tus empleados.");
        OnStoreUpdated?.Invoke();
    }

    public void EndDay()
    {
        currentDay++;

        dailyRevenue = 0;
        dailyExpenses = 0;

        Notify($"Comienza el día {currentDay}.");
        OnStoreUpdated?.Invoke();
    }

    public float GetGrossProfit()
    {
        float totalCost = 0f;

        foreach (StoreProduct product in products)
        {
            totalCost += product.costOfGoodsSold;
        }

        return dailyRevenue - totalCost;
    }

    private bool ValidIndex(int index)
    {
        if (index >= 0 && index < products.Count)
            return true;

        Debug.LogWarning($"Índice de producto inválido: {index}");
        return false;
    }

    private void Notify(string message)
    {
        Debug.Log(message);
        OnNotification?.Invoke(message);
    }

    public void ConfirmarPrecios()
    {
        preciosConfirmados = true;

        Notify("Precios confirmados. Ya puedes comenzar a vender.");
        OnStoreUpdated?.Invoke();
    }

    public void DesbloquearPrecios()
    {
        preciosConfirmados = false;

        Notify("Precios desbloqueados. Puedes ajustarlos nuevamente.");
        OnStoreUpdated?.Invoke();
    }

    private void IntentarVenta()
    {
        if (products.Count == 0)
            return;

        int indice = UnityEngine.Random.Range(0, products.Count);

        StoreProduct producto = products[indice];

        if (producto.stock <= 0)
            return;

        // Precio máximo que un cliente estaría dispuesto a pagar
        float precioMaximo = producto.purchaseCost * 1.5f;

        if (producto.sellingPrice > precioMaximo)
        {
            Notify(
                $"{producto.productName} es muy caro, nadie lo comprará."
            );

            return;
        }

        SellProduct(indice);
    }
}
