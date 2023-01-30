using System.Collections.Concurrent;

namespace Migale.Core.Extensions;

public static class EnumerableExtension
{
    /// <summary>
    /// Basically <see cref="Parallel.ForEach"/> but async (Thanks Laiteuxx)
    /// </summary>
    internal static Task ForEachAsync<TSource>(this IEnumerable<TSource> source, int partitionCount, Func<TSource, Task> body)
    {
        return Task.WhenAll(Partitioner.Create(source).GetPartitions(partitionCount)
            .Select(partitions => Task.Run(async () =>
            {
                while (partitions.MoveNext())
                {
                    await body(partitions.Current).ConfigureAwait(false);
                }
            }))
        );
    }
}