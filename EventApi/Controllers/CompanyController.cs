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
    public class CompanyController : BaseController
    {
        private readonly ICompanyRepository companyRepository;

        public CompanyController(ICompanyRepository companyRepository) => this.companyRepository = companyRepository;

        [Authorize]
        [HttpPost]
        [Route("api/Company/InsertCompany")]
        public async Task<IHttpActionResult> AddCompany(CompanyViewModel objCompanyViewModel)
        {
            IHttpActionResult returnResult;
            if (this.Validation(objCompanyViewModel, out returnResult))
            {
                return returnResult;
            }

            var objCompany = await this.companyRepository.GetCompanyByName(objCompanyViewModel.Name, 0);

            if (objCompany)
            {
                return this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Company already exist!!", null));
            }

            objCompanyViewModel.CreatedBy = Convert.ToInt64(this.UserId);

            return await this.companyRepository.AddCompany(objCompanyViewModel)
                       ? this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Ok, "Company added successfully!!", null))
                       : this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Company not found!!", null));
        }

        [Authorize]
        [HttpPost]
        [Route("api/Company/DeleteCompany")]
        public async Task<IHttpActionResult> DeleteCompany(Entity objEntity)
        {
            if (string.IsNullOrEmpty(Convert.ToString(objEntity.Id)))
            {
                return this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Enter Valid Id!!", null));
            }

            return await this.companyRepository.DeleteCompany(objEntity.Id)
                       ? this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Ok, "Company deleted successfully!!", null))
                       : this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Company not exists!!", null));
        }

        [HttpPost]
        [Route("api/Company/UpdateCompany")]
        public async Task<IHttpActionResult> EditCompany(CompanyViewModel objCompanyViewModel)
        {
            IHttpActionResult returnResult;
            if (this.Validation(objCompanyViewModel, out returnResult))
            {
                return returnResult;
            }

            var objCompany = await this.companyRepository.GetCompanyByName(
                                 objCompanyViewModel.Name,
                                 objCompanyViewModel.Id);

            if (objCompany)
            {
                return this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Company already exist!!", null));
            }

            objCompanyViewModel.CreatedBy = Convert.ToInt64(this.UserId);

            return await this.companyRepository.EditCompany(objCompanyViewModel)
                       ? this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Ok, "Company updated successfully!!", null))
                       : this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "You have not authorized to change company detail!!", null));
        }

        [Authorize]
        [HttpGet]
        [Route("api/Company/GetAllCompany")]
        public async Task<IHttpActionResult> GetAllCompany(int pageSize, int pageIndex)
        {
            var objResult = await this.companyRepository.GetAllCompany(
                                pageIndex,
                                pageSize,
                                Convert.ToInt32(this.UserId));

            var data = objResult.Columns.Count > 0
                           ? Utility.ConvertDataTable<CompanyDisplayViewModel>(objResult).ToList()
                           : null;

            var maximumPage = await this.companyRepository.GetMaximumPage(Convert.ToInt32(this.UserId));

            var response = new { data, page = maximumPage };

            return this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Get Data Successfully!!", response));
        }

        [Authorize]
        [HttpGet]
        [Route("api/Company/GetMaximumPage")]
        public async Task<IHttpActionResult> GetMaximumPage()
        {
            var maximumPage = await this.companyRepository.GetMaximumPage(Convert.ToInt32(this.UserId));

            return this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Get Data Successfully!!", maximumPage));
        }

        private bool Validation(CompanyViewModel objCompanyViewModel, out IHttpActionResult returnResult)
        {
            if (objCompanyViewModel.Name.IsEmpty())
            {
                returnResult = this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Enter company name", null));
                return true;
            }

            if (objCompanyViewModel.Email.IsEmpty())
            {
                returnResult = this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Enter email address", null));
                return true;
            }

            if (!objCompanyViewModel.Email.IsEmail())
            {
                returnResult = this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Enter valid email", null));
                return true;
            }

            if (objCompanyViewModel.MobileNo.IsEmpty())
            {
                returnResult = this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Enter mobile no", null));
                return true;
            }

            if (!objCompanyViewModel.MobileNo.IsMobileNo())
            {
                returnResult = this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Enter valid mobile no", null));
                return true;
            }

            returnResult = null;
            return false;
        }
    }
}