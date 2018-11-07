using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class Synastry<TKey, TMatched> where TKey : struct, IConvertible
                                          where TMatched : struct, IConvertible
    {
        private IDictionary<TKey, IDictionary<TMatched, double>> synastryValueMap;

        public Synastry()
        {
            synastryValueMap = new Dictionary<TKey, IDictionary<TMatched, double>>();
        }

        public Func<TMatched, double> GetValue(TKey key)
        {
            if (synastryValueMap.ContainsKey(key) == false)
            {
                return matched => 0.0;
            }

            return matched =>
            {
                double accumulatedValue = 0.0;

                foreach (var matchedString in Enum.GetNames(typeof(TMatched)))
                {
                    var comparedValue = (TMatched) Enum.Parse(typeof(TMatched), matchedString);

                    if ((matched.ToInt32(null) & comparedValue.ToInt32(null)) != 0 &&
                        synastryValueMap[key].ContainsKey(comparedValue))
                    {
                        accumulatedValue += synastryValueMap[key][comparedValue];
                    }
                }

                return accumulatedValue;
            };
        }

        public void AddValue(TKey key, TMatched matched, double value)
        {
            if (synastryValueMap.ContainsKey(key) == false)
            {
                synastryValueMap.Add(key, new Dictionary<TMatched, double>());
            }

            AddMatchedValue(key, matched, value);
        }

        private void AddMatchedValue(TKey key, TMatched matched, double value)
        {
            if (synastryValueMap[key].ContainsKey(matched) == false)
            {
                synastryValueMap[key].Add(matched, value);
            }
        }
    }
}
