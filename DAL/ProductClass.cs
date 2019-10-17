using AXISAutomation.Solvers.BOM;
using Erp.BO;
using Erp.Proxy.BO;
using Ice.Core;
using Ice.Lib.Framework;
using Ice.Proxy.BO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Axis_ProdTimeDB.DAL
{

    public class ProductClass : Utilities
    {
        private AXISAutomation.Solvers.BOM._BOM BOM { get; set; }

        public static Session epiSession = new Session("Dang", "Lebaodang96!", "net.tcp://EPICORERP/Epicor10Test", Session.LicenseType.Default, @"C:\Epicor\ERP10.1Client\Client\Config\Epicor10Test.sysconfig");
        private UD03Impl UD30BO = WCFServiceSupport.CreateImpl<UD03Impl>((Ice.Core.Session)epiSession, "Ice/BO/UD03");

        public Dictionary<string, double> ProdTime = new Dictionary<string, double>();
        public Dictionary<string, double> ProdTimeERP = new Dictionary<string, double>();
        private List<string> ParamNames = new List<string>();
        private List<string> ParamValue = new List<string>();
        private int lengthParam;
        private QuoteAsmImpl quoteAsmImpl = WCFServiceSupport.CreateImpl<QuoteAsmImpl>((Ice.Core.Session)epiSession, Erp.Proxy.BO.QuoteAsmImpl.UriPath);
        private QuoteAsmDataSet QuoteDS = new QuoteAsmDataSet();
        private int sectionNum;
        private Dictionary<string, string> ParamInput = new Dictionary<string, string>();
        public List<string> usedOptionGlobal = new List<string>();
        private Dictionary<string, List<string>> WiresQty = new Dictionary<string, List<string>>();

        public Dictionary<string, double> OH = new Dictionary<string, double>();
        private string fixtureCode;
        private string productCode;
        private string ProdFamCode;

        public ProductClass(_BOM _BOM, int quoteNum = 0, int quoteLine = 0, int sectionNum = 0)
        {
            this.sectionNum = sectionNum;
            bool morepage;
            this.BOM = _BOM;
            if (this.BOM.GetselectedproductID_Category == null)
            {
                throw new System.Exception("This product cannot be configured");

            }
            var BO = UD30BO.GetRows(string.Format("Key1 = 'PRODUCTID' and key2 = '{0}'", this.BOM.GetselectedproductID_Category), "", 0, 0, out morepage);

            this.productCode = BO.UD03[0]["Key2"].ToString();
            this.fixtureCode = BO.UD03[0]["Character03"].ToString();
            this.ProdFamCode = BO.UD03[0]["Key3"].ToString();


            try
            {
                ParamInput.Add("Circuits", this.BOM.FixtureConfiguration.Selection.Circuits.SelectionBaseValue);
            }
            catch (NullReferenceException)
            {
                ParamInput.Add("Circuits", "2A/B");
            }
            try
            {
                ParamInput.Add("Driver", this.BOM.FixtureConfiguration.Selection.Driver.SelectionBaseValue);
            }
            catch (NullReferenceException)
            {
                ParamInput.Add("Driver", "DP");
            }
            try
            {
                List<string> recess_mounting = new List<string>(
                    new string[] {
                    "D",
                    "DS"
                });
                if (recess_mounting.Contains(this.BOM.FixtureConfiguration.Selection.Mounting.SelectionBaseValue) && this.fixtureCode == "Recessed")
                    ParamInput.Add("Mounting", this.BOM.FixtureConfiguration.Selection.Mounting.SelectionBaseValue);
                else ParamInput.Add("Mounting", "-");
            }
            catch (NullReferenceException)
            {
                ParamInput.Add("Mounting", "-");
            }

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
            if (!ParamInput.ContainsKey("WiresQty")) ParamInput.Add("WiresQty", "3");
            this.lengthParam = ((int)((double)this.BOM.FixtureConfiguration.Selection.RequestedLengthNormalizedToDecimalInch * 0.0833333));
            this.ParamInput["LampQty"] = "1";

            try
            {
                if (this.BOM.GetSelectedOpticsDirect_Category != null) ParamInput.Add("Optics", this.BOM.GetSelectedOpticsDirect_Category.ToString());
                else ParamInput.Add("Optics", this.BOM.GetSelectedOpticsIndirect_Category.ToString());
            }
            catch (NullReferenceException)
            {
                ParamInput.Add("Optics", "S");
            }


            try
            {
                if (this.BOM.GetselectedproductID_Category.Contains("LED")) ParamInput.Add("Light", "led");
                else ParamInput.Add("Light", "fluorescent");
            }
            catch (NullReferenceException)
            {
                ParamInput.Add("Light", "led");
            }

            this.Initial();
            try
            {
                if (this.BOM.BOM_Exist == true)
                    this.ProductTime();
                else this.ProductTimeManual();
            }
            catch (NullReferenceException)
            {
                this.ProductTimeManual();
            }
            this.ProdFamTime();
            this.FixtureTime();

            this.ProdTimeERP["SALE0000"] = this.ProdTimeERP["SALE0000"] * 2.5;
            this.ProdTimeERP["SAPREP"] = this.ProdTimeERP["SAPREP"] * 2.5;
            this.ProdTimeERP["FA000000"] = this.ProdTimeERP["FA000000"] * 2.5 / 7;

            if (quoteNum != 0 && quoteLine != 0)
                this.getProdStd(quoteNum, quoteLine);
        }


        private void getProdStd(int quotenum, int quoteline)
        {

            if (quotenum == 0 || quoteline == 0)
            {
                return;
            }

            this.QuoteDS = this.quoteAsmImpl.GetByID(quotenum, quoteline, 0);

            QuoteDtlSearchImpl quoteDtlSearchImpl = WCFServiceSupport.CreateImpl<QuoteDtlSearchImpl>((Ice.Core.Session)epiSession, Erp.Proxy.BO.QuoteDtlSearchImpl.UriPath);
            string whereClause = string.Format("QuoteNum = {0} and QuoteLine = {1}", quotenum, quoteline);
            bool morePage;

            var quotetest = quoteDtlSearchImpl.GetRows(whereClause, 0, 0, out morePage);
            //decimal runQty = quotetest.QuoteDtl[0].OrderQty;




            foreach (var time in this.ProdTimeERP.Where(r => r.Value != 0).ToList())
            {
                QuoteAsmDataSet.QuoteOprRow quote = (QuoteAsmDataSet.QuoteOprRow)this.QuoteDS.QuoteOpr.AsEnumerable().Where(r => r.Field<string>("OpCode") == time.Key).FirstOrDefault();
                if (quote == null) continue;
                string estScrapType = quote.EstScrapType;
                decimal runQty = quote.QtyPer;
                decimal qtyPer = quote.QtyPer;
                int OperationsPerPart = quote.OpsPerPart;
                decimal OperationEstScrap = quote.EstScrap;
                decimal AssxEstScrap = quote.EstScrap;
                decimal value = 1 / Axis.Utilities.ProductionCalculation.GetEstimatedProductionLaborHours(runQty, estScrapType, OperationEstScrap, 0.0m, qtyPer, (decimal)time.Value, "MP", OperationsPerPart);
                OH.Add(time.Key, (double)value);
                this.UpdateTime(time.Key, value);
            }
            //this.quoteAsmImpl.Update(QuoteDS);
        }


        private void UpdateTime(string OpCode, decimal ProdStd)
        {

            foreach (QuoteAsmDataSet.QuoteOprRow row in QuoteDS.QuoteOpr.Rows)
            {
                if (row.OpCode == OpCode)
                {
                    row.ProdStandard = ProdStd;
                    row["SystemCalculate_c"] = true;
                    this.quoteAsmImpl.Update(QuoteDS);
                    
                    break;
                }
            }

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
                List<string> OpCodes = db.Prod.Where(r => r.OpCode != null).Select(m => m.OpCode).Distinct().ToList();
                foreach (var opCode in OpCodes)
                {

                    ProdTimeERP[opCode] = 0;
                }
            }
        }


        private void ProductTime()
        {

            foreach (var section in this.BOM.BOM_Sections)
            {

                int length = (int)Math.Ceiling((double)section.Length / 40) * 4;
                if (length > 12) length = 12;
                if (this.BOM.BOM_Sections.Count() == 1)
                {
                    length = Int32.Parse(this.BOM.GetSelectedLength_Category);
                    this.ParamInput["Section"] = "Complete Section";
                }
                else if (section.IsAtStart == true) this.ParamInput["Section"] = "SR1";
                else if (section.IsAtMiddle == true) this.ParamInput["Section"] = "SRM";
                else if (section.IsAtEnd == true) this.ParamInput["Section"] = "SRE";

                using (var db = new TimeContext())
                {

                    List<ProdTB> product = db.Prod.Where(item => item.Type == prodtype && item.Code == this.productCode).ToList();

                    this.ParamNames = db.Params.Select(m => m.ParamName).Distinct().ToList();

                    foreach (var productindex in product)
                    {
                        List<string> usedOption = new List<string>();

                        foreach (var item in productindex.Options.Where(item => !usedOption.Contains(item.OptionName)
                        && item.sectionLength == length))
                        {

                            this.addTime(item, ref usedOption, productindex);

                        }
                    }


                }


            }
        }
        private void ProductTimeManual()
        {
            if (this.sectionNum == 1)
            {
                this.ParamInput["Section"] = "Complete Section";
                using (var db = new TimeContext())
                {
                    int length = 12;
                    List<ProdTB> product = db.Prod.Where(item => item.Type == prodtype && item.Code == this.productCode).ToList();

                    this.ParamNames = db.Params.Select(m => m.ParamName).Distinct().ToList();

                    foreach (var productindex in product)
                    {
                        List<string> usedOption = new List<string>();

                        foreach (var item in productindex.Options.Where(item => !usedOption.Contains(item.OptionName)
                        && item.sectionLength == length))
                        {

                            this.addTime(item, ref usedOption, productindex);

                        }
                    }


                }
                return;
            }
            else
            {

                int sectionLength = this.lengthParam / this.sectionNum;
                sectionLength = (sectionLength > 12) ? 12 : (sectionLength > 8) ? 8 : 4;
                for (int i = 1; i <= this.sectionNum; i++)
                {


                    this.ParamInput["Section"] = (i == 1) ? "SR1" : (i == this.sectionNum) ? "SRE" : "SRM";
                    using (var db = new TimeContext())
                    {

                        List<ProdTB> product = db.Prod.Where(item => item.Type == prodtype && item.Code == this.productCode).ToList();

                        this.ParamNames = db.Params.Select(m => m.ParamName).Distinct().ToList();

                        foreach (var productindex in product)
                        {
                            List<string> usedOption = new List<string>();

                            foreach (var item in productindex.Options.Where(item => !usedOption.Contains(item.OptionName)
                            && item.sectionLength == sectionLength))
                            {

                                this.addTime(item, ref usedOption, productindex);

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
                foreach (var product in prodfam)
                {


                    List<string> usedOption = new List<string>();

                    foreach (var item in product.Options.Where(item => !usedOption.Contains(item.OptionName)))

                    {


                        this.addTime(item, ref usedOption, product);



                    }

                }
            }

        }

        private void addTime(OptionTB item, ref List<string> usedOption, ProdTB product)
        {
            if (item.Params.Count == 0)
            {
                usedOptionGlobal.Add(item.OptionName + ',' + item.ProdTime.ToString() + ',' + product.WorkCenter.ToString());
                usedOption.Add(item.OptionName);
                ProdTime[product.WorkCenter.ToString()] += item.ProdTime;
                if (!string.IsNullOrEmpty(product.OpCode))
                    ProdTimeERP[product.OpCode] += item.ProdTime;
                return;
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
                ProdTime[product.WorkCenter.ToString()] += item.ProdTime;
                if (!string.IsNullOrEmpty(product.OpCode))
                    ProdTimeERP[product.OpCode] += item.ProdTime;
            }

        }

        private void FixtureTime()
        {
            using (var db = new TimeContext())
            {
                this.ParamInput["Section"] = "Complete Section";
                //List<FixtureTB> fixture = db.Fixtures.Where(item => item.FxCode == this.fixtureCode).ToList();
                try
                {
                    this.ParamInput["Mounting"] = this.BOM.GetSelectedMounting_Category;
                }
                catch (NullReferenceException)
                {

                }
                List<ProdTB> fixture = db.Prod.Where(item => item.Code == this.fixtureCode && item.Type == fixturetype).ToList();
                foreach (var fixtureindex in fixture)
                {
                    List<string> usedOption = new List<string>();

                    foreach (var item in fixtureindex.Options.Where(item => !usedOption.Contains(item.OptionName)))

                    {


                        this.addTime(item, ref usedOption, fixtureindex);


                    }


                }

            }

        }



    }



}
