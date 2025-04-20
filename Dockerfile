FROM mcr.microsoft.com/dotnet/sdk:9.0-preview AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/runtime:9.0-preview AS runtime
WORKDIR /app
COPY --from=build /app .

ENTRYPOINT ["dotnet", "Authentication_Service.dll"]
