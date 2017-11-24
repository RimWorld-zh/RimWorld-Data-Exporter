using Verse;

namespace RimWorldDataExporter {
	[StaticConstructorOnStartup]
	static class ExporterMod {
		static ExporterMod() {
			LongEventHandler.ExecuteWhenFinished(Core.Exporter.Export);
		}
	}
}