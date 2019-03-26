XWidget.SwaggerUI
=====

Newer Swagger UI for .NET Core, and support client generate.

## Getting Started

#### 1.Install NuGet package.
```shell
Install-Package XWidget.SwaggerUI
```

#### 2.Add SwaggerUI middleware in your `Startup.Configure` method.
```
app.UseSwagger(); // your swagger document generator
app.UseSwaggerUI("/swagger", "/swagger/v1/swagger.json"); // add this line
```

#### 3.Run project and browse 'http://localhost:5000/swagger'.
![Imgur](https://i.imgur.com/KNaP8jj.png)

#### 4.Scroll to bottom, you can see `Generate Client` function.
![Imgur](https://i.imgur.com/1YBokf0.png)
