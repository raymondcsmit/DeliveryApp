using Core;
using Delivery.Services.Abstract;
using DeliveryApp.DAL.Entities;

namespace Delivery.Services.Implementation
{
    public class DeliveryService : IDeliveryService
    {
        private readonly IRepository<DeliveryDetails> _deliveryRepository;

        public DeliveryService(IRepository<DeliveryDetails> deliveryRepository)
        {
            _deliveryRepository = deliveryRepository;
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
