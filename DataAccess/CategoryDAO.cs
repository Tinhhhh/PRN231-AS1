using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class CategoryDAO
    {
        private SilverJewelry2023DbContext _context;
        private static CategoryDAO _instance;

        public CategoryDAO()
        {
            _context = new SilverJewelry2023DbContext();
        }

        public static CategoryDAO Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CategoryDAO();
                }
                return _instance;
            }
        }
        public List<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }
        public Category GetCategory(string id)
        {
            return _context.Categories.SingleOrDefault(c => c.CategoryId.Equals(id));
        }
    }
}
