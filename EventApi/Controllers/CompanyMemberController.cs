namespace EventApi.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Event.Core;
    using Event.Repository.Interface;

    using EventApi.Utility;

    public class CompanyMemberController : BaseController
    {
        private readonly ICompanyMemberRepository companyMemberRepository;
        private readonly IUserRepository userRepository;

        public CompanyMemberController(ICompanyMemberRepository companyMemberRepository, IUserRepository userRepository)
        {
            this.companyMemberRepository = companyMemberRepository;
            this.userRepository = userRepository;
        }

        [Authorize]
        [HttpGet]
        [Route("api/CompanyMember/GetAllCompanyMember")]
        public async Task<IHttpActionResult> GetAllCompanyMember(int pageIndex, int pageSize, int? companyId)
        {
            var objResult = await this.companyMemberRepository.GetAllCompanyMember(pageIndex, pageSize, Convert.ToInt32(this.UserId), companyId);

            var data = objResult.Columns.Count > 0
                           ? Utility.ConvertDataTable<CompanyMemberDisplayViewModel>(objResult).ToList()
                           : null;

            return Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Get Data Successfully!!", data));
        }

        [Authorize]
        [HttpPost]
        [Route("api/CompanyMember/InsertCompanyMember")]
        public async Task<IHttpActionResult> AddCompanyMember(CompanyMemberViewModel objCompanyMemberViewModel)
        {
            if (!this.ModelState.IsValid)
                return Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Enter All data!!",
                     null));

            var checkEmailExist = await this.userRepository.CheckEmailExist(objCompanyMemberViewModel.Email);

            if (!checkEmailExist)
                return Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "User not found. please register first!!", null));

            var objCompanyMember = await this.companyMemberRepository.CheckUserExist(objCompanyMemberViewModel.CompanyId, objCompanyMemberViewModel.Email);

            if (objCompanyMember)
                return Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "User already exist!!", null));

            objCompanyMemberViewModel.UserId = await this.userRepository.GetUserIdByEmail(objCompanyMemberViewModel.Email);

            return await this.companyMemberRepository.AddCompanyMember(objCompanyMemberViewModel) ? Ok(ApiResponse.SetResponse(ApiResponseStatus.Ok, "Member added successfully!!",
           null)) : Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Something wen't wrong!!",
           null));
        }

        [Authorize]
        [HttpPost]
        [Route("api/CompanyMember/UpdateCompanyMember")]
        public async Task<IHttpActionResult> EditCompanyMember(CompanyMemberViewModel objCompanyMemberViewModel)
        {
            if (!this.ModelState.IsValid)
                return Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Enter All data!!",
          null));

            var checkEmailExist = await this.userRepository.CheckEmailExist(objCompanyMemberViewModel.Email);

            if (!checkEmailExist)
                return Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "User not found. please register first!!", null));

            var objCompanyMember = await this.companyMemberRepository.CheckUserExist(objCompanyMemberViewModel.CompanyId, objCompanyMemberViewModel.Email);

            if (objCompanyMember)
                return Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Member already exist!!",
               null));

            return await this.companyMemberRepository.EditCompanyMember(objCompanyMemberViewModel) ? Ok(ApiResponse.SetResponse(ApiResponseStatus.Ok, "Member updated successfully!!",
             null)) : Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Company Member not found!!",
             null));
        }

        [Authorize]
        [HttpPost]
        [Route("api/CompanyMember/DeleteCompanyMember")]
        public async Task<IHttpActionResult> DeleteCompanyMember(Entity objEntity)
        {
            if (string.IsNullOrEmpty(Convert.ToString(objEntity.Id)))
                return Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Enter Valid Id!!",
                null));

            return await this.companyMemberRepository.DeleteCompanyMember(objEntity.Id) ? Ok(ApiResponse.SetResponse(ApiResponseStatus.Ok, "Company member deleted successfully!!",
             null)) : Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Member not exists!!",
             null));
        }
    }
}