using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NavOne.PaymentAPI;
using PaymentAPI.Model;
using System.Net;
using System.Net.Security;
using System.ServiceModel;
using System.ServiceModel.Security;

namespace PaymentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NavOnePaymentAPI : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _url;
        public NavOnePaymentAPI(ILogger<NavOnePaymentAPI> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _url = _configuration.GetValue<string>("ServiceUrl");
        }
        [HttpPost]
        [Route("CreatePayment")]
        public async Task<IActionResult> CreatePayment(string ClientNo)
        {
            Response response = new Response();
            try
            {
                _logger.LogInformation("Create Payment");

                CreateFunction_PortClient serviceFunction = new CreateFunction_PortClient();
                serviceFunction.Endpoint.Address = new EndpointAddress(_url);
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                var paymentResponse = await serviceFunction.CreateTWMPaymentAsync(ClientNo).ConfigureAwait(false);
                response.Status = ServiceStatus.Success;
                response.data = paymentResponse.return_value;
                _logger.LogInformation("Create Payment completed");
                return Ok(paymentResponse.return_value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create Payment:Error Inner Exception:{ex.InnerException}", ex.InnerException);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("CancelPayment")]
        public async Task<IActionResult> CancelPayment(string PaymentReference)
        {
            Response response = new Response();
            try
            {
                _logger.LogInformation("Cancel Payment");

                CreateFunction_PortClient serviceFunction = new CreateFunction_PortClient();
                serviceFunction.Endpoint.Address = new System.ServiceModel.EndpointAddress(_url);
                var paymentResponse = await serviceFunction.CancelTWMPaymentAsync(PaymentReference).ConfigureAwait(false);
                response.Status = ServiceStatus.Success;
                response.data = paymentResponse.return_value.ToString();

                _logger.LogInformation("Cancel Payment Completed");
                return Ok(paymentResponse.return_value.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cancel Payment:Error Inner Exception:{ex.InnerException}", ex.InnerException);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("ValidatePayment")]
        public async Task<IActionResult> ValidatePayment(string PaymentReference)
        {
            Response response = new Response();
            try
            {
                _logger.LogInformation("Validate Payment");
                CreateFunction_PortClient serviceFunction = new CreateFunction_PortClient();
                serviceFunction.Endpoint.Address = new System.ServiceModel.EndpointAddress(_url);
                var paymentResponse = await serviceFunction.ValidateTWMPaymentAsync(PaymentReference).ConfigureAwait(false);
                response.Status = ServiceStatus.Success;
                response.data = paymentResponse.return_value.ToString();
                _logger.LogInformation("Validate Payment completed");
                return Ok(paymentResponse.return_value.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Validate Payment :Error Inner Exception:{ex.InnerException}", ex.InnerException);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost, Route("CreateProspect")]
        public async Task<IActionResult> CreateProspect(string ProspectType,string Gender,string Title,string Forename,string Surname,string Initial,string FirstPartitionCode,string EntityNo)
        {
           Response httpresponse = new Response();
            int pt = 0; 
            int gen = 0;
            if (ProspectType == "Personal" || ProspectType == "personal")
            {
                 pt = (int)prospectType.Personal ;
            }
            if (ProspectType == "Corporate" || ProspectType == "corporate")
            {
                 pt = (int)prospectType.Corporate;
            }
            if (Gender == "Male" || ProspectType == "male")
            {
                gen = 1;
            }
            if (Gender == "Female" || ProspectType == "female")
            {
                gen = 2;
            }
            try
            {
                _logger.LogInformation("Create Prospect : Entering");
                CreateFunction_PortClient serviceFunction = new CreateFunction_PortClient();
                serviceFunction.Endpoint.Address = new System.ServiceModel.EndpointAddress(_url);
                _logger.LogInformation("Create Prospect Service is going to be hit");
                var res = await serviceFunction.CreateProspectAsync(pt, gen, Title, Forename, Surname, Initial, FirstPartitionCode, EntityNo).ConfigureAwait(false);
                _logger.LogInformation("Create Prospect Service succesfully hitted");
                httpresponse.Status = ServiceStatus.Success;
                httpresponse.data = res.return_value.ToString();
                httpresponse.StatusCode = (int)HttpStatusCode.OK;
                _logger.LogInformation("Create Prospect Leaving");
                return Ok(httpresponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create Prospect :Error Inner Exception:{ex.InnerException}", ex.InnerException);
                httpresponse.Status = ServiceStatus.DataError;
                httpresponse.Error = ex.Message;
                httpresponse.StatusCode = (int)HttpStatusCode.BadRequest;
                return BadRequest(httpresponse);
            }
        }
        [HttpPost]
        [Route("ConvertProspecttoOwner")]
        public async Task<IActionResult> ConvertProspecttoOwner(string ProspectNo, DateTime DateAgreementSent, DateTime DateAgreementReceived, DateTime ClientAgreementDated, string EntityNo)
        {
            Response response = new Response();
            try
            {
                _logger.LogInformation("ConvertProspecttoOwner:Entering");
                CreateFunction_PortClient serviceFunction = new CreateFunction_PortClient();
                serviceFunction.Endpoint.Address = new System.ServiceModel.EndpointAddress(_url);
                _logger.LogInformation("ConvertProspecttoOwner: Service going to be hit");
                var Result = await serviceFunction.ConvertProspectToOwnerAsync(ProspectNo, DateAgreementSent, DateAgreementReceived, ClientAgreementDated, EntityNo).ConfigureAwait(false);
                _logger.LogInformation("ConvertProspecttoOwner: Service was hitted successfully");
                response.Status = ServiceStatus.Success;
                response.data = Result.return_value.ToString();
                response.StatusCode = (int)HttpStatusCode.OK;
                _logger.LogInformation("ConvertProspecttoOwner completed");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ConvertProspecttoOwner:Error Inner Exception:{ex.InnerException}", ex.InnerException);
                response.Status = ServiceStatus.DataError;
                response.Error = ex.Message;
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                return BadRequest(response);
            }

        }
    }
}
