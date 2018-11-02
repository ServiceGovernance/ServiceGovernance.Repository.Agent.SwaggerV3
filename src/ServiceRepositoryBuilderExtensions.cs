using ServiceGovernance.Repository.Agent;
using ServiceGovernance.Repository.Agent.Configuration;
using ServiceGovernance.Repository.Agent.SwaggerV3;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Builder extension methods for registering ApiExplorer provider
    /// </summary>
    public static class ServiceRepositoryBuilderExtensions
    {
        /// <summary>
        /// Use Api description from Swagger
        /// </summary>
        /// <param name="builder">The builder instance.</param>        
        /// <param name="documentName">The document name as set in the swagger configuration (e.g. "v1")</param>
        /// <returns></returns>
        public static IServiceRepositoryAgentBuilder UseSwagger(this IServiceRepositoryAgentBuilder builder, string documentName)
        {
            return builder.UseSwagger(new SwaggerOptions { DocumentName = documentName });
        }

        /// <summary>
        /// Use Api description from Swagger
        /// </summary>
        /// <param name="builder">The builder instance.</param>  
        /// <param name="setupAction">Delegate to define the options</param>
        /// <returns></returns>
        public static IServiceRepositoryAgentBuilder UseSwagger(this IServiceRepositoryAgentBuilder builder, Action<SwaggerOptions> setupAction)
        {
            if (setupAction == null)
                throw new ArgumentNullException(nameof(setupAction));

            var options = new SwaggerOptions();
            setupAction.Invoke(options);

            return builder.UseSwagger(options);
        }

        /// <summary>
        /// Use Api description from Swagger
        /// </summary>
        /// <param name="builder">The builder instance.</param>   
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public static IServiceRepositoryAgentBuilder UseSwagger(this IServiceRepositoryAgentBuilder builder, SwaggerOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            builder.Services.AddSingleton(options);
            builder.Services.AddTransient<IApiDescriptionProvider, SwaggerApiDescriptionProvider>();

            return builder;
        }
    }
}
