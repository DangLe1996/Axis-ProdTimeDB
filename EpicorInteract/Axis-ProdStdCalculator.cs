using System.Linq;
using Ice.Core;
using Ice.Lib.Framework;
using System.Data;
using Erp.Proxy.BO;
using Axis_ProdTimeDB.DAL;
using Erp.BO;


namespace Axis_ProdTimeDB.EpicorInteract
{
    class AxisProdStdCalculator
    {

        public static Session epiSession = new Session("Dang", "Lebaodang96!", "net.tcp://EPICORERP/Epicor10Test", Session.LicenseType.Default, @"C:\Epicor\ERP10.1Client\Client\Config\Epicor10Test.sysconfig");
        private QuoteAsmImpl quoteAsmImpl = WCFServiceSupport.CreateImpl<QuoteAsmImpl>((Ice.Core.Session)epiSession, Erp.Proxy.BO.QuoteAsmImpl.UriPath);
        private QuoteAsmDataSet QuoteDS = new QuoteAsmDataSet();

        public void getProdStd(int quotenum, int quoteline)
        {


            this.QuoteDS = this.quoteAsmImpl.GetByID(quotenum, quoteline, 0);

            QuoteDtlSearchImpl quoteDtlSearchImpl = WCFServiceSupport.CreateImpl<QuoteDtlSearchImpl>((Ice.Core.Session)epiSession, Erp.Proxy.BO.QuoteDtlSearchImpl.UriPath);
            string whereClause = string.Format("QuoteNum = {0} and QuoteLine = {1}",quotenum,quoteline);
            bool morePage;
          
            var quotetest =  quoteDtlSearchImpl.GetRows(whereClause, 0, 0, out morePage);
            decimal runQty = quotetest.QuoteDtl[0].OrderQty;
            
            string estScrapType = this.QuoteDS.QuoteOpr[0].EstScrapType;
            decimal qtyPer = this.QuoteDS.QuoteOpr[0].QtyPer;
            int OperationsPerPart = this.QuoteDS.QuoteOpr[0].OpsPerPart;
            decimal OperationEstScrap = this.QuoteDS.QuoteOpr[0].EstScrap;
            decimal AssxEstScrap = this.QuoteDS.QuoteAsm[0].EstScrap;

            ProductClass product = new ProductClass(quotetest.QuoteDtl[0].LineDesc);

            foreach (var time in product.ProdTimeERP.Where(r => r.Value != 0).ToList())
            {
                decimal value = Axis.Utilities.ProductionCalculation.GetEstimatedProductionLaborHours(runQty, estScrapType, OperationEstScrap, 0.0m, qtyPer, (decimal)time.Value, "MP", OperationsPerPart);
                this.UpdateTime(time.Key, 1.0m);
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
                    //this.quoteAsmImpl.Update(QuoteDS);
                    break;
                }
            }

        }

    }
}
