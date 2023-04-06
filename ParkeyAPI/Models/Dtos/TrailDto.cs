using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using static ParkeyAPI.Models.Trail;

namespace ParkeyAPI.Models.Dtos
{
    public class TrailDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Distance { get; set; }
        public DifficultyType Difficulty { get; set; }
        public int NationalParkId { get; set; }
        public NationalParkDto NationalPark { get; set; }
    }
}
