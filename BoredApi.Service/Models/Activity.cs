using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace BoredApi.Service.Models
{
    public class Activity 
    {
        
        [Required]
        public string Id { get; set; }
        [Required]
        
        public string Type { get; set; }
        [Required]
        
        public string Name { get; set; }
        [Required]
        
        public double Accessibility { get; set; }
        [Required]
        
        public int Participants { get; set; }
        [Required]
        public double Price { get; set; }
    }
}
