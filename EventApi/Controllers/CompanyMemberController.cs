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
    public class CompanyMemberController : BaseController
    {
        private readonly ICompanyMemberRepository companyMemberRepository;

        public CompanyMemberController(ICompanyMemberRepository companyMemberRepository) => this.companyMemberRepository = companyMemberRepository;

        [HttpPost]
        [Route("api/CompanyMember/InsertCompanyMember")]
        public async Task<Message> AddCompanyMember(CompanyMemberViewModel objCompanyMemberViewModel)
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

            var objCompanyMember = await this.companyMemberRepository.CheckUserExist(objCompanyMemberViewModel.CompanyId, objCompanyMemberViewModel.Email);

            if (objCompanyMember)
            {
                return new Message
                {
                    Description = "Already exist this user.",
                    Status = ResponseStatus.Error.ToString(),
                    IsSuccess = false
                };
            }

            return await this.companyMemberRepository.AddCompanyMember(objCompanyMemberViewModel)
                       ? new Message
                       {
                           Description = "CompanyMember added successfully.",
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
        [Route("api/CompanyMember/DeleteCompanyMember")]
        public async Task<Message> DeleteCompanyMember(Entity objEntity)
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

            return await this.companyMemberRepository.DeleteCompanyMember(objEntity.Id)
                       ? new Message
                       {
                           Description = "CompanyMember deleted successfully.",
                           Status = ResponseStatus.Ok.ToString(),
                           IsSuccess = true
                       }
                       : new Message
                       {
                           Description = "CompanyMember not exists.",
                           Status = ResponseStatus.Error.ToString(),
                           IsSuccess = false
                       };
        }

        [HttpPost]
        [Route("api/CompanyMember/UpdateCompanyMember")]
        public async Task<Message> EditCompanyMember(CompanyMemberViewModel objCompanyMemberViewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return new Message { Description = "Enter All data", Status = ResponseStatus.Error.ToString() };
            }

            var objCompanyMember = await this.companyMemberRepository.CheckUserExist(objCompanyMemberViewModel.CompanyId, objCompanyMemberViewModel.Email);

            if (objCompanyMember)
            {
                return new Message
                {
                    Description = "Already exist this user.",
                    Status = ResponseStatus.Error.ToString(),
                    IsSuccess = false
                };
            }

            return await this.companyMemberRepository.EditCompanyMember(objCompanyMemberViewModel)
                       ? new Message
                       {
                           Description = "CompanyMember updated successfully.",
                           Status = ResponseStatus.Ok.ToString(),
                           IsSuccess = true
                       }
                       : new Message
                       {
                           Description = "CompanyMember not found.",
                           Status = ResponseStatus.Error.ToString(),
                           IsSuccess = false
                       };
        }

        [HttpGet]
        [Route("api/CompanyMember/GetAllCompanyMember")]
        public async Task<Response<List<CompanyMemberViewModel>>> GetAllCompanyMember(int pageIndex, int pageSize, int? companyId)
        {
            var objResult = await this.companyMemberRepository.GetAllCompanyMember(pageIndex, pageSize, Convert.ToInt32(this.UserId), companyId);

            var data = objResult.Columns.Count > 0
                           ? Utility.ConvertDataTable<CompanyMemberViewModel>(objResult).ToList()
                           : null;

            return new Response<List<CompanyMemberViewModel>>
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