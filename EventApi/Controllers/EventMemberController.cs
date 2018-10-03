namespace EventApi.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Event.Core;
    using Event.Repository.Interface;

    using EventApi.Utility;

    public class EventMemberController : BaseController
    {
        private readonly IEventMemberRepository eventRepository;

        public EventMemberController(IEventMemberRepository eventRepository) => this.eventRepository = eventRepository;

        [Authorize]
        [HttpPost]
        [Route("api/EventMember/InsertEventMember")]
        public async Task<IHttpActionResult> AddEventMember(EventMemberViewModel objEventMemberViewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Enter All data!!", null));
            }

            var objEventMember = await this.eventRepository.CheckEventUserExist(
                                     Convert.ToInt32(this.UserId),
                                     objEventMemberViewModel.EventId);

            if (objEventMember)
            {
                return this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Event Member already exist!!", null));
            }

            return await this.eventRepository.AddEventMember(objEventMemberViewModel)
                       ? this.Ok(
                           ApiResponse.SetResponse(ApiResponseStatus.Ok, "Event Member added successfully!!", null))
                       : this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Something wen't wrong!!", null));
        }

        [Authorize]
        [HttpPost]
        [Route("api/EventMember/DeleteEventMember")]
        public async Task<IHttpActionResult> DeleteEventMember(Entity objEntity)
        {
            if (string.IsNullOrEmpty(Convert.ToString(objEntity.Id)))
            {
                return this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Enter Valid Id!!", null));
            }

            return await this.eventRepository.DeleteEventMember(objEntity.Id)
                       ? this.Ok(
                           ApiResponse.SetResponse(ApiResponseStatus.Ok, "Event Member deleted successfully!!", null))
                       : this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Event Member not exists!!", null));
        }

        [Authorize]
        [HttpPost]
        [Route("api/EventMember/UpdateEventMember")]
        public async Task<IHttpActionResult> EditEventMember(EventMemberViewModel objEventMemberViewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Enter All data!!", null));
            }

            var objEventMember = await this.eventRepository.CheckEventUserExist(
                                     Convert.ToInt32(this.UserId),
                                     objEventMemberViewModel.EventId);

            if (objEventMember)
            {
                return this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Event Member already exist!!", null));
            }

            return await this.eventRepository.EditEventMember(objEventMemberViewModel)
                       ? this.Ok(
                           ApiResponse.SetResponse(ApiResponseStatus.Ok, "Event Member updated successfully!!", null))
                       : this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Event Member not found!!", null));
        }

        [Authorize]
        [HttpGet]
        [Route("api/EventMember/GetAllEventMember")]
        public async Task<IHttpActionResult> GetAllEventMember(int pageIndex, int pageSize, int? eventId)
        {
            var objResult = await this.eventRepository.GetAllEventMember(
                                pageIndex,
                                pageSize,
                                Convert.ToInt32(this.UserId),
                                eventId);

            var data = objResult.Columns.Count > 0
                           ? Utility.ConvertDataTable<EventMemberViewModel>(objResult).ToList()
                           : null;

            return this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Get Data Successfully!!", data));
        }
    }
}