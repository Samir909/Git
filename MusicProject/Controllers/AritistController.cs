using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MusicProject.Models;
namespace MusicProject.Controllers
{
    public class AritistController : Controller
    {
        // GET: Aritist
        public ActionResult Aritist()
        {
            return View();
        }



        public ActionResult GetData()
        {
            using (DbModels db = new DbModels())
            {
               // List<Category_tbl> category = db.Category_tbl.ToList<Category_tbl>();

               List<Aritist> aritists = db.Aritists.ToList<Aritist>();
                return Json(new { data = aritists }, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                return View(new Aritist());
            }
            else
            {

                using (DbModels db = new DbModels())
                {
                    return View(db.Aritists.Where(x => x.Aritist_Id == id).FirstOrDefault<Aritist>()); //update 
                }
            }


        }

        [HttpPost]
        public ActionResult AddOrEdit(Aritist aritist)
        {
            using (DbModels db = new DbModels())
            {
                if (aritist.Aritist_Id == 0 )
                {
                     string fileName = Path.GetFileNameWithoutExtension(aritist.ImageFile.FileName);
                     string extension = Path.GetExtension(aritist.ImageFile.FileName);
                     fileName = fileName + DateTime.Now.ToString("yymmssfff")+ extension;
                    aritist.Aritist_Image = "~/APPFile/Images/" + fileName;
                    aritist.ImageFile.SaveAs(Path.Combine(Server.MapPath("~/APPFile/Images/"),fileName));


                    db.Aritists.Add(aritist);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Save Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Entry(aritist).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult Delete(int id)
        {
            using (DbModels db = new DbModels())
            {
                Category_tbl category = db.Category_tbl.Where(x => x.C_Id == id).FirstOrDefault<Category_tbl>();
                Models.Aritist aritist = db.Aritists.Where(x => x.Aritist_Id == id).FirstOrDefault<Aritist>();
                db.Aritists.Remove(aritist);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }


        }




    }
}