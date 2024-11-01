using KoiOrderingSystemInJapan.Data.Context;
using KoiOrderingSystemInJapan.Data.Repositories;

namespace KoiOrderingSystemInJapan.Data
{
    public class UnitOfWork
    {
        private KoiOrderingSystemInJapanContext context;
        private UserRepository userRepository;
        private DeliveryRepository deliveryRepository;
        private DeliveryDetailRepository deliveryDetailRepository;
        private InvoiceRepository invoiceRepository;
        private ServiceOrderRepository serviceOrderRepository;
        private SaleRepository saleRepository;
        private KoiOrderRepository koiOrderRepository;
        private BookingRequestRepository bookingRequestRepository;
        private ServiceRepository serviceRepository;
        public TravelRepository travelRepository;
        public FarmRepository farmRepository;
        private OrderDetailRepository orderDetailRepository;
        private KoiFishRepository koifishRepository;
        private CategoryRepository categoryRepository;
        public UnitOfWork()
        {
            context ??= new KoiOrderingSystemInJapanContext();
        }

        public UserRepository User
        {
            get { return userRepository ??= new UserRepository(context); }
        }

        public DeliveryRepository Delivery
        {
            get
            {
                return deliveryRepository ??= new DeliveryRepository(context);
            }
        }
        public DeliveryDetailRepository DeliveryDetail
        {
            get
            {
                return deliveryDetailRepository ??= new DeliveryDetailRepository(context);
            }
        }
        public InvoiceRepository Invoice
        {
            get { return invoiceRepository ??= new InvoiceRepository(context); }
        }
        public ServiceOrderRepository ServiceOrder
        {
            get { return serviceOrderRepository ??= new ServiceOrderRepository(context); }
        }
        public KoiOrderRepository KoiOrder
        {
            get { return koiOrderRepository ??= new KoiOrderRepository(context); }
        }

        public SaleRepository Sale
        {
            get { return saleRepository ??= new SaleRepository(context); }
        }

        public BookingRequestRepository BookingRequest
        {
            get { return bookingRequestRepository ??= new BookingRequestRepository(context); }
        }

        public ServiceRepository Service
        {
            get { return serviceRepository ??= new ServiceRepository(context); }
        }

        public TravelRepository Travel
        {
            get { return travelRepository ??= new TravelRepository(context); }
        }

        public FarmRepository Farm
        {
            get {return farmRepository ??= new FarmRepository(context); }
        }

        public OrderDetailRepository OrderDetail
        {
            get { return orderDetailRepository ??= new OrderDetailRepository(context); }
        }

        public KoiFishRepository KoiFish
        {
            get { return koifishRepository ??= new KoiFishRepository(context); }
        }
        public CategoryRepository Category
        {
            get { return categoryRepository ??= new CategoryRepository(context); }
        }
        ////TO-DO CODE HERE/////////////////

        #region Set transaction isolation levels

        /*
        Read Uncommitted: The lowest level of isolation, allows transactions to read uncommitted data from other transactions. This can lead to dirty reads and other issues.

        Read Committed: Transactions can only read data that has been committed by other transactions. This level avoids dirty reads but can still experience other isolation problems.

        Repeatable Read: Transactions can only read data that was committed before their execution, and all reads are repeatable. This prevents dirty reads and non-repeatable reads, but may still experience phantom reads.

        Serializable: The highest level of isolation, ensuring that transactions are completely isolated from one another. This can lead to increased lock contention, potentially hurting performance.

        Snapshot: This isolation level uses row versioning to avoid locks, providing consistency without impeding concurrency. 
         */

        public int SaveChangesWithTransaction()
        {
            int result = -1;

            //System.Data.IsolationLevel.Snapshot
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    result = context.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    //Log Exception Handling message                      
                    result = -1;
                    dbContextTransaction.Rollback();
                }
            }

            return result;
        }

        public async Task<int> SaveChangesWithTransactionAsync()
        {
            int result = -1;

            //System.Data.IsolationLevel.Snapshot
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    result = await context.SaveChangesAsync();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    //Log Exception Handling message                      
                    result = -1;
                    dbContextTransaction.Rollback();
                }
            }

            return result;
        }
        #endregion
    }
}
