using Axis_ProdTimeDB.DAL;
using Ice.Core;
using Ice.Lib.Framework;
using Ice.Proxy.BO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis_ProdTimeDB.InputClasses
{
    class InputClass
    {

        public static void addProduct(string prodCode, string workCenter)
        {
            using (var db = new TimeContext())
            {
                var prod = db.Prod.Where(item => item.ProdCode == prodCode && item.WorkCenter == workCenter).FirstOrDefault();
                if (prod == null)
                {
                    db.Prod.Add(new ProdTB
                    {
                        ProdCode = prodCode,
                        WorkCenter = workCenter
                    });
                }
                db.SaveChanges();
            }
            
        }
        public static void addProdFam(string FamCode, string workCenter)
        {
            using (var db = new TimeContext())
            {
            
                var prodFam = db.ProdFam.Where(item => item.FamCode == FamCode && item.WorkCenter == workCenter).FirstOrDefault();
                if (prodFam == null)
                {
                    db.ProdFam.Add(new ProdFamTB
                    {
                        FamCode = FamCode,
                        WorkCenter = workCenter
                    });
                }
                db.SaveChanges();
            }

        }
        public static void addParam(string ParamName, string ParamValue)
        {
            using (var db = new TimeContext())
            {
                var paramindex = db.Params.Where(item => item.ParamName == ParamName && item.ParamValue == ParamValue).FirstOrDefault();
                if (paramindex == null)
                {
                    db.Params.Add(new ParametersTB { ParamName = ParamName, ParamValue = ParamValue });
                }

                db.SaveChanges();
            }

        }
        public static void addOption(string optionName, double ProdTime, int? SectionLength = null)
        {
            using (var db = new TimeContext())
            {
                var optionindex = db.Options.Where(item => item.OptionName == optionName && item.ProdTime == ProdTime 
                && item.sectionLength == SectionLength).FirstOrDefault();
                if (optionindex == null)
                {
                    db.Options.Add(new OptionTB
                    {
                        OptionName = optionName,
                        ProdTime = ProdTime, 
                        sectionLength = SectionLength
                        
                    });
                }

                db.SaveChanges();


            }

        }
        public static void addFixture(string FixtureCode, string WorkCenter)
        {
            using (var db = new TimeContext())
            {

                var fixture = db.Fixtures.Where(item => item.FxCode == FixtureCode && item.WorkCenter == WorkCenter).FirstOrDefault();
                if (fixture == null)
                {
                    db.Fixtures.Add(new FixtureTB
                    {
                        FxCode = FixtureCode,
                        WorkCenter = WorkCenter
                    });
                }

                db.SaveChanges();
            }

        }
     
      
       
    }



   
}
