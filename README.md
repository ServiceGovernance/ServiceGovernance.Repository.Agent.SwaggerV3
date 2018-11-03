# ServiceGovernance.Repository.Agent.SwaggerV3

[![Build status](https://ci.appveyor.com/api/projects/status/ugdtsyqr348ooasx/branch/master?svg=true)](https://ci.appveyor.com/project/twenzel/servicegovernance-repository-agent-swaggerv3/branch/master)
[![NuGet Version](http://img.shields.io/nuget/v/ServiceGovernance.Repository.Agent.SwaggerV3.svg?style=flat)](https://www.nuget.org/packages/ServiceGovernance.Repository.Agent.SwaggerV3/)
[![License](https://img.shields.io/badge/license-Apache-blue.svg)](LICENSE)

Provides an Agent (client) for the [ServiceRepository](https://github.com/ServiceGovernance/ServiceGovernance.Repository). This agent publishes the API documentation, by using Swagger, to the repository.

## Usage

Install the NuGet package `ServiceGovernance.Repository.Agent.SwaggerV3`.

```CSharp
public void ConfigureServices(IServiceCollection services)
{
    ...
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
    });

    services.AddServiceRepositoryAgent(options => {
        options.Repository = new Uri("http://localhost:5005");
        options.ServiceIdentifier = "Api1";                
    }).UseSwagger("v1");
}

public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    ...
    app.UseSwagger();    
    app.UseServiceRepositoryAgent();
}
```

## Configuration

It's also possible to provide these options via the configuration:

```CSharp
public void ConfigureServices(IServiceCollection services)
{
    ...
    services.AddServiceRepositoryAgent(options => Configuration.Bind("ServiceRepository", options));
}
```

```json
{
    "ServiceRepository": {
        "Repository": "https://myservicerepository.mycompany.com",
        "ServiceIdentifier": "Api1"
    }
}
```

## Background

This agent collects the Api Descriptions as [OpenApi document](https://github.com/Microsoft/OpenAPI.NET) and sends it to the [ServiceRepository](https://github.com/ServiceGovernance/ServiceGovernance.Repository) where it can be viewed among other Api documentations from other services.
