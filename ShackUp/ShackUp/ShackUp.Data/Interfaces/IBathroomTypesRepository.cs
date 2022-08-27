using ShackUp.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShackUp.Data.Interfaces
{
    public interface IBathroomTypesRepository
    {
        List<BathroomType> GetAll();
    }
}
