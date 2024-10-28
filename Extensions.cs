using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TelegramChatBot
{
    public static class Extensions
    {
        private static Dictionary<Sex, string> _SexFormated = new Dictionary<Sex, string>
        {
            { Sex.Male, "Мужской" },
            { Sex.Female, "Женский" }
        };

        public static string Format(this Sex? _Sex)
        {
            if (_Sex == null)
                return "Не указан";
            return _SexFormated[(Sex)_Sex];
        }

        public static string ToSerializedString(this Enum _Enum)
        {
            Type EnumType = _Enum.GetType();
            MemberInfo[] Members = EnumType.GetMember(_Enum.ToString());
            List<EnumMemberAttribute> Attributes = Members[0].GetCustomAttributes<EnumMemberAttribute>().ToList();
            if (Attributes.Count == 0 || !(Attributes[0] is EnumMemberAttribute _EnumMemberAttribute) || _EnumMemberAttribute.Value == null)
                return _Enum.ToString();

            return _EnumMemberAttribute.Value;
        }
    }
}
