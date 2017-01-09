using System;
using System.Collections.Generic;
using Com.Centaline.Framework.Kernel.Injection.Interface;
using Spring.Context;
using Spring.Context.Support;

namespace SqlToExcel.Module.ClassLibrary
{
    public class ServiceLocator : IObjectContainer
    {
        private object _lockObject = new object();
        private static IApplicationContext _container;

        public ServiceLocator()
        {
            
        }
        
        public T GetObject<T>() where T:class 
        {
            return _container.GetObject<T>();
        }

        public T GetObject<T>(string name) where T:class 
        {
            return _container.GetObject<T>(name);
        }

        public object GetObject(string name)
        {
            return _container.GetObject(name);
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
            IDictionary<string,object> dictionary = _container.GetObjectsOfType(serviceType);
            return dictionary[serviceType.FullName];
        }

        public void Init()
        {
            lock (_lockObject)
            {
                if (null == _container)
                {
                    try
                    {
                        _container = ContextRegistry.GetContext();

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