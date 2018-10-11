namespace EventApi.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Event.Core;
    using Event.Repository.Interface;

    using EventApi.Utility;

    public class EventController : BaseController
    {
        private readonly IEventRepository eventRepository;

        public EventController(IEventRepository eventRepository) => this.eventRepository = eventRepository;

        [Authorize]
        [HttpPost]
        [Route("api/Event/InsertEvent")]
        public async Task<IHttpActionResult> AddEvent(EventViewModel objEventViewModel)
        {
            IHttpActionResult returnResult;
            if (this.Validation(objEventViewModel, out returnResult))
            {
                return returnResult;
            }

            var objEvent = await this.eventRepository.GetEventByName(objEventViewModel.Name, 0);

            if (objEvent)
            {
                return this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Event already exist!!", null));
            }

            return await this.eventRepository.AddEvent(objEventViewModel)
                       ? this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Ok, "Event added successfully!!", null))
                       : this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Something wen't wrong!!", null));
        }

        [Authorize]
        [HttpPost]
        [Route("api/Event/DeleteEvent")]
        public async Task<IHttpActionResult> DeleteEvent(Entity objEntity)
        {
            if (string.IsNullOrEmpty(Convert.ToString(objEntity.Id)))
            {
                return this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Enter Valid Id!!", null));
            }

            return await this.eventRepository.DeleteEvent(objEntity.Id)
                       ? this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Ok, "Event deleted successfully!!", null))
                       : this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Event not exists!!", null));
        }

        [HttpPost]
        [Route("api/Event/UpdateEvent")]
        public async Task<IHttpActionResult> EditEvent(EventViewModel objEventViewModel)
        {
            IHttpActionResult returnResult;
            if (this.Validation(objEventViewModel, out returnResult))
            {
                return returnResult;
            }

            var objEvent = await this.eventRepository.GetEventByName(objEventViewModel.Name, objEventViewModel.Id);

            if (objEvent)
            {
                return this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Event already exist!!", null));
            }

            return await this.eventRepository.EditEvent(objEventViewModel)
                       ? this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Ok, "Event updated successfully!!", null))
                       : this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Something wen't wrong!!", null));
        }

        [HttpGet]
        [Route("api/Event/GetAllEvent")]
        public async Task<IHttpActionResult> GetAllEvent(int pageIndex, int pageSize, int? eventId)
        {
            var objResult = await this.eventRepository.GetAllEvent(
                                pageIndex,
                                pageSize,
                                Convert.ToInt32(this.UserId),
                                eventId);

            var data = objResult.Columns.Count > 0
                           ? Utility.ConvertDataTable<EventViewModel>(objResult).ToList()
                           : null;

            return this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Get Data Successfully!!", data));
        }

        private bool Validation(EventViewModel objEventViewModel, out IHttpActionResult returnResult)
        {
            if (objEventViewModel.Name.IsEmpty())
            {
                returnResult = this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Enter event name", null));
                return true;
            }

            if (objEventViewModel.Vanue.IsEmpty())
            {
                returnResult = this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Enter vanue", null));
                return true;
            }

            if (objEventViewModel.Description.IsEmpty())
            {
                returnResult = this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Enter description", null));
                return true;
            }

            if (objEventViewModel.StartDate.IsValidDateFormat())
            {
                returnResult = this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Enter description", null));
                return true;
            }

            if (objEventViewModel.StartTime.IsValidTimeFormat())
            {
                returnResult = this.Ok(
                    ApiResponse.SetResponse(ApiResponseStatus.Error, "Enter valid start time", null));
                return true;
            }

            if (objEventViewModel.EndDate.IsValidDateFormat())
            {
                returnResult = this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Enter description", null));
                return true;
            }

            if (objEventViewModel.EndTime.IsValidTimeFormat())
            {
                returnResult = this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Enter valid end time", null));
                return true;
            }

            returnResult = null;
            return false;
        }
    }
}