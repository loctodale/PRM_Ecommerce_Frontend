namespace KoiOrderingSystemInJapan.Data
{
    public class ConstEnum
    {
        public enum StatusSale
        {
            Pending,
            Approved,
            Rejected
        }

        public enum BookingRequestStatus
        {
            Pending,
            Approved,
            Rejected
        }

        public enum Role
        {
            Manager,
            SaleStaff,
            Deliver,
            Customer,
        }

        public enum Gender
        {
            Male,
            Female,
            Other
        }

    }
}
