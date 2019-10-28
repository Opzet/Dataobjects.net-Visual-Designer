using System.Collections.Generic;
using System.Linq;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    public static class InheritanceTreeCache
    {
        private static readonly Dictionary<IInterface, InheritanceTree> cache = new Dictionary<IInterface, InheritanceTree>();
        private static readonly object sync = new object();

        public static void Initialize()
        {
            lock(sync)
            {
                cache.Clear();
            }
        }

        public static void Purge(IInterface @interface)
        {
            lock (sync)
            {
                if (cache.ContainsKey(@interface))
                {
                    cache.Remove(@interface);
                }
            }
        }

        public static IEnumerable<InheritanceTree> Get(params IInterface[] interfaces)
        {
            List<InheritanceTree> result = new List<InheritanceTree>();
            lock (sync)
            {
                result.AddRange(interfaces.Select(Get));
            }

            return result;
        }

        public static InheritanceTree Get(IInterface @interface)
        {
            InheritanceTree result;

            lock (sync)
            {
                if (cache.ContainsKey(@interface))
                {
                    result = cache[@interface];
                }
                else
                {
                    result = @interface.GetInheritanceTree();
                    cache.Add(@interface, result);
                }
            }

            return result;
        }
    }
}