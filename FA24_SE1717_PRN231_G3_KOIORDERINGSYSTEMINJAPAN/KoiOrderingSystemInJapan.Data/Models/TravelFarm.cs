namespace KoiOrderingSystemInJapan.Data.Models;

public partial class TravelFarm
{
    public Guid TravelId { get; set; }

    public Guid FarmId { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool IsDeleted { get; set; }

    public string? Note { get; set; }

    public virtual Farm? Farm { get; set; }

    public virtual Travel? Travel { get; set; }
}