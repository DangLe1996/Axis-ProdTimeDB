using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ice.Proxy.BO;
using Ice.Core;
using Ice.Lib.Framework;
using AXISAutomation;
using AXISAutomation.Solvers.BOM;
using System.Data;
using System.Text.RegularExpressions;


namespace Axis_ProdTimeDB.DAL
{

    class ProductClass
    {
        private AXISAutomation.Solvers.BOM._BOM BOM { get; set; }

        public static Session epiSession = new Session("Dang", "Lebaodang96!", "net.tcp://EPICORERP/Epicor10Test", Session.LicenseType.Default, @"C:\Epicor\ERP10.1Client\Client\Config\Epicor10Test.sysconfig");
        public static UD03Impl UD30BO = WCFServiceSupport.CreateImpl<UD03Impl>((Ice.Core.Session)epiSession, "Ice/BO/UD03");

        public Dictionary<string, double> ProdTime = new Dictionary<string, double>();
        private List<string> ParamNames = new List<string>();
        private List<string> ParamValue = new List<string>();
        private List<int?> lengthParam = new List<int?>();
        private string fixtureCode;
        private string productCode;
        private string ProdFamCode;
        public ProductClass(string ProductCode)
        {

            var DriveWorkdb = new AXISAutomation.Tools.DBConnection.AXIS_AutomationEntities();
            this.BOM = new _BOM(ProductCode, DriveWorkdb);
            bool morepage;
            var BO = UD30BO.GetRows(string.Format("Key1 = 'PRODUCTID' and key2 = '{0}'", this.BOM.GetselectedproductID_Category), "", 0, 0, out morepage);

            this.productCode = BO.UD03[0]["Key2"].ToString();
            this.fixtureCode = BO.UD03[0]["Character03"].ToString();
            this.ProdFamCode = BO.UD03[0]["Key3"].ToString();


            string optic = this.BOM.GetSelectedOpticsDirect_Category.ToString();
            string mounting = this.BOM.GetSelectedMounting_Category.ToString();
            string driver = this.BOM.GetSelectedDriver_Category;
            string defaultMounting = "-";
            var x = (this.BOM.GetSelectedLength_Category.ToString()).Replace("'", "");
            int length = Int32.Parse(x);

            lengthParam.Add(null);




            List<string> sectionType = new List<string>();

            if (length < 12)
            {
                lengthParam.Add(length);
                sectionType.Add("Complete Section");
                using (var db = new TimeContext())
                {
                    int dummy;
                    var lengthtest = db.Options.Where(r => r.OptionName == "Length" && r.sectionLength == length).FirstOrDefault();
                    var cartridge = (from row in lengthtest.Params where !int.TryParse(row.ParamValue, out dummy) select row.ParamValue).ToList();
                    sectionType.AddRange(cartridge);

                }
            }
            else
            {
                using (var db = new TimeContext())
                {
                    int dummy;
                    var lengthtest = db.Options.Where(r => r.OptionName == "Length" && r.sectionLength == length).FirstOrDefault();
                    var cartridge = (from row in lengthtest.Params where !int.TryParse(row.ParamValue, out dummy) select row.ParamValue).ToList();
                    List<int> Length = (from row in lengthtest.Params where int.TryParse(row.ParamValue, out dummy) select Int32.Parse(row.ParamValue)).ToList();
                    sectionType.AddRange(cartridge);
                    var newList = Length.Select(i => (int?)i).ToList();
                    lengthParam.AddRange(newList);
                }

            }


            ParamValue.AddRange(new List<string>
            {
                optic,
                mounting,
                defaultMounting,
                driver, //Driver
               
            });
            ParamValue.AddRange(sectionType);
            this.Initial();
            this.retrieveTime();
        }



        private void Initial()
        {
            using (var db = new TimeContext())
            {

                List<FixtureTB> fixture = db.Fixtures.Where(item => item.FxCode == this.fixtureCode).ToList();
                List<ProdTB> product = db.Prod.Where(item => item.ProdCode == this.productCode).ToList();
                List<ProdFamTB> prodfam = db.ProdFam.Where(item => item.FamCode == this.ProdFamCode).ToList();



                foreach (var workcenter in product)
                {
                    ProdTime[workcenter.WorkCenter.ToString()] = 0;

                }
                foreach (var workcenter in fixture)
                {
                    ProdTime[workcenter.WorkCenter.ToString()] = 0;

                }
                foreach (var workcenter in prodfam)
                {
                    ProdTime[workcenter.WorkCenter.ToString()] = 0;

                }

            }
        }

        private void retrieveTime()
        {
            for (int i = 0; i < this.lengthParam.Count; i++ )
            {
                if (this.lengthParam[i] <= 4) this.lengthParam[i] = 4;
                else if (this.lengthParam[i] <= 8) this.lengthParam[i] = 8;
                else  this.lengthParam[i] = 12;
            }


                foreach (var length in this.lengthParam)
            {
               

                using (var db = new TimeContext())
                {

                    List<FixtureTB> fixture = db.Fixtures.Where(item => item.FxCode == this.fixtureCode).ToList();
                    List<ProdTB> product = db.Prod.Where(item => item.ProdCode == this.productCode).ToList();
                    List<ProdFamTB> prodfam = db.ProdFam.Where(item => item.FamCode == this.ProdFamCode).ToList();

                    var ParamNames = db.Params.Select(m => m.ParamName).Distinct().ToList();
                    this.ParamNames = ParamNames;


                    foreach (var workcenter in product)
                    {


                        List<string> usedOption = new List<string>();

                        foreach (var item in workcenter.Options.Where(item => !usedOption.Contains(item.OptionName)
                        && item.sectionLength == length))
                        {

                            if (item.Params.Count == 0)
                            {
                                usedOption.Add(item.OptionName);
                                ProdTime[workcenter.WorkCenter.ToString()] += item.ProdTime;
                                continue;
                            }


                            var check = item.Params.Where(r => this.ParamNames.Contains(r.ParamName) && ParamValue.Contains(r.ParamValue)).ToList();
                            if (check.Count > 0)
                            {
                                usedOption.Add(item.OptionName);
                                ProdTime[workcenter.WorkCenter.ToString()] += item.ProdTime;
                            }


                        }

                    }

                    foreach (var workcenter in fixture)
                    {
                        List<string> usedOption = new List<string>();

                        foreach (var item in workcenter.Options.Where(item => !usedOption.Contains(item.OptionName)
                        && item.sectionLength == length))
                        {


                            var check = item.Params.Where(r => this.ParamNames.Contains(r.ParamName) && ParamValue.Contains(r.ParamValue)).ToList();
                            if (check.Count > 0)
                            {
                                usedOption.Add(item.OptionName);
                                ProdTime[workcenter.WorkCenter.ToString()] += item.ProdTime;
                                
                            }


                        }

                    }

                    foreach (var workcenter in prodfam)
                    {


                        List<string> usedOption = new List<string>();

                        foreach (var item in workcenter.Options.Where(item => !usedOption.Contains(item.OptionName)
                        && item.sectionLength == length))
                        {


                            var check = item.Params.Where(r => this.ParamNames.Contains(r.ParamName) && ParamValue.Contains(r.ParamValue)).ToList();
                            if (check.Count > 0)
                            {
                                usedOption.Add(item.OptionName);
                                ProdTime[workcenter.WorkCenter.ToString()] += item.ProdTime;
                                
                            }


                        }

                    }




                }

            }


        }
    }


   
}
