using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TExp.Models;
using FlowJs;
using FlowJs.Interface;


namespace TExp.Controllers
{
    public class FileUploadController : ApiController
    {
        //private BinaryFile _fileManager;
        private readonly IFlowJsRepo _flowJs;
        private readonly TExpEntities _db;
        private string _uploadFolder;

        public FileUploadController()
        {

            _flowJs = new FlowJsRepo();
            _db = new TExpEntities();
            _uploadFolder = ConfigurationManager.AppSettings["UploadFolder"];
          //  _uploadFolder = @"C:\Temp\PicUpload\";
        }

        //const string  _uploadFolder = @"C:\Temp\PicUpload\";

        [HttpGet]
        [Route("api/FileUpload/Upload/{id}")]
        public async Task<IHttpActionResult> PictureUploadGet(int id)
        {
            var request = HttpContext.Current.Request;
#if DEBUG
            Debug.WriteLine("request=" + request.ToString());
#endif
            var chunkExists = _flowJs.ChunkExists(_uploadFolder, request);
#if DEBUG
            Debug.WriteLine("chunkExists=" + chunkExists.ToString());
#endif

            if (chunkExists) return Ok();
            ModelState.AddModelError("chunk", "error");
            return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
            //return BadRequest(ModelState);
        }

        [HttpPost]
        [Route("api/FileUpload/Upload/{id}")]
        public async Task<IHttpActionResult> PictureUploadPost(int id)
        {
#if DEBUG
            Debug.WriteLine("HttpPost Begin");
#endif
            var request = HttpContext.Current.Request;

            var validationRules = new FlowValidationRules();
            validationRules.AcceptedExtensions.AddRange(new List<string> { "jpeg", "jpg", "png", "bmp" });
            validationRules.MaxFileSize = 5000000;
            //validationRules.MaxFileSize = 50;
            //validationRules.MaxFileSizeMessage = "File's size is too big!";

            // try
            // {
            var status = _flowJs.PostChunk(request, _uploadFolder, validationRules);


            if (status.Status == PostChunkStatus.Done)
            {
                // file uploade is complete. Below is an example of further file handling
                var filePath = Path.Combine(_uploadFolder, status.FileName);
#if DEBUG
                Debug.WriteLine("filePath=" + filePath);
#endif
                var file = File.ReadAllBytes(filePath);
                //var picture = await _fileManager.UploadPictureToS3(User.Identity.GetUserId(), file, status.FileName);
                ModelState.AddModelError("file", "done");
                //ModelState.Add(KeyValuePair<"1122",ModelState.AddModelError("file", "done") >);
                //_fileManager = new BinaryFile(filePath);
                //byte[] bytesF = _fileManager.GetBinaryData();
                t_User user = _db.t_User.Where(w=>w.UserID==id).FirstOrDefault();
                if (user != null)
                {

                    user.Sign = File.ReadAllBytes(filePath);
                    _db.Entry(user).State = EntityState.Modified;
                    _db.SaveChanges();
                }
#if DEBUG
                else
                {
                    Debug.WriteLine("user==null");
                }
#endif
                //File.Delete(filePath);
                return Ok();
                //return BadRequest(ModelState);
            }

            if (status.Status == PostChunkStatus.PartlyDone)
            {
                return Ok();
            }

            status.ErrorMessages.ForEach(x => ModelState.AddModelError("file", x));
#if DEBUG
            foreach (var m in ModelState)
            {
                Debug.WriteLine("Key=" + m.Key + " value=" + m.Value.ToString());


            }
#endif
            return BadRequest(ModelState);
            //}
            /*catch (Exception)
            {
                ModelState.AddModelError("file", "exception");
                return BadRequest(ModelState);
            }*/
        }

    }
}
