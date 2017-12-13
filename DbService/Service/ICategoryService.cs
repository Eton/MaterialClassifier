using DbService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbService.Service
{
    public interface ICategoryService
    {
        void InsertCategory(Category category);
        Category UpadteCategory(int categoryId, string mainCategory, string subCategory);
        void DeleteCategoryById(int Id);
        Category GetCategoryById(int Id);
        IList<CategoryModel> GetCategoryListByMainCategory(string mainCategory);
        /// <summary>
        /// 回傳相似的分類
        /// </summary>
        /// <param name="mainCategory"></param>
        /// <param name="subCategory"></param>
        /// <returns></returns>
        Category GetSimilarCategory(string mainCategory, string subCategory);
        IList<string> GetALLMainCategory();
    }
}
