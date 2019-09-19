using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axis_ProdTimeDB.DAL;
using System;
using Ice.Proxy.BO;
using Ice.Core;
using Ice.Lib.Framework;
using System.Data;

namespace Axis_ProdTimeDB.InputClasses
{
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



                        ProdTB.AddInstance(prod, row.workcenter);
                        OptionTB.AddInstance(optionName, row.Sum);
                        ParametersTB.AddInstance("Type", row.type);
                        ProdTB.AddOption(prod, row.workcenter, optionName, row.Sum);
                        OptionTB.AddParam(optionName, row.Sum, "Type", row.type);


                    }
                }
            }
        }
    }
}
