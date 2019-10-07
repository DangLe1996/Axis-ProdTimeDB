using Axis_ProdTimeDB.DAL;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
namespace Axis_ProdTimeDB
{

    public class ProdTB
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Type { get; set; }
        public string Code { get; set; }
       
        public ProdTB GetProd(string ID, string type, string workcenter)
        {
            using (var db = new TimeContext())
            {

                ProdTB prod = db.Prod.Where(item => item.Code == ID && item.Type == type && item.WorkCenter == workcenter).FirstOrDefault();
                return prod;
            }

                
        }
        public string OpCode { get; set; }
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
       
        public static void AddOption(string type, string ID, string workcenter, string optionName, double prodTime, int? SectionLength = null)
        {
            using (var db = new TimeContext())
            {

                var prod = db.Prod.Where(item => item.Code == ID && item.Type == type && item.WorkCenter == workcenter).FirstOrDefault();

                if(prod == null)
                {
                    ProdTB.AddInstance(type, ID, workcenter);
                }
                prod = db.Prod.Where(item => item.Code == ID && item.Type == type && item.WorkCenter == workcenter).FirstOrDefault();

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
        public static void AddInstance(string type, string ID, string workCenter)
        {
            using (var db = new TimeContext())
            {
                var prod = db.Prod.Where(item => item.Code == ID && item.Type == type && item.WorkCenter == workCenter).FirstOrDefault();
                if (prod == null)
                {
                    db.Prod.Add(new ProdTB
                    {
                        Code = ID,
                        Type = type,
                        WorkCenter = workCenter
                    });
                }
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
   



        public virtual ICollection<ProdTB> Prods { get; set; }
        

        public static void AddParam(string optionName, double prodTime, string ParamName, string ParamValue, int? SectionLength = null)
        {
            using (var db = new TimeContext())
            {
                OptionTB optionindex = new OptionTB();
                if (SectionLength == null)
                {

                    optionindex = db.Options.Where(item => item.OptionName == optionName && item.ProdTime == prodTime).FirstOrDefault();


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


        //public virtual Parameter ParameterRef { get; set; }


    


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


