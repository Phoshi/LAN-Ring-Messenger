using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Extensions;

namespace RingLAN {
    public static class Names {
        private static List<string> _names = new List<string> {
                                                          "Magichorn",
                                                          "Lightning Lesbian",
                                                          "Song Smile",
                                                          "Tree Kicker",
                                                          "Yellowquiet",
                                                          "Squiggletail",
                                                      };
        private static Dictionary<char, string> assoc = new Dictionary<char, string>();

        public static string GetName(char address, bool withAddress = true) {
            if (address == ' ') {
                return "Nobody";
            }
            if (!assoc.ContainsKey(address)) {
                if (_names.Count == 0) {
                    return "Generic ({0})".With(address);
                }
                _names = _names.OrderBy(name => Guid.NewGuid()).ToList(); //Simple, secure shuffle.
                string newName = _names[0];
                _names.RemoveAt(0);

                assoc[address] = newName;
            }
            if (!withAddress) {
                return assoc[address];
            }
            return "{0} ({1})".With(assoc[address], address);
        }

        public static void DelUser(char address) {
            if (assoc.ContainsKey(address)) {
                string name = assoc[address];
                _names.Add(name);
                assoc.Remove(address);
            }
        }

        public static Image GetImage(char address) {
            if (address == ' ') {
                return null;
            }
            string name = GetName(address, false);
            Image horse = Image.FromFile(@"Horses\{0}.png".With(name));
            return horse;
        }
    }
}
