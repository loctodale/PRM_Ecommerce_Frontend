namespace KoiOrderingSystemInJapan.Data.Models;

public partial class Invoice
{
    public Guid Id { get; set; }

    public decimal? PaymentAmount { get; set; }

    public DateTime? PaymentDate { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool IsDeleted { get; set; }

    public string? Note { get; set; }

    public virtual KoiOrder? KoiOrder { get; set; }

    public virtual ServiceOrder? ServiceOrder { get; set; }
}