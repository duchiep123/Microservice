using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GarageManagementModels
{
    public class Car
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } = "alex";
        public string Color { get; set; }
        public string Brand { get; set; }
        public virtual Garage Garage { get; set; }
    }
}
