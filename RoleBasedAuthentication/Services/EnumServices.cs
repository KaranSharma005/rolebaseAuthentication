using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace RoleBasedAuthentication.Services
{
    public static class EnumServices
    {
        public static string GetEnumDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            ?.Name ?? enumValue.ToString();
        }
    }
}
