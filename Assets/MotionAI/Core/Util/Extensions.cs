using System;
using System.Collections.Generic;

namespace MotionAI.Core.Util {
	public static class Extensions {

		public static string CleanFromDB(this string str) {
			return str.Replace(" ", "_").Replace("-", "_").Trim();
		}
		public static IEnumerable<TSource> DistinctBy<TSource, TKey>
			(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			HashSet<TKey> seenKeys = new HashSet<TKey>();
			foreach (TSource element in source)
			{
				if (seenKeys.Add(keySelector(element)))
				{
					yield return element;
				}
			}
		}	
	}
}