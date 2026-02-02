# Imagen base .NET 8
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copia los binarios publicados
COPY bin/Release/net8.0/publish .

# Puerto interno del contenedor
EXPOSE 8080

# Ejecutar API
ENTRYPOINT ["dotnet", "Redarbor.Test.Api.dll"]