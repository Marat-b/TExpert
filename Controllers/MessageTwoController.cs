using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TExp.Models;
#if DEBUG
using System.Diagnostics;
#endif

namespace TExp.Controllers
{
    public class MessageTwoController : ApiController
    {
        private readonly TExpEntities db;

        public MessageTwoController()
        {
            db = new TExpEntities();
        }

      
        public t_ListForPrint Get()
        {
#if DEBUG
            Debug.WriteLine("Gett_ListForPrint");
#endif
            t_ListForPrint message = db.t_ListForPrint.FirstOrDefault();
            if (message == null)
            {
                message = new t_ListForPrint();
                message.ListId = 0;
            }
            return message;
        }


        [ResponseType(typeof(void))]
        public IHttpActionResult Put(int id, t_ListForPrint t_ListForPrint)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != t_ListForPrint.ListId)
            {
                return BadRequest();
            }

            db.Entry(t_ListForPrint).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!t_ListForPrintExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Message
        [ResponseType(typeof(t_ListForPrint))]
        public IHttpActionResult Post(t_ListForPrint t_ListForPrint)
        {
#if DEBUG
            Debug.WriteLine("Postt_ListForPrint");
#endif
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
#if DEBUG
            //Debug.WriteLine(String.Format("MessageId={0}"),t_ListForPrint.MessageId.ToString());
#endif
            db.t_ListForPrint.Add(t_ListForPrint);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = t_ListForPrint.ListId }, t_ListForPrint);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool t_ListForPrintExists(int id)
        {
            return db.t_ListForPrint.Count(e => e.ListId == id) > 0;
        }
    }
}
