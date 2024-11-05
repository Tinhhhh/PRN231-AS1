using BusinessObject;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class BranchAccountRepo : IBranchAccountRepo
    {
        public BranchAccount GetBranchAccount(string email, string password)
        {
            return BranchAccountDAO.Instance.GetBranchAccount(email, password);
        }
    }
}
