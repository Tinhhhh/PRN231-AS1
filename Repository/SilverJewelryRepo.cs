using BusinessObject;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class SilverJewelryRepo : ISilverJewelryRepo
    {
        public bool AddJewelry(SilverJewelry silverJewelry)
        {
            return SilverJewelryDAO.Instance.AddJewelry(silverJewelry);
        }

        public bool DeleteJewelry(string jewelryId)
        {
            return SilverJewelryDAO.Instance.DeleteJewelry(jewelryId);
        }


        public List<SilverJewelry> GetSilverJewelries()
        {
            return SilverJewelryDAO.Instance.GetSilverJewelries();
        }

        public SilverJewelry GetSilverJewelry(string id)
        {
            return SilverJewelryDAO.Instance.GetSilverJewelry(id);
        }

        public bool UpdateJewelry(SilverJewelry silverJewelry)
        {
            return SilverJewelryDAO.Instance.UpdateJewelry(silverJewelry);
        }
    }
}
