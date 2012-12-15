using System;
using System.Collections.Generic;

namespace GameBuilder
{
    /// <summary>
    /// Implementa IServiceProvider para la aplicación. Este tipo se expone a través de la propiedad App.Services
    /// y se puede usar para ContentManagers u otros tipos que necesitan acceder a un IServiceProvider.
    /// </summary>
    public class AppServiceProvider : IServiceProvider
    {
        // Un mapa de tipo de servicio a los propios servicios
        private readonly Dictionary<Type, object> services = new Dictionary<Type, object>();

        /// <summary>
        /// Agrega un nuevo servicio proveedor de servicios.
        /// </summary>
        /// <param name="serviceType">El tipo de servicio para agregar.</param>
        /// <param name="service">El propio objeto de servicio.</param>
        public void AddService(Type serviceType, object service)
        {
            // Validar la entrada
            if (serviceType == null)
                throw new ArgumentNullException("serviceType");
            if (service == null)
                throw new ArgumentNullException("service");
            if (!serviceType.IsAssignableFrom(service.GetType()))
                throw new ArgumentException("service does not match the specified serviceType");

            // Agregar el servicio al diccionario
            services.Add(serviceType, service);
        }

        /// <summary>
        /// Obtiene un servicio del proveedor de servicios.
        /// </summary>
        /// <param name="serviceType">El tipo de servicio para recuperar.</param>
        /// <returns>El objeto de servicio registrado para el tipo especificado.</returns>
        public object GetService(Type serviceType)
        {
            // Validar la entrada
            if (serviceType == null)
                throw new ArgumentNullException("serviceType");

            // Recuperar el servicio del diccionario
            return services[serviceType];
        }

        /// <summary>
        /// Quita un servicio del proveedor de servicios.
        /// </summary>
        /// <param name="serviceType">El tipo de servicio para quitar.</param>
        public void RemoveService(Type serviceType)
        {
            // Validar la entrada
            if (serviceType == null)
                throw new ArgumentNullException("serviceType");

            // Quitar el servicio del diccionario
            services.Remove(serviceType);
        }
    }
}
