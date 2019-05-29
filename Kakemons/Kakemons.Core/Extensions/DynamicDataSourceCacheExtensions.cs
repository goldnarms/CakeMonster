using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Akavache;
using DynamicData;
using Serilog;

namespace Kakemons.Core.Extensions
{
    public static class DynamicDataSourceCacheExtensions
    {
        public static IObservable<IChangeSet<TSource, TKey>> CacheChangeSet<TSource, TKey>(
            this IObservable<IChangeSet<TSource, TKey>> source, string key, ILogger log)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            return Observable.Create<IChangeSet<TSource, TKey>>(observer => source.ToCollection()
                .Concat(
                    BlobCache.LocalMachine.GetObject<List<TSource>>(key)
                        .Catch(Observable.Return(new List<TSource>())))
                .Subscribe(items =>
                {
                    //log.Trace("Writing items " + items.Count + " to cache with id: " + key);
                    BlobCache.LocalMachine.InsertObject(key, items.ToList())
                        .Catch(Observable.Return(Unit.Default).Do(unit => log.Error("Failed to add items to cache")));
                }));
        }

        public static IObservable<IChangeSet<TSource>> CacheSourceList<TSource>(this IObservable<IChangeSet<TSource>> source, string key, ILogger log)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            return Observable.Create<IChangeSet<TSource>>(observer => source.ToCollection()
                .Concat(
                    BlobCache.LocalMachine.GetObject<List<TSource>>(key)
                        .Catch(Observable.Return(new List<TSource>())))
                .Subscribe(items =>
                {
                    //log.Trace("Writing items " + items.Count + " to cache with id: " + key);
                    BlobCache.LocalMachine.InsertObject(key, items.ToList())
                        .Catch(Observable.Return(Unit.Default).Do(unit => log.Error("Failed to add items to cache")));
                }));
        }

        public static async Task FromCache<TObject, TKey>(this ISourceCache<TObject, TKey> cache, string cacheKey,
            IEqualityComparer<TObject> comparer, InsertStrategy strategy = InsertStrategy.EditDiff)
        {
            var list =
                await
                    BlobCache.LocalMachine.GetObject<List<TObject>>(cacheKey)
                        .Catch(Observable.Return(new List<TObject>()));
            if (strategy == InsertStrategy.EditDiff)
                cache.EditDiff(list, comparer);
            else
            {
                cache.AddOrUpdate(list);
            }
        }

        public static IObservable<Unit> FromCacheObservable<TObject, TKey>(this ISourceCache<TObject, TKey> cache,
            string cacheKey,
            IEqualityComparer<TObject> comparer, InsertStrategy strategy = InsertStrategy.EditDiff)
        {
            return Observable.Create<Unit>(observer =>
            {
                return BlobCache.LocalMachine.GetObject<List<TObject>>(cacheKey)
                    .Catch(Observable.Return(new List<TObject>()))
                    .Do(objects =>
                    {
                        if (strategy == InsertStrategy.EditDiff)
                            cache.EditDiff(objects, comparer);
                        else
                        {
                            cache.AddOrUpdate(objects);
                        }

                        observer.OnNext(Unit.Default);
                        observer.OnCompleted();
                    })
                    .Subscribe();
            });
        }

        public static async Task FromCache<TObject>(this ISourceList<TObject> sourceList, string cacheKey,
            IEqualityComparer<TObject> comparer)
        {
            var list =
                await
                    BlobCache.LocalMachine.GetObject<List<TObject>>(cacheKey)
                        .Catch(Observable.Return(new List<TObject>()));
            sourceList.EditDiff(list, comparer);
        }

        public static IObservable<Unit> FromCacheObservable<TObject>(this ISourceList<TObject> sourceList, string cacheKey,
            IEqualityComparer<TObject> comparer)
        {
            return
                BlobCache.LocalMachine.GetObject<List<TObject>>(cacheKey)
                    .Catch(Observable.Return(new List<TObject>()))
                    .Do(objects => sourceList.EditDiff(objects, comparer))
                    .Select(_ => Unit.Default)
                    .Take(1);
        }
    }
}
