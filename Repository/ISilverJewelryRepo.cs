using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface ISilverJewelryRepo
    {
        public List<SilverJewelry> GetSilverJewelries();
        public SilverJewelry GetSilverJewelry(string id);
        public bool UpdateJewelry(SilverJewelry silverJewelry);
        public bool DeleteJewelry(string jewelryId);
        public bool AddJewelry(SilverJewelry silverJewelry);


    }
}
