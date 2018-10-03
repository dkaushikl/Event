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
        [HttpPost]
        [Route("api/CompanyMember/InsertCompanyMember")]
        public async Task<IHttpActionResult> AddCompanyMember(CompanyMemberViewModel objCompanyMemberViewModel)
        {
            IHttpActionResult returnResult;
            if (this.Validation(objCompanyMemberViewModel, out returnResult))
            {
                return returnResult;
            }

            var checkEmailExist = await this.userRepository.CheckEmailExist(objCompanyMemberViewModel.Email);

            if (!checkEmailExist)
            {
                return this.Ok(
                    ApiResponse.SetResponse(ApiResponseStatus.Error, "User not found. please register first!!", null));
            }

            var objCompanyMember = await this.companyMemberRepository.CheckUserExist(
                                       objCompanyMemberViewModel.CompanyId,
                                       objCompanyMemberViewModel.Email);

            if (objCompanyMember)
            {
                return this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "User already exist!!", null));
            }

            objCompanyMemberViewModel.UserId =
                await this.userRepository.GetUserIdByEmail(objCompanyMemberViewModel.Email);

            return await this.companyMemberRepository.AddCompanyMember(objCompanyMemberViewModel)
                       ? this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Ok, "Member added successfully!!", null))
                       : this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Something wen't wrong!!", null));
        }

        [Authorize]
        [HttpPost]
        [Route("api/CompanyMember/DeleteCompanyMember")]
        public async Task<IHttpActionResult> DeleteCompanyMember(Entity objEntity)
        {
            if (string.IsNullOrEmpty(Convert.ToString(objEntity.Id)))
            {
                return this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Enter Valid Id!!", null));
            }

            return await this.companyMemberRepository.DeleteCompanyMember(objEntity.Id)
                       ? this.Ok(
                           ApiResponse.SetResponse(ApiResponseStatus.Ok, "Company member deleted successfully!!", null))
                       : this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Member not exists!!", null));
        }

        [Authorize]
        [HttpPost]
        [Route("api/CompanyMember/UpdateCompanyMember")]
        public async Task<IHttpActionResult> EditCompanyMember(CompanyMemberViewModel objCompanyMemberViewModel)
        {
            IHttpActionResult returnResult;
            if (this.Validation(objCompanyMemberViewModel, out returnResult))
            {
                return returnResult;
            }

            var checkEmailExist = await this.userRepository.CheckEmailExist(objCompanyMemberViewModel.Email);

            if (!checkEmailExist)
            {
                return this.Ok(
                    ApiResponse.SetResponse(ApiResponseStatus.Error, "User not found. please register first!!", null));
            }

            var objCompanyMember = await this.companyMemberRepository.CheckUserExist(
                                       objCompanyMemberViewModel.CompanyId,
                                       objCompanyMemberViewModel.Email);

            if (objCompanyMember)
            {
                return this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Member already exist!!", null));
            }

            return await this.companyMemberRepository.EditCompanyMember(objCompanyMemberViewModel)
                       ? this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Ok, "Member updated successfully!!", null))
                       : this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Company Member not found!!", null));
        }

        [Authorize]
        [HttpGet]
        [Route("api/CompanyMember/GetAllCompanyMember")]
        public async Task<IHttpActionResult> GetAllCompanyMember(int pageIndex, int pageSize, int companyId)
        {
            var objResult = await this.companyMemberRepository.GetAllCompanyMember(
                                pageIndex,
                                pageSize,
                                companyId);

            var data = objResult.Columns.Count > 0
                           ? Utility.ConvertDataTable<CompanyMemberDisplayViewModel>(objResult).ToList()
                           : null;

            return this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Get Data Successfully!!", data));
        }

        private bool Validation(CompanyMemberViewModel objCompanyMemberViewModel, out IHttpActionResult returnResult)
        {
            if (objCompanyMemberViewModel.Email.IsEmpty())
            {
                returnResult = this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Enter email address", null));
                return true;
            }

            if (!objCompanyMemberViewModel.Email.IsEmail())
            {
                returnResult = this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Enter valid email", null));
                return true;
            }

            returnResult = null;
            return false;
        }
    }
}