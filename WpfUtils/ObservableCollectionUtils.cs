using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace WpfUtils
{
    public static class ObservableCollectionUtils
    {
        public static void Replace<T>(this ObservableCollection<T> collection, T target, T replacement)
        {
            var dex = collection.IndexOf(target);
            if(dex >= 0)
            {
                collection.RemoveAt(dex);
                collection.Insert(dex, replacement);
            }
        }

        public static void AddMany<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                collection.Add(item);
            }
        }

        public static void InsertWhen<T>
            (
                this ObservableCollection<T> collection, 
                T item, 
                Predicate<T> condition
            )
        {
            for (var i = 0; i < collection.Count; i++)
            {
                if (condition.Invoke(collection[i]))
                {
                    collection.Insert(i, item);
                    return;
                }
            }
            collection.Add(item);
        }

        /// <summary>
        /// Assumes the list is already sorted from greatest to least using the comparer
        /// </summary>
        /// <param name="item"></param>
        /// <param name="comparer">returns true if a > b </param>
        public static void OrderedInsert<T>
        (
            this ObservableCollection<T> collection,
            T item,
            Func<T, T, bool> comparer,
            int maxItems
        )
        {
            if (collection.Count == 0)
            {
                collection.Add(item);
                return;
            }


            if (comparer(collection[collection.Count - 1], item))
            {
                if (collection.Count < maxItems)
                {
                    collection.Add(item);
                }
                return;
            }



            for (var i = collection.Count - 1; i > -1; i--)
            {
                if (comparer(collection[i], item))
                {
                    collection.Insert(i+1, item);
                    i=-1;
                }
                if (i == 0)
                {
                    collection.Insert(0, item);
                }
            }


            while (collection.Count > maxItems)
            {
                collection.RemoveAt(collection.Count - 1);
            }
        }

        public static T SelectedItem<T>(this ObservableCollection<T> collection)
        {
            var collectionView = CollectionViewSource.GetDefaultView(collection);
            return (T) (collectionView != null ? collectionView.CurrentItem : default(T));
        }

        public static void MoveCurrentTo<T>(this ObservableCollection<T> collection, T item)
        {
            if (! collection.Contains(item))
            {
                return;
            }
            var collectionView = CollectionViewSource.GetDefaultView(collection);
            if (collectionView != null)
            {
                collectionView.MoveCurrentTo(item);
            }
        }
    }
}
