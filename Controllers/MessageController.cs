using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TExp.Models;
using System.Diagnostics;

namespace TExp.Controllers
{
    public class MessageController : ApiController
    {
        private TExpEntities db = new TExpEntities();

        // GET: api/Message
        public MessageModel Gett_Message()
        {
#if DEBUG
            Debug.WriteLine("Gett_Message");
#endif
            MessageModel message = db.t_Message.Select(s => new MessageModel()
            {
                MessageId = s.MessageId, Effective = s.Effective, Limit = s.Limit, Deadline = s.Deadline,
                CostRepair = s.CostRepair, OverCostRepair = s.OverCostRepair,LastNumber = s.LastNumber
            }).FirstOrDefault();
            if (message==null)
            {
                message = new MessageModel();
                message.MessageId = 0;
            }
            return message;
        }

        // GET: api/Message/5
        [ResponseType(typeof(t_Message))]
        public IHttpActionResult Gett_Message(int id)
        {
            t_Message t_Message = db.t_Message.Find(id);
            if (t_Message == null)
            {
                return NotFound();
            }

            return Ok(t_Message);
        }

        // PUT: api/Message/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putt_Message(int id, t_Message t_Message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != t_Message.MessageId)
            {
                return BadRequest();
            }

            db.Entry(t_Message).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!t_MessageExists(id))
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
        [ResponseType(typeof(t_Message))]
        public IHttpActionResult Postt_Message(t_Message t_Message)
        {
#if DEBUG
            Debug.WriteLine("Postt_Message");
#endif
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
#if DEBUG
            //Debug.WriteLine(String.Format("MessageId={0}"),t_Message.MessageId.ToString());
#endif
            db.t_Message.Add(t_Message);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = t_Message.MessageId }, t_Message);
        }

        // DELETE: api/Message/5
        [ResponseType(typeof(t_Message))]
        public IHttpActionResult Deletet_Message(int id)
        {
            t_Message t_Message = db.t_Message.Find(id);
            if (t_Message == null)
            {
                return NotFound();
            }

            db.t_Message.Remove(t_Message);
            db.SaveChanges();

            return Ok(t_Message);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool t_MessageExists(int id)
        {
            return db.t_Message.Count(e => e.MessageId == id) > 0;
        }
    }
}