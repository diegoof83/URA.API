using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Remoting;
using System.Reflection;

namespace URA.API.Config.Attributes
{
    public interface ITransactionInterceptor<T> where T : class
    {
        public T Target { get; set; }
    }
}
