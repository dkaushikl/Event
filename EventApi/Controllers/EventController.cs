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
    public class EventController : BaseController
    {
        private readonly IEventRepository eventRepository;

        public EventController(IEventRepository eventRepository) => this.eventRepository = eventRepository;

        [HttpPost]
        [Route("api/Event/InsertEvent")]
        public async Task<Message> AddEvent(EventViewModel objEventViewModel)
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

            var objEvent = await this.eventRepository.GetEventByName(objEventViewModel.Name);

            if (objEvent)
            {
                return new Message
                           {
                               Description = "Event already exist.",
                               Status = ResponseStatus.Error.ToString(),
                               IsSuccess = false
                           };
            }

            return await this.eventRepository.AddEvent(objEventViewModel)
                       ? new Message
                             {
                                 Description = "Event added successfully.",
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
        [Route("api/Event/DeleteEvent")]
        public async Task<Message> DeleteEvent(Entity objEntity)
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

            return await this.eventRepository.DeleteEvent(objEntity.Id)
                       ? new Message
                             {
                                 Description = "Event deleted successfully.",
                                 Status = ResponseStatus.Ok.ToString(),
                                 IsSuccess = true
                             }
                       : new Message
                             {
                                 Description = "Event not exists.",
                                 Status = ResponseStatus.Error.ToString(),
                                 IsSuccess = false
                             };
        }

        [HttpPost]
        [Route("api/Event/UpdateEvent")]
        public async Task<Message> EditEvent(EventViewModel objEventViewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return new Message { Description = "Enter All data", Status = ResponseStatus.Error.ToString() };
            }

            var objEvent = await this.eventRepository.GetEventByName(objEventViewModel.Name);

            if (objEvent)
            {
                return new Message
                           {
                               Description = "Event already exist.",
                               Status = ResponseStatus.Error.ToString(),
                               IsSuccess = false
                           };
            }

            return await this.eventRepository.EditEvent(objEventViewModel)
                       ? new Message
                             {
                                 Description = "Event updated successfully.",
                                 Status = ResponseStatus.Ok.ToString(),
                                 IsSuccess = true
                             }
                       : new Message
                             {
                                 Description = "Event not found.",
                                 Status = ResponseStatus.Error.ToString(),
                                 IsSuccess = false
                             };
        }

        [HttpGet]
        [Route("api/Event/GetAllEvent")]
        public async Task<Response<List<EventViewModel>>> GetAllEvent(int pageIndex, int pageSize, int? eventId)
        {
            var userId = 1;
            var objResult = await this.eventRepository.GetAllEvent(pageIndex, pageSize, userId, eventId);
            var data = objResult.Columns.Count > 0
                           ? Utility.ConvertDataTable<EventViewModel>(objResult).ToList()
                           : null;

            return new Response<List<EventViewModel>>
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