using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Renamer.Models
{
    /// <summary>
    /// Observable collection which also notifies when the contained data changed
    /// </summary>
    /// <typeparam name="T">The type of object contained in the collection</typeparam>
    public class ObservableContentCollection<T> : ObservableCollection<T> where T : INotifyPropertyChanged
    {
        #region Public Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Justification = "Need to subscribe to the collection changed event.")]
        public ObservableContentCollection()
        {
            CollectionChanged += ObservableContentCollection_CollectionChanged;
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Add a set of items to the collection
        /// </summary>
        /// <param name="items">The set of items to be added</param>
        public void AddRange(IEnumerable<T> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            foreach (var item in items)
            {
                Add(item);
            }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Called when an item in the collection changes
        /// </summary>
        /// <param name="sender">The event sender</param>
        /// <param name="e">The event arguments</param>
        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        /// Called when the collection content changes
        /// </summary>
        /// <param name="sender">The event sender</param>
        /// <param name="e">The event arguments</param>
        private void ObservableContentCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    ((INotifyPropertyChanged)item).PropertyChanged += Item_PropertyChanged;
                }
            }

            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    ((INotifyPropertyChanged)item).PropertyChanged -= Item_PropertyChanged;
                }
            }
        }

        #endregion Private Methods
    }
}