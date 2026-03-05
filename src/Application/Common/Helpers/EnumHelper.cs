using Application.Common.DTOs;

namespace Application.Common.Helpers
{
    public static class EnumHelper
    {
        public static int ToEnumId(this Enum enumValue)
        {
            if (enumValue == null)
            {
                return 0;
            }

            return Convert.ToInt32(enumValue);
        }


        public static EnumDTO ToEnumDto(this Enum enumValue)
        {
            if (enumValue == null)
            {
                return new EnumDTO
                {
                    Id = 0,
                    Nome = string.Empty
                };
            }

            return new EnumDTO
            {
                Id = enumValue.ToEnumId(),
                Nome = enumValue.ToString()
            };
        }
    }
}
