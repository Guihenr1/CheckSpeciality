using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CheckSpeciality
{
    public static class Function1
    {
        private static readonly List<string> ValidSpecialties = new List<string>
        {
            "Cardiology",
            "Dermatology",
            "Endocrinology",
            "Gastroenterology",
            "Hematology"
        };

        [FunctionName("ValidateDoctorSpecialty")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            string doctorSpeciality = data?.doctorSpeciality;

            if (string.IsNullOrEmpty(doctorSpeciality))
            {
                return new BadRequestObjectResult("Please provide a doctor specialty in the request body.");
            }

            if (ValidSpecialties.Contains(doctorSpeciality))
            {
                return new OkObjectResult($"The specialty '{doctorSpeciality}' is valid.");
            } else
            {
                return new BadRequestObjectResult($"The specialty '{doctorSpeciality}' is not valid.");
            }
        }
    }
}
