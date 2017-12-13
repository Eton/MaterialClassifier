using DbService.Model;
using DbService.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaterialCollector.Controllers
{
    public class CategoryController : BaseController
    {
        // GET: Category
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetCategory(string mainCategory)
        {
            ICategoryService categoryService = new CategoryService();

            var categorys = categoryService.GetCategoryListByMainCategory(mainCategory)
                .Select(s =>
                {
                    return new { s.Id, s.MainCategory, s.SubCategory, Num = s.MaterialCount };
                });

            return ResponseJson("", categorys);
        }

        public ActionResult GetALLMainCategory()
        {
            ICategoryService categoryService = new CategoryService();
            var mainCategorys = categoryService.GetALLMainCategory();
            return ResponseJson("", mainCategorys);
        }

        [HttpPost]
        public ActionResult AddCategory(string mainCategory, string subCategory)
        {
            ICategoryService categoryService = new CategoryService();

            var similarCategory = categoryService.GetSimilarCategory(mainCategory, subCategory);
            if (similarCategory != null)
            {
                var message = "已有相似的分類,主分類 : " + similarCategory.MainCategory + " ,次分類 : " + similarCategory.SubCategory;
                return ResponseJson(message);
            }

            Category newCategory = new Category();
            newCategory.MainCategory = mainCategory;
            newCategory.SubCategory = subCategory;

            categoryService.InsertCategory(newCategory);

            return ResponseJson("新增成功");
        }
        [HttpPost]
        public ActionResult UpdateCategory(int categoryId, string mainCategory, string subCategory)
        {
            ICategoryService categoryService = new CategoryService();

            var similarCategory = categoryService.GetSimilarCategory(mainCategory, subCategory);
            if (similarCategory != null)
            {
                var message = "已有相似的分類,主分類 : " + similarCategory.MainCategory + " ,次分類 : " + similarCategory.SubCategory;
                return ResponseJson(message);
            }

            var category = categoryService.UpadteCategory(categoryId, mainCategory, subCategory);
            return ResponseJson("修改成功", new { category.MainCategory, category.SubCategory });
        }

        [HttpPost]
        public ActionResult DeleteCategory(int categoryId)
        {
            ICategoryService categoryService = new CategoryService();

            categoryService.DeleteCategoryById(categoryId);

            return ResponseJson("刪除成功");
        }
    }
}