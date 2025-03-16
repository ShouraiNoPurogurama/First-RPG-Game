using System.Collections.Generic;

namespace Save_and_Load
{
    [System.Serializable]
    public class DictionaryWrapper<T>
    {
        public List<string> keys = new List<string>();
        public List<T> values = new List<T>();

        public DictionaryWrapper() { }

        public DictionaryWrapper(Dictionary<string, T> dict)
        {
            foreach (var kvp in dict)
            {
                keys.Add(kvp.Key);
                values.Add(kvp.Value);
            }
        }
    }
}
