using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;

namespace Test.Units {
    class EnumFlagTest {
        private static IEnumerable<T> GetUniqueFlags<T>(T flags) where T : IComparable, IFormattable, IConvertible {
            return Enum
                    .GetValues(flags.GetType())
                    .Cast<T>()
                    .Where(value => (
                        value.Equals(0) && flags.Equals(0)
                        ||
                        !value.Equals(0) && (Convert.ToUInt64(flags) & Convert.ToUInt64(value)) == Convert.ToUInt64(value)
                    ));
        }

        public static void Test() {
            var tag = WorkTags.Animals;
            Console.WriteLine(string.Join(',', GetUniqueFlags(tag).ToArray()));

            var flags = WorkTags.Animals | WorkTags.Cleaning | WorkTags.Crafting;
            Console.WriteLine(string.Join(',', GetUniqueFlags(flags).ToArray()));
        }
    }
}
