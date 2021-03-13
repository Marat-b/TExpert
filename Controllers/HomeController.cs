using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TExp.Models;
/*using System.Web.Http;
using System.Net.Http;
using System.Net;*/
using System.Diagnostics;

namespace TExp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        //private TExpEntities db;

        public HomeController()
        {
           // db = new TExpEntities();
        }



        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TypeOfEquipment()
        {
            return View();
        }

        public ActionResult Malfunction()
        {
            return View();
        }
        /*
        public ActionResult Wizard()
        {
            return View();
        } */

        public ActionResult Message()
        {
            return View();
        }

        /*
        public ActionResult MessageTwo()
        {
            return View();
        } */

        public ActionResult Document()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "TechExpert.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Контактные данные.";

            return View();
        }

        /*
        public ActionResult FindStuff(string stuff, string select) //,string select
        {
#if DEBUG
            Debug.WriteLine(String.Format("stuff={0}", stuff));
            Debug.WriteLine(String.Format("select={0}", select));
#endif
            //IList FoundStuff;
            IEnumerable<FinderModel> FoundStuff;
            switch (select)
            {
                case "1":
                    FoundStuff = (from c in db.t_Equipment where c.Name.Contains(stuff) orderby c.Name select new FinderModel { FindedStuff = c.Name }).ToList();
                    break;
                case "2":
                    FoundStuff = (from c in db.t_Equipment where c.SerialNumber.Contains(stuff) orderby c.SerialNumber select new FinderModel { FindedStuff = c.SerialNumber }).ToList();
                    break;
                case "3":
                    //Debug.WriteLine("Case=3");
                    FoundStuff = (from c in db.t_Equipment where c.InventoryNumber.Contains(stuff) orderby c.InventoryNumber select new FinderModel { FindedStuff = c.InventoryNumber }).ToList();
                   // Debug.WriteLine("Count=" + FoundStuff.Count().ToString());
                    break;
                case "4":
                    FoundStuff = (from c in db.t_Equipment where c.MOL.Contains(stuff) orderby c.MOL select new FinderModel { FindedStuff = c.MOL }).ToList();
                    break;
                default:
                    FoundStuff = (from c in db.t_Equipment where c.Name.StartsWith(stuff) orderby c.Name select new FinderModel { FindedStuff = c.Name }).ToList();
                    break;
            }
            return Json(FoundStuff, JsonRequestBehavior.AllowGet);
        } */

            /*
        public ActionResult PrintList()
        {
            List<EquipmentIdModel> equipmentsId = new List<EquipmentIdModel>();
            equipmentsId = Session["equipments"] as List<EquipmentIdModel>;
            ListOfPrintModel listOfPrint = new ListOfPrintModel();
            listOfPrint.ActOfExpertise    = new List<ActOfExpertiseModel>();

            foreach (EquipmentIdModel item in equipmentsId)
            {
#if DEBUG
                Debug.WriteLine(String.Format("EquipmentID={0}",item.EquipmentId.ToString()));
#endif
                EquipmentModel equipment = db.t_Equipment.Where(w => w.EquipmentId == item.EquipmentId).Select(s => new EquipmentModel() { EquipmentId = s.EquipmentId, Name = s.Name, Price = s.Price, StartupDate = s.StartupDate, InventoryNumber = s.InventoryNumber, SerialNumber = s.SerialNumber }).SingleOrDefault();
                if (equipment == null)
                {
                    return HttpNotFound();
                }
                ExpertiseModel expertise = db.t_Expertise.Where(w => w.EquipmentId == item.EquipmentId).Select(s => new ExpertiseModel() { ExpertiseId =  s.ExpertiseId, Conclusion = s.Conclusion, Reason = s.Reason, Staff = s.Staff }).SingleOrDefault();
                if (expertise == null)
                {
                    expertise = new ExpertiseModel();
                    expertise.ExpertiseId = 0;
                    expertise.Conclusion = "";
                    expertise.Reason = "";
                    expertise.Staff = "";
                }

                listOfPrint.ActOfExpertise.Add(new ActOfExpertiseModel() { equipment = equipment, expertise = expertise });
            }

            t_ListForPrint t_listForPrint = db.t_ListForPrint.FirstOrDefault();
            if (listOfPrint!=null)
            {
                listOfPrint.Item1 = t_listForPrint.Item1;
                listOfPrint.Item2 = t_listForPrint.Item2;
                listOfPrint.Item3 = t_listForPrint.Item3;
            }
            else
            {
                listOfPrint.Item1 = "";
                listOfPrint.Item2 = "";
                listOfPrint.Item3 = "";
            }

            return View(listOfPrint);

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        */
    }
}