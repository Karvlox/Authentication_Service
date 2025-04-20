# Usar la imagen del SDK de .NET 9.0 para construir el proyecto
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /app

# Copiar los archivos del proyecto y restaurar dependencias
COPY . ./
RUN dotnet restore

# Construir y publicar el proyecto
RUN dotnet publish -c Release -o out

# Usar la imagen de runtime de .NET 9.0 para ejecutar la aplicación
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Authentication_Service.dll"]
