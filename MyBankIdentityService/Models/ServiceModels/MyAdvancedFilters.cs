using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Threading.Tasks;

namespace MyBankIdentityService.Models.ServiceModels
{
    public class MyAdvancedFilters : AdvancedFilters
    {
        public MyAdvancedFilters(Principal principal) : base(principal)
        {
        }

        public void WhereCompanyNotSet()
        {
            this.AdvancedFilterSet("company", "", typeof(string), MatchType.NotEquals);
        }
        public void WhereAttrEqualsValue(ADQuery query)
        {
            if (!string.IsNullOrEmpty(query.ValueType) && query.ValueType.Equals("dt"))
            {
                // -4 hour is related with ad datetime, i have no time for investigate that why it happens
                this.AdvancedFilterSet(query.Attribute, DateTime.Parse(query.Value).AddHours(-4).ToString("yyyyMMddHHmmss.0Z"), typeof(string), (MatchType)query.MatchType);
            }
            else
            {
                this.AdvancedFilterSet(query.Attribute, query.Value, typeof(string), (MatchType)query.MatchType);
            }

        }

    }
}
