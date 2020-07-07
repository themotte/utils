
using System;
using System.Collections.Generic;
using System.Linq;

public static class Util
{
    public class FloatUnixDateTimeConverter : Newtonsoft.Json.JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
        }

        public override object ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds((double)reader.Value);
        }

        public override void WriteJson(Newtonsoft.Json.JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    public static IEnumerable<T> ProgressBar<T>(this IEnumerable<T> input, bool shuffle = true)
    {
        var values = input.ToArray();

        if (shuffle)
        {
            values.Shuffle();
        }

        // in seconds
        float accumulatedTime = 0;
        float accumulatedItems = 0;

        DateTimeOffset lastShown = DateTimeOffset.Now;

        for (int i = 0; i < values.Length; ++i)
        {
            var startTime = DateTimeOffset.Now;

            yield return values[i];

            var deltaTime = (DateTimeOffset.Now - startTime);

            // exponential falloff calculation; 50% every minute
            var falloff = (float)Math.Pow(0.5f, deltaTime.TotalMinutes);

            accumulatedTime *= falloff;
            accumulatedItems *= falloff;

            accumulatedTime += (float)deltaTime.TotalSeconds;
            accumulatedItems += 1;

            if (i > 0 && (DateTimeOffset.Now - lastShown).TotalSeconds > 0.2f)
            {
                var remaining = (values.Length - i) * (accumulatedTime / accumulatedItems);

                Dbg.Inf($"{i} / {values.Length} -- ETA {remaining / 60:F2}m");
                lastShown = DateTimeOffset.Now;
            }
        }
    }

    private static Random shuffleRng = new Random();
    public static void Shuffle<T>(this T[] input)
    {
        int n = input.Length;
        while (n > 1)
        {
            n--;
            int k = shuffleRng.Next(n + 1);
            T value = input[k];
            input[k] = input[n];
            input[n] = value;
        }
    }

    public static V TryGetValue<K, V>(this Dictionary<K, V> dict, K key)
    {
        if (dict.TryGetValue(key, out V result))
        {
            return result;
        }
        else
        {
            return default(V);
        }
    }
}
