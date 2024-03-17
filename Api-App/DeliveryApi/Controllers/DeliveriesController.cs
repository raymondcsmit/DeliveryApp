using Core;
using Delivery.Services.Abstract;
using DeliveryApp.DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveriesController : ControllerBase
    {
        private readonly IDeliveryService _deliveryService;

        public DeliveriesController(IDeliveryService deliveryService)
        {
            _deliveryService = deliveryService;
        }

        [HttpGet]
        public async Task<ResponseResult> GetAllDeliveries(int pageNumber = 1, int pageSize = 10)
        {
            return await _deliveryService.GetAllDeliveriesAsync(pageNumber, pageSize);

        }

        [HttpGet("{id}")]
        public async Task<ResponseResult> GetDeliveryById(int id)
        {
            return await _deliveryService.GetDeliveryByIdAsync(id);

        }

        [HttpPost]
        public async Task<ResponseResult> AddDelivery([FromBody] DeliveryDetails delivery)
        {
            if (!ModelState.IsValid)
            {
                return ResponseFactory.CreateBadRequest("Model invalid");
            }
            return await _deliveryService.AddDeliveryAsync(delivery);

        }

        [HttpPut("{id}")]
        public async Task<ResponseResult> UpdateDelivery(int id, [FromBody] DeliveryDetails delivery)
        {
            if (id != delivery.DeliveryDetailsId)
            {
                return ResponseFactory.CreateBadRequest("ID mismatch");
            }

            if (!ModelState.IsValid)
            {
                return ResponseFactory.CreateBadRequest("Model invalid");
            }

            return await _deliveryService.UpdateDeliveryAsync(delivery);

        }

        [HttpDelete("{id}")]
        public async Task<ResponseResult> DeleteDelivery(int id)
        {

            return await _deliveryService.DeleteDeliveryAsync(id);
        }
    }
}
