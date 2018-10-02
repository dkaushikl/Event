namespace EventApi.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Event.Core;
    using Event.Repository.Interface;

    using EventApi.Utility;

    [Authorize]
    public class EventController : BaseController
    {
        private readonly IEventRepository eventRepository;

        public EventController(IEventRepository eventRepository) => this.eventRepository = eventRepository;

        [HttpPost]
        [Route("api/Event/InsertEvent")]
        public async Task<IHttpActionResult> AddEvent(EventViewModel objEventViewModel)
        {
            if (!this.ModelState.IsValid)
                return Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Enter All data!!",
           null));

            var objEvent = await this.eventRepository.GetEventByName(objEventViewModel.Name);

            if (objEvent)
                return Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Event already exist!!",
            null));

            return await this.eventRepository.AddEvent(objEventViewModel) ? Ok(ApiResponse.SetResponse(ApiResponseStatus.Ok, "Event added successfully!!",
             null)) : Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Event not found!!",
             null));
        }

        [HttpPost]
        [Route("api/Event/DeleteEvent")]
        public async Task<IHttpActionResult> DeleteEvent(Entity objEntity)
        {
            if (string.IsNullOrEmpty(Convert.ToString(objEntity.Id)))
                return Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Enter Valid Id!!",
            null));

            return await this.eventRepository.DeleteEvent(objEntity.Id) ? Ok(ApiResponse.SetResponse(ApiResponseStatus.Ok, "Event deleted successfully!!",
             null)) : Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Event not exists!!",
             null));
        }

        [HttpPost]
        [Route("api/Event/UpdateEvent")]
        public async Task<IHttpActionResult> EditEvent(EventViewModel objEventViewModel)
        {
            if (!this.ModelState.IsValid)
                return Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Enter All data!!",
           null));

            var objEvent = await this.eventRepository.GetEventByName(objEventViewModel.Name);

            if (objEvent)
                return Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Event already exist!!",
            null));

            return await this.eventRepository.EditEvent(objEventViewModel) ? Ok(ApiResponse.SetResponse(ApiResponseStatus.Ok, "Event updated successfully!!",
            null)) : Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Event not found!!",
            null));
        }

        [HttpGet]
        [Route("api/Event/GetAllEvent")]
        public async Task<IHttpActionResult> GetAllEvent(int pageIndex, int pageSize, int? eventId)
        {
            var objResult = await this.eventRepository.GetAllEvent(pageIndex, pageSize, Convert.ToInt32(this.UserId), eventId);

            var data = objResult.Columns.Count > 0
                           ? Utility.ConvertDataTable<EventViewModel>(objResult).ToList()
                           : null;

            return Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Get Data Successfully!!", data));
        }
    }
}