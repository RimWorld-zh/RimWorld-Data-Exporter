using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace RimWorldDataExporter.Helper.Serialization {
    public static class DataExtension {

        /// <summary>
        /// Convert Unity color to CSS color text.
        /// </summary>
        /// <param name="color">The Unity color</param>
        /// <returns>CSS color text</returns>
        public static string ToCssColor(this Color color) {
            return $"rgba({Mathf.Round(color.r * 255f)}, {Mathf.Round(color.g * 255f)}, {Mathf.Round(color.b * 255f)}, {color.a})";
        }
    }
}
