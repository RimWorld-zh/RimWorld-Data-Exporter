using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace RimWorldDataExporter.Helper {
    static class DataExtension {

        /// <summary>
        /// Convert Unity color to CSS color text.
        /// </summary>
        /// <param name="color">The Unity color</param>
        /// <returns>CSS color text</returns>
        public static string ToCssColor(this Color color) {
            return $"rgba({(int)color.r * 255}, {(int)color.g * 255}, {(int)color.b * 255}, {color.a})";
        }
    }
}
