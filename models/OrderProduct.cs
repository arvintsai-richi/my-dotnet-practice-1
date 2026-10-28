using System.Text.Json.Serialization;

/// <summary>
/// 訂單裡的商品
/// </summary>
public class OrderProduct
{
  /// <summary>
  /// 商品編號
  /// </summary>
  [JsonPropertyName("商品編號")]
  public int productId { get; }
  /// <summary>
  /// 商品名稱
  /// </summary>
  [JsonPropertyName("商品名稱")]
  public string productName { get; }
  /// <summary>
  /// 購買數量
  /// </summary>
  [JsonPropertyName("購買數量")]
  public int quantity { get; }
  /// <summary>
  /// 單價
  /// </summary>
  [JsonPropertyName("單價")]
  public decimal unitPrice { get; }
  /// <summary>
  /// 初始化訂單裡的商品
  /// </summary>
  /// <param name="productId">商品編號</param>
  /// <param name="productName">商品名稱</param>
  /// <param name="quantity">購買數量</param>
  /// <param name="unitPrice">單價</param>
  public OrderProduct(int productId, string productName, int quantity, decimal unitPrice)
  {
    this.productId = productId;
    this.productName = productName;
    this.quantity = quantity;
    this.unitPrice = unitPrice;
  }
}
