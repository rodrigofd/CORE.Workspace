using System.Collections.Generic;
using System.Linq;
using DevExpress.Xpo;

namespace FDIT.Core.Util
{
    public static class XpoExtensions
    {
        public static void Empty<T>(this IEnumerable<T> collection) where T : XPBaseObject
        {
            collection.ToList().ForEach(item => { item.Delete(); });
        }
    }
}