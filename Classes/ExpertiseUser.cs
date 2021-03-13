using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TExp.Interfaces;
using TExp.Models;

namespace TExp.Classes
{
    public class ExpertiseUser:IExpertiseUser
    {
        private readonly TExpEntities _db;

        public ExpertiseUser()
        {
            _db=new TExpEntities();
        }


        public bool ReplaceUserForExpertise(int expertiseId, int userId)
        {
             
            t_Expertise expertise = _db.t_Expertise.SingleOrDefault(s => s.ExpertiseId== expertiseId);
            if (expertise != null)
            {
                t_User userOld = _db.t_Expertise.Find(expertiseId).t_User.FirstOrDefault();
                if (userOld != null)
                {
                    expertise.t_User.Remove(userOld);
                    _db.SaveChanges();
                }
                
                t_User userNew = _db.t_User.Find(userId);
                if (userNew != null)
                {
                    expertise.t_User.Add(userNew);
                    _db.SaveChanges();
                    return true;
                }
                return false;
            }
            return false;
        }

        public bool AddUserToExpertise(int expertiseId, int userId)
        {
            t_Expertise expertise = _db.t_Expertise.SingleOrDefault(s => s.ExpertiseId == expertiseId);
            if (expertise != null)
            {
               
                t_User userNew = _db.t_User.Find(userId);
                if (userNew != null)
                {
                    expertise.t_User.Add(userNew);
                    _db.SaveChanges();
                    return true;
                }
                return false;
            }
            return false;
        }

       
    }
}