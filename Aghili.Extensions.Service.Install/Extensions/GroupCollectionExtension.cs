using System.Text.RegularExpressions;

namespace Aghili.Extensions.Service.Install.Extensions
{
    internal static class GroupCollectionExtension
    {
#if NETSTANDARD
        public static bool ContainsKey(this GroupCollection items,string value)
        {
            foreach (Group item in items)
                if (item.Value.Equals(value))
                    return true;
            return false;
        }
    
#endif
    }
}
