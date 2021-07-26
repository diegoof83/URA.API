using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace URA.API.Config.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class TransactionalAttribute : Attribute
    {
        public TransactionScopeAsyncFlowOption Option { get; private set; }

        public TransactionalAttribute()
        {
            Option = TransactionScopeAsyncFlowOption.Suppress;
        }

        public TransactionalAttribute(TransactionScopeAsyncFlowOption option)
        {
            Option = option;
        }
    }
}
