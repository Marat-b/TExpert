using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.OData;
using System.Web.OData.Query;
using System.Web.OData.Routing;
using Microsoft.OData.Core;
using TExp.Models;


namespace TExp.Controllers
{
    
    [ODataRoutePrefix("Equipments")]
    public class EquipmentsController:ODataController
    {
        private readonly TExpEntities _db=new TExpEntities();

        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public async Task<IEnumerable<SearchViewModel>> GetEquipments(ODataQueryOptions<SearchViewModel> queryOptions)
        {
            //t_Equipment equipment=_db.t_Equipment.Select(s=>s.EquipmentId)
            IQueryable<SearchViewModel> searchView= _db.t_Equipment.Select(s => new SearchViewModel()
            {
                EquipmentId = s.EquipmentId,
                Name = s.Name,
                SerialNumber = s.SerialNumber,
                InventoryNumber = s.InventoryNumber,
                Price = s.Price,
                MOL = s.MOL,
                StartupDate = s.StartupDate,
                Decommission = s.Decommission,
                hasExpertise = _db.t_Equipment.Where(w => w.EquipmentId == s.EquipmentId).FirstOrDefault().t_Expertise.Select(s1=>s1.ExpertiseId).Count() //.Where(w => w.EquipmentId == s.EquipmentId).Select(s1 => s1.EquipmentId).Count()

            });

            var searchViewQueryable = (IQueryable<SearchViewModel>) queryOptions.ApplyTo(searchView);
            return await searchViewQueryable.ToArrayAsync();
        }
    }
}