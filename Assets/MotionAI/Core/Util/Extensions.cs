using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MotionAI.Core.Util {
	public static class Extensions {
		public static string ToDescriptionString(this Enum val)
		{
			DescriptionAttribute[] attributes = (DescriptionAttribute[])val
				.GetType()
				.GetField(val.ToString())
				.GetCustomAttributes(typeof(DescriptionAttribute), false);
			return attributes.Length > 0 ? attributes[0].Description : string.Empty;
		}
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