namespace KoiOrderingSystemInJapan.Data.Models;

public partial class User
{
    public Guid Id { get; set; }

    public string? Image { get; set; }

    public string? Firstname { get; set; }

    public string? Lastname { get; set; }

    public DateOnly? Dob { get; set; }

    public string? Address { get; set; }

    public ConstEnum.Gender? Gender { get; set; }

    public ConstEnum.Role? Role { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool IsDeleted { get; set; }

    public string? Note { get; set; }

    public virtual ICollection<BookingRequest> BookingRequests { get; set; } = new List<BookingRequest>();

    public virtual ICollection<Delivery> Deliveries { get; set; } = new List<Delivery>();

    public virtual ICollection<KoiOrder> KoiOrders { get; set; } = new List<KoiOrder>();

    public virtual ICollection<Sale> SaleSaleStaffs { get; set; } = new List<Sale>();
}