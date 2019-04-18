using BusinessInk.Common.Logging;
using CSG.ProcessAutomation.Coversheet.WebApi.Models;
using NLog;
using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DevExpress.XtraReports.UI;
using DevExpress.XtraRichEdit.Commands;
using CSG.ProcessAutomation.Coversheet.WebApi.Validation;
using System.Configuration;
using System.Net.Http.Headers;

namespace CSG.ProcessAutomation.Coversheet.WebApi.Controllers
{
    public class CoversheetController : ApiController
    {
        private Logger _logger;
        private string _reportLayout;
        private bool loggingTraceOn;

        /// <summary>
        /// The coversheet controller generate a printable coversheet report including the required matrix type barcodes.
        /// </summary>
        public CoversheetController()
        {
            
            //Init Central Logging
            bool.TryParse(ConfigurationManager.AppSettings["loggingTraceOn"], out loggingTraceOn);
            LogManager.Configuration = LogConfiguration.CentralLogging(isTraceOn: loggingTraceOn);
            _logger = LogManager.GetLogger("CoversheetWebApi");

            //get layout location from config file
            _reportLayout = System.Configuration.ConfigurationManager.AppSettings["ReportLayoutLocation"].ToString();
        }

       /// <summary>
       ///      This method will load the report layout and generate a printable coversheet report
       /// </summary>
       /// <param name="jobId"></param>
       /// <param name="startSeq"></param>
       /// <param name="endSeq"></param>
       /// <param name="match"></param>
       /// <param name="description"></param>
       /// <param name="firstSeqName"></param>
       /// <param name="lastSeqName"></param>
       /// <param name="duplex"></param>
       /// <param name="pageSize"></param>
       /// <param name="barcodeYstring"></param>
       /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage Get(string jobId, int startSeq, int endSeq, string match, string description, string firstSeqName, string lastSeqName, string duplex = "S", string pageSize = "S", string barcodeYstring = "", string inventory = "")
        {
            // Create a list as a datasource. 
            ArrayList listDataSource = new ArrayList();

            try
            {
                //ensure required parameters are sent
                Ensure.NotNullOrEmpty(jobId, "JobId cannot be null or empty.");
                Ensure.NotNull(startSeq);
                Ensure.NotNull(endSeq);
                Ensure.NotNullOrEmpty(match, "Match code cannot be null or empty.");

                // Populate the list with the report record. 
                listDataSource.Add(new CoversheetData
                {
                    JobId = jobId,
                    StartSeq = startSeq,
                    EndSeq = endSeq,
                    Match = match,
                    Description = description,
                    FirstSeqName = firstSeqName,
                    LastSeqName = lastSeqName,
                    Duplex = duplex,
                    PageSize = pageSize,
                    Inventory = inventory,
                    BarcodeYString = barcodeYstring
                });

                _logger.Trace(
                    $"[CoversheetWebApi] Request made with the following param values: {jobId},{startSeq},{endSeq},{match},{description},{firstSeqName},{lastSeqName},{barcodeYstring}");

                //instance the devExpress report and load the layout report
                XtraReport report = new XtraReport();
                report.LoadLayout(_reportLayout);

                //set the report datasource to the array list.
                report.DataSource = listDataSource;

                // Create a stream.
                MemoryStream stream = new MemoryStream();

                // Save the output report to the stream.
                report.ExportToPdf(stream);

                byte[] reportFile = stream.ToArray();

                //declare reponse message
                HttpResponseMessage result = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new ByteArrayContent(stream.ToArray())
                };
                result.Content.Headers.ContentType =
                    new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");
                ContentDispositionHeaderValue contentDisposition = null;
                if (ContentDispositionHeaderValue.TryParse($"inline; filename=Coversheet_{jobId.Replace(" ", "")}.pdf", out contentDisposition))
                {
                    result.Content.Headers.ContentDisposition = contentDisposition;
                }

                _logger.Trace($"[CoversheetWebApi Get()] Report generated sucessfully.");

                return result;

            }
            catch (Exception ex)
            {
                var errorMsg = "[Coversheet Get()] Error getting the coversheet report. " + ex.Message;
                _logger.Error(ex, errorMsg);
                _logger.Trace(ex, errorMsg);
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(errorMsg),
                    ReasonPhrase = "Critical Exception"
                });
            }

        }
    }
}
