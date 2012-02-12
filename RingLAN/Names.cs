using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Extensions;

namespace RingLAN {
    public static class Names {
        private static List<string> _names = new List<string> {
                                                          "Magichorn",
                                                          "Speedycloud",
                                                          "Song Smile",
                                                          "Tree Kicker",
                                                          "Yellowquiet",
                                                          "Squiggletail",
                                                      };
        private static readonly Dictionary<char, string> assoc = new Dictionary<char, string>();
        private static readonly Dictionary<string, Image> images = new Dictionary<string, Image> {
                                                                                            {"Magichorn", Properties.Resources.magichorn},
                                                                                            {"Speedycloud", Properties.Resources.speedycloud},
                                                                                            {"Song Smile", Properties.Resources.song_smile},
                                                                                            {"Tree Kicker", Properties.Resources.tree_kicker},
                                                                                            {"Yellowquiet", Properties.Resources.yellowquiet},
                                                                                            {"Squiggletail", Properties.Resources.squiggletail},
    };

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
            return withAddress ? "{0} ({1})".With(assoc[address], address) : assoc[address];
        }

        public static void DelUser(char address) {
            if (!assoc.ContainsKey(address)) {
                return;
            }
            string name = assoc[address];
            _names.Add(name);
            assoc.Remove(address);
        }

        public static Image GetImage(char address) {
            if (address == ' ') {
                return null;
            }
            string name = GetName(address, false);
            return images.ContainsKey(name) ? images[name] : Properties.Resources.generic;
        }
    }
}
