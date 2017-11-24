using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using RimWorld;
using Verse;

using RimWorldDataExporter.Model;
using RimWorldDataExporter.Helper.Serialization;

namespace RimWorldDataExporter.Core {
	static class Exporter {
        private static string OriginLangFolderName;

        private static List<List<string>> LanguageMap = new List<List<string>> {
            new List<string> { "en", "English" },
            new List<string> { "zhcn", "ChineseSimplified" },
            new List<string> { "zhtw", "ChineseTraditional" },
        };

        public static void Export() {
			if (ModsConfig.ActiveModsInLoadOrder.Count() > 2) {
				Log.Error("Active mods is more than 2. Must only active 'Core' and 'RimWorld Data Exporter'.");
				return;
			}
            ExportDatabase();
            ExportAllLangbase();
            
            Log.Message("[RimWorld Data Exporter] Data has been output.");
        }

		private static void ExportDatabase() {
			var outputPath = Path.Combine(GenFilePaths.DevOutputFolderPath, "database");
			if (Directory.Exists(outputPath)) {
				Directory.Delete(outputPath, true);
			}
			Directory.CreateDirectory(outputPath);

            var allDataTypes = typeof(EData).AllSubclassesNonAbstract().ToList();
            allDataTypes.Sort((Type a, Type b) => {
                return a.Name.CompareTo(b.Name);
            });

            foreach (var dataType in allDataTypes) {
                var typeArguments = new Type[] { dataType, (Activator.CreateInstance(dataType) as EData).DefType };
                var databaseType = typeof(Database<,>).MakeGenericType(typeArguments);
                var category = databaseType.GetProperty("Category").GetValue(null, null) as string;
                databaseType.GetMethod("Save", BindingFlags.Static | BindingFlags.Public).Invoke(null, new[] { Path.Combine(outputPath, category) });
            }
		}

        private static void ExportAllLangbase() {
            var rootPath = Path.Combine(GenFilePaths.DevOutputFolderPath, "languages");
            if (Directory.Exists(rootPath)) {
                Directory.Delete(rootPath, true);
            }
            Directory.CreateDirectory(rootPath);

            OriginLangFolderName = Prefs.LangFolderName;
            ExportLangbase(rootPath, 0);
        }

        private static void ExportLangbase(string path, int index) {
            if (index >= LanguageMap.Count) {
                Prefs.LangFolderName = OriginLangFolderName;
                LongEventHandler.QueueLongEvent(delegate
                {
                    PlayDataLoader.ClearAllPlayData();
                    PlayDataLoader.LoadAllPlayData(false);
                    ExportDeclaration();
                }, "LoadingLongEvent", false, null);
                return;
            }

            string code = LanguageMap[index][0];
            string folderName = LanguageMap[index][1];
            LoadedLanguage lang = null;
            foreach (var loadedLanugage in LanguageDatabase.AllLoadedLanguages) {
                if (loadedLanugage.folderName == folderName) {
                    lang = loadedLanugage;
                }
            }
            if (lang == null) {
                throw new Exception($"[RimWorld Data Exporter] Language '{code}' no found.");
            }

            Prefs.LangFolderName = lang.folderName;
            LongEventHandler.QueueLongEvent(delegate {
                PlayDataLoader.ClearAllPlayData();
                PlayDataLoader.LoadAllPlayData(false);

                var outputPath = Path.Combine(path, code);
                Directory.CreateDirectory(outputPath);

                var allLangTypes = typeof(ELang).AllSubclassesNonAbstract().ToList();
                allLangTypes.Sort((Type a, Type b) => {
                    return a.Name.CompareTo(b.Name);
                });

                foreach (var langType in allLangTypes) {
                    var typeArguments = new Type[] { langType, (Activator.CreateInstance(langType) as ELang).DefType };
                    var databaseType = typeof(Database<,>).MakeGenericType(typeArguments);
                    var category = databaseType.GetProperty("Category").GetValue(null, null) as string;
                    databaseType.GetMethod("Save", BindingFlags.Static | BindingFlags.Public).Invoke(null, new[] { Path.Combine(outputPath, category) });
                }

                Log.Message($"[RimWorld Data Exporter] Language '{code}' has been output.");
                ExportLangbase(path, index + 1);
            }, "LoadingLongEvent", false, null);
        }

        private static void ExportDeclaration() {
            var outputPath = Path.Combine(GenFilePaths.DevOutputFolderPath, "typings");
            if (Directory.Exists(outputPath)) {
                Directory.Delete(outputPath, true);
            }
            Directory.CreateDirectory(outputPath);
            TypeScriptHelper.SaveAllTypesDeclaration(outputPath);
        }
	}
}
