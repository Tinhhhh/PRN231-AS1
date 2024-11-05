using BusinessObject;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class CategoryRepo : ICategoryRepo
    {
        public List<Category> GetCategories()
        {
            return CategoryDAO.Instance.GetCategories();
        }

        public Category GetCategory(string id)
        {
            return CategoryDAO.Instance.GetCategory(id);
        }
    }
}
