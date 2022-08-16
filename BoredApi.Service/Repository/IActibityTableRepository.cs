using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Threading.Tasks;
using FunctionApp1.Models.Dto;

namespace BoredApi.Service.Repository
{
    public interface IActibityTableRepository
    {
        Task<ActivityDto> AddAsync(TableEntity activity);
        Task<ActivityDto> UpdateAsync(TableEntity activity);
        Task<bool> IsExist(ActivityDto activity);
        Task<bool> DeleteAsync(TableEntity entity);
        Task<List<ActivityDto>> ReadAllAsync();
        Task<List<ActivityDto>> ReadAllAsync(string type, double price);
        Task<ActivityDto> GetByIdAsync(string id);
    }
}
