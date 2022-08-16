using FunctionApp1.Models.Dto;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoredApi.Service.Repository
{
    public class ActibityTableRepository : IActibityTableRepository
    {

        public readonly CloudTable _cloudTable;
        public readonly string _accountName;
        public readonly string _Key;

        public ActibityTableRepository()
        {
            _accountName = Environment.GetEnvironmentVariable("TABLE_ACCOUNT_NAME");
            _Key = Environment.GetEnvironmentVariable("KEY_VALUE");
            var _storageAccount = new CloudStorageAccount(new StorageCredentials(
                _accountName, _Key), true);
            _cloudTable = _storageAccount.CreateCloudTableClient().GetTableReference("activity");
            _cloudTable.CreateIfNotExistsAsync();

            //CloudStorageAccount account = CloudStorageAccount.Parse("UseDevelopmentStorage=true");
            //_cloudTable = account.CreateCloudTableClient().GetTableReference("activity");
        }

        public ActibityTableRepository(CloudTable cloudTableClient)
        {
            _cloudTable = cloudTableClient;
            _cloudTable.CreateIfNotExistsAsync();

        }

        public async Task<ActivityDto> AddAsync(TableEntity activity)
        {
            await _cloudTable.CreateIfNotExistsAsync();
            TableOperation insertOperation = TableOperation.Insert(activity);
            var result  = await _cloudTable.ExecuteAsync(insertOperation);
            ActivityDto entity = result.Result as ActivityDto;
            return entity;
        }


        public async Task<bool> DeleteAsync(TableEntity entity)
        {
            await _cloudTable.CreateIfNotExistsAsync();
            var tableOperation = TableOperation.Delete(entity);
            await _cloudTable.ExecuteAsync(tableOperation);
            return true;
        }

        public async Task<List<ActivityDto>> ReadAllAsync()
        {
            TableContinuationToken token = null;
            await _cloudTable.CreateIfNotExistsAsync();
            var activities = _cloudTable.ExecuteQuerySegmentedAsync(new TableQuery<ActivityDto>(), token).Result.OrderBy(p => p.Timestamp).ToList();
            return activities;
        }

        public async Task<List<ActivityDto>> ReadAllAsync(string type, double price)
        {
            TableContinuationToken token = null;
            await _cloudTable.CreateIfNotExistsAsync();
            var activities = _cloudTable.ExecuteQuerySegmentedAsync(new TableQuery<ActivityDto>(), token).Result.Where(p => p.type == type && p.price < price).OrderBy(p => p.Timestamp).Take(50).ToList();
            return activities;
        }

        public async Task<ActivityDto> GetByIdAsync(string id)
        {
            await _cloudTable.CreateIfNotExistsAsync();
            TableContinuationToken token = null;
            var result = _cloudTable.ExecuteQuerySegmentedAsync(new TableQuery<ActivityDto>(), token).Result.FirstOrDefault(p => p.RowKey == id);
            return result;
        }

        public async Task<ActivityDto> UpdateAsync(TableEntity activity)
        {
            await _cloudTable.CreateIfNotExistsAsync();
            TableOperation replaceOperation = TableOperation.Replace(activity);
            TableResult result = await _cloudTable.ExecuteAsync(replaceOperation);
            ActivityDto entity = result.Result as ActivityDto;
            return entity;
        }


        public async Task<bool> IsExist(ActivityDto activity)
        {
            bool isexist = true;
            await _cloudTable.CreateIfNotExistsAsync();
            var result = await _cloudTable.ExecuteAsync(TableOperation.Retrieve<ActivityDto>("Activity", activity.key));
            if (result.Result == null)
            {
                isexist = false;
            }
            return isexist;
        }
    }
}
