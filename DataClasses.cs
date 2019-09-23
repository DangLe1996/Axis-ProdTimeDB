using Axis_ProdTimeDB.DAL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AXISAutomation;
namespace Axis_ProdTimeDB
{

    public class ProdFamTB
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string FamCode { get; set; }
        public string OpCode { get; set; }
        public string OpDesc { get; set; }
        public string WorkCenter { get; set; }

        public static void AddInstance(string FamCode, string workcenter)
        {
            
                using (var db = new TimeContext())
                {
                    var prod = db.ProdFam.Where(item => item.FamCode == FamCode && item.WorkCenter == workcenter).FirstOrDefault();
                    if (prod == null)
                    {
                        db.ProdFam.Add(new ProdFamTB
                        {
                            FamCode = FamCode,
                            WorkCenter = workcenter
                        });
                    }
                    db.SaveChanges();
                }
            
        }

        private ICollection<OptionTB> _Options;
        public virtual ICollection<OptionTB> Options
        {
            get
            {
                return _Options ?? (_Options = new Collection<OptionTB>());
            }

            set { _Options = value; }
        }
        public void AddOption(OptionTB option)
        {
            Options.Add(option);
        }

        public static void AddOption(string FamCode, string workcenter, string optionName, double prodTime, int? SectionLength = null)
        {
            using (var db = new TimeContext())
            {

                var prodfam = db.ProdFam.Where(item => item.FamCode == FamCode && item.WorkCenter == workcenter).FirstOrDefault();
                if(prodfam == null)
                {
                    ProdFamTB.AddInstance(FamCode, workcenter);
                }
                prodfam = db.ProdFam.Where(item => item.FamCode == FamCode && item.WorkCenter == workcenter).FirstOrDefault();
                var optionindex = db.Options.Where(item => item.OptionName == optionName && item.ProdTime == prodTime
                && item.sectionLength == SectionLength).FirstOrDefault();

                if (!prodfam.Options.Contains(optionindex))
                    prodfam.Options.Add(optionindex);

                db.SaveChanges();

            }
        }
    }


    public class ProdTB
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ProdCode { get; set; }
        public string OpCode { get; set; }
        public string OpDesc { get; set; }
        public string WorkCenter { get; set; }


        private ICollection<OptionTB> _Options;
        public virtual ICollection<OptionTB> Options
        {
            get
            {
                return _Options ?? (_Options = new Collection<OptionTB>());
            }

            set { _Options = value; }
        }
        public void AddOption(OptionTB option)
        {
            Options.Add(option);
        }
        public static void AddInstance(string prodCode, string workCenter)
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
            public static void AddOption(string product, string workcenter, string optionName, double prodTime, int? SectionLength = null)
        {
            using (var db = new TimeContext())
            {

                var prod = db.Prod.Where(item => item.ProdCode == product && item.WorkCenter == workcenter).FirstOrDefault();


                OptionTB optionindex = new OptionTB();
                if (SectionLength == null)
                {

                    optionindex = db.Options.Where(item => item.OptionName == optionName && item.ProdTime == prodTime).FirstOrDefault();


                }
                else
                {
                    optionindex = db.Options.Where(item => item.OptionName == optionName && item.ProdTime == prodTime && item.sectionLength == SectionLength).FirstOrDefault();

                }


                if (!prod.Options.Contains(optionindex))
                    prod.Options.Add(optionindex);

                db.SaveChanges();

            }
        }

    }
    public class FixtureTB
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string FxCode { get; set; }
        public string OpCode { get; set; }
        public string OpDesc { get; set; }
        public string WorkCenter { get; set; }

        public static void AddInstance(string FixtureCode, string WorkCenter)
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


        private ICollection<OptionTB> _Options;
        public virtual ICollection<OptionTB> Options
        {
            get
            {
                return _Options ?? (_Options = new Collection<OptionTB>());
            }

            set { _Options = value; }
        }
        public void AddOption(OptionTB option)
        {
            Options.Add(option);
        }

        public static void AddOption(string fixtureID, string workcenter, string optionName, double prodTime, int? SectionLength = null)
        {
            using (var db = new TimeContext())
            {
                var fixture = db.Fixtures.Where(item => item.FxCode == fixtureID && item.WorkCenter == workcenter).FirstOrDefault();
                if(fixture == null)
                {
                    FixtureTB.AddInstance(fixtureID, workcenter);
                }
                fixture = db.Fixtures.Where(item => item.FxCode == fixtureID && item.WorkCenter == workcenter).FirstOrDefault();
                OptionTB optionindex = new OptionTB();
                if (SectionLength == null)
                {

                    optionindex = db.Options.Where(item => item.OptionName == optionName && item.ProdTime == prodTime).FirstOrDefault();


                }
                else
                {
                    optionindex = db.Options.Where(item => item.OptionName == optionName && item.ProdTime == prodTime && item.sectionLength == SectionLength).FirstOrDefault();

                }


                if (!fixture.Options.Contains(optionindex))
                    fixture.Options.Add(optionindex);

                db.SaveChanges();

            }
        }
    }


    public class OptionTB
    {


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OptionID { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string OptionName { get; set; }
        //public string? sectionType { get; set; }
        public int? sectionLength { get; set; }
        public double ProdTime { get; set; }


        private ICollection<ParametersTB> _Params;
        public virtual ICollection<ParametersTB> Params
        {
            get
            {
                return _Params ?? (_Params = new Collection<ParametersTB>());
            }

            set { _Params = value; }
        }
        public void AddParams(ParametersTB param)
        {
            param.AddOptions(this);
            Params.Add(param);

        }



        public virtual ICollection<ProdTB> Prods { get; set; }
        public virtual ICollection<FixtureTB> Fxs { get; set; }

        public virtual ICollection<ProdFamTB> ProdFam { get; set; }

        public static void AddParam(string optionName, double prodTime, string ParamName, string ParamValue, int? SectionLength = null)
        {
            using (var db = new TimeContext())
            {
                OptionTB optionindex = new OptionTB();
                if (SectionLength == null) {

                     optionindex = db.Options.Where(item => item.OptionName == optionName && item.ProdTime == prodTime ).FirstOrDefault();


                }
                else
                {
                     optionindex = db.Options.Where(item => item.OptionName == optionName && item.ProdTime == prodTime && item.sectionLength == SectionLength).FirstOrDefault();

                }
                var paramindex = db.Params.Where(item => item.ParamName == ParamName && item.ParamValue == ParamValue).FirstOrDefault();
                if (!optionindex.Params.Contains(paramindex))
                {
                    optionindex.Params.Add(paramindex);
                    db.SaveChanges();
                }

            }
        }

        public static void AddInstance(string optionName, double ProdTime, int? SectionLength = null)
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





    }

    public class ParametersTB
    {


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ParamsID { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ParamName { get; set; }
        public string ParamValue { get; set; }

        public Parameter parameter { get; set; }

        //public Category category { get; set; }

        private ICollection<OptionTB> _Options;
        public virtual ICollection<OptionTB> Options
        {
            get
            {
                return _Options ?? (_Options = new Collection<OptionTB>());
            }

            set { _Options = value; }
        }
        public void AddOptions(OptionTB Option)
        {
            Options.Add(Option);
        }

        public static void AddInstance(string ParamName, string ParamValue)
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


    }



}
