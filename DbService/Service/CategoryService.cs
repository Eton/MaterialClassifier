using DbService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DbService.Service
{
    public class CategoryService : ICategoryService
    {        
        public void InsertCategory(Category category)
        {
            using (DatabaseEntities entities = new DatabaseEntities())
            {
                entities.Category.Add(category);
                entities.SaveChanges();
            }
        }

        public Category UpadteCategory(int categoryId, string mainCategory, string subCategory)
        {
            using (DatabaseEntities entities = new DatabaseEntities())
            {
                var old = entities.Category.FirstOrDefault(f => f.Id == categoryId);
                if (old == null)
                    return null;

                old.MainCategory = mainCategory;
                old.SubCategory = subCategory;
                entities.SaveChanges();
                return old;
            }
        }

        public void DeleteCategoryById(int Id)
        {            
            using (DatabaseEntities entities = new DatabaseEntities())
            {
                var deleteEntity = new Category() { Id = Id };

                entities.Category.Attach(deleteEntity);
                entities.Category.Remove(deleteEntity);
                entities.SaveChanges();
            }
        }

        public Category GetCategoryById(int Id)
        {
            Category category = null;
            using (DatabaseEntities entities = new DatabaseEntities())
            {
                category = entities.Category.FirstOrDefault(f => f.Id == Id);
            }

            return category;
        }

        public IList<CategoryModel> GetCategoryListByMainCategory(string mainCategory)
        {
            using (DatabaseEntities entities = new DatabaseEntities())
            {
                var query = entities.Category.AsQueryable();

                if (!string.IsNullOrEmpty(mainCategory))
                    query = query.Where(c => c.MainCategory == mainCategory);

                var category = from c in query.OrderBy(o => o.MainCategory)
                               select new
                               {
                                   c.Id,c.MainCategory, c.SubCategory, c.Category_Material.Count
                               };

                return category.ToList().Select(c =>
                {
                    CategoryModel model = new CategoryModel();
                    model.Id = c.Id;
                    model.MainCategory = c.MainCategory;
                    model.SubCategory = c.SubCategory;
                    model.MaterialCount = c.Count;
                    return model;
                }).ToList();
            }
        }        

        public Category GetSimilarCategory(string mainCategory, string subCategory)
        {
            using (DatabaseEntities entities = new DatabaseEntities())
            {
                return entities.Category.Where(c => (c.MainCategory.Contains(mainCategory) || mainCategory.Contains(c.MainCategory)) &&
                (c.SubCategory.Contains(subCategory) || subCategory.Contains(c.SubCategory))).FirstOrDefault();
            }
        }

        public IList<string> GetALLMainCategory()
        {
            using (DatabaseEntities entities = new DatabaseEntities())
            {
                return entities.Category.Select(c => c.MainCategory).Distinct().ToList();
            }
        }
    }
}
