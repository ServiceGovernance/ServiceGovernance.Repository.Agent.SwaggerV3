using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Linq;

namespace ServiceGovernance.Repository.Agent.SwaggerV3
{
    /// <summary>
    /// Implementation of <see cref="IApiDescriptionProvider"/> using the Swagger
    /// </summary>
    public class SwaggerApiDescriptionProvider : IApiDescriptionProvider
    {
        private readonly ISwaggerProvider _swaggerProvider;
        private readonly JsonSerializer _swaggerSerializer;
        private readonly SwaggerOptions _options;

        public SwaggerApiDescriptionProvider(ISwaggerProvider swaggerProvider, IOptions<MvcJsonOptions> mvcJsonOptions, SwaggerOptions options)
        {
            _swaggerProvider = swaggerProvider ?? throw new ArgumentNullException(nameof(swaggerProvider));
            _swaggerSerializer = SwaggerSerializerFactory.Create(mvcJsonOptions);
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public OpenApiDocument GetDescriptions()
        {
            try
            {
                var swaggerDocument = _swaggerProvider.GetSwagger(_options.DocumentName);

                return ConvertToOpenApi(swaggerDocument);
            }
            catch (UnknownSwaggerDocument e)
            {
                throw new SwaggerGenerationException($"Error on generate the OpenApi document. The given document name '{_options.DocumentName}' was not found.", e);
            }
        }

        /// <summary>
        /// Converts the swagger document to an OpenApi document via json de/serialization
        /// </summary>
        /// <param name="swaggerDocument"></param>
        /// <returns></returns>
        private OpenApiDocument ConvertToOpenApi(SwaggerDocument swaggerDocument)
        {
            using (var stream = new MemoryStream())
            {
                using (var writer = new StreamWriter(stream))
                {
                    _swaggerSerializer.Serialize(writer, swaggerDocument);
                    writer.Flush();

                    stream.Position = 0;

                    var reader = new OpenApiStreamReader();
                    var document = reader.Read(stream, out var diagnostic);

                    if (diagnostic.Errors.Count > 0)
                        throw new SwaggerGenerationException("Errors occured while reading the OpenApi document." + Environment.NewLine + string.Join(Environment.NewLine, diagnostic.Errors.Select(e => e.Message)));

                    return document;
                }
            }
        }
    }
}
