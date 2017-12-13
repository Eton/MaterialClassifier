using DbService.Model;
using DbService.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaterialCollector.Controllers
{
    public class MaterialController : BaseController
    {
        // GET: Material
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddMaterial(string content, string comment, int[] categoryId)
        {
            if(categoryId == null || categoryId.Length < 1)
                return ResponseJson("請選擇分類");

            IMaterialService materialService = new MaterialService();           

            materialService.InsertMaterial(content, comment, categoryId);

            return ResponseJson("新增成功");
        }

        [HttpPost]
        public ActionResult GetMaterial(int? categoryId, string[] orderbyMember, string[] orderby, int page, int count)
        {
            if (orderbyMember.Length != orderby.Length)
                return ResponseJson("資料有誤");

            try
            {
                IMaterialService materialService = new MaterialService();

                // youtube url 需改成 embed
                var materials = materialService.GetMaterials(categoryId, orderbyMember, orderby, page, count).Select(m =>
                {
                    var uri = new Uri(m.Url);
                    var vedioId = HttpUtility.ParseQueryString(uri.Query).Get("v");
                    return new
                    {
                        Id = m.Id,
                        Url = "https://www.youtube.com/embed/" + vedioId,
                        m.UsedCount,
                        m.Comment,
                        LastUsedTime = (m.LastUsedTime == DateTime.MinValue) ? string.Empty : m.LastUsedTime.ToString("yyyy/MM/dd")
                    };
                });

                return ResponseJson("", materials);
            }
            catch (Exception e)
            {
                return ResponseJson(e.Message);
            }
        }

        [HttpPost]
        public ActionResult DeleteMaterial(int materialId)
        {
            IMaterialService materialService = new MaterialService();

            materialService.DeleteMaterialById(materialId);

            return ResponseJson("刪除成功");
        }

        [HttpPost]
        public ActionResult UseMaterial(int materialId, string noteContent)
        {
            IMaterialService materialService = new MaterialService();
            var material = materialService.UseMaterial(materialId, noteContent);
            if (material == null)
                return ResponseJson("資料錯誤，找不到此素材");

            var returnInfo = new
            {
                UsedCount = material.MaterialNote.Count,
                LastUsedTime = material.MaterialNote.FirstOrDefault().CreatedOn.ToString("yyyy/MM/dd")
            };

            return ResponseJson("新增使用紀錄成功", returnInfo);
        }

        [HttpPost]
        public ActionResult GetMaterialNotes(int materialId)
        {
            IMaterialService materialService = new MaterialService();
            var material = materialService.GetMaterialById(materialId);
            if (material == null)
                return ResponseJson("資料錯誤，找不到此素材");

            var notes = material.MaterialNote.OrderByDescending(mn => mn.CreatedOn).Select(mn =>
            {
                return new { mn.Id, mn.Content, CreatedOn = mn.CreatedOn.ToString("yyyy/MM/dd hh:mm:ss") };
            }).ToList();

            return ResponseJson("", notes);
        }

        [HttpPost]
        public ActionResult DeleteMaterialNote(int materialNoteId)
        {
            IMaterialService materialService = new MaterialService();
            var material = materialService.DeleteMaterialNoteById(materialNoteId);

            var returnInfo = new
            {
                UsedCount = material.MaterialNote.Count,
                LastUsedTime = material.MaterialNote.FirstOrDefault().CreatedOn.ToString("yyyy/MM/dd")
            };

            return ResponseJson("刪除成功", returnInfo);
        }
    }
}