using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TExp.Models;
using System.Diagnostics;

namespace TExp.Classes
{
    public class DecisionMessage
    {
        private readonly TExpEntities db;
        private t_Equipment Equipment;
        private t_Message Message;
        private t_CostOfRepair CostOfRepair;

        public DecisionMessage(int EquipmentID)
        {
            db = new TExpEntities();
            Equipment = db.t_Equipment.Find(EquipmentID);
            Message = db.t_Message.FirstOrDefault();
            CostOfRepair = db.t_CostOfRepair.Find(EquipmentID);
        }

        private string GetMessage()
        {
            decimal Price = 0, costOfRepair = 0;
            DateTime Startup;
            int Warranty=0, Effective=0, Limit=0, Years=0,costOfRepairPercent=0;
            int PercentCost = 0;
            string Result = "";
            if (Equipment != null)
            {
                 Price = Equipment.Price;
                Startup = Equipment.StartupDate.Value;
#if DEBUG
                Debug.WriteLine("Startup="+Startup.ToShortDateString());
#endif
                Warranty = Equipment.t_TypeOfEquipment.Select(s => s.Warranty).FirstOrDefault();
                Effective = Equipment.t_TypeOfEquipment.Select(s => s.Effective).FirstOrDefault();
                Limit = Equipment.t_TypeOfEquipment.Select(s => s.Limit).FirstOrDefault();
                Years =Convert.ToInt32( Math.Ceiling( (DateTime.Today - Startup).TotalDays/365)); //
#if DEBUG
                Debug.WriteLine("Years="+Years.ToString());
#endif
                
                
            }
            else
            {
                return "N/A";
            }

            if (Message != null)
            {
                PercentCost = Message.CostRepair;
                
            }
#if DEBUG
            else{
                Debug.WriteLine("Object Message is null!");

            }
#endif

            if (CostOfRepair!=null)
            {
                costOfRepair = CostOfRepair.Price;
                costOfRepairPercent = Convert.ToInt32((costOfRepair / Price) * 100);
            }

            if (Years<Warranty)
            {
                if (Message != null)
                {
                    Result = Message.Effective;
                }
            }

            if (Warranty<=Years && Years<Warranty+Effective)
            { 
                if (Message!=null)
                {
                    Result = Message.Effective;
                }
            }

            if (PercentCost < costOfRepairPercent)
            { 
                if (Message!=null)
                {
                    Result = Message.OverCostRepair;
                }
            }

            if (Warranty+Effective<=Years && Years<Warranty+Effective+Limit)
            { 
                if (Message!=null)
                {
                    Result = Message.Limit;
                }
            }

            if (Warranty+Effective+Limit<=Years)
            { 
                if (Message!=null)
                {
                    Result = Message.Deadline;
                }
            }

            return Result;
        }

        public string Text
        {
            get
            {
                return GetMessage();
            }
        }
    }
}