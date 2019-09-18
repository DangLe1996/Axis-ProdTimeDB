using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axis_ProdTimeDB.DAL;
using System.Data;
using System.Transactions;

namespace Axis_ProdTimeDB.InputClasses
{
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

                    using (var tran = db.Database.BeginTransaction())
                    {
                        var prod = db.Prod.Where(item => item.ProdCode == row.Product && item.WorkCenter == row.workcenter).FirstOrDefault();
                    if (prod == null)
                    {
                        db.Prod.Add(new ProdTB
                        {
                            ProdCode = row.Product,
                            WorkCenter = row.workcenter
                        });
                    }

                    var optionindex = db.Options.Where(item => item.OptionName == optionName && item.ProdTime == row.Sum).FirstOrDefault();
                    if (optionindex == null)
                    {
                        db.Options.Add(new OptionTB
                        {
                            OptionName = optionName,
                            ProdTime = row.Sum,
                            sectionLength = Int32.Parse( row.length)
                            
                        });
                    }

                    
                        var paramindex = db.Params.Where(item => item.ParamName == "Mounting" && item.ParamValue == mouting).FirstOrDefault();
                        if (paramindex == null)
                        {
                            db.Params.Add(new ParametersTB { ParamName = "Mounting", ParamValue = mouting });
                        }

                        db.SaveChanges();
                        tran.Commit();
                    }

                    using (var tran = db.Database.BeginTransaction())
                    {
                        var prod = db.Prod.Where(item => item.ProdCode == row.Product && item.WorkCenter == row.workcenter).FirstOrDefault();
                        int length = Int32.Parse(row.length);
                        var optionindex = db.Options.Where(item => item.OptionName == optionName && item.ProdTime == row.Sum && item.sectionLength == length).FirstOrDefault();
                        prod = db.Prod.Where(item => item.ProdCode == row.Product && item.WorkCenter == row.workcenter).FirstOrDefault();
                        optionindex = db.Options.Where(item => item.OptionName == optionName && item.ProdTime == row.Sum).FirstOrDefault();


                        if (!prod.Options.Contains(optionindex))
                        {
                            prod.Options.Add(optionindex);
                            db.SaveChanges();
                        }
                   
                        var paramindex = db.Params.Where(item => item.ParamName == "Mounting" && item.ParamValue == mouting).FirstOrDefault();
                        if (!optionindex.Params.Contains(paramindex))
                        {
                            optionindex.Params.Add(paramindex);
                            db.SaveChanges();
                        }
                        tran.Commit();
                    }


                }











            }
        }
    }
}
