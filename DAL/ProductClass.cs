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

    class ProductClass:Program
    {
        private AXISAutomation.Solvers.BOM._BOM BOM { get; set; }

        public static Session epiSession = new Session("Dang", "Lebaodang96!", "net.tcp://EPICORERP/Epicor10Test", Session.LicenseType.Default, @"C:\Epicor\ERP10.1Client\Client\Config\Epicor10Test.sysconfig");
        public static UD03Impl UD30BO = WCFServiceSupport.CreateImpl<UD03Impl>((Ice.Core.Session)epiSession, "Ice/BO/UD03");

        public Dictionary<string, double> ProdTime = new Dictionary<string, double>();
        private List<string> ParamNames = new List<string>();
        private List<string> ParamValue = new List<string>();
        private List<int?> lengthParam = new List<int?>();


        private Dictionary<string, string> ParamInput = new Dictionary<string, string>();

        private Dictionary<string, List<string>> WiresQty = new Dictionary<string, List<string>>();


        private string fixtureCode;
        private string productCode;
        private string ProdFamCode;
        public ProductClass(string ProductCode)
        {

            var DriveWorkdb = new AXISAutomation.Tools.DBConnection.AXIS_AutomationEntities();
            this.BOM = new _BOM(ProductCode, DriveWorkdb);


            

                bool morepage;
                var BO = UD30BO.GetRows(string.Format("Key1 = 'PRODUCTID' and key2 = '{0}'", this.BOM.GetselectedproductID_Category), "", 0, 0, out morepage);

                ParamInput.Add("circuits", this.BOM.GetSelectedCircuits_Category.ToString());
                ParamInput.Add("Driver", this.BOM.GetSelectedDriver_Category.ToString());
                ParamInput.Add("Mounting", this.BOM.GetSelectedMounting_Category.ToString());

                WiresQty["3"] = new List<string>
            {
                "E",
                "ERS"
            };
                WiresQty["4"] = new List<string>
            {
                "BI"
            };
                WiresQty["5"] = new List<string>
            {
                "D",
                "DP",
                "MD"

            };
                WiresQty["6"] = new List<string>
            {
                "LT"
            };

                foreach (var row in WiresQty)
                {
                    if (row.Value.Contains(ParamInput["Driver"]))
                    {
                        ParamInput.Add("WiresQty", row.Key);
                        break;
                    }
                }

                this.ParamInput["LampQty"] = "1";


                if (this.BOM.GetSelectedOpticsDirect_Category != null) ParamInput.Add("Optics", this.BOM.GetSelectedOpticsDirect_Category.ToString());
                else if (this.BOM.GetSelectedOpticsIndirect_Category != null) ParamInput.Add("Optics", this.BOM.GetSelectedOpticsIndirect_Category.ToString());

                if (this.BOM.GetselectedproductID_Category.Contains("LED")) ParamInput.Add("Light", "led");
                else ParamInput.Add("Light", "fluorescent");



                this.productCode = BO.UD03[0]["Key2"].ToString();
                this.fixtureCode = BO.UD03[0]["Character03"].ToString();
                this.ProdFamCode = BO.UD03[0]["Key3"].ToString();


                this.Initial();
                this.ProductTime();
                this.FixtureTime();
                this.ProdFamTime();
            
        }



        private void Initial()
        {
            using (var db = new TimeContext())
            {


                List<string> workcenters = db.Prod.Select(m => m.WorkCenter).Distinct().ToList();
                foreach (var workcenter in workcenters)
                {
                    ProdTime[workcenter] = 0;
                }

            }
        }


        private void ProductTime()
        {
            if (this.BOM.BOM_Exist == true)
            {
                foreach (var section in this.BOM.BOM_Sections)
                {
                    
                    int length = (int)Math.Round((double)section.Length / 12.0) * 1;
                    if (this.BOM.BOM_Sections.Count() == 1)
                    {
                        this.ParamInput["Section"] = "Complete Section";
                    }
                    else if (section.IsAtStart == true) this.ParamInput["Section"] = "SR1";
                    else if (section.IsAtMiddle == true) this.ParamInput["Section"] = "SRM";
                    else if (section.IsAtEnd == true) this.ParamInput["Section"] = "SRE";

                    using (var db = new TimeContext())
                    {

                        List<ProdTB> product = db.Prod.Where(item => item.Type == prodtype && item.Code == this.productCode).ToList();

                        this.ParamNames = db.Params.Select(m => m.ParamName).Distinct().ToList();

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

                                var ParamNameList = item.Params.Select(m => m.ParamName).Distinct().ToList();
                                List<string> ParamValues = new List<string>();
                                foreach (var row in ParamNameList)
                                {
                                    ParamValues.Add(this.ParamInput[row]);
                                }
                                


                                var check = item.Params.Where(r => ParamValues.Contains(r.ParamValue)).ToList();
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

        private void ProdFamTime()
        {
            using (var db = new TimeContext())
            {

                
                //List<ProdFamTB> prodfam = db.ProdFam.Where(item => item.FamCode == this.ProdFamCode).ToList();
                List<ProdTB> prodfam = db.Prod.Where(item => item.Type == prodfamtype && item.Code == this.ProdFamCode).ToList();
                foreach (var workcenter in prodfam)
                {


                    List<string> usedOption = new List<string>();

                    foreach (var item in workcenter.Options.Where(item => !usedOption.Contains(item.OptionName)))
                   
                    {


                        if (item.Params.Count == 0)
                        {
                            usedOption.Add(item.OptionName);
                            ProdTime[workcenter.WorkCenter.ToString()] += item.ProdTime;
                            continue;
                        }

                        var ParamNameList = item.Params.Select(m => m.ParamName).Distinct().ToList();
                        List<string> ParamValues = new List<string>();
                        foreach (var row in ParamNameList)
                        {
                            ParamValues.Add(this.ParamInput[row]);
                        }



                        var check = item.Params.Where(r => ParamValues.Contains(r.ParamValue)).ToList();
                        if (check.Count > 0)
                        {
                            usedOption.Add(item.OptionName);
                            ProdTime[workcenter.WorkCenter.ToString()] += item.ProdTime;
                        }



                    }

                }
            }

        }

        private void FixtureTime()
        {
            using (var db = new TimeContext())
            {
                this.ParamInput["Section"] = "Complete Section";
                //List<FixtureTB> fixture = db.Fixtures.Where(item => item.FxCode == this.fixtureCode).ToList();
                List<ProdTB> fixture = db.Prod.Where(item => item.Code == this.fixtureCode && item.Type == fixturetype).ToList();
                foreach (var workcenter in fixture)
                {
                    List<string> usedOption = new List<string>();

                    foreach (var item in workcenter.Options.Where(item => !usedOption.Contains(item.OptionName)))
                   
                    {


                        if (item.Params.Count == 0)
                        {
                            usedOption.Add(item.OptionName);
                            ProdTime[workcenter.WorkCenter.ToString()] += item.ProdTime;
                            continue;
                        }

                        var ParamNameList = item.Params.Select(m => m.ParamName).Distinct().ToList();
                        List<string> ParamValues = new List<string>();
                        foreach (var row in ParamNameList)
                        {
                            
                                ParamValues.Add(this.ParamInput[row]);
                            
                            
                            
                        }



                        var check = item.Params.Where(r => ParamValues.Contains(r.ParamValue)).ToList();
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
