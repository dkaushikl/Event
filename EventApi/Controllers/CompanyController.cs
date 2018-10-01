namespace EventApi.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Event.Core;
    using Event.Repository.Interface;

    using EventApi.Utility;

    [Authorize]
    public class CompanyController : ApiController
    {
        private readonly ICompanyRepository companyRepository;

        public CompanyController(ICompanyRepository companyRepository) =>
            this.companyRepository = companyRepository;

        [Route("api/Company")]
        public string[] Get() => new string[] { "value1", "value2" };

        [HttpPost]
        [Route("api/Company/InsertCompany")]
        public async Task<Message> AddCompany(CompanyViewModel objCompanyViewModel)
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

            var objCompany = await this.companyRepository.GetCompanyByName(objCompanyViewModel.Name);

            if (objCompany != null)
            {
                return new Message
                {
                    Description = "Company already exist.",
                    Status = ResponseStatus.Error.ToString(),
                    IsSuccess = false
                };
            }

            return await this.companyRepository.AddCompany(objCompanyViewModel)
                       ? new Message
                       {
                           Description = "Company added successfully.",
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
        [Route("api/Company/DeleteCompany")]
        public async Task<Message> DeleteCompany(Entity objEntity)
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

            return await this.companyRepository.DeleteCompany(objEntity.Id)
                       ? new Message
                       {
                           Description = "Company deleted successfully.",
                           Status = ResponseStatus.Ok.ToString(),
                           IsSuccess = true
                       }
                       : new Message
                       {
                           Description = "Company not exists.",
                           Status = ResponseStatus.Error.ToString(),
                           IsSuccess = false
                       };
        }

        [HttpPost]
        [Route("api/Company/UpdateCompany")]
        public async Task<Message> EditCompany(CompanyViewModel objCompanyViewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return new Message { Description = "Enter All data", Status = ResponseStatus.Error.ToString() };
            }

            var objCompany = await this.companyRepository.GetCompanyByName(objCompanyViewModel.Name);

            if (objCompany != null)
            {
                return new Message
                {
                    Description = "Company already exist.",
                    Status = ResponseStatus.Error.ToString(),
                    IsSuccess = false
                };
            }

            return await this.companyRepository.EditCompany(objCompanyViewModel)
                       ? new Message
                       {
                           Description = "Company updated successfully.",
                           Status = ResponseStatus.Ok.ToString(),
                           IsSuccess = true
                       }
                       : new Message
                       {
                           Description = "Company not found.",
                           Status = ResponseStatus.Error.ToString(),
                           IsSuccess = false
                       };
        }
    }
}