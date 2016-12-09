using System;
using System.Collections.Generic;
using Com.Centaline.Framework.Kernel.Injection.Interface;
using Spring.Context;
using Spring.Context.Support;

namespace SqlToExcel.Module.ClassLibrary
{
    public class ServiceLocator : IObjectContainer
    {
        private object _LockObject = new object();
        private static IApplicationContext _Container;

        public ServiceLocator()
        {
            
        }
        
        public T GetObject<T>() where T:class 
        {
            return _Container.GetObject<T>();
        }

        public T GetObject<T>(string name) where T:class 
        {
            return _Container.GetObject<T>(name);
        }

        public object GetObject(string name)
        {
            return _Container.GetObject(name);
        }

        public void InitializeFromConfigFile(string configSectionName)
        {
            throw new NotImplementedException();
        }

        public T GetWrappedContainer<T>()
        {
            throw new NotImplementedException();
        }

        public T GetObject<T>(object overridedArguments) where T : class
        {
            throw new NotImplementedException();
        }

        public object GetObject(Type serviceType, object overridedArguments)
        {
            throw new NotImplementedException();
        }

        public Array ResolveAll(Type serviceType)
        {
            throw new NotImplementedException();
        }

        public T[] ResolveAll<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public object GetService(Type serviceType)
        {
            IDictionary<string,object> dictionary = _Container.GetObjectsOfType(serviceType);
            return dictionary[serviceType.FullName];
        }

        public void Init()
        {
            lock (_LockObject)
            {
                if (null == _Container)
                {
                    try
                    {
                        _Container = ContextRegistry.GetContext();

                    }
                    catch (System.Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }
    }
}