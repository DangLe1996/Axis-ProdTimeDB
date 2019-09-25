using Axis_ProdTimeDB.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis_ProdTimeDB.InputClasses
{
    class Housing : Program
    {
        public Housing(string paramFilePath)
        {
            string optionName = this.GetType().Name;
            var dt = ConvertCSVtoDataTable(paramFilePath);
            var newSort = (from row in dt.AsEnumerable()

                           group row by new
                           {
                               ID = row.Field<string>("Product ID"),
                               Length = row.Field<string>("Length"),
                               mounting = row.Field<string>("Flange"),
                               section = row.Field<string>("Section"),
                               Workcenter = row.Field<string>("Work Center")
                           } into grp
                           //orderby grp.Key
                           select new
                           {
                               Product = grp.Key.ID,
                               length = Int32.Parse(grp.Key.Length),
                               workcenter = grp.Key.Workcenter,
                               section = grp.Key.section,
                               mounting = grp.Key.mounting,
                               Sum = grp.Sum(r => Double.Parse(r.Field<string>("Time(min)")))
                           }).ToList();

            foreach (var row in newSort)
            {
                var mounting = row.mounting;
                switch (mounting)
                {
                    case "Spackle Flange":
                        mounting = "DS";
                        break;
                    case "Flangeless":
                        mounting = "D";
                        break;
                    default:
                        mounting = null;
                        break;
                }

                OptionTB.AddInstance(optionName, row.Sum, row.length);
                ProdTB.AddInstance(prodtype,row.Product, row.workcenter);
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("Section", row.section);
                if(mounting != null) parameters.Add("Mounting", mounting);


                foreach (var instace in parameters)
                {
                    ParametersTB.AddInstance(instace.Key, instace.Value);
                    OptionTB.AddParam(optionName, row.Sum, instace.Key, instace.Value, row.length);
                }


                ProdTB.AddOption(prodtype,row.Product, row.workcenter, optionName, row.Sum, row.length);

            }
        }
    }
}
