using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Model
{
    public class SynastryReader
    {
        public Synastry<TKey, TMatched> ParseXml<TKey, TMatched>(string xmlString)
            where TKey : struct, IConvertible
            where TMatched : struct, IConvertible
        {
            var newSynastry = new Synastry<TKey, TMatched>();

            var document = XDocument.Parse(xmlString);
            var keys = document.Root.Elements("Key");
            
            foreach (var keyElement in keys)
            {
                var keyValueString = keyElement.Attribute("Name").Value;
                var keyValue = (TKey) Enum.Parse(typeof(TKey), keyValueString);

                foreach (var matchedElement in keyElement.Elements("Matched"))
                {
                    var matchedValueString = matchedElement.Attribute("Name").Value;
                    var matchedValue = (TMatched) Enum.Parse(typeof(TMatched), matchedValueString);

                    double ratioValue = double.Parse(matchedElement.Attribute("Value").Value);
                    newSynastry.AddValue(keyValue, matchedValue, ratioValue);
                }
            }

            return newSynastry;
        }
    }
}
