using Axis_ProdTimeDB.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis_ProdTimeDB.InputClasses
{
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
                    var prodFam = db.ProdFam.Where(item => item.FamCode == row.ProdFam && item.WorkCenter == row.workcenter).FirstOrDefault();
                    if (prodFam == null)
                    {
                        db.ProdFam.Add(new ProdFamTB
                        {
                            FamCode = row.ProdFam,
                            WorkCenter = row.workcenter
                        });
                    }
                    int length = Int32.Parse(row.Length);
                    var optionindex = db.Options.Where(item => item.OptionName == optionName && item.ProdTime == row.Sum && item.sectionLength == length).FirstOrDefault();
                    if (optionindex == null)
                    {
                        
                        db.Options.Add(new OptionTB
                        {
                            OptionName = optionName,
                            sectionLength = length,
                            ProdTime = row.Sum
                        });
                    }

                    var paramindex = db.Params.Where(item => item.ParamName == "Optic" && item.ParamValue == row.Optic).FirstOrDefault();
                    if (paramindex == null)
                    {
                        db.Params.Add(new ParametersTB { ParamName = "Optic", ParamValue = row.Optic });
                    }
                    db.SaveChanges();




                    prodFam = db.ProdFam.Where(item => item.FamCode == row.ProdFam && item.WorkCenter == row.workcenter).FirstOrDefault();
                    optionindex = db.Options.Where(item => item.OptionName == optionName && item.ProdTime == row.Sum && item.sectionLength == length).FirstOrDefault();
                    paramindex = db.Params.Where(item => item.ParamName == "Optic" && item.ParamValue == row.Optic).FirstOrDefault();

                    if (!prodFam.Options.Contains(optionindex))
                        prodFam.Options.Add(optionindex);

                    
                    if (!optionindex.Params.Contains(paramindex))
                        optionindex.Params.Add(paramindex);



                    db.SaveChanges();
                }

                



            }
        }

    }
}
