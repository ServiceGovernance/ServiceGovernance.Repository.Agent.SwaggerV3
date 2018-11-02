using System;

namespace ServiceGovernance.Repository.Agent.SwaggerV3
{
    /// <summary>
    /// Exception used while generating documents
    /// </summary>
    public class SwaggerGenerationException : Exception
    {
        public SwaggerGenerationException() : base()
        {
        }

        public SwaggerGenerationException(string message) : base(message)
        {
        }

        public SwaggerGenerationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
