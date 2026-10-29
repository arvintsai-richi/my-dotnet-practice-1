using System.Text.Json.Serialization;

/// <summary>
/// 資料庫的訂單資訊
/// </summary>
public class Order
{
  /// <summary>
  /// 訂單編號
  /// </summary>
  [JsonPropertyName("訂單編號")]
  public int Id { get; set; }
  /// <summary>
  /// 訂購日期 (字串型態)
  /// </summary>
  [JsonPropertyName("訂購日期")]
  public string DateString => Date.ToString("yyyy/M/d");
  /// <summary>
  /// 訂購日期 (日期型態)
  /// </summary>
  [JsonIgnore]
  public DateTime Date { get; set; }
  /// <summary>
  /// 訂購金額
  /// </summary>
  [JsonPropertyName("訂購金額")]
  public decimal Total { get; private set; }
  /// <summary>
  /// 訂購商品清單
  /// </summary>
  [JsonPropertyName("訂購商品清單")]
  public List<OrderProduct> ProductList
  {
    get; set
    {
      Total = value.Sum(product => product.Quantity * product.UnitPrice);
      field = value;
    }
  } = [];
}
