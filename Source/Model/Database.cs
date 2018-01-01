using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

using RimWorldDataExporter.Helper;
using RimWorldDataExporter.Helper.Serialization;

namespace RimWorldDataExporter.Model {
    class Database<EType, DefType>
        where EType : EObj, new()
        where DefType : Def, new() {

        private static readonly EType hook = new EType();

        public static string Category => hook.Category;

        private static List<EObj> dataList;

        public static IEnumerable<EObj> AllDatas {
            get {
                return dataList;
            }
        }

        public static void Init() {
            dataList = GenerateDatas();
        }

        private static List<EObj> GenerateDatas() {
            var allDefs = DefDatabase<DefType>.AllDefs.Where(hook.Filter).ToList();
            var result = new List<EObj>();
            foreach (var def in allDefs) {
                var eobj = new EType();
                eobj.Crawl(def);
                result.Add(eobj);
            }
            return result;
        }

        public static void Save(string path) {
            if (Directory.Exists(path)) {
                Directory.Delete(path, true);
            }
            Directory.CreateDirectory(path);

            // initial
            Init();

            string ebase = "Database";
            if (typeof(EType).IsSubclassOf(typeof(ELang))) {
                ebase = "Langbase";
            }

            StringBuilder sbIndex = new StringBuilder();

            // import
            foreach (var data in AllDatas) {
                data.Save(Path.Combine(path, $"{data.defName}.ts"));
                sbIndex.AppendLine($"import {data.defName} from './{data.defName}';");
            }
            sbIndex.AppendLine();

            // collection
            sbIndex.AppendLine($"const {Category}: {ebase}{Category} = {{");
            foreach (var data in AllDatas) {
                sbIndex.AppendLine($"  {data.defName},");
            }
            sbIndex.AppendLine($"}};");
            sbIndex.AppendLine();

            // aggregation
            EAggr aggregation = (hook as EData)?.GetAggregation();
            if (aggregation != null) {
                sbIndex.AppendLine($"export const aggregation{Category}: {aggregation.GetType().Name} = {aggregation.ToTypeSrcipt()}");
                sbIndex.AppendLine();
            }

            // export
            sbIndex.AppendLine($"export default {Category};");

            File.WriteAllText(Path.Combine(path, "index.ts"), sbIndex.ToString());

            // declaration
            StringBuilder sbDeclaration = new StringBuilder();
            sbDeclaration.AppendLine($"declare interface {ebase}{Category} {{");
            foreach (var data in AllDatas) {
                sbDeclaration.AppendLine($"  readonly {data.defName}: {typeof(EType).Name};");
            }
            sbDeclaration.AppendLine($"}}");
            TypeScriptHelper.AddEbaseDeclaration(ebase, Category, sbDeclaration);
        }
    }
}
