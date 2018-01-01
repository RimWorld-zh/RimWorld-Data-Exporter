using System;
using Verse;

namespace RimWorldDataExporter {
	[StaticConstructorOnStartup]
	static class ExporterMod {
        public static string Name => "[RimWorld Data Exporter]";

        private static DateTime lastStartTime = DateTime.Now;

        public static void Log(string message, bool completed = false) {
            if (completed) {
                Verse.Log.Message($"[RimWorld Data Exporter] {message} in {(DateTime.Now - lastStartTime).Seconds} seconds");
            } else {
                Verse.Log.Message($"[RimWorld Data Exporter] {message}");
            }
            lastStartTime = DateTime.Now;
        }

        static ExporterMod() {
			LongEventHandler.ExecuteWhenFinished(Core.Exporter.Export);
		}
	}
}