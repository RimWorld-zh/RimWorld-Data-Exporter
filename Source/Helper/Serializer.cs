using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

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
            {
                typeof(string),
                value => $"'{(value as string).Replace("'", "\\'")}'"
            },
            {
                typeof(Color),
                value => $"'{((Color)value).ToCssColor()}'"
            }
        };

        private static string IListToTypeScript<T> (IList<T> list, int level) {
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
                sb.AppendLine(string.Join(separator, list.Select(item => $"{item.ToTypeSrcipt(level + 1)}").ToArray()));
                for (int i = 0; i < level - 1; i++) {
                    sb.Append("  ");
                }
                sb.Append("]");
                return sb.ToString();
            }
        }

        private static string IDictionaryToTypeScript<TKey, TValue>(IDictionary<TKey, TValue> dict, int level) {
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
            sb.AppendLine(string.Join(separator, dict.Select(kvp => $"'{kvp.Key}': {kvp.Value.ToTypeSrcipt(level + 1)}").ToArray()));
            for (int i = 0; i < level - 1; i++) {
                sb.Append("  ");
            }
            sb.Append("}");
            return sb.ToString();
        }

        public static string ToTypeSrcipt(this object obj, int level = 1) {
            if (obj == null) return "null";

            Type objType = obj.GetType();

            if (basicConverters.TryGetValue(objType, out Func<object, string> converter)) {
                return converter.Invoke(obj);
            }

            if (objType.IsSubclassOf(typeof(Def))) {
                return $"'{(obj as Def).defName}'";
            }

            if (objType.IsEnum) {
                return $"{objType.Name}.{obj.ToString()}";
            }

            foreach (Type iType in objType.GetInterfaces()) {
                if (iType.IsGenericType) {
                    Type genericTypeDefinition = iType.GetGenericTypeDefinition();
                    if (genericTypeDefinition == typeof(IList<>)) {
                        return (string)typeof(Serializer).GetMethod("IListToTypeScript", BindingFlags.Static | BindingFlags.NonPublic)
                            .MakeGenericMethod(iType.GetGenericArguments())
                            .Invoke(null, new object[] { obj, level});
                    }
                    if (genericTypeDefinition == typeof(IDictionary<,>)) {
                        return (string)typeof(Serializer).GetMethod("IDictionaryToTypeScript", BindingFlags.Static | BindingFlags.NonPublic)
                            .MakeGenericMethod(iType.GetGenericArguments())
                            .Invoke(null, new object[] { obj, level });
                    }
                }
            }

            if (Attribute.GetCustomAttribute(objType, typeof(Serializable), true) != null ||
                objType.IsSubclassOf(typeof(ITypeable)) ||
                TypeScriptHelper.additinalTypes.Contains(objType)) {

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
                    sb.Append($"{fieldName}: {fieldValue.ToTypeSrcipt(level + 1)}");
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
