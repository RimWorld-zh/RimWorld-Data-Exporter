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

        public static IEnumerable<T> GetUniqueFlags<T>(this T flags) where T : IComparable, IFormattable, IConvertible {
            return Enum
                    .GetValues(flags.GetType())
                    .Cast<T>()
                    .Where(value => (
                        Convert.ToUInt64(flags).Equals(0) && Convert.ToUInt64(value).Equals(0)
                        ||
                        !Convert.ToUInt64(value).Equals(0) && (Convert.ToUInt64(flags) & Convert.ToUInt64(value)) == Convert.ToUInt64(value)
                    ));
        }
    }
}
