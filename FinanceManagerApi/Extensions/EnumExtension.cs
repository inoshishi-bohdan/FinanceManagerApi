using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace FinanceManagerApi.Extensions
{
    public static class EnumExtension
    {
        public static string GetDisplayName(this Enum value)
        {
            var members = value.GetType().GetMember(value.ToString());
            if (members.Length == 0) return value.ToString();
            
            var attribute = members.First().GetCustomAttribute<DisplayAttribute>();
            if (attribute == null || attribute.Name == null) return value.ToString();
            
            return attribute.Name;
        }
    }
}
