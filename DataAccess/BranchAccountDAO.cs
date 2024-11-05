using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class BranchAccountDAO
    {
        private SilverJewelry2023DbContext _context;
        private static BranchAccountDAO _instance;

        public BranchAccountDAO()
        {
            _context = new SilverJewelry2023DbContext();
        }

        public static BranchAccountDAO Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BranchAccountDAO();
                }
                return _instance;
            }
        }
        public BranchAccount GetBranchAccount(string email, string password)
        {
            return _context.BranchAccounts.SingleOrDefault(a => a.EmailAddress.Equals(email) && a.AccountPassword.Equals(password));
        }
    }
}
