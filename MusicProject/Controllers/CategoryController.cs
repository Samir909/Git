using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Management;
using System.Web.Mvc;
using System.Web.Services.Description;
using MusicProject.Models;


namespace MusicProject.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult GetData()
        {
            using (DbModels db= new DbModels())
            {
                List<Category_tbl> category = db.Category_tbl.ToList<Category_tbl>();
                return Json(new {data = category}, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public ActionResult AddOrEdit(int id=0)
        {
            if (id==0)
            {
                return View(new Category_tbl());
            }
            else 
            { 
            
                using (DbModels db=new DbModels())
                {
                    return View(db.Category_tbl.Where(x => x.C_Id == id).FirstOrDefault<Category_tbl>()); //update 
                }
            }
            

        }

        [HttpPost]
        public ActionResult AddOrEdit(Category_tbl category)
        {
            using (DbModels db=new DbModels())
            {
                if (category.C_Id == 0)
                {


                    db.Category_tbl.Add(category);
                    db.SaveChanges();
                    return Json(new {success = true, message = "Save Successfully"}, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Entry(category).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult Delete(int id)
        {
            using (DbModels db=new DbModels())
            {
                Category_tbl category = db.Category_tbl.Where(x => x.C_Id == id).FirstOrDefault<Category_tbl>();
                db.Category_tbl.Remove(category);
                db.SaveChanges();
                return Json(new {success = true, message = "Deleted Successfully"}, JsonRequestBehavior.AllowGet);
            }


        }

    }
}