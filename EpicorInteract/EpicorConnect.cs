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
using Erp.Contracts;
using Erp.Proxy.BO;
using Axis_ProdTimeDB.DAL;
using Erp.BO;
using Axis.Utilities;


namespace Axis_ProdTimeDB.EpicorInteract
{
    class EpicorConnect
    {

        public static Session epiSession = new Session("Dang", "Lebaodang96!", "net.tcp://EPICORERP/Epicor10Test", Session.LicenseType.Default, @"C:\Epicor\ERP10.1Client\Client\Config\Epicor10Test.sysconfig");
        private QuoteAsmImpl quoteAsmImpl = WCFServiceSupport.CreateImpl<QuoteAsmImpl>((Ice.Core.Session)epiSession, Erp.Proxy.BO.QuoteAsmImpl.UriPath);
        private QuoteAsmDataSet QuoteDS = new QuoteAsmDataSet();

        public void getQuote(int quotenum, int quoteline)
        {


            this.QuoteDS = this.quoteAsmImpl.GetByID(quotenum, quoteline, 0);

            QuoteDtlSearchImpl quoteDtlSearchImpl = WCFServiceSupport.CreateImpl<QuoteDtlSearchImpl>((Ice.Core.Session)epiSession, Erp.Proxy.BO.QuoteDtlSearchImpl.UriPath);
            string whereClause = string.Format("QuoteNum = {0} and QuoteLine = {1}",quotenum,quoteline);
            bool morePage;
          
            var quotetest =  quoteDtlSearchImpl.GetRows(whereClause, 0, 0, out morePage);
            decimal runQty = quotetest.QuoteDtl[0].OrderQty;
            
            ProductClass product = new ProductClass(quotetest.QuoteDtl[0].LineDesc);
            foreach (var time in product.ProdTime)
            {
                decimal value = Axis.Utilities.ProductionCalculation.GetEstimatedProductionLaborHours(runQty, "", 0.0m, 0.0m, 1.0m, (decimal)time.Value, "MP", 1);
                this.UpdateTime(time.Key, value);
            }
            this.quoteAsmImpl.Update(QuoteDS);
        }

        private void UpdateTime(string OpCode, decimal ProdStd)
        {
            
            foreach (QuoteAsmDataSet.QuoteOprRow row in QuoteDS.QuoteOpr.Rows)
            {
                if (row.OpCode == OpCode)
                {
                    row.ProdStandard = ProdStd;

                    break;
                }
            }

        }

    }
}
