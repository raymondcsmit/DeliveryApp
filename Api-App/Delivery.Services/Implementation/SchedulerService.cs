using Core;
using Delivery.Services.Abstract;
using Delivery.ViewModel;
using DeliveryApp.DAL;
using DeliveryApp.DAL.Entities;

namespace Delivery.Services.Implementation
{
    public class SchedulerService : ISchedulerService
    {
        private readonly DeliveryContext _context;
        private readonly IRepository<Scheduler> _scheduleRepository;
        private readonly IRepository<DeliveryDetails> _deliveryRepository;
        private readonly IRepository<DeliveryLog> _deliverylogRepository;
        private readonly IRepository<Order> _OrderRepository;
        private readonly IRepository<OrderItem> _OrderItemRepository;
        public SchedulerService(DeliveryContext context, IRepository<Scheduler> scheduleRepository, IRepository<DeliveryDetails> deliveryRepository
            , IRepository<Order> OrderRepository, IRepository<OrderItem> orderItemRepository, IRepository<DeliveryLog> deliverylogRepository)
        {
            _context = context;
            _scheduleRepository = scheduleRepository;
            _deliveryRepository = deliveryRepository;
            _OrderRepository = OrderRepository;
            _OrderItemRepository = orderItemRepository;
            _deliverylogRepository = deliverylogRepository;
        }

        public async Task<ResponseResult> GetAllAsync(int pageNumber, int pageSize)
        {
            var deliveries = await _scheduleRepository.GetAllAsync(pageNumber, pageSize);
            if (deliveries.Any())
            {
                return ResponseFactory.CreateSuccess("Scheduler retrieved successfully.", deliveries);
            }
            else
            {
                return ResponseFactory.CreateNotFound("No Scheduler found.");
            }
        }

        public async Task<ResponseResult> GetByIdAsync(int id)
        {
            var delivery = await _scheduleRepository.GetByIdAsync(id);
            if (delivery != null)
            {
                return ResponseFactory.CreateSuccess("Scheduler retrieved successfully.", delivery);
            }
            else
            {
                return ResponseFactory.CreateNotFound($"Scheduler with ID {id} not found.");
            }
        }
        public async Task<ResponseResult> Subscribe(SubScribeVM subscribe, int userId)
        {
            using var connection = _context.CreateConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                Scheduler objScheduler = new Scheduler()
                {
                    FromDate = subscribe.FromDate,
                    AssignedTo = subscribe.AssignedTo,
                    DeliverySlot = subscribe.DeliverySlot,
                    Note = subscribe.Note,
                    PeriodId = subscribe.PeriodId,
                    ToDate = subscribe.ToDate,

                };
                var schedulerId = await _scheduleRepository.InsertAsync(objScheduler, transaction);
                objScheduler.SchedulerId = schedulerId;
                Order objOrder = new Order()
                {
                    Total = subscribe.Total,
                    UserId = userId,
                    OrderStatusId = (short?)OrderDeliveryStatus.UpComming
                };
                var orderId = await _OrderRepository.InsertAsync(objOrder, transaction);
                objOrder.OrderId = orderId;
                List<OrderItem> objOrderItems = new List<OrderItem>();
                foreach (var item in subscribe.Items)
                {
                    objOrderItems.Add(new OrderItem()
                    {
                        ItemId = item.ItemId,
                        Quantity = item.Quantity,
                        OrderId = orderId
                    });
                }
                await _OrderItemRepository.InsertRangeAsync(objOrderItems, transaction);
                var listOfDeliveries = GenerateDeliveryRequests(subscribe, objOrder);
                if (listOfDeliveries.Count > 0)
                {
                    await _deliveryRepository.InsertRangeAsync(listOfDeliveries, transaction);

                    var insertedlistOfDeliveries = await _deliveryRepository.GetByAsync(new Dictionary<string, object>
                    {
                        { "ORDER_ID", objOrder.OrderId }
                    }, transaction);
                    List<DeliveryLog> logList = new List<DeliveryLog>();
                    foreach (var delivery in insertedlistOfDeliveries)
                    {
                        logList.Add(new DeliveryLog()
                        {
                            ActivityDate = DateTime.UtcNow,
                            ActivityDescription = "New subscription",
                            ActivityTypeId = (short)DeliveryActivityType.Adding,
                            DeliveryDetailsId = delivery.DeliveryDetailsId
                        });
                    }
                    await _deliverylogRepository.InsertRangeAsync(logList, transaction);
                }

                transaction.Commit();
                return ResponseFactory.CreateSuccess("Subscription added successfully.");
            }
            catch (Exception ex)
            {
                transaction?.Rollback();
                return ResponseFactory.CreateError(ex, System.Net.HttpStatusCode.FailedDependency);
            }
        }
        private List<DeliveryDetails> GenerateDeliveryRequests(SubScribeVM subscription, Order objOrder)
        {
            List<DeliveryDetails> deliveryRequests = new List<DeliveryDetails>();
            DateTime currentDate = subscription.FromDate;
            bool shouldContinue = true;

            while (currentDate <= subscription.ToDate && shouldContinue)
            {
                deliveryRequests.Add(new DeliveryDetails
                {
                    DeliveryDate = currentDate,
                    DeliveryStatus = (short)OrderDeliveryStatus.UpComming,
                    Confirmation = false,
                    OrderId = objOrder.OrderId
                });

                switch ((SubScriptionPeriod)subscription.PeriodId)
                {
                    case SubScriptionPeriod.None:
                        shouldContinue = false;
                        break;
                    case SubScriptionPeriod.Weekly:
                        currentDate = currentDate.AddDays(7);
                        break;
                    case SubScriptionPeriod.BiWeekly:
                        currentDate = currentDate.AddDays(14);
                        break;
                    case SubScriptionPeriod.Monthly:
                        currentDate = currentDate.AddMonths(1);
                        break;
                    case SubScriptionPeriod.BiMonthly:
                        currentDate = currentDate.AddMonths(2);
                        break;
                }
            }

            return deliveryRequests;
        }

        public async Task<ResponseResult> Subscribe_discard(SubScribeVM subscribe)
        {
            Scheduler objScheduler = new Scheduler()
            {
                FromDate = subscribe.FromDate,
                AssignedTo = subscribe.AssignedTo,
                DeliverySlot = subscribe.DeliverySlot,
                Note = subscribe.Note,
                PeriodId = subscribe.PeriodId,
                ToDate = subscribe.ToDate,
            };
            await _scheduleRepository.AddAsync(objScheduler);
            Order objOrder = new Order()
            {
                Total = subscribe.Total,
                OrderStatusId = (short?)OrderDeliveryStatus.UpComming
            };
            await _OrderRepository.AddAsync(objOrder);
            List<OrderItem> objOrderItems = new List<OrderItem>();
            foreach (var item in subscribe.Items)
            {
                objOrderItems.Add(new OrderItem()
                {
                    ItemId = item.ItemId,
                    Quantity = item.Quantity,
                    OrderId = objOrder.OrderId
                });
            }
            await _OrderItemRepository.AddRangeAsync(objOrderItems);

            return ResponseFactory.CreateSuccess("Scheduler added successfully.");
        }
        public async Task<ResponseResult> AddAsync(Scheduler delivery)
        {
            await _scheduleRepository.AddAsync(delivery);
            return ResponseFactory.CreateSuccess("Scheduler added successfully.");
        }

        public async Task<ResponseResult> UpdateAsync(Scheduler delivery)
        {
            await _scheduleRepository.UpdateAsync(delivery);
            return ResponseFactory.CreateSuccess("Scheduler updated successfully.");
        }

        public async Task<ResponseResult> DeleteAsync(int id)
        {
            var delivery = await _scheduleRepository.GetByIdAsync(id);
            if (delivery != null)
            {
                await _scheduleRepository.DeleteAsync(id);
                return ResponseFactory.CreateSuccess("Scheduler deleted successfully.");
            }
            else
            {
                return ResponseFactory.CreateNotFound($"Scheduler with ID {id} not found.");
            }

        }
    }

}
