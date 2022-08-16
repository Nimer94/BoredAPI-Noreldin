using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using BoredApi.Service.Repository;
using BoredApi.Service.Models;
using FunctionApp1.Models.Dto;
using Microsoft.WindowsAzure.Storage.Table;

namespace BoredApi.Service.Services
{
    public class BoredAPIManagerService : IBoredAPIManagerService
    {

        private IActibityTableRepository _repository;

        public BoredAPIManagerService()
        {
            _repository = new ActibityTableRepository();

        }

        public BoredAPIManagerService(IActibityTableRepository repository)
        {
            _repository = repository;
        }

        public async Task<IList<Activity>> RetrievesActivities()
        {
            var random = new Random();
            var threshold = Environment.GetEnvironmentVariable("ACTIVITIES_THRESHOLD");
            var id = 0;

            var activies = new List<Activity>();

            if (threshold is not null)
            {
                _ = int.TryParse(threshold, out id);
                id = Math.Min(id, 20);
            }
            else
            {
                id = random.Next(1, 20);
            }

            for (int i = 0; i < id; i++)
            {
                var client = new HttpClient();

                var request = new HttpRequestMessage(HttpMethod.Get, $"http://www.boredapi.com/api/activity");

                var response = await client.SendAsync(request);

                var result = await response.Content.ReadAsAsync<ActivityDto>();

                Activity ac = new Activity
                {
                    Id = result.key,
                    Name = result.type,
                    Type = result.type,
                    Price = result.price,
                    Accessibility = result.accessibility,
                    Participants = result.participants,
                };
                activies.Add(ac);
            }
            return activies;
        }

        public async Task<ActivityDto> AddAsync(TableEntity activity)
        {
            var activityDto = new ActivityDto();
            try
            {
                activityDto = await _repository.AddAsync(activity);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
            }
            return activityDto;
        }

        public async Task<bool> DeleteAsync(TableEntity entity)
        {
            try
            {
                  await _repository.DeleteAsync(entity);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return false;
            }
            return true;
        }

        public async Task<ActivityDto> GetByIdAsync(string id)
        {
            var activityDto = new ActivityDto();
            try
            {
                activityDto = await _repository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
            }
            return activityDto;
        }

        public async Task<bool> IsExist(ActivityDto activity)
        {
            return await _repository.IsExist(activity);
        }

        public async Task<List<ActivityDto>> ReadAllAsync()
        {
            return await _repository.ReadAllAsync();
        }

        public async Task<List<ActivityDto>> ReadAllAsync(string type, double price)
        {
            return await _repository.ReadAllAsync(type, price);
        }


        public async Task<ActivityDto> UpdateAsync(TableEntity activity)
        {
            return await _repository.UpdateAsync(activity);
        }

        
    }
}
