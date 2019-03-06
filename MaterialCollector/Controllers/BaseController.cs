using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaterialCollector.Controllers
{
    public class BaseController : Controller
    {        
        [NonAction]
        public ActionResult ResponseJson(string message, object data = null)
        {
            // to do : 動態產生object
            //dynamic dataOb = new ExpandoObject();
            //dataOb.msg = message;

            //if(data != null)
            //{
            //    foreach (var p in data.GetType().GetProperties())
            //        dataOb
            //}
            

            return new JsonResult
            {
                Data = new
                {
                    msg = message,
                    data = data
                }
            };
            
        }
    }
}