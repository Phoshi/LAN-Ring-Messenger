using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RingLAN {
    public static class listExtensions {

        /// <summary>
        /// Returns a list with only elements with unique keys. 
        /// Where elements have equivilent keys, the first element is returned.
        /// </summary>
        /// <typeparam name="T">The type of element</typeparam>
        /// <typeparam name="KEY">The key</typeparam>
        /// <param name="workOn">The list to work on</param>
        /// <param name="func">The key function</param>
        /// <returns></returns>
        public static IEnumerable<T> Unique<T, KEY>(this IEnumerable<T> workOn, Func<T, KEY> func) {
            List<T> newList = new List<T>();
            List<KEY> foundKeys = new List<KEY>();
            foreach (T element in workOn) {
                KEY key = func.Invoke(element);
                if (!foundKeys.Contains(key)) {
                    newList.Add(element);
                    foundKeys.Add(key);
                }
            }
            return newList;
        }

        /// <summary>
        /// Returns the next pending item to the given client
        /// </summary>
        /// <param name="actOn">The pending list</param>
        /// <param name="target">The client to target</param>
        /// <returns>The pending object</returns>
        public static Pending NextTo(this List<Pending> actOn, char target) {
            IEnumerable<Pending> items = actOn.OrderByDescending(item => item.LastSend).Unique(item => item.Message.SenderAddress).Where(
                                    item => item.Message.Address == target);
            if (items.Count() > 0) {
                return items.First();
            }
            return null;
        }
    }
}
