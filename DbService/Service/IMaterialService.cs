using DbService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbService.Service
{
    public interface IMaterialService
    {
        void InsertMaterial(string content, string comment, int[] categoryId);
        void UpadteMaterial(Material material);
        void DeleteMaterialById(int Id);
        Material GetMaterialById(int Id);
        IList<MaterialMdoel> GetMaterials(int? CategoryId, string[] orderbyMember, string[] orderby, int page, int count);
        Material UseMaterial(int materialId, string noteContent);
        Material DeleteMaterialNoteById(int Id);
    }
}
