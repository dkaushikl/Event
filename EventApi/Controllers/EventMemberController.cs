namespace EventApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Event.Core;
    using Event.Repository.Interface;

    using EventApi.Utility;

    [Authorize]
    public class EventMemberController : BaseController
    {
        private readonly IEventMemberRepository eventRepository;

        public EventMemberController(IEventMemberRepository eventRepository) => this.eventRepository = eventRepository;

        [HttpPost]
        [Route("api/EventMember/InsertEventMember")]
        public async Task<Message> AddEventMember(EventMemberViewModel objEventMemberViewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return new Message
                {
                    Description = "Enter All data",
                    Status = ResponseStatus.Error.ToString(),
                    IsSuccess = false
                };
            }

            var objEventMember = await this.eventRepository.CheckEventUserExist(
                                     Convert.ToInt32(this.UserId),
                                     objEventMemberViewModel.EventId);

            if (objEventMember)
            {
                return new Message
                {
                    Description = "EventMember already exist.",
                    Status = ResponseStatus.Error.ToString(),
                    IsSuccess = false
                };
            }

            return await this.eventRepository.AddEventMember(objEventMemberViewModel)
                       ? new Message
                       {
                           Description = "EventMember added successfully.",
                           Status = ResponseStatus.Ok.ToString(),
                           IsSuccess = true
                       }
                       : new Message
                       {
                           Description = "Something wen't wrong.",
                           Status = ResponseStatus.Error.ToString(),
                           IsSuccess = false
                       };
        }

        [HttpPost]
        [Route("api/EventMember/DeleteEventMember")]
        public async Task<Message> DeleteEventMember(Entity objEntity)
        {
            if (string.IsNullOrEmpty(Convert.ToString(objEntity.Id)))
            {
                return new Message
                {
                    Description = "Enter Valid Id.",
                    Status = ResponseStatus.Error.ToString(),
                    IsSuccess = false
                };
            }

            return await this.eventRepository.DeleteEventMember(objEntity.Id)
                       ? new Message
                       {
                           Description = "EventMember deleted successfully.",
                           Status = ResponseStatus.Ok.ToString(),
                           IsSuccess = true
                       }
                       : new Message
                       {
                           Description = "EventMember not exists.",
                           Status = ResponseStatus.Error.ToString(),
                           IsSuccess = false
                       };
        }

        [HttpPost]
        [Route("api/EventMember/UpdateEventMember")]
        public async Task<Message> EditEventMember(EventMemberViewModel objEventMemberViewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return new Message { Description = "Enter All data", Status = ResponseStatus.Error.ToString() };
            }

            var objEventMember = await this.eventRepository.CheckEventUserExist(
                                     Convert.ToInt32(this.UserId),
                                     objEventMemberViewModel.EventId);

            if (objEventMember)
            {
                return new Message
                {
                    Description = "EventMember already exist.",
                    Status = ResponseStatus.Error.ToString(),
                    IsSuccess = false
                };
            }

            return await this.eventRepository.EditEventMember(objEventMemberViewModel)
                       ? new Message
                       {
                           Description = "EventMember updated successfully.",
                           Status = ResponseStatus.Ok.ToString(),
                           IsSuccess = true
                       }
                       : new Message
                       {
                           Description = "EventMember not found.",
                           Status = ResponseStatus.Error.ToString(),
                           IsSuccess = false
                       };
        }

        [HttpGet]
        [Route("api/EventMember/GetAllEventMember")]
        public async Task<Response<List<EventMemberViewModel>>> GetAllEventMember(int pageIndex, int pageSize, int? eventId)
        {
            var objResult = await this.eventRepository.GetAllEventMember(pageIndex, pageSize, Convert.ToInt32(this.UserId), eventId);

            var data = objResult.Columns.Count > 0
                           ? Utility.ConvertDataTable<EventMemberViewModel>(objResult).ToList()
                           : null;

            return new Response<List<EventMemberViewModel>>
            {
                Data = data,
                Status = ResponseStatus.Ok.ToString(),
                IsSuccess = true,
                TotalCount = data?.Count ?? 0,
                Message = data != null && data.Count > 0
                                         ? "Get Data Successfully"
                                         : "Data not found"
            };
        }
    }
}