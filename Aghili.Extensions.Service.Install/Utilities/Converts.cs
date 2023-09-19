namespace Aghili.Extensions.Service.Install.Utilities
{
    public static partial class Converts
    {
        public static string ConvertToString(this Enum value)
        {
            if (value == null)
            {
                return "";
            }
            else
                return value.ToString().Replace("_dash_", "-");
        }
    }
}