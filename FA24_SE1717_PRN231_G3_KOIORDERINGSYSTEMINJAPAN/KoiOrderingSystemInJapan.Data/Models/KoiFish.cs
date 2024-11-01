namespace KoiOrderingSystemInJapan.Data.Models;

public partial class KoiFish
{
    public Guid Id { get; set; }

    public DateTime? Dob {  get; set; }

    public string? Name {  get; set; }

    public Guid? CategoryId { get; set; }

    public string? Picture { get; set; }

    public decimal? Price { get; set; }

    public string? Description { get; set; }

    public string? Origin { get; set; }

    public string? Status { get; set; } //bán hay chưa

    public DateTime? DateSold { get; set; }//ngày bán

    public ConstEnum.Gender? Gender { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool IsDeleted { get; set; }

    public string? Note { get; set; }

    public virtual Category? Category { get; set; }

    public virtual Size? Size { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

}