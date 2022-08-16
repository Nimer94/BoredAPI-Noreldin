using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AzureFunctions.Extensions.Swashbuckle.Attribute;
using BoredApi.Service.Models;
using BoredApi.Service.Services;
using FunctionApp1.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace BoredApi.Service
{
    public class CallerService
    {
        private readonly IBoredAPIManagerService _service;

        public CallerService()
        {
            _service = new BoredAPIManagerService();
        }

        [FunctionName("RetrievesActivities")]
        public async Task Run([TimerTrigger("%RETRIVE_ACTIVITIES_SCHEDULER%")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.UtcNow}");
            IBoredAPIManagerService _service = new BoredAPIManagerService();
            await _service.RetrievesActivities();
        }

        [FunctionName("GetActivities")]
        public async Task<IActionResult> GetActivities(
           [HttpTrigger(AuthorizationLevel.Function, "get", Route = "activities")] HttpRequest request,
           ILogger loggery)
        {
            var activsdata = await _service.ReadAllAsync();
            var result = activsdata.Select(activity => new Activity
            {
                Accessibility = activity.accessibility,
                Id = activity.key,
                Name = activity.type,
                Participants = activity.participants,
                Price = activity.price,
                Type = activity.type,
            }).ToList();
            return new OkObjectResult(result);
        }

        [FunctionName("GetActivitiesByType")]
        public async Task<IActionResult> GetActivitiesByType(
           [HttpTrigger(AuthorizationLevel.Function, "get", Route = "activities/{type}/{price}")] HttpRequest request,
           ILogger loggery, string type, double price)
        {
            var activs = await _service.ReadAllAsync(type, price);
            var result = activs.Select(activity => new Activity
            {
                Accessibility = activity.accessibility,
                Id = activity.key,
                Name = activity.type,
                Participants = activity.participants,
                Price = activity.price,
                Type = activity.type,
            }).ToList();
            return new OkObjectResult(result);
        }

        [FunctionName("GetActivityById")]
        public async Task<IActionResult> GetActivityById(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "activity/{id}")] HttpRequest request,
            ILogger logger, string id)
        {
            var ac = await _service.GetByIdAsync(id);
            Activity activity = new Activity();
            if (ac != null)
            {
                activity = new Activity
                {
                    Accessibility = ac.accessibility,
                    Id = ac.key,
                    Name = ac.type,
                    Participants = ac.participants,
                    Price = ac.price,
                    Type = ac.type,
                };
            }
            return new OkObjectResult(activity);
        }

        [FunctionName("AddActivity")]
        public async Task<IActionResult> AddActivity(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "activity")]
        [RequestBodyType(typeof(Activity), "request")] HttpRequest request,
            ILogger logger)
        {
            var requestBody = request.Body;
            //string requestContent = await new StreamReader(requestBody).ReadToEndAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var acdto = JsonSerializer.Deserialize<Activity>(requestBody, options);
            ActivityDto ac = new ActivityDto
            {
                key = acdto.Id,
                type = acdto.Name,
                price = acdto.Price,
                PartitionKey = "Activity",
                RowKey = acdto.Id,
                accessibility = acdto.Accessibility,
                participants = acdto.Participants,
            };
            var isxist = await _service.IsExist(ac);
            if (isxist == false)
            {
                var entites = await _service.ReadAllAsync();
                if (entites.ToList().Count == 100)
                {
                    await _service.DeleteAsync(entites.First());
                }
                logger.LogInformation($"the activity was created and stored");
                await _service.AddAsync(ac);

                return new CreatedResult($"activities/{ac.key}", ac);
            }

            logger.LogInformation($"an activity with the activity already exists");
            return new ConflictResult();
        }

        [FunctionName("UpdateActivity")]
        public async Task<IActionResult> UpdateActivity(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "activity/{id}")][RequestBodyType(typeof(Activity), "request")] HttpRequest request,
            ILogger logger, string id)
        {
            var requestBody = request.Body;
            var streamReader = new StreamReader(requestBody);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var acdto = JsonSerializer.Deserialize<Activity>(requestBody, options);
            var result = await _service.GetByIdAsync(id);
            result.price = acdto.Price;
            result.accessibility = acdto.Accessibility;
            result.type = acdto.Type;
            result.participants = acdto.Participants;
            await _service.UpdateAsync(result);
            return new OkObjectResult(result);
        }


        [FunctionName("DeleteActivity")]
        public async Task<IActionResult> DeleteActivity(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "activity/{id}")] HttpRequest request,
            ILogger logger, string id)
        {
            var result = await _service.GetByIdAsync(id);
            var delete = _service.DeleteAsync(result);
            return new OkObjectResult(delete);
        }
    }
}
