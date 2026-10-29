using System.Text.Json.Serialization;

/// <summary>
/// 訂單裡的商品
/// </summary>
/// <remarks>
/// 初始化訂單裡的商品
/// </remarks>
/// <param name="productId">商品編號</param>
/// <param name="productName">商品名稱</param>
/// <param name="quantity">購買數量</param>
/// <param name="unitPrice">單價</param>
public class OrderProduct(int productId, string productName, int quantity, decimal unitPrice)
{
  /// <summary>
  /// 商品編號
  /// </summary>
  [JsonPropertyName("商品編號")]
  public int ProductId { get; set; } = productId;
  /// <summary>
  /// 商品名稱
  /// </summary>
  [JsonPropertyName("商品名稱")]
  public string ProductName { get; set; } = productName;
  /// <summary>
  /// 購買數量
  /// </summary>
  [JsonPropertyName("購買數量")]
  public int Quantity { get; set; } = quantity;
  /// <summary>
  /// 單價
  /// </summary>
  [JsonPropertyName("單價")]
  public decimal UnitPrice { get; set; } = unitPrice;
}
