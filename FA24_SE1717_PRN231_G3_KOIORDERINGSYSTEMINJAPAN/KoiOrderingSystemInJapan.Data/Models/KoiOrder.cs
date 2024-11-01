﻿namespace KoiOrderingSystemInJapan.Data.Models;

public partial class KoiOrder
{
    public Guid Id { get; set; }

    public Guid? CustomerId { get; set; }

    public Guid? InvoiceId { get; set; }

    public int? Quantity { get; set; }

    public decimal? TotalPrice { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool IsDeleted { get; set; }

    public string? Note { get; set; }

    public virtual User? Customer { get; set; }

    public virtual Delivery? Deliveries { get; set; }

    public virtual Invoice? Invoice { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}