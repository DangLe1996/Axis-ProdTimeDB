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


    class Ballast : Program
    {
        public Ballast(string paramFilePath)
        {
            string optionName = "Ballast";

            var dt = ConvertCSVtoDataTable(paramFilePath);
            var newSort = (from row in dt.AsEnumerable()

                           group row by new
                           {
                               ID = row.Field<string>("Product ID"),

                               Workcenter = row.Field<string>("Work Center")
                           } into grp
                           //orderby grp.Key
                           select new
                           {
                               Product = grp.Key.ID,
                                workcenter = grp.Key.Workcenter,
                               Sum = grp.Sum(r => Double.Parse(r.Field<string>("UNIT TIME (MIN)")))
                           }).ToList();


            using (var db = new TimeContext())
            {
                foreach (var row in newSort)
                {
                   
                    InputClass.addProduct(row.Product, row.workcenter);
                    InputClass.addOption(optionName, row.Sum);
                    ProdTB.AddOption(row.Product, row.workcenter, optionName, row.Sum);
                }



              

            }


        }
    }

    class MKits : Program
    {
        public MKits(string paramFilePath)
        {
            string optionName = "MKits";

            var dt = ConvertCSVtoDataTable(paramFilePath);
            var newSort = (from row in dt.AsEnumerable()

                           group row by new
                           {
                               ID = row.Field<string>("Fixture Type"),
                               Mouting = row.Field<string>("Mounting Suspension"),

                               Workcenter = row.Field<string>("Work Center")
                           } into grp
                           //orderby grp.Key
                           select new
                           {
                               Fixture = grp.Key.ID,
                               mounting = grp.Key.Mouting,

                               workcenter = grp.Key.Workcenter,
                               Sum = grp.Sum(r => Double.Parse(r.Field<string>("Total Time (min)")))
                           }).ToList();


            using (var db = new TimeContext())
            {
                foreach (var row in newSort)
                {

                    InputClass.addFixture(row.Fixture, row.workcenter);
                    InputClass.addOption(optionName, row.Sum);
                    InputClass.addParam("Mounting", row.mounting);

                    OptionTB.AddParam(optionName, row.Sum, "Mounting", row.mounting);
                    FixtureTB.AddOption(row.Fixture, row.workcenter, optionName, row.Sum);

                }


            }


        }
    }

    class SubEndP : Program
    {
        public SubEndP(string paramFilePath)
        {
            string optionName = "SubEndP";
            var dt = ConvertCSVtoDataTable(paramFilePath);
            var newSort = (from row in dt.AsEnumerable()

                           group row by new
                           {
                               ID = row.Field<string>("Product ID"),
                               Mouting = row.Field<string>("Description"),
                               Workcenter = row.Field<string>("Work Center")
                           } into grp
                           //orderby grp.Key
                           select new
                           {
                               Product = grp.Key.ID,
                               mounting = grp.Key.Mouting,
                               workcenter = grp.Key.Workcenter,
                               Sum = grp.Sum(r => Double.Parse(r.Field<string>("Total Time (min)")))
                           }).ToList();
            using (var db = new TimeContext())
            {
                foreach (var row in newSort)
                {
                    var mouting = row.mounting;
                    switch (mouting)
                    {
                        case "Spackle Flange":
                            mouting = "DS";
                            break;
                        case "Flangeless":
                            mouting = "D";
                            break;
                        default:
                            mouting = "-";
                            break;
                    }
                  

                    InputClass.addProduct(row.Product, row.workcenter);
                    InputClass.addOption(optionName, row.Sum);
                    InputClass.addParam("Mounting", mouting);

                    ProdTB.AddOption(row.Product, row.workcenter, optionName, row.Sum);
                   
                    OptionTB.AddParam(optionName, row.Sum, "Mounting", mouting);


                }

            }




        }
    }

    class Optic : Program
    {
        public Optic(string paramFilePath)
        {
            string optionName = "Optics";
            var dt = ConvertCSVtoDataTable(paramFilePath);
            var newSort = (from row in dt.AsEnumerable()

                           group row by new
                           {
                               ID = row.Field<string>("Family Product"),
                               Optic = row.Field<string>("Optic"),
                               Length = row.Field<string>("Length"),
                               Workcenter = row.Field<string>("Work Center")
                           } into grp
                           select new
                           {
                               ProdFam = grp.Key.ID,
                               Optic = grp.Key.Optic,
                               Length = grp.Key.Length,
                               workcenter = grp.Key.Workcenter,
                               Sum = grp.Sum(r => Double.Parse(r.Field<string>("Time (min)")))
                           }).ToList();

            using (var db = new TimeContext())
            {
                foreach (var row in newSort)
                {

                    InputClass.addProdFam(row.ProdFam, row.workcenter);
                    int length = Int32.Parse(row.Length);
                    InputClass.addOption(optionName, row.Sum, length);

                    InputClass.addParam("Optics", row.Optic);


                    OptionTB.AddParam(optionName, row.Sum, "Optics", row.Optic);
                    ProdFamTB.AddOption(row.ProdFam, row.workcenter, optionName, row.Sum, length);

                    db.SaveChanges();
                }





            }
        }

    }

    class MSTime : Program
    {
        public MSTime(string paramFilePath)
        {
            string optionName = "MSTime";
            var dt = ConvertCSVtoDataTable(paramFilePath);
            var newSort = (from row in dt.AsEnumerable()

                           group row by new
                           {
                               ID = row.Field<string>("Product ID"),
                               Mouting = row.Field<string>("Description"),
                               Length = row.Field<string>("Length"),
                               Workcenter = row.Field<string>("Work Center")
                           } into grp
                           //orderby grp.Key
                           select new
                           {
                               Product = grp.Key.ID,
                               mounting = grp.Key.Mouting,
                               length = grp.Key.Length,
                               workcenter = grp.Key.Workcenter,
                               Sum = grp.Sum(r => Double.Parse(r.Field<string>("Total Time (min)")))
                           }).ToList();


            using (var db = new TimeContext())
            {
                foreach (var row in newSort)
                {
                    var mouting = row.mounting;
                    switch (mouting)
                    {
                        case "Spackle Flange":
                            mouting = "DS";
                            break;
                        case "Flangeless":
                            mouting = "D";
                            break;
                        default:
                            mouting = "-";
                            break;
                    }
                    int length = Int32.Parse(row.length);
                  
                        InputClass.addProduct(row.Product, row.workcenter);
                        InputClass.addOption(optionName, row.Sum, length);
                        InputClass.addParam("Mounting", mouting);
                        ProdTB.AddOption(row.Product, row.workcenter, optionName, row.Sum, length);
                        OptionTB.AddParam(optionName, row.Sum, "Mounting", mouting);

                       
                    


                }











            }
        }
    }

    class CartBoard : Program
    {

        public static Session epiSession = new Session("Dang", "Lebaodang96!", "net.tcp://EPICORERP/Epicor10Test", Session.LicenseType.Default, @"C:\Epicor\ERP10.1Client\Client\Config\Epicor10Test.sysconfig");
        public static UD03Impl UD03BO = WCFServiceSupport.CreateImpl<UD03Impl>((Ice.Core.Session)epiSession, "Ice/BO/UD03");
        public CartBoard(string paramFilePath)
        {
            string optionName = "CartBoard";

            var dt = ConvertCSVtoDataTable(paramFilePath);
            var newSort = (from row in dt.AsEnumerable()

                           group row by new
                           {
                               ID = row.Field<string>("Cartridge/Board"),
                               Desc = row.Field<string>("Description"),
                               Type = row.Field<string>("Type"),
                               Workcenter = row.Field<string>("Work Center")
                           } into grp
                           //orderby grp.Key
                           select new
                           {
                               cartboard = grp.Key.ID,
                               desc = grp.Key.Desc,
                               type = grp.Key.Type,
                               workcenter = grp.Key.Workcenter,
                               Sum = grp.Sum(r => Double.Parse(r.Field<string>("Total Time")))
                           }).ToList();
            bool morepage;
            var products = UD03BO.GetRows(@"Key1 = 'PRODUCTID'", "", 0, 0, out morepage);
            foreach (var row in newSort)
            {

                var prodlist = (from myRow in products.UD03.AsEnumerable()
                                where myRow.Field<string>("CartBoard_c") == row.cartboard
                                && myRow.Field<string>("Direct_C") == row.desc
                                select myRow.Field<string>("Key2")).ToList();


                foreach (var prod in prodlist)
                {

                    using (var db = new TimeContext())
                    {

                      

                        InputClass.addProduct(prod, row.workcenter);
                        InputClass.addOption(optionName, row.Sum);
                        InputClass.addParam("SectionType", row.type);
                        ProdTB.AddOption(prod, row.workcenter, optionName, row.Sum);
                        OptionTB.AddParam(optionName, row.Sum, "SectionType", row.type);

                       
                    }
                }
            }
        }
    }
}
