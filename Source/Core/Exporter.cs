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
        private static string originLangFolderName;
        private static string outputPath;

        private static List<List<string>> LanguageMap = new List<List<string>> {
            new List<string> { "en", "English" },
            new List<string> { "zhcn", "ChineseSimplified" },
            new List<string> { "zhtw", "ChineseTraditional" },
        };

        public static void Export() {
            var currentDir = new DirectoryInfo(Directory.GetCurrentDirectory())?.Parent?.Parent?.Parent;
            if (currentDir == null) {
                throw new Exception("Can not get RimWorld installation directory.");
            }

            Exporter.outputPath = Path.Combine(currentDir.FullName, "RimWorld-Database");

			if (ModsConfig.ActiveModsInLoadOrder.Count() > 2) {
				Log.Error("Active mods is more than 2. Must only active 'Core' and 'RimWorld Data Exporter'.");
				return;
			}
            ExportDatabase();
            ExportAllLangbase();
            
            Log.Message("[RimWorld Data Exporter] Data has been output.");
        }

		private static void ExportDatabase() {
			var databasePath = Path.Combine(Exporter.outputPath, "database");
			if (Directory.Exists(databasePath)) {
				Directory.Delete(databasePath, true);
			}
			Directory.CreateDirectory(databasePath);

            var allDataTypes = typeof(EData).AllSubclassesNonAbstract().ToList();
            allDataTypes.Sort((Type a, Type b) => {
                return a.Name.CompareTo(b.Name);
            });

            foreach (var dataType in allDataTypes) {
                var typeArguments = new Type[] { dataType, (Activator.CreateInstance(dataType) as EData).DefType };
                var databaseType = typeof(Database<,>).MakeGenericType(typeArguments);
                var category = databaseType.GetProperty("Category").GetValue(null, null) as string;
                databaseType.GetMethod("Save", BindingFlags.Static | BindingFlags.Public).Invoke(null, new[] { Path.Combine(databasePath, category) });
            }
		}

        private static void ExportAllLangbase() {
            var languagesPath = Path.Combine(Exporter.outputPath, "languages");
            if (Directory.Exists(languagesPath)) {
                Directory.Delete(languagesPath, true);
            }
            Directory.CreateDirectory(languagesPath);

            originLangFolderName = Prefs.LangFolderName;
            ExportLangbase(languagesPath, 0);
        }

        private static void ExportLangbase(string path, int index) {
            if (index >= LanguageMap.Count) {
                Prefs.LangFolderName = originLangFolderName;
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
            var outputPath = Path.Combine(Exporter.outputPath, "typings");
            if (Directory.Exists(outputPath)) {
                Directory.Delete(outputPath, true);
            }
            Directory.CreateDirectory(outputPath);
            TypeScriptHelper.SaveAllTypesDeclaration(outputPath);
        }
	}
}
