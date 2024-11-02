﻿namespace KoiOrderingSystemInJapan.Data.Models;

public partial class Delivery
{
    public Guid Id { get; set; }

    public Guid? KoiOrderId { get; set; }

    public Guid? DeliveryStaffId { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public decimal? TotalAmount { get; set; }

    public bool? PaymentReceived { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool IsDeleted { get; set; }

    public string? Note { get; set; }


    public virtual ICollection<DeliveryDetail> DeliveryDetails { get; set; } = new List<DeliveryDetail>();

    public virtual User? DeliveryStaff { get; set; }

    public virtual KoiOrder? KoiOrder { get; set; }
}