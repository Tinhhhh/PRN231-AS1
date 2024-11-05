using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class SilverJewelryDAO
    {
        private SilverJewelry2023DbContext _context;
        private static SilverJewelryDAO _instance;

        public SilverJewelryDAO()
        {
            _context = new SilverJewelry2023DbContext();
        }

        public static SilverJewelryDAO Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SilverJewelryDAO();
                }
                return _instance;
            }
        }
        public SilverJewelry GetSilverJewelry(string id)
        {
            return _context.SilverJewelries.Include(s => s.Category).SingleOrDefault(s => s.SilverJewelryId.Equals(id));
        }

        public List<SilverJewelry> GetSilverJewelries()
        {
            return _context.SilverJewelries.Include(s => s.Category).ToList();
        }

        public bool AddJewelry(SilverJewelry silverJewelry)
        {
            bool result = false;
            SilverJewelry silverJewelry1 = this.GetSilverJewelry(silverJewelry.SilverJewelryId);
            if (silverJewelry1 == null)
            {
                try
                {
                    _context.SilverJewelries.Add(silverJewelry);
                    _context.SaveChanges();
                    result = true;
                }
                catch (Exception ex)
                {

                }
            }

            return result;
        }
        public bool DeleteJewelry(string jewelryId)
        {
            bool result = false;
            SilverJewelry silverJewelry1 = this.GetSilverJewelry(jewelryId);
            if (silverJewelry1 != null)
            {
                try
                {
                    _context.SilverJewelries.Remove(silverJewelry1);
                    _context.SaveChanges();
                    result = true;
                }
                catch (Exception ex)
                {

                }
            }

            return result;
        }
        public bool UpdateJewelry(SilverJewelry silverJewelry)
        {
            bool result = false;
            SilverJewelry silverJewelry1 = this.GetSilverJewelry(silverJewelry.SilverJewelryId);
            if (silverJewelry1 != null)
            {
                try
                {
                    /*                    _context.Entry<SilverJewelry>(silverJewelry1).State
                                = Microsoft.EntityFrameworkCore.EntityState.Modified;*/

                    _context.Entry(silverJewelry1).CurrentValues.SetValues(silverJewelry);
                    _context.SaveChanges();
                    result = true;
                }
                catch (Exception ex)
                {

                }
            }

            return result;
        }
    }
}
