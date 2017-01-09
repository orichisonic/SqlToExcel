using Com.Centaline.Framework.Kernel.Injection;
using Com.Centaline.Framework.Kernel.Injection.Interface;

namespace SqlToExcel.Module.ClassLibrary
{
    public class ServiceLocatorFatory : ObjectContainerFatory
    {
        public override IObjectContainer Create(params object[] args)
        {
            ServiceLocator serviceLocator = new ServiceLocator();
            serviceLocator.Init();
            Register(serviceLocator);
            _ObjectContainer = serviceLocator;
            return _ObjectContainer;
        }
    }
}