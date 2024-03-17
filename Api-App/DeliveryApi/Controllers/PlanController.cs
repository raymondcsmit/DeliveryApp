using Core;
using Delivery.Services.Abstract;
using Delivery.ViewModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PlanController : BaseController
    {
        private readonly ISchedulerService _SubscribeService;

        public PlanController(ISchedulerService SubscribeService)
        {
            _SubscribeService = SubscribeService;
        }
        [HttpPost(nameof(SubScribe))]
        public async Task<ResponseResult> SubScribe([FromBody] SubScribeVM subscribe)
        {
            if (!ModelState.IsValid)
            {
                return ResponseFactory.CreateBadRequest("Model invalid");
            }
            int userId = GetUserID();
            if (userId == 0) { return ResponseFactory.CreateBadRequest("Invalid User"); }
            return await _SubscribeService.Subscribe(subscribe, userId);

        }

        //[HttpGet(nameof(GetItems))]
        //public async Task<ResponseResult> GetItems()
        //{
        //    var activityTypeService = _factory.Create<Item>();
        //    return await activityTypeService.GetAllAsync();
        //}

    }
}
