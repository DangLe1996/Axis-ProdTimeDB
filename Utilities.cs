using Axis_ProdTimeDB.InputClasses;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;

namespace Axis_ProdTimeDB
{
    public class Utilities
    {
        static string currentDir = Directory.GetCurrentDirectory();
        static string dataDir = currentDir + @"\Data\";
        public static string prodtype = "ProdID";
        public static string prodfamtype = "ProdFamID";
        public static string fixturetype = "FixtureID";
        public DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            strFilePath = dataDir + strFilePath;
            StreamReader sr = new StreamReader(strFilePath);

            string[] headers = sr.ReadLine().Split(',');

            DataTable dt = new DataTable();
            foreach (string header in headers)
            {

                dt.Columns.Add(header.Trim());
            }
            while (!sr.EndOfStream)
            {
                string[] rows = Regex.Split(sr.ReadLine(), ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                DataRow dr = dt.NewRow();
                for (int i = 0; i < headers.Length; i++)
                {
                    dr[i] = rows[i].Trim();
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public void initializer()
        {
            MKits mKitsInput = new MKits("Mkits.csv");
            MR mRInput = new MR("MR.csv");
            MSTime MSTimeInput = new MSTime(@"MSTime.csv");
            Driver ballastInput = new Driver(@"BallastFix.csv");
            SubEndP subEndPInput = new SubEndP(@"SubEndP.csv");
            Optic opticInput = new Optic(@"Optic.csv");
            CartBoard cartBoardInput = new CartBoard(@"CartridgeBoard.csv");
            Pack packInput = new Pack(@"Pack.csv");
            ExitW exitInput = new ExitW(@"ExitW.csv");
            PowerC powerCInput = new PowerC(@"PowerC.csv");
            ScrewRef screwInput = new ScrewRef(@"ScrewRef.csv");
            Housing housingInput = new Housing(@"Housing.csv");
            Battery batteryInput = new Battery("Battery.csv");
            BackC backCInput = new BackC("BackC.csv");
            CB cBInput = new CB("CB.csv");
            ChicPle chicPleInput = new ChicPle("ChicPle.csv");
            CounterW counterInputer = new CounterW("CounterW.csv");
            Length lengthInput = new Length("Length.csv");
            Emergency emergencyInput = new Emergency("Emergency.csv");
            EndC endCInput = new EndC("EndC.csv");
            EndFeed endFeedInput = new EndFeed("EndFeed.csv");
            EndP endPInput = new EndP("EndP.csv");
            Nightlight nightlightInput = new Nightlight("Nightlight.csv");
            FlexWhip flexWhipInput = new FlexWhip("FlexWhip.csv");
            Fuse fuseInput = new Fuse("Fuse.csv");
            Hanger hangerInput = new Hanger("Hanger.csv");
            Inspection inspectionInput = new Inspection("Inspection.csv");
            ITS iTSInput = new ITS("ITS.csv");
            LightC lightCInput = new LightC("LightC.csv");
            OpTest opTestInput = new OpTest("OpTest.csv");
            PowerCord powerCordinput = new PowerCord("PowerCord.csv");
            Remo remoInput = new Remo("Remo.csv");
            SubEndC subEndCInput = new SubEndC("SubEndC.csv");
            SubOp subOpInput = new SubOp("SubOp.csv");
        }
    }
}


//// Specify the provider name, server and database.
//string providerName = "System.Data.SqlClient";
//string serverName = @"VAULT\DRIVEWORKS";
//string databaseName = "AXIS Automation";

//// Initialize the connection string builder for the
//// underlying provider.
//SqlConnectionStringBuilder sqlBuilder =
//    new SqlConnectionStringBuilder();

//// Set the properties for the data source.
//sqlBuilder.DataSource = serverName;
//sqlBuilder.InitialCatalog = databaseName;
//sqlBuilder.IntegratedSecurity = true;
//sqlBuilder.MultipleActiveResultSets = true;

//// Build the SqlConnection connection string.
//string providerString = sqlBuilder.ToString();

//// Initialize the EntityConnectionStringBuilder.
//EntityConnectionStringBuilder entityBuilder =
//    new EntityConnectionStringBuilder();

////Set the provider name.
//entityBuilder.Provider = providerName;

//// Set the provider-specific connection string.
//entityBuilder.ProviderConnectionString = providerString;

//// Set the Metadata location.
//entityBuilder.Metadata = @"res://*/DBConnection.csdl|res://*/DBConnection.ssdl|res://*/DBConnection.msl";
//Console.WriteLine(entityBuilder.ToString());



//private void getProdStd(int quotenum, int quoteline)
//{
//    Session epiSession = new Session("Dang", "Lebaodang96!", "net.tcp://EPICORERP/Epicor10Test", Session.LicenseType.Default, @"C:\Epicor\ERP10.1Client\Client\Config\Epicor10Test.sysconfig");
//    QuoteAsmImpl quoteAsmImpl = WCFServiceSupport.CreateImpl<QuoteAsmImpl>((Ice.Core.Session)epiSession, Erp.Proxy.BO.QuoteAsmImpl.UriPath);
//    QuoteImpl quoteImpl = WCFServiceSupport.CreateImpl<QuoteImpl>((Ice.Core.Session)epiSession, Erp.Proxy.BO.QuoteImpl.UriPath);
//    QuoteAsmDataSet QuoteDS = new QuoteAsmDataSet();
//    OpMasterImpl opMasterImpl = WCFServiceSupport.CreateImpl<OpMasterImpl>((Ice.Core.Session)epiSession, Erp.Proxy.BO.OpMasterImpl.UriPath);
//    //var row = quoteAsmImpl.BpmContext.BpmData.NewBpmDataRow();


//    if (quotenum == 0 || quoteline == 0)
//    {
//        return;
//    }

//    QuoteDS = quoteAsmImpl.GetByID(quotenum, quoteline, 0);

//    QuoteDtlSearchImpl quoteDtlSearchImpl = WCFServiceSupport.CreateImpl<QuoteDtlSearchImpl>((Ice.Core.Session)epiSession, Erp.Proxy.BO.QuoteDtlSearchImpl.UriPath);
//    string whereClause = string.Format("QuoteNum = {0} and QuoteLine = {1}", quotenum, quoteline);
//    bool morePage;

//    var quotetest = quoteDtlSearchImpl.GetRows(whereClause, 0, 0, out morePage);
//    //decimal runQty = quotetest.QuoteDtl[0].OrderQty;

//    QuoteDataSet quoteHed = quoteImpl.GetByID(quotenum);
//    bool closequote = false;
//    if (quoteHed.QuoteHed[0].QuoteClosed == true)
//    {
//        closequote = true;
//        quoteImpl.OpenCloseQuote(quotenum, false);
//    }
//    string whereclause = "ReportSection_c = 1";
//    bool morepage;
//    var OpList = opMasterImpl.GetList(whereclause, 0, 0, out morepage);


//    foreach (var time in this.ProdTimeERP.Where(r => r.Value != 0).ToList())
//    {
//        QuoteAsmDataSet.QuoteOprRow quote = (QuoteAsmDataSet.QuoteOprRow)QuoteDS.QuoteOpr.AsEnumerable().Where(r => r.Field<string>("OpCode") == time.Key).FirstOrDefault();
//        var ReportBySection = OpList.OpMasterList.AsEnumerable().Where(r => r.Field<string>("OpCode") == time.Key).FirstOrDefault();
//        if (quote == null) continue;
//        string estScrapType = quote.EstScrapType;
//        decimal runQty = quote.RunQty;
//        decimal qtyPer;
//        qtyPer = ReportBySection == null ? 1 : this.sectionNum;

//        int OperationsPerPart = quote.OpsPerPart;
//        decimal OperationEstScrap = quote.EstScrap;
//        decimal AssxEstScrap = quote.EstScrap;
//        decimal value = 1 / Axis.Utilities.ProductionCalculation.GetEstimatedProductionLaborHours(runQty, estScrapType, OperationEstScrap, 0.0m, qtyPer, (decimal)time.Value, "MP", OperationsPerPart);
//        OperationHour.Add(time.Key, (double)value);

//        foreach (QuoteAsmDataSet.QuoteOprRow row in QuoteDS.QuoteOpr.Rows)
//        {
//            if (row.OpCode == time.Key)
//            {
//                row.ProdStandard = value;
//                row["SystemCalculate_c"] = true;
//                quoteAsmImpl.Update(QuoteDS);

//                break;
//            }
//        }
//    }

//    if (closequote == true)
//        quoteImpl.OpenCloseQuote(quotenum, true);

//}