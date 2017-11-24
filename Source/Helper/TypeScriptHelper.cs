using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Verse;
using RimWorldDataExporter.Model;

namespace RimWorldDataExporter.Helper.Serialization {
    interface ITypeable { }

    static class TypeScriptHelper {
        private readonly static BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;

        #region To TypeScript type name

        private static Dictionary<Type, string> basicTypeMap = new Dictionary<Type, string> {
            { typeof(bool), "boolean" },
            { typeof(string), "string" },
            { typeof(int), "number" },
            { typeof(float), "number" },
            { typeof(double), "number" },
        };

        private static string IListToTypeName<T> () {
            return $"ReadonlyArray<{typeof(T).ToTypeName()}>";
        }

        private static string IDictToTypeName<TKey, TValue>() {
            return $"ReadonlyDict<{typeof(TValue).ToTypeName()}>";
        }

        public static string ToTypeName(this Type type) {
            if (basicTypeMap.TryGetValue(type, out string typeName)) {
                return typeName;
            }

            if (type == typeof(EObj)
                || type.IsSubclassOf(typeof(EObj)) 
                || type.IsSubclassOf(typeof(ITypeable))
                ) {
                return type.Name;
            }
            
            foreach (Type iType in type.GetInterfaces()) {
                if (iType.IsGenericType) {
                    Type genericTypeDefinition = iType.GetGenericTypeDefinition();
                    if (genericTypeDefinition == typeof(IList<>)) {
                        return (string)typeof(TypeScriptHelper).GetMethod("IListToTypeName", BindingFlags.Static | BindingFlags.NonPublic)
                            .MakeGenericMethod(iType.GetGenericArguments())
                            .Invoke(null, null);
                    }
                    if (genericTypeDefinition == typeof(IDictionary<,>)) {
                        return (string)typeof(TypeScriptHelper).GetMethod("IDictToTypeName", BindingFlags.Static | BindingFlags.NonPublic)
                            .MakeGenericMethod(iType.GetGenericArguments())
                            .Invoke(null, null);
                    }
                }
            }
            
            return "any";
        }

        #endregion

        #region Save Declaration

        private static Dictionary<string, Dictionary<string, StringBuilder>> declarationMap = new Dictionary<string, Dictionary<string, StringBuilder>> {
            { "Database", new Dictionary<string, StringBuilder>() },
            { "Langbase", new Dictionary<string, StringBuilder>() },
        };

        public static void AddEbaseDeclaration(string ebase, string category, StringBuilder sb) {
            if (declarationMap[ebase].ContainsKey(category)) {
                return;
            }
            declarationMap[ebase].Add(category, sb);
        }

        public static void SaveAllTypesDeclaration(string path) {
            SaveAllITypeableClassesDeclaration(Path.Combine(path, "basic.d.ts"));
            SaveDeclaration(Path.Combine(path, "base.d.ts"), new[] { typeof(EData), typeof(ELang) }, typeof(EObj), "base");

            SaveAllEObjSubclassesDeclaration(Path.Combine(path, "data.d.ts"), typeof(EData), "data");
            SaveAllEObjSubclassesDeclaration(Path.Combine(path, "lang.d.ts"), typeof(ELang), "language");
        }

        private static void SaveAllITypeableClassesDeclaration(string path) {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("// Declaration for basic model types.");

            sb.AppendLine("declare interface ReadonlyDict<T> {");
            sb.AppendLine("  readonly [key: string]: T");
            sb.AppendLine("}");
            sb.AppendLine();

            var allITypeableClasses = typeof(ITypeable).AllSubclassesNonAbstract().ToList();
            allITypeableClasses.Sort((Type a, Type b) => {
                return a.Name.CompareTo(b.Name);
            });

            foreach (var iTypeableClass in allITypeableClasses) {
                sb.AppendLine();
                sb.AppendLine($"declare interface {iTypeableClass.Name} {{");
                foreach (var fieldInfo in iTypeableClass.GetFields(flags)) {
                    sb.AppendLine($"  readonly {fieldInfo.Name}: {fieldInfo.FieldType.ToTypeName()};");
                }
                sb.AppendLine("}");
            }

            File.WriteAllText(path, sb.ToString());
        }

        private static void SaveDeclaration(string path, Type[] types, Type baseType, string comment) {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"// Declaration for {comment} types.");

            sb.AppendLine();
            sb.AppendLine($"declare interface {baseType.Name} {{");
            foreach (var fieldInfo in baseType.GetFields(flags)) {
                sb.AppendLine($"  readonly {fieldInfo.Name}: {fieldInfo.FieldType.ToTypeName()};");
            }
            sb.AppendLine("}");

            foreach (Type type in types) {
                sb.AppendLine();
                sb.AppendLine($"declare interface {type.Name} extends {baseType.Name} {{");
                foreach (var fieldInfo in type.GetFields(flags)) {
                    sb.AppendLine($"  readonly {fieldInfo.Name}: {fieldInfo.FieldType.ToTypeName()};");
                }
                sb.AppendLine("}");
            }

            File.WriteAllText(path, sb.ToString());
        }

        private static void SaveAllEObjSubclassesDeclaration(string path, Type baseType, string comment) {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"// Declaration for {comment} types.");

            var allSubclasses = baseType.AllSubclasses().ToList();
            allSubclasses.Sort((Type a, Type b) => {
                return a.Name.CompareTo(b.Name);
            });

            foreach (var subclass in allSubclasses) {
                sb.AppendLine();
                sb.AppendLine($"declare interface {subclass.Name} extends {baseType.Name} {{");
                foreach (var fieldInfo in subclass.GetFields(flags)) {
                    sb.AppendLine($"  readonly {fieldInfo.Name}: {fieldInfo.FieldType.ToTypeName()};");
                }
                sb.AppendLine("}");
            }

            string ebase = baseType == typeof(EData) ? "Database" : "Langbase";
            foreach (var kvp in declarationMap[ebase]) {
                sb.AppendLine();
                sb.AppendLine(kvp.Value.ToString());
            }

            File.WriteAllText(path, sb.ToString());
        }

        #endregion
    }
}
