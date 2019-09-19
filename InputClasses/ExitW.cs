using Axis_ProdTimeDB.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis_ProdTimeDB.InputClasses
{
    class ExitW : Program
    {
        public ExitW(string paramFilePath)
        {
           
            string optionName = "ExitW";
            var dt = ConvertCSVtoDataTable(paramFilePath);
            var newSort = (from row in dt.AsEnumerable()

                           group row by new
                           {
                               ID = row.Field<string>("Fixture Type"),
                               Section = row.Field<string>("Section"),
                               Circtuit = row.Field<string>("Circuit"),
                               Ballast = row.Field<string>("Kind Of Ballast/Driver"),
                       
                               Workcenter = row.Field<string>("Work Center")
                           } into grp
                           select new
                           {
                               fixture = grp.Key.ID,
                               Section = grp.Key.Section,
                               Circtuit = grp.Key.Circtuit,
                               workcenter = grp.Key.Workcenter,
                               Ballast = grp.Key.Ballast,
                              
                               Sum = grp.Sum(r => Double.Parse(r.Field<string>("Time (min)")))
                           }).ToList();

           
            foreach (var row in newSort)
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("Section", row.Section);
                parameters.Add("Circuit", row.Circtuit);
                parameters.Add("Ballast", row.Ballast);


                FixtureTB.AddInstance(row.fixture, row.workcenter);
                OptionTB.AddInstance(optionName, row.Sum);


                
                foreach (var instace in parameters)
                {
                    ParametersTB.AddInstance(instace.Key, instace.Value);
                    OptionTB.AddParam(optionName, row.Sum, instace.Key, instace.Value);
                }

                FixtureTB.AddOption(row.fixture, row.workcenter, optionName, row.Sum);



            }







        }
    }
    
}
