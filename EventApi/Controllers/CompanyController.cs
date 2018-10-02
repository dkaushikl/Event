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
    public class CompanyController : BaseController
    {
        private readonly ICompanyRepository companyRepository;

        public CompanyController(ICompanyRepository companyRepository) => this.companyRepository = companyRepository;

        [Authorize]
        [HttpGet]
        [Route("api/Company/GetAllCompany")]
        public async Task<IHttpActionResult> GetAllCompany(int pageIndex, int pageSize, int? companyId)
        {
            var objResult = await this.companyRepository.GetAllCompany(pageIndex, pageSize, Convert.ToInt32(this.UserId), companyId);

            var data = objResult.Columns.Count > 0
                           ? Utility.ConvertDataTable<CompanyDisplayViewModel>(objResult).ToList()
                           : null;

            return Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Get Data Successfully!!", data));
        }

        [Authorize]
        [HttpPost]
        [Route("api/Company/InsertCompany")]
        public async Task<IHttpActionResult> AddCompany(CompanyViewModel objCompanyViewModel)
        {
            if (!this.ModelState.IsValid)
                return Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Enter All data!!",
              null));

            var objCompany = await this.companyRepository.GetCompanyByName(objCompanyViewModel.Name, 0);

            if (objCompany)
                return Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Company already exist!!", null));

            objCompanyViewModel.CreatedBy = Convert.ToInt64(this.UserId);

            return await this.companyRepository.AddCompany(objCompanyViewModel) ? Ok(ApiResponse.SetResponse(ApiResponseStatus.Ok, "Company added successfully!!",
            null)) : Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Company not found!!",
            null));
        }

        [HttpPost]
        [Route("api/Company/UpdateCompany")]
        public async Task<IHttpActionResult> EditCompany(CompanyViewModel objCompanyViewModel)
        {
            if (!this.ModelState.IsValid)
                return Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Enter All data!!",
           null));

            var objCompany = await this.companyRepository.GetCompanyByName(objCompanyViewModel.Name, objCompanyViewModel.Id);

            if (objCompany)
                return Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Company already exist!!",
             null));

            objCompanyViewModel.CreatedBy = Convert.ToInt64(this.UserId);

            return await this.companyRepository.EditCompany(objCompanyViewModel) ? Ok(ApiResponse.SetResponse(ApiResponseStatus.Ok, "Company updated successfully!!",
             null)) : Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Company not found!!",
             null));
        }

        [Authorize]
        [HttpPost]
        [Route("api/Company/DeleteCompany")]
        public async Task<IHttpActionResult> DeleteCompany(Entity objEntity)
        {
            if (string.IsNullOrEmpty(Convert.ToString(objEntity.Id)))
                return Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Enter Valid Id!!",
             null));
            

            return await this.companyRepository.DeleteCompany(objEntity.Id) ? Ok(ApiResponse.SetResponse(ApiResponseStatus.Ok, "Company deleted successfully!!",
             null)) : Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Company not exists!!",
             null));
        }
    }
}