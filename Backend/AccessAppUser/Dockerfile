# Usa la imagen base de .NET SDK para la compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src
COPY ["Backend/AccessAppUser/AccessAppUser.csproj", "AccessAppUser/"]
RUN dotnet restore "AccessAppUser/AccessAppUser.csproj"

COPY . .
WORKDIR /src/Backend/AccessAppUser
RUN dotnet build -c Release -o /app/build

# Publicación de la aplicación
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Uso de la imagen de runtime para la ejecución
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AccessAppUser.dll"]
