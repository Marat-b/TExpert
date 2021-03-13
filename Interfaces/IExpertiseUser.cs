using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TExp.Interfaces
{
    interface IExpertiseUser
    {
        bool ReplaceUserForExpertise(int expertiseId, int userId);
        bool AddUserToExpertise(int expertiseId, int userId);
    }
}
