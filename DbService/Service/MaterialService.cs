using DbService.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbService.Extension;

namespace DbService.Service
{
    public class MaterialService : IMaterialService
    {
        public void InsertMaterial(string content, string comment, int[] categoryId)
        {
            using (DatabaseEntities entities = new DatabaseEntities())
            {                                
                Material material = null;
                material = entities.Material.Where(m => m.Url == content).FirstOrDefault();
                if(material == null)
                {
                    material = new Material();
                    material.Url = content;
                    material.Comment = comment;
                    entities.Material.Add(material);
                    entities.SaveChanges();
                }                
                
                foreach(var Id in categoryId)
                {
                    // 不允許一樣素材且一樣分類的情形
                    if (material.Category_Material.Any(cm => cm.MaterialId == material.Id && cm.CategoryId == Id))
                        continue;

                    var newCM = new Category_Material();
                    newCM.CategoryId = Id;
                    newCM.MaterialId = material.Id;
                    material.Category_Material.Add(newCM);                    
                }

                entities.SaveChanges();
            }
        }

        public void UpadteMaterial(Material material)
        {
            using (DatabaseEntities entities = new DatabaseEntities())
            {
                var old = entities.Material.FirstOrDefault(m => m.Id == material.Id);
                if (old == null)
                    return;

                old.Url = material.Url;
                entities.SaveChanges();
            }
        }

        public void DeleteMaterialById(int Id)
        {
            using (DatabaseEntities entities = new DatabaseEntities())
            {
                var deleteEntity = new Material() { Id = Id };

                entities.Material.Attach(deleteEntity);
                entities.Material.Remove(deleteEntity);
                entities.SaveChanges();
            }
        }

        public Material GetMaterialById(int Id)
        {
            Material material = null;
            using (DatabaseEntities entities = new DatabaseEntities())
            {
                // 載入單一素材相關note
                material = entities.Material.Where(m => m.Id == Id)
                    .Include(m => m.MaterialNote).FirstOrDefault();
            }

            return material;
        }

        

        public IList<MaterialMdoel> GetMaterials(int? CategoryId, string[] orderbyMember, string[] orderby, int page, int count)
        {
            using (DatabaseEntities entities = new DatabaseEntities())
            {
                var query = entities.Material.AsQueryable();
                if (CategoryId.HasValue)
                {
                    query = query.Where(m => m.Category_Material.Any(cm => cm.CategoryId == CategoryId.Value));
                }

                var materials = from m in query.OrderByDescending(q => q.Id)
                                select new
                                {
                                    m.Id,
                                    UsedCount = m.MaterialNote.Count,
                                    m.Url,
                                    m.Comment,
                                    LastUsedTime = (m.MaterialNote.Count > 0) ? m.MaterialNote.OrderByDescending(o => o.CreatedOn).FirstOrDefault().CreatedOn : DateTime.MinValue,
                                };

                // sort
                if (orderbyMember.Length > 0)
                {
                    if (orderby[0] == "desc")
                        materials = materials.OrderByDescendingDynamic(orderbyMember[0]);
                    else
                        materials = materials.OrderByDynamic(orderbyMember[0]);

                    for(int i = 1; i < orderbyMember.Length; ++i)
                    {
                        if (orderby[i] == "desc")
                            materials = materials.ThenByDescendingDynamic(orderbyMember[i]);
                        else
                            materials = materials.ThenByDynamic(orderbyMember[i]);
                    }
                }
                
                return materials.Skip(page * count).Take(count).ToList().Select(m =>
                {
                    var mm = new MaterialMdoel();
                    mm.Id = m.Id;
                    mm.Url = m.Url;
                    mm.UsedCount = m.UsedCount;
                    mm.LastUsedTime = m.LastUsedTime;
                    mm.Comment = m.Comment;
                    return mm;
                }).ToList();
            }
        }

        public Material UseMaterial(int materialId, string noteContent)
        {
            using (DatabaseEntities entities = new DatabaseEntities())
            {
                var material = entities.Material.Where(m => m.Id == materialId).FirstOrDefault();
                if (material == null)
                    return null;

                material.UsedCount++;
                MaterialNote note = new MaterialNote();
                note.Content = noteContent;
                note.CreatedOn = DateTime.Now;
                note.MaterialId = materialId;
                material.MaterialNote.Add(note);
                entities.SaveChanges();

                return material;
            }
        }

        public Material DeleteMaterialNoteById(int Id)
        {
            int materialId = 0;
            using (DatabaseEntities entities = new DatabaseEntities())
            {
                var materialNote = entities.MaterialNote.FirstOrDefault(mn => mn.Id == Id);
                materialId = materialNote.MaterialId;
                entities.MaterialNote.Remove(materialNote);
                entities.SaveChanges();
            }

            return GetMaterialById(materialId);
        }
    }
}
