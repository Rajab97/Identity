using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Threading.Tasks;

namespace MyBankIdentityService.Models.ServiceModels
{
    [DirectoryObjectClass("user")]
    public class MyUserPrincipal : UserPrincipal
    {
        private MyAdvancedFilters _myAdvancedFilters;

        public MyUserPrincipal(PrincipalContext context) : base(context) { }

        public MyAdvancedFilters MyAdvancedFilters
        {
            get
            {
                return this.AdvancedSearchFilter as MyAdvancedFilters;
            }
        }

        public override AdvancedFilters AdvancedSearchFilter
        {
            get
            {
                if (_myAdvancedFilters == null)
                {
                    _myAdvancedFilters = new MyAdvancedFilters(this);
                }

                return _myAdvancedFilters;
            }
        }
    }
}
