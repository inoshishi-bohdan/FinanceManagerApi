using FinanceManager.Util;
using System.Reflection;

namespace FinanceManagerApi.Extensions
{
    public static class EnumExtension
    {
        public static string GetDisplayAsOrName(this Enum value)
        {
            var members = value.GetType().GetMember(value.ToString());
            if (members.Length == 0) return value.ToString();
            var attribute = members.First().GetCustomAttribute<DisplayAsAttribute>();
            if (attribute == null) return value.ToString();
            return attribute.DisplayAs;
        }
    }
}
