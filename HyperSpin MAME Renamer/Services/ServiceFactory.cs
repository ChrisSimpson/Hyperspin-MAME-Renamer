using System;
using System.Collections.Generic;

namespace Renamer.Services
{
    /// <summary>
    /// Service factory
    /// </summary>
    public static class ServiceFactory
    {
        #region Private Fields

        private static Dictionary<Type, object> _instances = new Dictionary<Type, object>();
        private static Dictionary<Type, Type> _registry = new Dictionary<Type, Type>();

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        /// Get a service instance
        /// </summary>
        /// <typeparam name="TInterface">The type of the service interface</typeparam>
        /// <returns>An implementation of the requested service</returns>
        public static TInterface Get<TInterface>()
        {
            object instance;

            var interfaceType = typeof(TInterface);

            if (!_instances.TryGetValue(interfaceType, out instance))
            {
                instance = Activator.CreateInstance(_registry[interfaceType]);
                _instances.Add(interfaceType, instance);
            }

            return (TInterface)instance;
        }

        /// <summary>
        /// Register a service
        /// </summary>
        /// <typeparam name="TInterface">The type of the service interface</typeparam>
        /// <typeparam name="TImplementation">The type of the service implementation</typeparam>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Parameters are not required here.")]
        public static void RegisterService<TInterface, TImplementation>()
        {
            _registry.Add(typeof(TInterface), typeof(TImplementation));
        }

        #endregion Public Methods
    }
}