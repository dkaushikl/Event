using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using VideoStatus.Core;

namespace VideoStatus.Repository.Interface
{
    public interface ICategoryRepository
    {
        Task<List<CategoryViewModel>> GetAllCategory();
        Task<CategoryViewModel> GetCategoryById(int id);
        Task<CategoryViewModel> GetCategoryByName(string name);
        Task<bool> AddCategory(CategoryViewModel objCategoryViewModel);
        Task<bool> EditCategory(CategoryViewModel objCategoryViewModel);
        Task<bool> DeleteCategory(int id);
        Task<DataTable> GetAllCategoryUsingSp();
    }
}