using Core;
using Delivery.Services.Abstract;
using DeliveryApp.DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LookupController : ControllerBase
    {
        private readonly ILookupServiceFactory _factory;

        public LookupController(ILookupServiceFactory factory)
        {
            _factory = factory;
        }

        [HttpGet(nameof(GetItems))]
        public async Task<ResponseResult> GetItems()
        {
            var activityTypeService = _factory.Create<Item>();
            return await activityTypeService.GetAllAsync();
        }
        [HttpGet(nameof(GetAddressTypes))]
        public async Task<ResponseResult> GetAddressTypes()
        {
            var activityTypeService = _factory.Create<AddressType>();
            return await activityTypeService.GetAllAsync();
        }
        [HttpGet(nameof(GetPeriods))]
        public async Task<ResponseResult> GetPeriods()
        {
            var activityTypeService = _factory.Create<Period>();
            return await activityTypeService.GetAllAsync();
        }
        [HttpGet(nameof(GetDeliveryStatus))]
        public async Task<ResponseResult> GetDeliveryStatus()
        {
            var activityTypeService = _factory.Create<DeliveryStatus>();
            return await activityTypeService.GetAllAsync();
        }
        [HttpGet(nameof(GetUserTypes))]
        public async Task<ResponseResult> GetUserTypes()
        {
            var activityTypeService = _factory.Create<UserType>();
            return await activityTypeService.GetAllAsync();
        }
        [HttpGet(nameof(GetCouriers))]
        public async Task<ResponseResult> GetCouriers()
        {
            var activityTypeService = _factory.Create<User>();
            Dictionary<string, object> filter = new Dictionary<string, object>();
            filter.Add("USERTYPE_ID", 2);
            return await activityTypeService.GetByAsync(filter);
        }
    }
}
