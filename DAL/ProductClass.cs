using AXISAutomation.FixtureConfiguration;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using Ice.Core;
using Ice.Lib.Framework;
using Ice.Proxy.BO;


namespace Axis_ProdTimeDB.DAL
{

    public class ProductClass : Utilities
    {
  
        private AXISAutomation.FixtureConfiguration._Fixture _FxConfig { get; set; }
        public Dictionary<string, double> ProdTime = new Dictionary<string, double>();
        public Dictionary<string, double> ProdTimeERP = new Dictionary<string, double>();
        private Dictionary<string, string> ParamInput = new Dictionary<string, string>();
        public List<string> usedOptionGlobal = new List<string>();
        private Dictionary<string, List<string>> WiresQty = new Dictionary<string, List<string>>();
        public Dictionary<string, double> OperationHour = new Dictionary<string, double>();
        private string fixtureCode;
        private string productCode;
        private string ProdFamCode;
        private int sectionNum;
        private int fixtureLength;

        public ProductClass(_Fixture FxConfig, string productCode, string fixtureCode, string ProdFamCode, int sectionNumCustom = 0)
        {

            
            this._FxConfig = FxConfig;

            bool morepage;
            fixtureLength = (int)_FxConfig.Selection.RequestedLengthNormalizedToDecimalInch / 12;
            sectionNum = _FxConfig.Sections.Count != 0 ? _FxConfig.Sections.Count : sectionNumCustom;
           
            if(sectionNum == 0)
            {
                throw new System.Exception("Cannot determine the number of section for this product. Please reconfigured the number of section and try again.");
            }
            string productID = _FxConfig.Selection.ProductID.SelectionBaseValue;
            if (productID == null)
            {
                throw new System.Exception("This product cannot be configured");

            }

            this.productCode = productCode;
            this.fixtureCode = fixtureCode;
            this.ProdFamCode = ProdFamCode;
           
            getMounting();
            getDriver();
            getWiresQty();
            getOptics();
            getCircuit();
            this.ParamInput["LampQty"] = "1";

            Console.WriteLine("ProdTime: Successfully configure all products");
            this.Initial();


            this.ProductTime();
            this.ProdFamTime();
            this.FixtureTime();

            this.ProdTimeERP["SALE0000"] = this.ProdTimeERP["SALE0000"] * 2.5;
            this.ProdTimeERP["SAPREP"] = this.ProdTimeERP["SAPREP"] * 2.5;
            this.ProdTimeERP["FA000000"] = this.ProdTimeERP["FA000000"] * 2.5 / 7;

            //if (quoteNum != 0 && quoteLine != 0)
            //    this.getProdStd(quoteNum, quoteLine);
        }

        #region GetInputParameters
        private void getCircuit()
        {
            try
            {
                ParamInput.Add("Circuits", this._FxConfig.Selection.Circuits.SelectionBaseValue);
            }
            catch (NullReferenceException)
            {
                ParamInput.Add("Circuits", "2A/B");
            }

        }
        private void getDriver()
        {
            try
            {
                ParamInput.Add("Driver", this._FxConfig.Selection.Driver.SelectionBaseValue);
            }
            catch (NullReferenceException)
            {
                ParamInput.Add("Driver", "DP");
            }
        }
        private void getMounting()
        {
            try
            {
                List<string> recess_mounting = new List<string>(
                    new string[] {
                    "D",
                    "DS"
                });
                if (recess_mounting.Contains(this._FxConfig.Selection.Mounting.SelectionBaseValue) && this.fixtureCode.ToLower() == "recessed")
                    ParamInput.Add("Mounting", this._FxConfig.Selection.Mounting.SelectionBaseValue);
                else ParamInput.Add("Mounting", "-");
            }
            catch (NullReferenceException)
            {
                ParamInput.Add("Mounting", "-");
            }
            catch (ArgumentException)
            {
                
            }

        }
        private void getOptics()
        {
            try
            {
                if (this._FxConfig.Selection.OpticsDirect.SelectionBaseValue != null) ParamInput.Add("Optics", this._FxConfig.Selection.OpticsDirect.SelectionBaseValue);
                else ParamInput.Add("Optics", this._FxConfig.Selection.OpticsIndirect.SelectionBaseValue);
            }
            catch (NullReferenceException)
            {
                ParamInput.Add("Optics", "S");
            }


            try
            {
                if (this.productCode.Contains("LED")) ParamInput.Add("Light", "led");
                else ParamInput.Add("Light", "fluorescent");
            }
            catch (NullReferenceException)
            {
                ParamInput.Add("Light", "led");
            }

        }
        private void getWiresQty()
        {
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

        }


        #endregion


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

            if (this._FxConfig.Sections.Items.Count == 0)
            {

                int sectionlength = fixtureLength / this.sectionNum;
                if (sectionlength >= 12) sectionlength = 12;
                else if (sectionlength >= 8) sectionlength = 8;
                else sectionlength = 4;
                ParamInput["Section"] = "SRM";
                for (int i = 0; i < this.sectionNum; i++)
                {
                    getSectionTime(sectionlength);

                }

            }
            else
            {
                foreach (var section in this._FxConfig.Sections.Items)
                {
                    int sectionlength = (int)section.CutLength / 12;
                    if (sectionlength >= 12) sectionlength = 12;
                    else if (sectionlength >= 8) sectionlength = 8;
                    else sectionlength = 4;
                    if (_FxConfig.Sections.Count == 1) ParamInput["Section"] = "Complete Section";
                    else if (section.IsAtStart == true) this.ParamInput["Section"] = "SR1";
                    else if (section.IsAtEnd == true) this.ParamInput["Section"] = "SRE";
                    else this.ParamInput["Section"] = "SRM";
                    getSectionTime(sectionlength);
                }

            }
        }

        private void getSectionTime(int sectionLength)
        {
            using (var db = new TimeContext())
            {

                List<ProdTB> product = db.Prod.Where(item => item.Type == prodtype && item.Code == this.productCode).ToList();
                if(product.Count == 0)
                {
                    product = db.Prod.Where(item => item.Type == prodtype && item.Code == "BBRLED").ToList();

                }
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
     
        private void ProdFamTime()
        {
            using (var db = new TimeContext())
            {

                List<ProdTB> prodfam = db.Prod.Where(item => item.Type == prodfamtype && item.Code == this.ProdFamCode).ToList();
                if(prodfam.Count == 0)
                {
                    prodfam = db.Prod.Where(item => item.Type == prodfamtype && item.Code == "B4LED").ToList();
                }
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
            //if this option contain no parameters, add the time 
            if (item.Params.Count == 0)
            {
                usedOptionGlobal.Add(item.OptionName + ',' + item.ProdTime.ToString() + ',' + product.WorkCenter.ToString());
                usedOption.Add(item.OptionName);
                ProdTime[product.WorkCenter.ToString()] += item.ProdTime;
                if (!string.IsNullOrEmpty(product.OpCode))
                    ProdTimeERP[product.OpCode] += item.ProdTime;
                return;
            }
            else
            {
                try
                {
                    List<string> ParamValues = new List<string>();
                    foreach (var row in item.Params.Select(m => m.ParamName).Distinct().ToList())
                    {
                        ParamValues.Add(ParamInput[row]);
                    }

                    if (item.Params.Where(r => ParamValues.Contains(r.ParamValue)).ToList().Count > 0)
                    {
                        usedOption.Add(item.OptionName);
                        ProdTime[product.WorkCenter.ToString()] += item.ProdTime;
                        if (!string.IsNullOrEmpty(product.OpCode))
                            ProdTimeERP[product.OpCode] += item.ProdTime;
                    }
                }
                catch( KeyNotFoundException)
                {
                    ProdTime[product.WorkCenter.ToString()] = item.ProdTime;
                    if (!string.IsNullOrEmpty(product.OpCode))
                        ProdTimeERP[product.OpCode] = item.ProdTime;
                }
            }

        }

        private void FixtureTime()
        {
            using (var db = new TimeContext())
            {
                this.ParamInput["Section"] = "Complete Section";
                getMounting();
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
