using System;
using System.Collections.Generic;
using System.Text;
using Bank4Us.Common.CanonicalSchema;
using Bank4Us.Common.Core;
using NRules.Fluent.Dsl;

namespace Bank4Us.BusinessLayer.Rules
{
    public class MinimumBalanceRule : Rule
    {
        public override void Define()
        {
            Account account = null;

            When()
                .Match<Account>(() => account, a => a.Balance < 100 
                                                    && !a.UpdatedBy.Equals("MinimumBalanceRule")
                                                    && !a.UpdatedOn.Equals(DateTime.Today));

            Then()
                .Do(ctx => ApplyMinBalanceFee(account))
                .Do(ctx => account.BusinessRuleNotifications.Add("An account with a minimum daily balance below $100.00, must be charged a $35.00 fee."));
        }

        private static void ApplyMinBalanceFee(Account account)
        {
            account.Balance -= 35;
            account.UpdatedBy = "MinimumBalanceRule";
            account.UpdatedOn = DateTime.Now;
        }
    }
}
