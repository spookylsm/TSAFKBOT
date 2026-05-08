FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# 1. Copia todos os ficheiros da tua pasta local para dentro do container
COPY . .

# 2. Restaura as dependências apontando explicitamente para o projeto principal
RUN dotnet restore "TSBot.Web/TSBot.Web.csproj"

# 3. Compila e publica o projeto
RUN dotnet publish "TSBot.Web/TSBot.Web.csproj" -c Release -o /app/out

# 4. Prepara a imagem final (mais leve) só com o necessário para correr
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/out .

# Cria a pasta que será mapeada para o volume
RUN mkdir -p /app/config
EXPOSE 8080

ENTRYPOINT ["dotnet", "TSBot.Web.dll"]