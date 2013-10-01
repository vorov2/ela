using System;
using System.Collections.Generic;

namespace Ela
{
	public interface IReadOnlyMap<K,V> : IEnumerable<KeyValuePair<K,V>>
	{
        bool TryGetValue(K key, out V value);

		bool ContainsKey(K key);

		V this[K key] { get; }

		int Count { get; }
	}
}
