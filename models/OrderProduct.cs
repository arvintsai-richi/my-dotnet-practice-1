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
}
