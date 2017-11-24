using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RimWorldDataExporter.Helper.Serialization {
    class Serializable : Attribute {}
    class Unserializable : Attribute { }

    static class Serializer {

        private readonly static BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        private static Dictionary<Type, Func<object, string>> basicConverters = new Dictionary<Type, Func<object, string>>{
            {
                typeof(bool),
                value => (bool)value ? "true" : "false"
            },
            {
                typeof(string),
                value => $"\"{value}\""
            },
            {
                typeof(int),
                value => value.ToString()
            },
            {
                typeof(float),
                value => value.ToString()
            },
            {
                typeof(double),
                value => value.ToString()
            },
        };

        private static string IListToJson<T> (IList<T> list, int level) {
            if (list == null) return "null";
            if (list.Count == 0) return "[]";

            Type itemType = typeof(T);

            if (basicConverters.TryGetValue(itemType, out Func<object, string> converter)) {
                return $"[{string.Join(", ", list.Select(item => converter.Invoke(item)).ToArray())}]";
            } else {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("[");
                StringBuilder separatorBuilder = new StringBuilder();
                separatorBuilder.AppendLine(",");
                for (int i = 0; i < level; i++) {
                    sb.Append("  ");
                    separatorBuilder.Append("  ");
                }
                string separator = separatorBuilder.ToString();
                sb.AppendLine(string.Join(separator, list.Select(item => $"{item.ToJson(level + 1)}").ToArray()));
                for (int i = 0; i < level - 1; i++) {
                    sb.Append("  ");
                }
                sb.Append("]");
                return sb.ToString();
            }
        }

        private static string IDictionaryToJson<TKey, TValue>(IDictionary<TKey, TValue> dict, int level) {
            if (dict == null) return "null";
            if (dict.Count == 0) return "{}";

            Type valueType = typeof(TValue);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("{");
            StringBuilder separatorBuilder = new StringBuilder();
            separatorBuilder.AppendLine(",");
            for (int i = 0; i < level; i++) {
                sb.Append("  ");
                separatorBuilder.Append("  ");
            }
            string separator = separatorBuilder.ToString();
            sb.AppendLine(string.Join(separator, dict.Select(kvp => $"\"{kvp.Key}\": {kvp.Value.ToJson(level + 1)}").ToArray()));
            for (int i = 0; i < level - 1; i++) {
                sb.Append("  ");
            }
            sb.Append("}");
            return sb.ToString();
        }

        public static string ToJson(this object obj, int level = 1) {
            if (obj == null) return "null";

            Type objType = obj.GetType();

            Func<object, string> converter;
            if (basicConverters.TryGetValue(objType, out converter)) {
                return converter.Invoke(obj);
            }

            foreach (Type iType in objType.GetInterfaces()) {
                if (iType.IsGenericType) {
                    Type genericTypeDefinition = iType.GetGenericTypeDefinition();
                    if (genericTypeDefinition == typeof(IList<>)) {
                        return (string)typeof(Serializer).GetMethod("IListToJson", BindingFlags.Static | BindingFlags.NonPublic)
                            .MakeGenericMethod(iType.GetGenericArguments())
                            .Invoke(null, new object[] { obj, level});
                    }
                    if (genericTypeDefinition == typeof(IDictionary<,>)) {
                        return (string)typeof(Serializer).GetMethod("IDictionaryToJson", BindingFlags.Static | BindingFlags.NonPublic)
                            .MakeGenericMethod(iType.GetGenericArguments())
                            .Invoke(null, new object[] { obj, level });
                    }
                }
            }

            if (Attribute.GetCustomAttribute(objType, typeof(Serializable), true) != null) {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("{");

                FieldInfo[] allFieldInfos = obj.GetType().GetFields(flags);
                for (int i = 0; i < allFieldInfos.Length; i++) {
                    FieldInfo fieldInfo = allFieldInfos[i];
                    if (Attribute.GetCustomAttribute(fieldInfo, typeof(Unserializable)) != null) {
                        continue;
                    }
                    string fieldName = fieldInfo.Name;
                    object fieldValue = fieldInfo.GetValue(obj);
                    for (int j = 0; j < level; j++) {
                        sb.Append("  ");
                    }
                    sb.Append($"\"{fieldName}\": {fieldValue.ToJson(level + 1)}");
                    if (i < allFieldInfos.Length - 1) {
                        sb.AppendLine(",");
                    } else {
                        sb.AppendLine();
                    }
                }
                for (int i = 0; i < level - 1; i++) {
                    sb.Append("  ");
                }
                sb.Append("}");
                return sb.ToString();
            }

            return "{}";
        }
    }
}
