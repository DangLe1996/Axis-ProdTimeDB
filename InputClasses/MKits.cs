using Axis_ProdTimeDB.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis_ProdTimeDB.InputClasses
{
    //class MKits:Program
    //{
    //    public  MKits(string paramFilePath)
    //    {
    //        string optionName = "MKits";
            
    //        var dt = ConvertCSVtoDataTable(paramFilePath);
    //        var newSort = (from row in dt.AsEnumerable()

    //                       group row by new
    //                       {
    //                           ID = row.Field<string>("Fixture Type"),
    //                           Mouting = row.Field<string>("Mounting Suspension"),

    //                           Workcenter = row.Field<string>("Work Center")
    //                       } into grp
    //                       //orderby grp.Key
    //                       select new
    //                       {
    //                           Fixture = grp.Key.ID,
    //                           mounting = grp.Key.Mouting,

    //                           workcenter = grp.Key.Workcenter,
    //                           Sum = grp.Sum(r => Double.Parse(r.Field<string>("Total Time (min)")))
    //                       }).ToList();


    //        using (var db = new TimeContext())
    //        {
    //            foreach (var row in newSort)
    //            {
    //                var fixture = db.Fixtures.Where(item => item.FxCode == row.Fixture && item.WorkCenter == row.workcenter).FirstOrDefault();
    //                if (fixture == null)
    //                {
    //                    db.Fixtures.Add(new FixtureTB
    //                    {
    //                        FxCode = row.Fixture,
    //                        WorkCenter = row.workcenter
    //                    });
    //                }

    //                var optionindex = db.Options.Where(item => item.OptionName == optionName && item.ProdTime == row.Sum).FirstOrDefault();
    //                if (optionindex == null)
    //                {
    //                    db.Options.Add(new OptionTB
    //                    {
    //                        OptionName = optionName,
    //                        ProdTime = row.Sum
    //                    });
    //                }

    //                var paramindex = db.Params.Where(item => item.ParamName == "Mounting" && item.ParamValue == row.mounting).FirstOrDefault();
    //                if (paramindex == null)
    //                {
    //                    db.Params.Add(new ParametersTB { ParamName = "Mounting", ParamValue = row.mounting });
    //                }




    //                db.SaveChanges();
    //            }



    //            foreach (var row in newSort)
    //            {
    //                var fixture = db.Fixtures.Where(item => item.FxCode == row.Fixture && item.WorkCenter == row.workcenter).FirstOrDefault();
    //                var optionindex = db.Options.Where(item => item.OptionName == optionName && item.ProdTime == row.Sum).FirstOrDefault();


    //                if (!fixture.Options.Contains(optionindex))
    //                    fixture.Options.Add(optionindex);

    //                var paramindex = db.Params.Where(item => item.ParamName == "Mounting" && item.ParamValue == row.mounting).FirstOrDefault();
    //                if (!optionindex.Params.Contains(paramindex))
    //                    optionindex.Params.Add(paramindex);


    //                db.SaveChanges();
    //            }

    //        }


    //    }
    //}
}
