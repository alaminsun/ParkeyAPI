using ParkeyAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkeyAPI.Repository.IRepository
{
    public interface INationalParkRepository
    {
        ICollection<NationalPark> GetNationalParks();
        NationalPark GetNationalPark(int nationalParkId);
        bool NationalParkExists(string name);
        bool NationalParkExists(int id);
        bool CreateNatinalPark(NationalPark nationalPark);
        bool UpadteNatinalPark(NationalPark nationalPark);
        bool DeleteNatinalPark(NationalPark nationalPark);
        bool Save();


    }
}
