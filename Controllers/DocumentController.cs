using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.ModelBinding;
using System.Web.OData;
using System.Web.OData.Query;
using System.Web.OData.Routing;
using TExp.Models;
using Microsoft.OData.Core;
using TExp.Classes;
using System.Diagnostics;

namespace TExp.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.OData.Builder;
    using System.Web.OData.Extensions;
    using TExp.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<DocumentModel>("Document");
    config.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    [ODataRoutePrefix("Document")]
    public class DocumentController : ODataController
    {
        private static ODataValidationSettings _validationSettings = new ODataValidationSettings();

        private readonly TExpEntities _db;
        private readonly DocumentClass _document;

        public DocumentController()
        {
            _db=new TExpEntities();
            _document=new DocumentClass(_db);
        }

        // GET: odata/Document
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public IEnumerable<DocumentModel> GetDocument(ODataQueryOptions<DocumentModel> queryOptions)
        {
            // validate the query.
            IEnumerable<DocumentModel> ret;
            try
            {
                ret =  _document.GetDocument();
            }
            catch (ODataException ex)
            {
                ret = null;
            }

            // return Ok<IEnumerable<DocumentModel>>(documentModels);
            return ret;
        }
        /*
        // GET: odata/Document(5)
        public async Task<IHttpActionResult> GetDocumentModel([FromODataUri] int key, ODataQueryOptions<DocumentModel> queryOptions)
        {
            // validate the query.
            try
            {
                queryOptions.Validate(_validationSettings);
            }
            catch (ODataException ex)
            {
                return BadRequest(ex.Message);
            }

            // return Ok<DocumentModel>(documentModel);
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        // PUT: odata/Document(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<DocumentModel> delta)
        {
            Validate(delta.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Get the entity here.

            // delta.Put(documentModel);

            // TODO: Save the patched entity.

            // return Updated(documentModel);
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        // POST: odata/Document
        public async Task<IHttpActionResult> Post(DocumentModel documentModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Add create logic here.

            // return Created(documentModel);
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        // PATCH: odata/Document(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<DocumentModel> delta)
        {
            Validate(delta.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Get the entity here.

            // delta.Patch(documentModel);

            // TODO: Save the patched entity.

            // return Updated(documentModel);
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        // DELETE: odata/Document(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            // TODO: Add delete logic here.

            // return StatusCode(HttpStatusCode.NoContent);
            return StatusCode(HttpStatusCode.NotImplemented);
        }*/
    }
}
