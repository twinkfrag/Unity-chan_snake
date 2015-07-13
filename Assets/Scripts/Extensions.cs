using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
	public static class Extensions
	{
		public static IEnumerable<TResult> Zip<T1, T2, TResult>(
			this IEnumerable<T1> left, IEnumerable<T2> right, Func<T1, T2, TResult> func)
		{
			if (left == null) throw new ArgumentNullException("left");
			if (right == null) throw new ArgumentNullException("right");

			using (var iteratorA = left.GetEnumerator())
			using (var iteratorB = right.GetEnumerator())
			{
				while (iteratorA.MoveNext() && iteratorB.MoveNext())
				{
					yield return func(iteratorA.Current, iteratorB.Current);
				}
			}
		}
	}
}
