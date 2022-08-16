using BoredApi.Service.Models;
using FunctionApp1.Models.Dto;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BoredApi.Service.Services
{
    public interface IBoredAPIManagerService
    {
        public Task<IList<Activity>> RetrievesActivities();
        Task<ActivityDto> AddAsync(TableEntity activity);
        Task<ActivityDto> UpdateAsync(TableEntity activity);
        Task<bool> IsExist(ActivityDto activity);
        Task<bool> DeleteAsync(TableEntity entity);
        Task<List<ActivityDto>> ReadAllAsync();
        Task<List<ActivityDto>> ReadAllAsync(string type, double price);
        Task<ActivityDto> GetByIdAsync(string id);
    }
}
