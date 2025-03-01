# Etapa 1: Construcción y publicación de la aplicación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Copiar solo el archivo de proyecto y restaurar dependencias
COPY Backend/AccessAppUser/AccessAppUser.csproj AccessAppUser/
WORKDIR /src/AccessAppUser
RUN dotnet restore

# Copiar el código fuente y compilar la aplicación
COPY Backend/AccessAppUser/ . 
RUN dotnet publish -c Release -o /app/publish

# Etapa 2: Imagen final para ejecución
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app

# Copiar los archivos publicados desde la etapa de compilación
COPY --from=build /app/publish .

# Establecer el punto de entrada
ENTRYPOINT ["dotnet", "AccessAppUser.dll"]


