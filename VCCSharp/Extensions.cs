using System.Collections.Generic;
using System.Linq;

namespace VCCSharp
{
    public static class Extensions
    {
        public static IEnumerable<IList<T>> Chunk<T>(this IEnumerable<T> source, int chunkSize)
        {
            int index = 0;
            var chunk = new List<T>();

            foreach (var each in source)
            {
                chunk.Add(each);

                if (++index == chunkSize)
                {
                    yield return chunk;

                    index = 0;
                    chunk = new List<T>();
                }
            }

            if (chunk.Any()) yield return chunk;
        }
    }
}
