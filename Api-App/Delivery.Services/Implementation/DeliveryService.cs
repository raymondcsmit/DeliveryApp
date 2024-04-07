using Core;
using Delivery.Services.Abstract;
using Delivery.ViewModel;
using DeliveryApp.DAL.Entities;
using Microsoft.Extensions.Options;

namespace Delivery.Services.Implementation
{
    public class DeliveryService : IDeliveryService
    {
        private readonly IRepository<DeliveryDetails> _deliveryRepository;
        private readonly IOptions<RepositoryOptions> _options;
        public DeliveryService(IRepository<DeliveryDetails> deliveryRepository, IOptions<RepositoryOptions> options)
        {
            _deliveryRepository = deliveryRepository;
            _options = options;
        }
        public async Task<ResponseResult> GetUpcoming(int count, int userId)
        {
            string deliveryTableName;
            _options.Value.TableNames.TryGetValue(typeof(DeliveryDetails).Name, out deliveryTableName);
            string orderTableName;
            _options.Value.TableNames.TryGetValue(typeof(Order).Name, out orderTableName);

            var query = $@"SELECT  Top(@Count)
                            o.ORDER_ID as OrderId,
                            dd.DELIVERY_DATE as DeliveryDate,
                            da.STREET as Street,
                            da.ADDRESS as Address,
                            u.USERNAME as CourierName,
                            i.NAME as ItemName,
                            i.ITEMS_ID as ItemId,
                            oi.QUANTITY as Quantity
                        FROM 
                            [Deliveries].[TBL_ORDERS] o
                        INNER JOIN 
                            [Deliveries].[TBL_DELIVERY_DETAILS] dd ON o.ORDER_ID = dd.ORDER_ID
                        INNER JOIN 
                            [Deliveries].[TBL_DELIVERY_ADDRESS] da ON dd.DELIVERY_DETAILS_ID = da.DELIVERY_DETAILS_ID
                        INNER JOIN 
                            [Deliveries].[TBL_ORDER_ITEMS] oi ON o.ORDER_ID = oi.ORDER_ID
                        INNER JOIN 
                            [Deliveries].[TBL_ITEMS] i ON oi.ITEM_ID = i.ITEMS_ID
                        LEFT JOIN 
                            [Schedule].[TBL_SCHEDULER] s ON o.ORDER_ID = s.ASSIGNED_TO
                        LEFT JOIN 
                            [Person].[TBL_USERS] u ON s.ASSIGNED_TO = u.USER_ID
                        WHERE 
                           
                            dd.DELIVERY_STATUS = @DeliveryStatus
                       
                             ";

            //o.ORDER_ID = @OrderId     AND
            // Assuming TKey is of type int, adjust TKey if your key property is of a different type
            Func<DeliveryViewModel, ItemViewModel, int, DeliveryViewModel> mapFunction = (delivery, item, key) =>
             {
                 delivery.Items = delivery.Items ?? new List<ItemViewModel>();
                 delivery.Items.Add(item);
                 return delivery;
             };


            var deliveries = await _deliveryRepository.ExecuteQueryMultiMappingAsync<DeliveryViewModel, ItemViewModel, int>(
                query,
                mapFunction,
                new { UserId = userId, DeliveryStatus = OrderDeliveryStatus.UpComming, Count = count },
                "ItemId", // The column name in your SQL result set to split on.
                "OrderId"  // The key property name of the TFirst type.
            );


            if (deliveries.Any())
            {
                return ResponseFactory.CreateSuccess("Deliveries retrieved successfully.", deliveries);
            }
            else
            {
                return ResponseFactory.CreateNotFound("No deliveries found.");
            }
        }
        public async Task<ResponseResult> GetUpcoming1(int count, int userId)
        {
            string deliveryTableName;
            _options.Value.TableNames.TryGetValue(typeof(DeliveryDetails).Name, out deliveryTableName);
            string orderTableName;
            _options.Value.TableNames.TryGetValue(typeof(Order).Name, out orderTableName);

            var query = $@"
                            SELECT Top(@Count) dt.*
                                FROM {deliveryTableName} dt
                                JOIN {orderTableName} ort ON dt.Order_ID = ort.Order_ID
                                WHERE ort.USER_ID = @UserId and dt.DELIVERY_STATUS=@DeliveryStatus
                                Order by dt.DELIVERY_DATE                        
                             ";

            var deliveries = await _deliveryRepository.ExecuteQueryAsync<DeliveryDetails>(query, new { Count = count, UserId = userId, DeliveryStatus = OrderDeliveryStatus.UpComming });
            if (deliveries.Any())
            {
                return ResponseFactory.CreateSuccess("Deliveries retrieved successfully.", deliveries);
            }
            else
            {
                return ResponseFactory.CreateNotFound("No deliveries found.");
            }
        }
        public async Task<ResponseResult> GetPast(int count, int userId)
        {
            string deliveryTableName;
            _options.Value.TableNames.TryGetValue(typeof(DeliveryDetails).Name, out deliveryTableName);
            string orderTableName;
            _options.Value.TableNames.TryGetValue(typeof(Order).Name, out orderTableName);

            var query = $@"
                            SELECT Top(@Count) dt.*
                                FROM {deliveryTableName} dt
                                JOIN {orderTableName} ort ON dt.Order_ID = ort.Order_ID
                                WHERE ort.USER_ID = @UserId and dt.DELIVERY_STATUS=@DeliveryStatus
                                Order by dt.DELIVERY_DATE                        
                             ";

            var deliveries = await _deliveryRepository.ExecuteQueryAsync<DeliveryDetails>(query, new { Count = count, UserId = userId, DeliveryStatus = OrderDeliveryStatus.Delivered });
            if (deliveries.Any())
            {
                return ResponseFactory.CreateSuccess("Deliveries retrieved successfully.", deliveries);
            }
            else
            {
                return ResponseFactory.CreateNotFound("No deliveries found.");
            }
        }
        public async Task<ResponseResult> GetAllDeliveriesAsync(int pageNumber, int pageSize)
        {
            var deliveries = await _deliveryRepository.GetAllAsync(pageNumber, pageSize);
            if (deliveries.Any())
            {
                return ResponseFactory.CreateSuccess("Deliveries retrieved successfully.", deliveries);
            }
            else
            {
                return ResponseFactory.CreateNotFound("No deliveries found.");
            }
        }

        public async Task<ResponseResult> GetDeliveryByIdAsync(int id)
        {
            var delivery = await _deliveryRepository.GetByIdAsync(id);
            if (delivery != null)
            {
                return ResponseFactory.CreateSuccess("Delivery retrieved successfully.", delivery);
            }
            else
            {
                return ResponseFactory.CreateNotFound($"Delivery with ID {id} not found.");
            }
        }

        public async Task<ResponseResult> AddDeliveryAsync(DeliveryDetails delivery)
        {
            await _deliveryRepository.AddAsync(delivery);
            return ResponseFactory.CreateSuccess("Delivery added successfully.");
        }

        public async Task<ResponseResult> UpdateDeliveryAsync(DeliveryDetails delivery)
        {
            await _deliveryRepository.UpdateAsync(delivery);
            return ResponseFactory.CreateSuccess("Delivery updated successfully.");
        }

        public async Task<ResponseResult> DeleteDeliveryAsync(int id)
        {
            var delivery = await _deliveryRepository.GetByIdAsync(id);
            if (delivery != null)
            {
                await _deliveryRepository.DeleteAsync(id);
                return ResponseFactory.CreateSuccess("Delivery deleted successfully.");
            }
            else
            {
                return ResponseFactory.CreateNotFound($"Delivery with ID {id} not found.");
            }

        }
    }

}
