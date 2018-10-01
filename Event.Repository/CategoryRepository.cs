using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using VideoStatus.Core;
using VideoStatus.Data;
using VideoStatus.Repository.Interface;

namespace VideoStatus.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly SqlConnection _conn =
            new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        private readonly VideoStatusEntities _entities = new VideoStatusEntities();

        public async Task<List<CategoryViewModel>> GetAllCategory()
        {
            return await (from objCategory in _entities.Categories
                join objVideoCategory in _entities.VideoCategories on objCategory.Id equals objVideoCategory.CategoryId
                    into objVideo
                where objCategory.ValidFlag
                select new CategoryViewModel
                {
                    Id = objCategory.Id,
                    Name = objCategory.Name,
                    ValidFlag = objCategory.ValidFlag,
                    CreatedDate = objCategory.CreatedDate,
                    ModifiedDate = objCategory.ModifiedDate,
                    VideoCount = objVideo.Count()
                }).ToListAsync();
        }

        public async Task<CategoryViewModel> GetCategoryById(int id)
        {
            var objCategory = await _entities.Categories.FirstOrDefaultAsync(x => x.Id == id);

            return objCategory == null
                ? null
                : new CategoryViewModel
                {
                    Id = objCategory.Id,
                    Name = objCategory.Name,
                    ValidFlag = objCategory.ValidFlag,
                    CreatedDate = objCategory.CreatedDate,
                    ModifiedDate = objCategory.ModifiedDate
                };
        }

        public async Task<CategoryViewModel> GetCategoryByName(string name)
        {
            var objCategory = await _entities.Categories.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());

            return objCategory == null
                ? null
                : new CategoryViewModel
                {
                    Id = objCategory.Id,
                    Name = objCategory.Name,
                    ValidFlag = objCategory.ValidFlag,
                    CreatedDate = objCategory.CreatedDate,
                    ModifiedDate = objCategory.ModifiedDate
                };
        }

        public async Task<DataTable> GetAllCategoryUsingSp()
        {
            try
            {
                return await SqlHelper.ExecuteDataTableAsync(_conn, CommandType.StoredProcedure, "GetCategory");
            }
            finally
            {
                _conn.Close();
            }
        }

        public async Task<bool> AddCategory(CategoryViewModel objCategoryViewModel)
        {
            var objCategory = new Category
            {
                Id = objCategoryViewModel.Id,
                Name = objCategoryViewModel.Name,
                ValidFlag = objCategoryViewModel.ValidFlag,
                ModifiedDate = DateTime.Now,
                CreatedDate = DateTime.Now
            };

            _entities.Categories.Add(objCategory);
            await _entities.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EditCategory(CategoryViewModel objCategoryViewModel)
        {
            var objCategory = await _entities.Categories.FirstOrDefaultAsync(x => x.Id == objCategoryViewModel.Id);

            if (objCategory == null) return false;

            objCategory.Id = objCategoryViewModel.Id;
            objCategory.Name = objCategoryViewModel.Name;
            objCategory.ValidFlag = objCategoryViewModel.ValidFlag;
            objCategory.ModifiedDate = DateTime.Now;
            _entities.Entry(objCategory).State = EntityState.Modified;
            await _entities.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteCategory(int id)
        {
            var itemToDeactive = _entities.Categories.FirstOrDefault(x => x.Id == id);
            if (itemToDeactive == null) return false;
            _entities.Categories.Remove(itemToDeactive);
            await _entities.SaveChangesAsync();
            return true;
        }
    }
}