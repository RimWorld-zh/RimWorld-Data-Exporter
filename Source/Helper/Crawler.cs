using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using RimWorldDataExporter.Model;


namespace RimWorldDataExporter.Helper {
    class NoCrawl : Attribute { }

    static class Crawler {
        private static readonly BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

        public static void Crawl(EObj eObj, Def def) {
            foreach (var fieldInfo in eObj.GetType().GetFields(flags)) {
                if (Attribute.GetCustomAttribute(fieldInfo, typeof(NoCrawl)) != null) {
                    continue;
                }
                var defFieldInfo = def.GetType().GetField(fieldInfo.Name, flags);
                if (defFieldInfo != null) {
                    fieldInfo.SetValue(eObj, defFieldInfo.GetValue(def));
                }
            }
        }
    }
}
