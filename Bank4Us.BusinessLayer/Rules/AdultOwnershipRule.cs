using System;
using System.Collections.Generic;
using System.Text;
using Bank4Us.Common.CanonicalSchema;
using NRules.Fluent.Dsl;

namespace Bank4Us.BusinessLayer.Rules
{
    public class AdultOwnershipRule : Rule
    {
        public override void Define()
        {
            Customer customer = null;

            //INFO: Left-hand side of the rule.
            //RULE: A customer must be an adult to own an account.  
            When()
                .Match<Customer>(() => customer, c => c.Age < 18 & c.Accounts.Count > 0);

            //INFO: Right-hand side of the rule.
            //REMARK: Remove accounts action, then notify user.  
            Then()
                .Do(ctx => customer.Accounts.Clear())
                .Do(ctx => customer.BusinessRuleNotifications.Add("A customer must be an adult to own an account."));
        }
    }
}
