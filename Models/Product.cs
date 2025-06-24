using System.ComponentModel.DataAnnotations.Schema;

namespace VetPharmacyApi.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    [Column(TypeName = "timestamp without time zone")]
    public DateTime ArrivalDate { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime ExpiryDate { get; set; }

    public bool IsWriteOffAllowed { get; set; }
    
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}