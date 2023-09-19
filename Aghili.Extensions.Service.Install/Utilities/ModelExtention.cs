using System.Text.RegularExpressions;
using Aghili.Extensions.Service.Install.Extensions;

namespace Aghili.Extensions.Service.Install.Utilities
{
    public class ModelExtention
    {
        public static void SetModelProperties(object model, string pattern, string text)
        {
            var regex = new System.Text.RegularExpressions.Regex(pattern);
            SetModelProperties(model, regex.Matches(text).First());
        }

        public static void SetModelProperty(object model, string name, string value)
        {
            name = name.Trim().Replace(' ','_');
            value = value.TrimEnd().TrimStart();
            var modelType = model.GetType();
            var property = modelType.GetProperty(name);
            if (property == null)
                return;
            if (property.PropertyType.IsArray)
            {
                var regex = new Regex("(\\w*)[\\,|]");
                var splited = regex.Split(value).Where(i=> !string.IsNullOrEmpty(i)).ToArray();
                var result = Array.CreateInstance(property.PropertyType.GetElementType(), splited.Length);
                for (int i = 0; i < splited.Length; i++)
                {
                    result.SetValue(ConvertValue(splited[i], property.PropertyType.GetElementType()), i);
                };
                property.SetValue(model, result, null);

            }
            else
            {
                var converted = ConvertValue(value, property.PropertyType);
                property.SetValue(model, converted, null);
            }
        }

        internal static List<Dictionary<string, string>> GetModelProperties(string pattern,string Text, string SectionStartFlag,bool StartFlagIsProperty = false)
        {
            List<Dictionary<string,string>> modelsProperties= new();

            RegexOptions options = RegexOptions.Multiline;

            var matches = Regex.Matches(Text,pattern, options);
             Dictionary<string,string>? modelPropertyList = null;
            foreach(Match match in matches)
            {
                if (match.Groups[SectionStartFlag].Success)
                {
                    modelPropertyList = new();
                    modelsProperties.Add(modelPropertyList);
                    if (StartFlagIsProperty)
                        modelPropertyList.Add(SectionStartFlag, match.Groups[SectionStartFlag].Value);
                }
                if (match.Groups["key"].Success)
                    modelPropertyList.Add(match.Groups["key"].Value, match.Groups["value"].Value);
            }
            return modelsProperties;
        }

        internal static void SetModelProperties(object model, Match match)
        {
            foreach (Group line in match.Groups)
            {
                //var property_raw = regex.Split(line);
                if (line.Length > 1)
                {
                    ModelExtention.SetModelProperty(model, line.Captures[1].Value, line.Captures[2].Value);
                }
            }
        }

        private static object ConvertValue(string value, Type type)
        {
            if (type.IsEnum)
            {
                return Enum.Parse(type, value, true);
            }
            else
            {
                return Convert.ChangeType(value, type);
            }
        }
    }
}