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
  public int id { get; }
  /// <summary>
  /// 訂購日期
  /// </summary>
  [JsonPropertyName("訂購日期")]
  public string date { get; }
  /// <summary>
  /// 訂購金額
  /// </summary>
  [JsonPropertyName("訂購金額")]
  public decimal total { get; }
  /// <summary>
  /// 訂購商品清單
  /// </summary>
  [JsonPropertyName("訂購商品清單")]
  public List<OrderProduct> productList { get; }
  /// <summary>
  /// 初始化訂單資訊
  /// </summary>
  /// <param name="id">訂單編號</param>
  /// <param name="date">訂購日期</param>
  /// <param name="productList">訂購商品清單</param>
  public Order(int id, DateTime date, List<OrderProduct> productList)
  {
    this.id = id;
    this.date = date.ToString("yyyy/M/d");
    this.productList = productList;
    total = productList.Sum(product => product.quantity * product.unitPrice);
  }
}
