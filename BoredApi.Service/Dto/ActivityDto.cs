using Microsoft.WindowsAzure.Storage.Table;

namespace FunctionApp1.Models.Dto
{
    public class ActivityDto : TableEntity
    {
        public string type { get; set; }
        public string activity { get; set; }
        public double accessibility { get; set; }
        public int participants { get; set; }
        public double price { get; set; }
        public string link { get; set; }
        public string key { get; set; }
    }
}
