using Com.Centaline.Framework.QuickQuery.Utils;
using Com.Centaline.Framework.Kernel.Injection.Interface;
using Com.Centaline.Framework.QuickQuery;

namespace SqlExport
{
    public class SqlExportContext
    {
        public static bool Debug { get; set; }

        public static string Connection { get; set; }

        public static QuickQueryConfig Config { get; set; }

        public static void Init(QuickQueryConfig config)
        {
            if (!string.IsNullOrEmpty(config.Connection))
            {
                Connection = config.Connection;
            }
            Config = config;
        }

        public static void Init(IObjectContainer objectContainer)
        {
            if (null == objectContainer) return;
            ObjectContainer.Instance = objectContainer; ;
        }
    }
}
