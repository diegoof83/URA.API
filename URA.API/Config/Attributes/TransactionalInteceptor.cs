using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Transactions;

namespace URA.API.Config.Attributes
{
    public class TransactionalInteceptor<T> : DispatchProxy, ITransactionInterceptor<T> where T : class
    {
        public T Target { get; set; }

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            object result = null;

            var attribute = targetMethod.GetCustomAttribute<TransactionalAttribute>();

            if (attribute is null)
            {
                result = targetMethod.Invoke(Target, args);
            }
            else
            {
                ////open a transaction 
                var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                //{
                result = targetMethod.Invoke(Target, args);
                var resultTask = result as Task;

                if (resultTask != null)
                {
                    resultTask.ContinueWith(task =>
                    {
                        if (task.Exception != null)
                        {
                            scope.Dispose();
                            //LogException(task.Exception.InnerException ?? task.Exception, targetMethod);
                        }
                        else
                        {
                            object taskResult = null;
                            if (task.GetType().GetTypeInfo().IsGenericType &&
                                task.GetType().GetGenericTypeDefinition() == typeof(Task<>))
                            {
                                var property = task.GetType().GetTypeInfo().GetProperties()
                                    .FirstOrDefault(p => p.Name == "Result");
                                if (property != null)
                                {
                                    taskResult = property.GetValue(task);
                                }
                            }
                            scope.Complete();
                            scope.Dispose();
                            //LogAfter(targetMethod, args, taskResult);
                        }
                    });
                }
                //else
                //{
                //try
                //{
                //    LogAfter(targetMethod, args, result);
                //}
                //catch (Exception ex)
                //{
                //    //Do not stop method execution if an exception  
                //    LogException(ex);
                //}
                //}

                //}
                //return result;


                //result = InvokeAsync(targetMethod, args);
            }
            return result;
        }

        private async Task<object> InvokeAsync(MethodInfo targetMethod, object[] args)
        {
            Task<object> result = null;

            //open a transaction 
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var ob = targetMethod.Invoke(Target, args);
                if (ob is not null)
                {
                    result = (Task<object>)await (ob as Task<object>);
                }

                scope.Complete();
            }

            return result;
        }

        public static T Create(T decorated)
        {
            object proxy = Create<T, TransactionalInteceptor<T>>();
            ((TransactionalInteceptor<T>)proxy).Target = decorated;

            return (T)proxy;
        }

        //// Implement the TryInvokeMember method of the DynamicObject class for
        //// dynamic member calls that have arguments.
        //public override bool TryInvokeMember(InvokeMemberBinder binder,
        //                                     object[] args,
        //                                     out object result)
        //{
        //    //reflection
        //    MethodInfo method = Target.GetType().GetMethod(binder.Name);

        //    var attribute = method.GetCustomAttribute<TransactionalAttribute>();

        //    if (attribute is null)
        //    {
        //        result = method.Invoke(Target, args);
        //    }
        //    else
        //    {
        //        //open a transaction 
        //        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        //        {
        //            result = method.Invoke(Target, args);
        //            scope.Complete();
        //        }
        //    }

        //    return result == null ? false : true;
        //}
    }
}
