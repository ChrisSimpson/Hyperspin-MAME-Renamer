using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Renamer.Models
{
    /// <summary>
    /// Base class for view models
    /// </summary>
    public abstract class PropertyChangedBase : INotifyPropertyChanged
    {
        #region Public Events

        /// <summary>
        /// Event raised when a property has changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Public Events

        #region Protected Methods

        /// <summary>
        /// Raise the PropertyChanged event
        /// </summary>
        /// <param name="propertyName">The name of the property that has changed</param>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Set a property value
        /// </summary>
        /// <typeparam name="T">The type of the property</typeparam>
        /// <param name="storage">Reference to the property to be set</param>
        /// <param name="value">The value to set the property to</param>
        /// <param name="propertyName">The name of the property</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#", Justification = "Need to pass the storage by reference.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Required by the [CallerMemberName] attribute.")]
        protected void SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(storage, value))
            {
                storage = value;
                OnPropertyChanged(propertyName);
            }
        }

        #endregion Protected Methods
    }
}