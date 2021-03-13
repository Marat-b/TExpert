using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TExp.Models;
using System.Diagnostics;
using System.EnterpriseServices;
using System.Web.Mvc;
using Org.BouncyCastle.Asn1.Esf;


namespace TExp.Classes
{
    
    
    public class DocumentClass
    {
        private readonly TExpEntities _db;

        public DocumentClass(TExpEntities db)
        {
            _db = db;
        }

        /// <summary>
        /// Get list of documents
        /// </summary>
        /// <returns></returns>
        public  IEnumerable<DocumentModel> GetDocument()
        {
            /*
            IEnumerable<DocumentModel> ret = _db.t_Expertise.Select(s=>new DocumentModel()
            {
                ExpertiseId = s.ExpertiseId,
                NumberExp=s.NumberExp,
                Name = "N",
                SerialNumber = "0",
                InventoryNumber = "0",
                DateExp = s.DateExp,
                Staff=s.Staff
            }).OrderBy(o=>o.ExpertiseId); */
            IEnumerable<DocumentModel> ret = (from tx in _db.t_Expertise
                where tx.t_Equipment.Any()
                orderby tx.ExpertiseId
                select new DocumentModel()
                {
                    ExpertiseId = tx.ExpertiseId,
                    RequestId = tx.RequestId,
                    NumberExp = tx.NumberExp,
                    Name = tx.t_Equipment.FirstOrDefault().Name,
                    SerialNumber = tx.t_Equipment.FirstOrDefault().SerialNumber,
                    InventoryNumber = tx.t_Equipment.FirstOrDefault().InventoryNumber,
                    DateExp = tx.DateExp,
                    Staff = tx.Staff
                }).ToList();
            
#if DEBUG
            foreach (var item in ret)
            {
                Debug.WriteLine("ExpertiseId="+item.ExpertiseId.ToString());
            }
            

#endif


            return  ret;
        }

        //protected ObjectDisposedException()
    }
}