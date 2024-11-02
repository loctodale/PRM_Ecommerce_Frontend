using Bogus;
using KoiOrderingSystemInJapan.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace KoiOrderingSystemInJapan.Data
{
    public static class DummyData
    {
        public static void SeedDatabase(DbContext context)
        {
            GenerateTravel(context, 20);
            GenerateFarm(context, 10);
            GenerateTravelFarm(context, 10);
            GenerateUser(context, 50);
            GenerateService(context, 20);
            GenerateBookingRequest(context, 20);
            GenerateServiceBookingRequest(context, 20);
            GenerateSale(context, 20);
            GenerateCategory(context, 10);
            GenerateFarmCategory(context, 30);
            GenerateKoiFish(context, 50);
            GenerateSize(context, 50);
            GenerateInvoice(context, 5);
            GenerateServiceOrder(context, 20);
            GenerateKoiOrder(context, 20);
            GenerateOrderDetail(context, 30);
            GenerateDelivery(context, 30);
            GenerateDeliveryDetail(context, 30);
        }
        public static void GenerateTravel(DbContext context, int count)
        {
            if (!context.Set<Travel>().Any())
            {
                var fakers = new Faker<Travel>()
                    .RuleFor(a => a.Id, f => Guid.NewGuid())
                    .RuleFor(a => a.Name, f => f.Name.FullName())
                    .RuleFor(a => a.Location, f => f.Address.FullAddress())
                    .RuleFor(a => a.Price, f => f.Random.Decimal(100, 10000))
                    .RuleFor(s => s.CreatedBy, f => "tsql@gmail.com")
                    .RuleFor(s => s.CreatedDate, f => f.Date.Past(2))
                    .RuleFor(s => s.UpdatedBy, f => "tsql@gmail.com")
                    .RuleFor(s => s.UpdatedDate, f => f.Date.Recent())
                    .RuleFor(s => s.IsDeleted, f => false)
                    .RuleFor(a => a.Note, f => f.Lorem.Sentence());
                var list = fakers.Generate(count);

                context.Set<Travel>().AddRange(list); // Thêm vào cơ sở dữ liệu
                context.SaveChanges(); // Lưu thay đổi
            }
        }

        public static void GenerateFarm(DbContext context, int count)
        {
            if (!context.Set<Farm>().Any())
            {
                var fakers = new Faker<Farm>()
                    .RuleFor(a => a.Id, f => Guid.NewGuid())
                    .RuleFor(a => a.Name, f => f.Company.CompanyName())
                    .RuleFor(a => a.Address, f => f.Address.FullAddress())
                    .RuleFor(a => a.Owner, f => f.Name.FullName())
                    .RuleFor(a => a.Phone, f => f.Phone.PhoneNumber())
                    .RuleFor(a => a.Email, f => f.Internet.Email())
                    .RuleFor(s => s.CreatedBy, f => "tsql@gmail.com")
                    .RuleFor(s => s.CreatedDate, f => f.Date.Past(2))
                    .RuleFor(s => s.UpdatedBy, f => "tsql@gmail.com")
                    .RuleFor(s => s.UpdatedDate, f => f.Date.Recent())
                    .RuleFor(s => s.IsDeleted, f => false)
                    .RuleFor(a => a.Note, f => f.Lorem.Sentence());
                var list = fakers.Generate(count);

                context.Set<Farm>().AddRange(list); // Thêm vào cơ sở dữ liệu
                context.SaveChanges(); // Lưu thay đổi
            }
        }

        public static void GenerateTravelFarm(DbContext context, int count)
        {
            if (!context.Set<TravelFarm>().Any())
            {
                // Lấy danh sách các Id đã tồn tại
                var travelIds = context.Set<Travel>().Select(u => u.Id).ToList();

                // Lấy danh sách các Id đã tồn tại
                var farmIds = context.Set<Farm>().Select(u => u.Id).ToList();

                var fakers = new Faker<TravelFarm>()
                    .RuleFor(a => a.TravelId, f => f.PickRandom(travelIds))
                    .RuleFor(a => a.FarmId, f => f.PickRandom(farmIds))
                    .RuleFor(s => s.CreatedBy, f => "tsql@gmail.com")
                    .RuleFor(s => s.CreatedDate, f => f.Date.Past(2))
                    .RuleFor(s => s.UpdatedBy, f => "tsql@gmail.com")
                    .RuleFor(s => s.UpdatedDate, f => f.Date.Recent())
                    .RuleFor(s => s.IsDeleted, f => false)
                    .RuleFor(a => a.Note, f => f.Lorem.Sentence());
                var list = fakers.Generate(count);

                context.Set<TravelFarm>().AddRange(list); // Thêm vào cơ sở dữ liệu
                context.SaveChanges(); // Lưu thay đổi
            }
        }

        public static void GenerateUser(DbContext context, int count)
        {
            if (!context.Set<User>().Any())
            {
                var fakers = new Faker<User>()
                    .RuleFor(a => a.Id, f => Guid.NewGuid())
                    .RuleFor(a => a.Firstname, f => f.Name.FirstName())
                    .RuleFor(a => a.Lastname, f => f.Name.LastName())
                    .RuleFor(a => a.Dob, f => DateOnly.FromDateTime(f.Date.Between(new DateTime(1990, 1, 1), DateTime.Now)))
                    .RuleFor(a => a.Address, f => f.Address.StreetAddress())
                    .RuleFor(a => a.Role, f => f.PickRandom<ConstEnum.Role>())
                    .RuleFor(a => a.Gender, f => f.PickRandom<ConstEnum.Gender>())
                    .RuleFor(a => a.Username, f => f.Internet.UserName())
                    .RuleFor(a => a.Password, f => f.Internet.Password())
                    .RuleFor(a => a.Phone, f => f.Phone.PhoneNumber())
                    .RuleFor(a => a.Email, f => f.Internet.Email())
                    .RuleFor(s => s.CreatedBy, f => "tsql@gmail.com")
                    .RuleFor(s => s.CreatedDate, f => f.Date.Past(2))
                    .RuleFor(s => s.UpdatedBy, f => "tsql@gmail.com")
                    .RuleFor(s => s.UpdatedDate, f => f.Date.Recent())
                    .RuleFor(s => s.IsDeleted, f => false)
                    .RuleFor(s => s.Note, f => f.Lorem.Sentence(1));
                var list = fakers.Generate(count);

                context.Set<User>().AddRange(list); // Thêm vào cơ sở dữ liệu
                context.SaveChanges(); // Lưu thay đổi
            }
        }

        public static void GenerateService(DbContext context, int count)
        {
            if (!context.Set<Service>().Any())
            {
                var fakers = new Faker<Service>()
                    .RuleFor(a => a.Id, f => Guid.NewGuid())
                    .RuleFor(a => a.ServiceName, f => f.Commerce.Product())
                    .RuleFor(a => a.Description, f => f.Commerce.ProductDescription())
                    .RuleFor(a => a.Price, f => f.Random.Decimal(100, 10000))
                    .RuleFor(s => s.CreatedBy, f => "tsql@gmail.com")
                    .RuleFor(s => s.CreatedDate, f => f.Date.Past(2))
                    .RuleFor(s => s.UpdatedBy, f => "tsql@gmail.com")
                    .RuleFor(s => s.UpdatedDate, f => f.Date.Recent())
                    .RuleFor(s => s.IsDeleted, f => false)
                    .RuleFor(a => a.Note, f => f.Lorem.Sentence());
                var list = fakers.Generate(count);

                context.Set<Service>().AddRange(list); // Thêm vào cơ sở dữ liệu
                context.SaveChanges(); // Lưu thay đổi
            }
        }

        public static void GenerateBookingRequest(DbContext context, int count)
        {

            if (!context.Set<BookingRequest>().Any())
            {
                // Lấy danh sách các Id đã tồn tại
                var userIds = context.Set<User>().Where(u => u.Role == ConstEnum.Role.Customer).Select(u => u.Id).ToList();

                // Lấy danh sách các Id đã tồn tại
                var travelIds = context.Set<Travel>().Select(u => u.Id).ToList();

                var fakers = new Faker<BookingRequest>()
                    .RuleFor(a => a.Id, f => Guid.NewGuid())
                    .RuleFor(a => a.CustomerId, f => f.PickRandom(userIds))
                    .RuleFor(a => a.TravelId, f => f.PickRandom(travelIds))
                    .RuleFor(a => a.QuantityService, f => f.Random.Int(1, 10))
                    .RuleFor(a => a.NumberOfPerson, f => f.Random.Int(1, 10))
                    .RuleFor(a => a.Status, f => f.PickRandom<ConstEnum.BookingRequestStatus>())
                    .RuleFor(s => s.CreatedBy, f => "tsql@gmail.com")
                    .RuleFor(s => s.CreatedDate, f => f.Date.Past(2))
                    .RuleFor(s => s.UpdatedBy, f => "tsql@gmail.com")
                    .RuleFor(s => s.UpdatedDate, f => f.Date.Recent())
                    .RuleFor(s => s.IsDeleted, f => false)
                    .RuleFor(a => a.Note, f => f.Lorem.Sentence());
                var list = fakers.Generate(count);

                context.Set<BookingRequest>().AddRange(list); // Thêm vào cơ sở dữ liệu
                context.SaveChanges(); // Lưu thay đổi
            }
        }

        public static void GenerateServiceBookingRequest(DbContext context, int count)
        {

            if (!context.Set<ServiceXBookingRequest>().Any())
            {
                // Lấy danh sách các Id đã tồn tại
                var sIds = context.Set<Service>().Select(u => u.Id).ToList();
                var csIds = context.Set<BookingRequest>().Select(u => u.Id).ToList();

                var fakers = new Faker<ServiceXBookingRequest>()
                    .RuleFor(a => a.ServiceId, f => f.PickRandom(sIds))
                    .RuleFor(a => a.BookingRequestId, f => f.PickRandom(csIds));

                var list = fakers.Generate(count);

                // Loại bỏ các bản ghi trùng lặp
                var distinctList = list.GroupBy(x => new { x.ServiceId, x.BookingRequestId })
                                       .Select(g => g.First()) // Chọn bản ghi đầu tiên trong nhóm trùng lặp
                                       .ToList();

                context.Set<ServiceXBookingRequest>().AddRange(distinctList); // Thêm vào cơ sở dữ liệu
                context.SaveChanges(); // Lưu thay đổi
            }
        }

        public static void GenerateSale(DbContext context, int count)
        {
            if (!context.Set<Sale>().Any())
            {
                // Lấy danh sách các Id đã tồn tại
                var ssIds = context.Set<User>().Where(u => u.Role == ConstEnum.Role.SaleStaff).Select(u => u.Id).ToList();
                var csIds = context.Set<BookingRequest>().Select(u => u.Id).ToList();
                var mIds = context.Set<User>().Where(u => u.Role == ConstEnum.Role.Manager).Select(u => u.Firstname + " " + u.Lastname).ToList();

                var usedBookingRequestIds = new HashSet<Guid>(); // Dùng HashSet để lưu trữ BookingRequestId đã sử dụng

                var fakers = new Faker<Sale>()
                    .RuleFor(a => a.Id, f => Guid.NewGuid())
                    .RuleFor(a => a.SaleStaffId, f => f.PickRandom(ssIds))
                    .RuleFor(a => a.BookingRequestId, f =>
                    {
                        // Tìm một BookingRequestId chưa được sử dụng
                        Guid customerServiceId;
                        do
                        {
                            customerServiceId = f.PickRandom(csIds);
                        } while (usedBookingRequestIds.Contains(customerServiceId));

                        usedBookingRequestIds.Add(customerServiceId); // Thêm vào danh sách đã sử dụng
                        return customerServiceId;
                    })
                    .RuleFor(a => a.ResponseBy, f => f.PickRandom(mIds))
                    .RuleFor(a => a.ProposalDetails, f => f.Lorem.Sentence())
                    .RuleFor(a => a.TotalPrice, f => f.Random.Decimal(200, 10000))
                    .RuleFor(s => s.Status, f => f.PickRandom<ConstEnum.StatusSale>())
                    .RuleFor(s => s.ResponseDate, f => f.Date.Past(2))
                    .RuleFor(s => s.CreatedBy, f => "tsql@gmail.com")
                    .RuleFor(s => s.CreatedDate, f => f.Date.Past(5))
                    .RuleFor(s => s.UpdatedBy, f => "tsql@gmail.com")
                    .RuleFor(s => s.UpdatedDate, f => f.Date.Recent())
                    .RuleFor(s => s.IsDeleted, f => false);

                var list = fakers.Generate(count);

                context.Set<Sale>().AddRange(list); // Thêm vào cơ sở dữ liệu
                context.SaveChanges(); // Lưu thay đổi
            }
        }

        public static void GenerateCategory(DbContext context, int count)
        {
            if (!context.Set<Category>().Any())
            {
                var fakers = new Faker<Category>()
                    .RuleFor(a => a.Id, f => Guid.NewGuid())
                    .RuleFor(a => a.Name, f => f.Commerce.Categories(1).First())
                    .RuleFor(s => s.CreatedBy, f => "tsql@gmail.com")
                    .RuleFor(s => s.CreatedDate, f => f.Date.Past(2))
                    .RuleFor(s => s.UpdatedBy, f => "tsql@gmail.com")
                    .RuleFor(s => s.UpdatedDate, f => f.Date.Recent())
                    .RuleFor(s => s.IsDeleted, f => false);
                var list = fakers.Generate(count);

                context.Set<Category>().AddRange(list); // Thêm vào cơ sở dữ liệu
                context.SaveChanges(); // Lưu thay đổi
            }
        }

        public static void GenerateFarmCategory(DbContext context, int count)
        {
            if (!context.Set<FarmCategory>().Any())
            {
                // Lấy danh sách các Id đã tồn tại
                var fIds = context.Set<Farm>().Select(u => u.Id).ToList();
                var cIds = context.Set<Category>().Select(u => u.Id).ToList();

                var existingFarmCategories = context.Set<FarmCategory>()
                    .Select(fc => new { fc.FarmId, fc.CategoryId })
                    .ToHashSet(); // Get existing combinations in a HashSet for quick look-up

                var fakers = new Faker<FarmCategory>()
                    .RuleFor(a => a.FarmId, f => f.PickRandom(fIds))
                    .RuleFor(a => a.CategoryId, f => f.PickRandom(cIds))
                    .RuleFor(s => s.CreatedBy, f => "tsql@gmail.com")
                    .RuleFor(s => s.CreatedDate, f => f.Date.Past(2))
                    .RuleFor(s => s.UpdatedBy, f => "tsql@gmail.com")
                    .RuleFor(s => s.UpdatedDate, f => f.Date.Recent())
                    .RuleFor(s => s.IsDeleted, f => false);

                var list = new List<FarmCategory>();
                while (list.Count < count)
                {
                    var newCategory = fakers.Generate();
                    // Ensure the generated combination is unique
                    if (!existingFarmCategories.Contains(new { FarmId = newCategory.FarmId, CategoryId = newCategory.CategoryId }))
                    {
                        list.Add(newCategory);
                        existingFarmCategories.Add(new { FarmId = newCategory.FarmId, CategoryId = newCategory.CategoryId });
                    }
                }

                context.Set<FarmCategory>().AddRange(list); // Thêm vào cơ sở dữ liệu
                context.SaveChanges(); // Lưu thay đổi
            }
        }

        public static void GenerateKoiFish(DbContext context, int count)
        {
            if (!context.Set<KoiFish>().Any())
            {
                // Lấy danh sách các Id đã tồn tại
                var cIds = context.Set<Category>().Select(u => u.Id).ToList();

                var fakers = new Faker<KoiFish>()
                    .RuleFor(a => a.Id, f => Guid.NewGuid())
                    .RuleFor(a => a.CategoryId, f => f.PickRandom(cIds))
                    .RuleFor(a => a.Name, f => "Cá KOI " + f.Name.FullName())
                    .RuleFor(a => a.Gender, f => f.PickRandom<ConstEnum.Gender>())
                    .RuleFor(a => a.Picture, f => f.Image.PicsumUrl())
                    .RuleFor(a => a.Description, f => f.Lorem.Paragraph(3))
                    .RuleFor(a => a.Price, f => f.Random.Decimal(50, 10000))
                    .RuleFor(s => s.CreatedBy, f => "tsql@gmail.com")
                    .RuleFor(s => s.CreatedDate, f => f.Date.Past(2))
                    .RuleFor(s => s.UpdatedBy, f => "tsql@gmail.com")
                    .RuleFor(s => s.UpdatedDate, f => f.Date.Recent())
                    .RuleFor(s => s.IsDeleted, f => false)
                    .RuleFor(s => s.Status, f => f.PickRandom("Still", "Sold"))
                    .RuleFor(s => s.Origin, f => f.Address.Country())
                    .RuleFor(s => s.DateSold, f => f.Date.Between(new DateTime(2024, 8, 1), DateTime.Now))
                    .RuleFor(s => s.Dob, f => f.Date.Between(new DateTime(2020, 1, 1), new DateTime(2024, 5, 1)));
                var list = fakers.Generate(count);
                context.Set<KoiFish>().AddRange(list); // Thêm vào cơ sở dữ liệu
                context.SaveChanges(); // Lưu thay đổi
            }
        }

        public static void GenerateSize(DbContext context, int count)
        {
            if (!context.Set<Size>().Any())
            {
                // Lấy danh sách các Id đã tồn tại
                var kfIds = context.Set<KoiFish>().Select(u => u.Id).ToList();

                var fakers = new Faker<Size>()
                    .RuleFor(a => a.Id, f => Guid.NewGuid())
                    .RuleFor(a => a.KoiFishId, f => f.PickRandom(kfIds))
                    .RuleFor(a => a.SizeInCm, f => f.Random.Decimal(10, 100))
                    .RuleFor(a => a.SizeInGram, f => f.Random.Decimal(100, 10000))
                    .RuleFor(s => s.CreatedBy, f => "tsql@gmail.com")
                    .RuleFor(s => s.CreatedDate, f => f.Date.Past(2))
                    .RuleFor(s => s.UpdatedBy, f => "tsql@gmail.com")
                    .RuleFor(s => s.UpdatedDate, f => f.Date.Recent())
                    .RuleFor(s => s.IsDeleted, f => false)
                    .RuleFor(a => a.Note, f => f.Lorem.Sentence());
                var list = fakers.Generate(count);

                context.Set<Size>().AddRange(list); // Thêm vào cơ sở dữ liệu
                context.SaveChanges(); // Lưu thay đổi
            }
        }

        public static void GenerateInvoice(DbContext context, int count)
        {
            if (!context.Set<Invoice>().Any())
            {
                var fakers = new Faker<Invoice>()
                    .RuleFor(a => a.Id, f => Guid.NewGuid())
                    .RuleFor(a => a.PaymentAmount, f => f.Random.Decimal(100, 10000))
                    .RuleFor(a => a.PaymentDate, f => f.Date.Between(new DateTime(2024, 1, 1), DateTime.Now))
                    .RuleFor(s => s.CreatedBy, f => "tsql@gmail.com")
                    .RuleFor(s => s.CreatedDate, f => f.Date.Past(2))
                    .RuleFor(s => s.UpdatedBy, f => "tsql@gmail.com")
                    .RuleFor(s => s.UpdatedDate, f => f.Date.Recent())
                    .RuleFor(s => s.IsDeleted, f => false);
                var list = fakers.Generate(count);

                context.Set<Invoice>().AddRange(list); // Thêm vào cơ sở dữ liệu
                context.SaveChanges(); // Lưu thay đổi
            }
        }

        public static void GenerateServiceOrder(DbContext context, int count)
        {
            if (!context.Set<ServiceOrder>().Any())
            {
                // Lấy danh sách các Id đã tồn tại
                var csIds = context.Set<BookingRequest>().Select(u => u.Id).ToList();

                // Lấy danh sách các Id đã tồn tại
                var iIds = context.Set<Invoice>().Select(u => u.Id).ToList();

                var fakers = new Faker<ServiceOrder>()
                    .RuleFor(a => a.Id, f => Guid.NewGuid())
                    .RuleFor(a => a.BookingRequestId, f => f.PickRandom(csIds))
                    .RuleFor(a => a.InvoiceId, f => f.PickRandom(iIds))
                    .RuleFor(a => a.Quantity, f => f.Random.Int(1, 10))
                    .RuleFor(a => a.TotalPrice, f => f.Random.Decimal(100, 10000))
                    .RuleFor(s => s.CreatedBy, f => "tsql@gmail.com")
                    .RuleFor(s => s.CreatedDate, f => f.Date.Past(2))
                    .RuleFor(s => s.UpdatedBy, f => "tsql@gmail.com")
                    .RuleFor(s => s.UpdatedDate, f => f.Date.Recent())
                    .RuleFor(s => s.IsDeleted, f => false)
                    .RuleFor(s => s.Note, f => f.Lorem.Sentence(1));
                var list = fakers.Generate(count);

                context.Set<ServiceOrder>().AddRange(list); // Thêm vào cơ sở dữ liệu
                context.SaveChanges(); // Lưu thay đổi
            }
        }

        public static void GenerateKoiOrder(DbContext context, int count)
        {

            if (!context.Set<KoiOrder>().Any())
            {
                // Lấy danh sách các Id đã tồn tại
                var cIds = context.Set<User>().Where(u => u.Role == ConstEnum.Role.Customer).Select(u => u.Id).ToList();

                // Lấy danh sách các Id đã tồn tại
                var iIds = context.Set<Invoice>().Select(u => u.Id).ToList();

                var fakers = new Faker<KoiOrder>()
                    .RuleFor(a => a.Id, f => Guid.NewGuid())
                    .RuleFor(a => a.CustomerId, f => f.PickRandom(cIds))
                    .RuleFor(a => a.InvoiceId, f => f.PickRandom(iIds))
                    .RuleFor(a => a.Quantity, f => f.Random.Int(1, 10))
                    .RuleFor(a => a.TotalPrice, f => f.Random.Decimal(100, 10000))
                    .RuleFor(s => s.CreatedBy, f => "tsql@gmail.com")
                    .RuleFor(s => s.CreatedDate, f => f.Date.Past(2))
                    .RuleFor(s => s.UpdatedBy, f => "tsql@gmail.com")
                    .RuleFor(s => s.UpdatedDate, f => f.Date.Recent())
                    .RuleFor(s => s.IsDeleted, f => false)
                    .RuleFor(s => s.Note, f => f.Lorem.Sentence(1));
                var list = fakers.Generate(count);

                context.Set<KoiOrder>().AddRange(list); // Thêm vào cơ sở dữ liệu
                context.SaveChanges(); // Lưu thay đổi
            }
        }

        public static void GenerateOrderDetail(DbContext context, int count)
        {
            if (!context.Set<OrderDetail>().Any())
            {
                // Lấy danh sách các Id đã tồn tại
                var kfIds = context.Set<KoiFish>().Select(u => u.Id).ToList();

                // Lấy danh sách các Id đã tồn tại
                var koIds = context.Set<KoiOrder>().Select(u => u.Id).ToList();

                var fakers = new Faker<OrderDetail>()
                    .RuleFor(a => a.Id, f => Guid.NewGuid())
                    .RuleFor(a => a.KoiFishId, f => f.PickRandom(kfIds))
                    .RuleFor(a => a.KoiOrderId, f => f.PickRandom(koIds))
                    .RuleFor(a => a.Price, f => f.Random.Decimal(100, 10000))
                    .RuleFor(s => s.CreatedBy, f => "tsql@gmail.com")
                    .RuleFor(s => s.CreatedDate, f => f.Date.Past(2))
                    .RuleFor(s => s.UpdatedBy, f => "tsql@gmail.com")
                    .RuleFor(s => s.UpdatedDate, f => f.Date.Recent())
                    .RuleFor(s => s.IsDeleted, f => false)
                    .RuleFor(s => s.Note, f => f.Lorem.Sentence(1));
                var list = fakers.Generate(count);

                context.Set<OrderDetail>().AddRange(list); // Thêm vào cơ sở dữ liệu
                context.SaveChanges(); // Lưu thay đổi
            }
        }

        public static void GenerateDelivery(DbContext context, int count)
        {

            if (!context.Set<Delivery>().Any())
            {
                // Lấy danh sách các Id đã tồn tại
                var koIds = context.Set<KoiOrder>().Select(u => u.Id).ToList();

                // Lấy danh sách các Id đã tồn tại
                var dIds = context.Set<User>().Where(u => u.Role == ConstEnum.Role.Deliver).Select(u => u.Id).ToList();

                var fakers = new Faker<Delivery>()
                    .RuleFor(a => a.Id, f => Guid.NewGuid())
                    .RuleFor(a => a.KoiOrderId, f => f.PickRandom(koIds))
                    .RuleFor(a => a.DeliveryStaffId, f => f.PickRandom(dIds))
                    .RuleFor(a => a.Name, f => f.Name.FullName())
                    .RuleFor(a => a.Code, f => f.Random.AlphaNumeric(10))
                    .RuleFor(a => a.Phone, f => f.Phone.PhoneNumber())
                    .RuleFor(a => a.Address, f => f.Address.FullAddress())
                    .RuleFor(a => a.TotalAmount, f => f.Random.Decimal(100, 10000))
                    .RuleFor(a => a.PaymentReceived, f => true)
                    .RuleFor(s => s.CreatedBy, f => "tsql@gmail.com")
                    .RuleFor(s => s.CreatedDate, f => f.Date.Past(2))
                    .RuleFor(s => s.UpdatedBy, f => "tsql@gmail.com")
                    .RuleFor(s => s.UpdatedDate, f => f.Date.Recent())
                    .RuleFor(s => s.IsDeleted, f => false)
                    .RuleFor(s => s.Note, f => f.Lorem.Sentence(1));
                var list = fakers.Generate(count);

                context.Set<Delivery>().AddRange(list); // Thêm vào cơ sở dữ liệu
                context.SaveChanges(); // Lưu thay đổi
            }
        }

        public static void GenerateDeliveryDetail(DbContext context, int count)
        {

            if (!context.Set<DeliveryDetail>().Any())
            {
                // Lấy danh sách các Id đã tồn tại
                var dIds = context.Set<Delivery>().Select(u => u.Id).ToList();

                var fakers = new Faker<DeliveryDetail>()
                    .RuleFor(a => a.Id, f => Guid.NewGuid())
                    .RuleFor(a => a.DeliveryId, f => f.PickRandom(dIds))
                    .RuleFor(a => a.Name, f => f.Name.FullName())
                    .RuleFor(a => a.Description, f => f.Lorem.Sentence(1))
                    .RuleFor(s => s.CreatedBy, f => "tsql@gmail.com")
                    .RuleFor(s => s.CreatedDate, f => f.Date.Past(2))
                    .RuleFor(s => s.UpdatedBy, f => "tsql@gmail.com")
                    .RuleFor(s => s.UpdatedDate, f => f.Date.Recent())
                    .RuleFor(s => s.IsDeleted, f => false)
                    .RuleFor(s => s.Note, f => f.Lorem.Sentence(1));
                var list = fakers.Generate(count);

                context.Set<DeliveryDetail>().AddRange(list); // Thêm vào cơ sở dữ liệu
                context.SaveChanges(); // Lưu thay đổi
            }
        }
    }
}
