namespace KoiOrderingSystemInJapan.Data.Models;

public partial class FarmCategory
{
    public Guid FarmId { get; set; }

    public Guid CategoryId { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Category? Category { get; set; }

    public virtual Farm? Farm { get; set; }
}