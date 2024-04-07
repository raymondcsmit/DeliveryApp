using Core;
using Delivery.Services.Abstract;
using DeliveryApp.DAL.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DeliveriesController : BaseController
    {
        private readonly IDeliveryService _deliveryService;

        public DeliveriesController(IDeliveryService deliveryService)
        {
            _deliveryService = deliveryService;
        }

        [HttpGet(nameof(GetAllDeliveries))]
        public async Task<ResponseResult> GetAllDeliveries(int pageNumber = 1, int pageSize = 10)
        {
            return await _deliveryService.GetAllDeliveriesAsync(pageNumber, pageSize);

        }
        [HttpGet(nameof(GetUpcoming))]
        public async Task<ResponseResult> GetUpcoming(int count = 3)
        {
            int userId = GetUserID();
            return await _deliveryService.GetUpcoming(count, userId);

        }
        [HttpGet(nameof(GetPast))]
        public async Task<ResponseResult> GetPast(int count = 3)
        {
            int userId = GetUserID();
            return await _deliveryService.GetPast(count, userId);

        }

        [HttpGet(nameof(GetDeliveryById))]
        //[HttpGet("{id}")]
        public async Task<ResponseResult> GetDeliveryById(int id)
        {
            return await _deliveryService.GetDeliveryByIdAsync(id);

        }

        [HttpPost(nameof(AddDelivery))]
        public async Task<ResponseResult> AddDelivery([FromBody] DeliveryDetails delivery)
        {
            if (!ModelState.IsValid)
            {
                return ResponseFactory.CreateBadRequest("Model invalid");
            }
            return await _deliveryService.AddDeliveryAsync(delivery);

        }

        //[HttpPut("{id}")]
        [HttpPut(nameof(UpdateDelivery))]
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

        //[HttpDelete("{id}")]
        [HttpDelete(nameof(DeleteDelivery))]
        public async Task<ResponseResult> DeleteDelivery(int id)
        {

            return await _deliveryService.DeleteDeliveryAsync(id);
        }
    }
}
