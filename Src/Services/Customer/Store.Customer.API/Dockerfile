# Acesse https://aka.ms/customizecontainer para saber como personalizar seu contêiner de depuração e como o Visual Studio usa este Dockerfile para criar suas imagens para uma depuração mais rápida.

# Esta fase é usada durante a execução no VS no modo rápido (Padrão para a configuração de Depuração) 
#dentro dessa imagem vai ter uma pasta app
#export na porta 8080 e 8081
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base 
USER app
WORKDIR /app 
EXPOSE 8080
EXPOSE 443 


# Esta fase é usada para compilar o projeto de serviço
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["./Services/Customer/Store.Customer.API/Store.Customer.API.csproj", "Services/Customer/Store.Customer.API/"]
COPY ["./BuildingBlocks/Core/Core.csproj", "BuildingBlocks/Core/"] 
COPY ["./BuildingBlocks/MessageBus/MessageBus.csproj", "BuildingBlocks/MessageBus/"] 

RUN dotnet restore "Services/Customer/Store.Customer.API/Store.Customer.API.csproj"
COPY . .
WORKDIR "Services/Customer/Store.Customer.API"
RUN dotnet build "Store.Customer.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Esta fase é usada para publicar o projeto de serviço a ser copiado para a fase final
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Store.Customer.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish 
#/p:UseAppHost=false


# Esta fase é usada na produção ou quando executada no VS no modo normal (padrão quando não está usando a configuração de Depuração)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish . 
ENTRYPOINT ["dotnet", "Store.Customer.API.dll"]

#Copia todos os arquivos do container "as publish" para a pasta app/publish
 #vai subir executando dotnet
