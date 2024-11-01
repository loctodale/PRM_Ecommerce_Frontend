namespace KoiOrderingSystemInJapan.Data.Models;

public partial class Sale
{
    public Guid Id { get; set; }

    public Guid? BookingRequestId { get; set; }

    public Guid? SaleStaffId { get; set; }

    public string? ProposalDetails { get; set; }

    public decimal? TotalPrice { get; set; }

    public ConstEnum.StatusSale? Status { get; set; }

    public DateTime? ResponseDate { get; set; }

    public string? ResponseBy { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool IsDeleted { get; set; }

    public string? Note { get; set; }

    public virtual BookingRequest? BookingRequest { get; set; }

    public virtual User? SaleStaff { get; set; }
}