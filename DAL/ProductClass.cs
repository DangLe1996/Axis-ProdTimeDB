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
        public ProductClass(string ProductCode)
        {

            var DriveWorkdb = new AXISAutomation.Tools.DBConnection.AXIS_AutomationEntities();
            this.BOM = new _BOM(ProductCode, DriveWorkdb);
            bool morepage;
            var BO = UD30BO.GetRows(string.Format("Key1 = 'PRODUCTID' and key2 = '{0}'", this.BOM.GetselectedproductID_Category), "", 0, 0, out morepage);

            string productCode = BO.UD03[0]["Key2"].ToString();
            string fixtureCode = BO.UD03[0]["Character03"].ToString();
            string ProdFamCode = BO.UD03[0]["Key3"].ToString();

           
            string optic = this.BOM.GetSelectedOpticsDirect_Category.ToString();
            string mouting = this.BOM.GetSelectedMounting_Category.ToString();

            var x = (this.BOM.GetSelectedLength_Category.ToString()).Replace("'", "");
            int length = Int32.Parse(x);
            
            lengthParam.Add(null);
            lengthParam.Add(length);

            ParamValue.AddRange(new List<string>
            {
                optic,
                mouting,
                "-",
                "C6-2"
            });


            using (var db = new TimeContext())
            {

                List<FixtureTB> fixture = db.Fixtures.Where(item => item.FxCode == fixtureCode).ToList();
                List<ProdTB> product = db.Prod.Where(item => item.ProdCode == productCode).ToList();
                List<ProdFamTB> prodfam = db.ProdFam.Where(item => item.FamCode == ProdFamCode).ToList();

                var ParamNames = db.Params.Select(m => m.ParamName).Distinct().ToList();
                this.ParamNames = ParamNames;


                foreach (var workcenter in product)
                {
                    ProdTime[workcenter.WorkCenter.ToString()] = 0;
                   
                    List<string> usedOption = new List<string>();

                    foreach (var item in workcenter.Options.Where(item => !usedOption.Contains(item.OptionName)
                    && lengthParam.Contains(item.sectionLength)))
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
                    && lengthParam.Contains(item.sectionLength)))
                    {
                       

                        var check = item.Params.Where(r => this.ParamNames.Contains(r.ParamName) && ParamValue.Contains(r.ParamValue)).ToList();
                        if (check.Count > 0)
                        {
                            usedOption.Add(item.OptionName);
                            if (!ProdTime.Keys.Contains(workcenter.WorkCenter.ToString()))
                            {
                                ProdTime[workcenter.WorkCenter.ToString()] = item.ProdTime;
                            }
                            else
                            {

                                ProdTime[workcenter.WorkCenter.ToString()] += item.ProdTime;
                            }
                        }


                    }

                }
                foreach (var workcenter in prodfam)
                {
                   

                    List<string> usedOption = new List<string>();

                    foreach (var item in workcenter.Options.Where(item => !usedOption.Contains(item.OptionName)
                    && lengthParam.Contains(item.sectionLength)))
                    {
                       

                        var check = item.Params.Where(r => this.ParamNames.Contains(r.ParamName) && ParamValue.Contains(r.ParamValue)).ToList();
                        if (check.Count > 0)
                        {
                            usedOption.Add(item.OptionName);
                            if (!ProdTime.Keys.Contains(workcenter.WorkCenter.ToString()))
                            {
                                ProdTime[workcenter.WorkCenter.ToString()] = item.ProdTime;
                            }
                            else
                            {

                                ProdTime[workcenter.WorkCenter.ToString()] += item.ProdTime;
                            }
                        }


                    }

                }




            }



        }
        

    }


   
}
