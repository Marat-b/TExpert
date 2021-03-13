using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
//using System.Web.Mvc;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using System.Text;
using System.Diagnostics;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using TExp.App_Start;
using TExp.Models;
using TExp.Classes;

namespace TExp.Controllers
{
    public class WizardStepFourController : ApiController
    {
        private readonly TExpEntities db;
        private MyUserManager _userManager;
        private string _userName;
        

        public WizardStepFourController()
        {
            db = new TExpEntities();
            _userManager = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<MyUserManager>();
            _userName = User.Identity.Name;
#if DEBUG
            Debug.WriteLine("~~~~~~~~ user name ->"+ User.Identity.Name);
#endif
           
        }

        /// <summary>
        /// Получение данных для создания или редактирования данных по акту экспертизы
        /// </summary>
        /// <param name="id">equipmentId</param>
        /// <param name="expertiseId"></param>
        /// <returns></returns>
        [Route("api/WizardStepFour/{id}/{expertiseId}")]
        public ExpertiseModel Get(int id,int expertiseId)
        {
            ExpertiseModel expertise = db.t_Expertise.Where(w => w.ExpertiseId == expertiseId).Select(s => new ExpertiseModel()
            {
                ExpertiseId = s.ExpertiseId, Staff = s.Staff, Reason = s.Reason,
                Conclusion = s.Conclusion,NumberExp = s.NumberExp,DateExp = s.DateExp,
                RequestId = s.RequestId,
                Staff2 = s.Staff2,
                IsServiceableEquipment = s.IsServiceableEquipment,
                IsWarrantyRepair = s.IsWarrantyRepair,
                IsOrganizationRepair = s.IsOrganizationRepair,
                IsPartsSupply = s.IsPartsSupply,
                IsServiceable = s.IsServiceable,
                IsForWriteoff = s.IsForWriteoff
            }).FirstOrDefault();
            if (expertise == null)
            {
                expertise = new ExpertiseModel();
                //expertise.ExpertiseId = id;
                StringBuilder nameOfMalfunctions = new StringBuilder();
                t_Equipment equipment = db.t_Equipment.Find(id);
                IEnumerable<string> malfunctions = equipment.t_Malfunction.Select(s => s.Name).ToList();
                //nameOfMalfunctions.Append("Выявлены неисправности следующих деталей:\n");
                foreach (string Name in malfunctions)
                {
                    nameOfMalfunctions.Append(Name + "\n");
                }
                expertise.Reason = nameOfMalfunctions.ToString();
                DecisionMessage decisionMessage = new DecisionMessage(id);
                expertise.Conclusion = decisionMessage.Text;
                expertise.NumberExp =
               db.t_Message.Select(s => s.LastNumber+1).FirstOrDefault();
                expertise.DateExp = DateTimeOffset.Now;

                // insert Position and FIO of staff
                var user = _userManager.FindByName(_userName);
                if (user != null)
                {
                    expertise.Staff = user.Position + " " + user.FIO;
                }
            }
            /*else
            {
                expertise.NumberExp =
                db.t_Message.Select(s => s.LastNumber).FirstOrDefault();
                expertise.DateExp = DateTime.Today;
            }*/

            if (expertise != null && String.IsNullOrWhiteSpace(expertise.Conclusion))
            {
                DecisionMessage decisionMessage = new DecisionMessage(id);
                expertise.Conclusion = decisionMessage.Text;
            }

            if (expertise != null && String.IsNullOrWhiteSpace(expertise.Reason))
            {
                StringBuilder nameOfMalfunctions = new StringBuilder();
                t_Equipment equipment = db.t_Equipment.Find(id);
                IEnumerable<string> malfunctions = equipment.t_Malfunction.Select(s => s.Name).ToList();
                //nameOfMalfunctions.Append("Выявлены неисправности следующих деталей:\n");
                foreach (string Name in malfunctions)
                {
                    nameOfMalfunctions.Append(Name + "\n");
                }
                expertise.Reason = nameOfMalfunctions.ToString();
            }

            // Изменить ИД пользователя на другого в t_UserExpertise
            if (expertise!=null && String.IsNullOrWhiteSpace(expertise.Staff))
            {

                /* var user = db.t_User.SingleOrDefault(w => w.UserName == _userName);
                if (user != null)
                {
                    expertise.Staff = user.Position + " " + user.FIO;
                   
                    var expertiseUser = db.t_Expertise.Find(expertiseId).t_User.FirstOrDefault();
                    
                    if (expertiseUser != null)
                    {
                        db.t_Expertise.Find(expertiseId).t_User.Remove(expertiseUser);
                        db.SaveChanges();
                        db.t_Expertise.Find(expertiseId).t_User.Add(user);
                        db.SaveChanges();
                    }
                }*/
                var user = _userManager.FindByName(_userName);
                
                ExpertiseUser expertiseUser =new ExpertiseUser();
                if (expertiseUser.ReplaceUserForExpertise(expertiseId, user.UserID))
                {
                    expertise.Staff = user.Position + " " + user.FIO;
                }
            }

            
            

            return expertise;
        }

        [Route("api/WizardStepFour/{id}")]
        public IHttpActionResult Put(int id, ExpertiseModel expertiseModel)
        {
#if DEBUG
            Debug.WriteLine(String.Format("Id={0},Staff={1}", expertiseModel.ExpertiseId.ToString(), expertiseModel.Staff));
            
#endif
            //ExpertiseModel expertise = db.t_Expertise.Where(w => w.EquipmentId == expertiseModel.EquipmentId).Select(s => new ExpertiseModel() {EquipmentId=s.EquipmentId,Staff=s.Staff,Reason=s.Reason,Conclusion=s.Conclusion }).FirstOrDefault();
            t_Expertise expertise = db.t_Expertise.Find(expertiseModel.ExpertiseId);
            //t_Expertise expertise = db.t_Expertise.Find(id);
            // if expertise is null then new act of expertise
            if (expertise == null)
            {
                expertise = new t_Expertise();
                //expertise.EquipmentId = id;
                expertise.Conclusion = expertiseModel.Conclusion;
                expertise.Reason = expertiseModel.Reason;
                expertise.Staff = expertiseModel.Staff;  //user
                expertise.NumberExp = expertiseModel.NumberExp;
#if DEBUG
                Debug.WriteLine("expertise == null expertiseModel.DateExp=" + expertiseModel.DateExp);
                //Debug.WriteLine(DateTimeOffset.Parse(expertiseModel.DateExp).ToString());
#endif
                expertise.DateExp =  expertiseModel.DateExp; //new DateTimeOffset()

                expertise.Staff2 = expertiseModel.Staff2;   // customer
                expertise.RequestId = expertiseModel.RequestId;
#if DEBUG
                Debug.WriteLine("expertiseModel.IsServiceableEquipment="+ expertiseModel.IsServiceableEquipment.ToString());
                
                //Debug.WriteLine("^^^^^ FIO=" + user.FIO);
               
#endif
                expertise.IsServiceableEquipment = expertiseModel.IsServiceableEquipment;
                expertise.IsWarrantyRepair = expertiseModel.IsWarrantyRepair;
                expertise.IsOrganizationRepair = expertiseModel.IsOrganizationRepair;
                expertise.IsPartsSupply = expertiseModel.IsPartsSupply;
                expertise.IsServiceable = expertiseModel.IsServiceable;
                expertise.IsForWriteoff = expertiseModel.IsForWriteoff;

                db.t_Expertise.Add(expertise);
                db.SaveChanges();
                db.Entry(expertise).GetDatabaseValues();
                t_Equipment equipment = db.t_Equipment.Find(id);
                if (equipment != null)
                {
                    equipment.t_Expertise.Add(expertise);
                    db.SaveChanges();
                }
#if DEBUG
                else
                {
                    Debug.WriteLine("equipment is null!");
                }
#endif


                // fill table t_UserExpertise
                var user = _userManager.FindByName(_userName);
                if (user!=null)
                {
                   ExpertiseUser expertiseUser=new ExpertiseUser();
                    expertiseUser.AddUserToExpertise(expertise.ExpertiseId, user.UserID);
                }
#if DEBUG
                else
                {
                    Debug.WriteLine("user is null!");
                }
#endif

                try
                {
                    t_Message message = db.t_Message.FirstOrDefault();
                    if (message!=null)
                    {
                        message.LastNumber = expertiseModel.NumberExp;
                        db.Entry(message).State = EntityState.Modified;
                        db.SaveChanges();
                    }
#if DEBUG
                    else
                    {
                        Debug.WriteLine("message is null");
                    }
#endif

                }
                catch (Exception)
                {
                    
                    throw;
                }
                
            }
            else
            {
                expertise.Conclusion = expertiseModel.Conclusion;
                expertise.Reason = expertiseModel.Reason;
                expertise.Staff = expertiseModel.Staff;
                expertise.NumberExp = expertiseModel.NumberExp;
                expertise.DateExp =expertiseModel.DateExp;
                expertise.Staff2 = expertiseModel.Staff2;
                expertise.RequestId = expertiseModel.RequestId;
                expertise.IsServiceableEquipment = expertiseModel.IsServiceableEquipment;
                expertise.IsWarrantyRepair = expertiseModel.IsWarrantyRepair;
                expertise.IsOrganizationRepair = expertiseModel.IsOrganizationRepair;
                expertise.IsPartsSupply = expertiseModel.IsPartsSupply;
                expertise.IsServiceable = expertiseModel.IsServiceable;
                expertise.IsForWriteoff = expertiseModel.IsForWriteoff;
                db.Entry(expertise).State = EntityState.Modified;
                db.SaveChanges();
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
