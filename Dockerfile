# Multi-stage build para otimizar o tamanho da imagem
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar arquivos de projeto
COPY ["Clude.TesteTecnico.API/Clude.TesteTecnico.API.csproj", "Clude.TesteTecnico.API/"]
COPY ["Clude.TesteTecnico.API.Application/Clude.TesteTecnico.API.Application.csproj", "Clude.TesteTecnico.API.Application/"]
COPY ["Clude.TesteTecnico.API.Domain/Clude.TesteTecnico.API.Domain.csproj", "Clude.TesteTecnico.API.Domain/"]
COPY ["Clude.TesteTecnico.API.Infrastructure/Clude.TesteTecnico.API.Infrastructure.csproj", "Clude.TesteTecnico.API.Infrastructure/"]

# Restaurar dependências
RUN dotnet restore "Clude.TesteTecnico.API/Clude.TesteTecnico.API.csproj"

# Copiar código fonte
COPY . .

# Build da aplicação
WORKDIR "/src/Clude.TesteTecnico.API"
RUN dotnet build "Clude.TesteTecnico.API.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "Clude.TesteTecnico.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Criar usuário não-root para segurança
RUN adduser --disabled-password --gecos '' appuser && chown -R appuser /app
USER appuser

ENTRYPOINT ["dotnet", "Clude.TesteTecnico.API.dll"] 