# 🐳 Instruções para Executar o Projeto CLUDE em Docker

Este documento contém as instruções completas para executar o projeto CLUDE Teste Técnico usando Docker.

## 📋 Pré-requisitos

Antes de começar, certifique-se de ter instalado:

- **Docker Desktop** (versão 20.10 ou superior)
- **Docker Compose** (incluído no Docker Desktop)
- **Git** (para clonar o repositório)

### Verificando a instalação:
```bash
docker --version
docker-compose --version
git --version
```

## 🚀 Executando o Projeto

### 1. **Clone o Repositório**
```bash
git clone https://github.com/victortiktokbugteste/Clude.TesteTecnico.API.git
cd Clude.TesteTecnico.API
```

### 2. **Executar com Docker Compose**
```bash
# Construir e iniciar todos os serviços
docker-compose up --build

# Para executar em background (modo detached)
docker-compose up --build -d
```

### 3. **Verificar Status dos Serviços**
```bash
docker-compose ps
```

## 📊 Serviços Disponíveis

Após a execução, os seguintes serviços estarão disponíveis:

| Serviço | URL | Descrição |
|---------|-----|-----------|
| **API Principal** | http://localhost:5000 | API REST da aplicação |
| **Swagger UI** | http://localhost:5000/swagger | Documentação interativa da API |
| **SQL Server** | localhost:1433 | Banco de dados SQL Server |
| **Worker Service** | - | Processamento de mensagens (background) |

## 🔐 Credenciais de Acesso

### **API Authentication**
- **Username**: `admin`
- **Password**: `123`

### **SQL Server DOCKER**
- **Server**: `localhost,1433`
- **Database**: `CludeTesteTecnicoAPI`
- **Username**: `sa`
- **Password**: `YourStrong@Passw0rd`

## 📝 Testando a API

### 1. **Obter Token de Autenticação**
```bash
curl -X POST "http://localhost:5000/api/Auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "username": "admin",
    "password": "123"
  }'
```

### 2. **Usar o Token para Acessar Endpoints Protegidos**
```bash
# Substitua {TOKEN} pelo token retornado no login
curl -X GET "http://localhost:5000/api/Paciente" \
  -H "Authorization: Bearer {TOKEN}"
```

### 3. **Acessar Swagger UI**
Abra o navegador e acesse: `http://localhost:5000/swagger`

## 🛠️ Comandos Úteis

### **Gerenciar Containers**
```bash
# Parar todos os serviços
docker-compose down

# Parar e remover volumes (apaga dados do banco)
docker-compose down -v

# Ver logs de um serviço específico
docker-compose logs api
docker-compose logs sqlserver
docker-compose logs worker

# Ver logs em tempo real
docker-compose logs -f api
```

### **Acessar Container**
```bash
# Acessar container da API
docker-compose exec api bash

# Acessar SQL Server
docker-compose exec sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P YourStrong@Passw0rd
```

### **Reconstruir Imagens**
```bash
# Reconstruir apenas a API
docker-compose build api

# Reconstruir todos os serviços
docker-compose build --no-cache
```

## 🔧 Configurações Avançadas

### **Variáveis de Ambiente**
As configurações estão definidas no `docker-compose.yml`. Principais variáveis:

- `ConnectionStrings__DefaultConnection`: String de conexão com o banco
- `ServiceBus__ConnectionString`: Conexão com Azure Service Bus
- `JwtSettings__SecretKey`: Chave secreta para JWT

### **Portas**
- **API**: 5000 (HTTP)
- **SQL Server**: 1433
- **Worker Service**: Execução em background

## 🐛 Solução de Problemas

### **Problema**: Container não inicia
```bash
# Verificar logs
docker-compose logs

# Verificar se as portas estão disponíveis
netstat -an | findstr :5000
netstat -an | findstr :1433
```

### **Problema**: Erro de conexão com banco
```bash
# Verificar se o SQL Server está rodando
docker-compose ps sqlserver

# Verificar logs do SQL Server
docker-compose logs sqlserver
```

### **Problema**: API não responde
```bash
# Verificar se a API está rodando
docker-compose ps api

# Verificar logs da API
docker-compose logs api

# Verificar se o banco foi inicializado
docker-compose logs init-db
```

### **Problema**: Erro de permissão no Windows**
```bash
# Executar PowerShell como Administrador
# Ou usar WSL2 se disponível
```

## 📁 Estrutura dos Arquivos Docker

```
CludeTest/
├── Dockerfile                 # Imagem da API principal
├── Dockerfile.worker          # Imagem do Worker Service
├── docker-compose.yml         # Orquestração dos serviços
├── .dockerignore              # Arquivos ignorados no build
├── init-db.sql               # Script de inicialização do banco
└── DOCKER_INSTRUCTIONS.md    # Este arquivo
```

## 🔄 Fluxo de Execução

1. **SQL Server** inicia e aguarda estar pronto
2. **Script de inicialização** cria o banco e tabelas
3. **API** inicia e conecta ao banco
4. **Worker Service** inicia para processar mensagens
5. **Sistema** está pronto para uso

## 📞 Suporte

Se encontrar problemas:

1. Verifique os logs: `docker-compose logs`
2. Certifique-se de que Docker Desktop está rodando
3. Verifique se as portas não estão sendo usadas por outros serviços
4. Tente reconstruir as imagens: `docker-compose build --no-cache`

## 🎯 Próximos Passos

Após executar com sucesso:

1. Acesse o Swagger UI: `http://localhost:5000/swagger`
2. Teste o login: `POST /api/Auth/login`
3. Explore os endpoints disponíveis
4. Teste as operações CRUD para Pacientes, Profissionais e Agendamentos

---

