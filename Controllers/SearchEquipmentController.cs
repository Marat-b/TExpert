using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Threading.Tasks;
using TExp.Models;
#if DEBUG
using System.Diagnostics;
#endif

namespace TExp.Controllers
{
    public class SearchEquipmentController : ApiController
    {
        private TExpEntities db;

        public SearchEquipmentController()
        {
            db = new TExpEntities();
        }

        [Route("api/SearchEquipment/typeahead/{id}/{searchFinder}/{iCount}")]
        [ResponseType(typeof(FinderModel))]
        public IQueryable<FinderModel> GetFinderModel(int id, string searchFinder,int iCount)
        {
#if DEBUG
            Debug.WriteLine(String.Format("searchFinder={0}", searchFinder));
            Debug.WriteLine(String.Format("select={0}", id));
#endif
            //IList FoundStuff;
            IQueryable<FinderModel> FoundStuff;
            switch (id)
            {
                case 1:
                    FoundStuff = (from c in db.t_Equipment where c.Name.Contains(searchFinder) orderby c.Name select new FinderModel { FindedStuff = c.Name }).Take(iCount).AsQueryable();
                    break;
                case 2:
                    FoundStuff = (from c in db.t_Equipment where c.SerialNumber.Contains(searchFinder) orderby c.SerialNumber select new FinderModel { FindedStuff = c.SerialNumber }).Take(iCount).AsQueryable();
                    break;
                case 3:
                    //Debug.WriteLine("Case=3");
                    FoundStuff = (from c in db.t_Equipment where c.InventoryNumber.Contains(searchFinder) orderby c.InventoryNumber select new FinderModel { FindedStuff = c.InventoryNumber }).Take(iCount).AsQueryable();
                    // Debug.WriteLine("Count=" + FoundStuff.Count().ToString());
                    break;
                case 4:
                    FoundStuff = (from c in db.t_Equipment where c.MOL.Contains(searchFinder) orderby c.MOL select new FinderModel { FindedStuff = c.MOL }).Take(iCount).AsQueryable();
                    break;
                default:
                    FoundStuff = (from c in db.t_Equipment where c.Name.StartsWith(searchFinder) orderby c.Name select new FinderModel { FindedStuff = c.Name }).Take(iCount).AsQueryable();
                    break;
            }
#if DEBUG
            Debug.WriteLine("Begin typeahead");
            foreach(FinderModel item in FoundStuff)
            {
                Debug.WriteLine(item.FindedStuff);

            }
#endif
            return FoundStuff;
        }

        /*
        [Route("api/SearchEquipment/{id}/{searchFinder}")]
        [ResponseType(typeof(SearchViewModel))]
        public IQueryable<SearchViewModel> GetSearchViewModel(int id, string searchFinder)
        {
#if DEBUG
            Debug.WriteLine("selectorFinder="+id.ToString()+", searchFinder="+searchFinder);
#endif
            IQueryable<SearchViewModel> searchView;
            //t_Expertise expertise = db.t_Expertise.Find(id);
            switch(id)
            {
                    
                case 1:
                    //IQueryable<t_Equipment> e = db.t_Equipment;
                    searchView = db.t_Equipment.Where(w => w.Name.Contains(searchFinder)).Select(s => new SearchViewModel()
                    {
                        EquipmentId = s.EquipmentId,
                        Name = s.Name,
                        SerialNumber = s.SerialNumber,
                        InventoryNumber = s.InventoryNumber,
                        Price = s.Price,
                        MOL = s.MOL,
                        StartupDate = s.StartupDate,
                        Decommission=s.Decommission,
                        hasExpertise=db.t_Expertise.Where(w=>w.EquipmentId==s.EquipmentId).Select(s1=>s1.EquipmentId).Count()
                        
                    }).AsQueryable();
                    break;
                case 2:
                    searchView = db.t_Equipment.Where(w => w.SerialNumber.Contains(searchFinder)).Select(s => new SearchViewModel()
                    {
                        EquipmentId = s.EquipmentId,
                        Name = s.Name,
                        SerialNumber = s.SerialNumber,
                        InventoryNumber = s.InventoryNumber,
                        Price = s.Price,
                        MOL = s.MOL,
                        StartupDate = s.StartupDate,
                        Decommission = s.Decommission,
                        hasExpertise = db.t_Expertise.Where(w => w.EquipmentId == s.EquipmentId).Select(s1 => s1.EquipmentId).Count()
                    }).AsQueryable();
                    break;
                case 3:
                    searchView = db.t_Equipment.Where(w => w.InventoryNumber.Contains(searchFinder)).Select(s => new SearchViewModel()
                    {
                        EquipmentId = s.EquipmentId,
                        Name = s.Name,
                        SerialNumber = s.SerialNumber,
                        InventoryNumber = s.InventoryNumber,
                        Price = s.Price,
                        MOL = s.MOL,
                        StartupDate = s.StartupDate,
                        Decommission = s.Decommission,
                        hasExpertise = db.t_Expertise.Where(w => w.EquipmentId == s.EquipmentId).Select(s1 => s1.EquipmentId).Count()
                    }).AsQueryable();
                    break;
                case 4:
                    searchView = db.t_Equipment.Where(w => w.MOL.Contains(searchFinder)).Select(s => new SearchViewModel()
                    {
                        EquipmentId = s.EquipmentId,
                        Name = s.Name,
                        SerialNumber = s.SerialNumber,
                        InventoryNumber = s.InventoryNumber,
                        Price = s.Price,
                        MOL = s.MOL,
                        StartupDate = s.StartupDate,
                        Decommission = s.Decommission,
                        hasExpertise = db.t_Expertise.Where(w => w.EquipmentId == s.EquipmentId).Select(s1 => s1.EquipmentId).Count()
                    }).AsQueryable();
                    break;
                default:
                    searchView = db.t_Equipment.Where(w => w.Name.Contains(searchFinder)).Select(s => new SearchViewModel()
                    {
                        EquipmentId = s.EquipmentId,
                        Name = s.Name,
                        SerialNumber = s.SerialNumber,
                        InventoryNumber = s.InventoryNumber,
                        Price = s.Price,
                        MOL = s.MOL,
                        StartupDate = s.StartupDate,
                        Decommission = s.Decommission,
                        hasExpertise = db.t_Expertise.Where(w => w.EquipmentId == s.EquipmentId).Select(s1 => s1.EquipmentId).Count()
                    }).AsQueryable();
                    break;
            }
            //t_Equipment t_Equipment = await db.t_Equipment.FindAsync(id);
            //*if (searchView == null)
           // {
           //     return NotFound();
           // }
#if DEBUG
            Debug.WriteLine("Check for return");
            foreach (SearchViewModel item in searchView)
            {
                Debug.WriteLine(String.Format( "Name={0}, hasExpertise={1}",item.Name,item.hasExpertise));
            }
#endif
            
            return searchView;
        }*/
    }
}
