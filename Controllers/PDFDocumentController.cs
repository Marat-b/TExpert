using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using iTextSharpWebAPI.Classes;
using TExp.Models;

namespace iTextSharpWebAPI.Controllers
{
    public class PDFDocumentController : ApiController
    {

        private readonly TExpEntities _db;

        public PDFDocumentController()
        {
            _db=new TExpEntities();
        }

        /// <summary>
        /// Create a PDF document
        /// </summary>
        /// <param name="id">ExpertiseId</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage Get(int id)
        {

            PDFDocument pdfDocument = new PDFDocument();
            t_Expertise expertise = _db.t_Expertise.Find(id);
            if (expertise != null)
            {
                if (expertise.DateExp == null)
                {
                    expertise.DateExp= DateTimeOffset.Now;
                }
                pdfDocument.DateExp = expertise.DateExp.Value.ToString("d");
                pdfDocument.Conclusion= expertise.Conclusion;
                pdfDocument.IsForWriteoff= expertise.IsForWriteoff;
                pdfDocument.IsOrganizationRepair= expertise.IsOrganizationRepair;
                pdfDocument.IsPartsSupply= expertise.IsPartsSupply;
                pdfDocument.IsServiceable= expertise.IsServiceable;
                pdfDocument.IsServiceableEquipment= expertise.IsServiceableEquipment;
                pdfDocument.IsWarrantyRepair= expertise.IsWarrantyRepair;
                pdfDocument.NumberExp= expertise.NumberExp;
                pdfDocument.Reason= expertise.Reason;
                pdfDocument.RequestId= expertise.RequestId;
                pdfDocument.Staff= expertise.Staff;
                pdfDocument.Staff2= expertise.Staff2;
                t_Equipment equipment = expertise.t_Equipment.FirstOrDefault();
                if (equipment != null)
                {
                    pdfDocument.InventoryNumber= equipment.InventoryNumber;
                    pdfDocument.SerialNumber= equipment.SerialNumber;
                    pdfDocument.StartupDate=equipment.StartupDate.Value.Year.ToString(); //.ToShortDateString();
                    pdfDocument.Name= equipment.Name;
                    
                }

                t_User user = expertise.t_User.FirstOrDefault();
                if (user != null)
                {
                    pdfDocument.ImgData = user.Sign;
                    
                }




            }
            

            MemoryStream stream = new MemoryStream(pdfDocument.GetPDFDocument());

            HttpResponseMessage result = new HttpResponseMessage(statusCode: HttpStatusCode.OK);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            result.Content.Headers.ContentDisposition.FileName = pdfDocument.GetFileName() + ".pdf"; 

             //var transactionViewQuerable = (IQueryable<TransactionFileReportModel>)queryOptions.ApplyTo(transactionView);
            return result; // await transactionViewQuerable.ToArrayAsync();
        }
    }
}
