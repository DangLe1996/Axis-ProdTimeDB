using Axis_ProdTimeDB.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis_ProdTimeDB.InputClasses
{
    class OpTest : Program
    {
        public OpTest(string paramFilePath)
        {
            string optionName = this.GetType().Name;
            var dt = ConvertCSVtoDataTable(paramFilePath);

            var newSort = (from row in dt.AsEnumerable()

                           group row by new
                           {
                               ID = row.Field<string>("Fixture Type"),
                                light = row.Field<string>("Lighting"),
                               citcuit = row.Field<string>("Circuit"),
                               LampQty = row.Field<string>("Lamp/Cartridge Qty"),
                               Length = row.Field<string>("Length"),
                               Workcenter = row.Field<string>("Work Center")
                           } into grp
                           //orderby grp.Key
                           select new
                           {
                               Fixture = grp.Key.ID,
                               light = grp.Key.light,
                               circuit = grp.Key.citcuit,
                               lampQty = grp.Key.LampQty,
                               length =  Int32.Parse(grp.Key.Length),
                               workcenter = grp.Key.Workcenter,
                               Sum = grp.Sum(r => Double.Parse(r.Field<string>("Time (min)")))
                           }).ToList();



            foreach (var row in newSort)
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                FixtureTB.AddInstance(row.Fixture, row.workcenter);
                OptionTB.AddInstance(optionName, row.Sum, row.length);
                parameters.Add("Circuit", row.circuit);
                parameters.Add("LampQty", row.lampQty);
                parameters.Add("Light", row.light);
                foreach(var value in parameters)
                {
                    ParametersTB.AddInstance(value.Key, value.Value);
                    OptionTB.AddParam(optionName, row.Sum, value.Key, value.Value, row.length);
                }
               
                FixtureTB.AddOption(row.Fixture, row.workcenter, optionName, row.Sum);


            }


            


        }
    }
}
