using Axis_ProdTimeDB.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis_ProdTimeDB.InputClasses
{
    class PowerCord :Utilities
    {
        public PowerCord(string paramFilePath)
        {
           
            string optionName = this.GetType().Name;
            var dt = ConvertCSVtoDataTable(paramFilePath);
            var newSort = (from row in dt.AsEnumerable()

                           group row by new
                           {
                             
                                wiresQty = row.Field<string>("Wires Qty"),
                               Workcenter = row.Field<string>("Work Center")
                           } into grp
                           select new
                           {
                            workcenter = grp.Key.Workcenter,
                            wires = grp.Key.wiresQty,
                               Sum = grp.Sum(r => Double.Parse(r.Field<string>("Time (min)")))
                           }).ToList();


            foreach (var row in newSort)
            {

                ParametersTB.AddInstance("WiresQty", row.wires);


                OptionTB.AddInstance(optionName, row.Sum);
                OptionTB.AddParam(optionName, row.Sum, "WiresQty", row.wires);
                using (var db = new TimeContext())
                {

                    foreach(var fixture in db.Prod.Where(r => r.Type == fixturetype).ToList())
                    {
                        ProdTB.AddOption(fixturetype, fixture.Code, row.workcenter, optionName, row.Sum);
                    }
                }
            }






        }
    }
    
}
