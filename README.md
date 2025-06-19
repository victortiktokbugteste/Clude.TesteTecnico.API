# CLUDE TESTE TÉCNICO - API

https://cludetesteapi.azurewebsites.net

Esse projeto é responsável por processar toda busca, inserção, atualização, exclusão, de pacientes, profissionais de saúde, agendamentos da nossa clínica CLUDE.
Ele é o motor da nossa aplicação, foi feito aplicando príncipios de Código Limpo, separando bem as responsabilidades de cada classe, 
aplicando as regras de negócio que foram solicitadas no escopo do teste.

!!! O Service Bus eu não publiquei como web application, mas você consegue rodar ele se deixar ele como projeto de inicialização, ele é o "WorkerService".
Também tem o projeto Clude.TesteTecnico.API.Tests com alguns testes implementados.

# Senha que é solicitada caso queira chamar o endpoint api/Auth/login
username:admin, password:123

## 🐳 Execução com Docker (Recomendado)

Para facilitar a execução do projeto, criamos uma solução Docker completa. 

### **Opção 1: Script Automático (Mais Fácil)**
- **Windows**: Execute `start-docker.bat`
- **Linux/Mac**: Execute `./start-docker.sh`

### **Opção 2: Comandos Manuais**
```bash
# Clone o repositório
git clone https://github.com/victortiktokbugteste/Clude.TesteTecnico.API.git
cd Clude.TesteTecnico.API

# Execute com Docker Compose
docker-compose up --build -d
```

### **Acesso aos Serviços**
- **API**: http://localhost:5000
- **Swagger UI**: http://localhost:5000/swagger
- **SQL Server**: localhost:1433

### **Credenciais**
- **Username**: `admin`
- **Password**: `123`

📖 **Instruções detalhadas**: Veja [DOCKER_INSTRUCTIONS.md](DOCKER_INSTRUCTIONS.md)

## Decisões técnicas EXPLICATIVO

1. **JWT Bearer Token:**  
   Decidi usar Authorize com JWT sendo Bearer Token para tornar nossas controladoras seguras, permitindo que apenas requisições autorizadas sejam aceitas.

2. **AuthenticationLoggingMiddleware:**  
   Criei o middleware para guardar as requisições e estourar exceção para Token inválido/login não autorizado.

3. **RequestLoggingMiddleware:**  
   Criei o middleware para guardar as requisições e estourar exceções para quaisquer erros que possam ocorrer no servidor, desde validação de regra de negócio até validação de entidades ou outros problemas.

4. **Logs centralizados:**  
   Os logs dos dois middlewares são salvos na tabela `ApplicationMiddlewareLogError` do banco de dados, permitindo rastreamento posterior. Poderia ser facilmente adaptado para usar Airbrake ou outro serviço.

5. **Redirecionamento para Swagger:**  
   Implementei um middleware que redireciona para o Swagger caso a URL base da aplicação seja chamada.

6. **CORS:**  
   Adicionei `AddCors` e `UseCors` para permitir que o frontend acesse a API.

7. **Responsabilidade das controllers:**  
   As controllers servem apenas para roteamento. Usei o MediatR para que cada endpoint tenha seu handler, responsável por executar suas próprias regras de negócio.

8. **Documentação e contratos enxutos:**  
   Documentei as commands usando Swashbuckle e solicito apenas as propriedades necessárias do frontend para o backend.

9. **Validação com FluentValidation:**  
   Utilizei FluentValidator para validar a entidade dentro do Handler, garantindo que o handler implemente apenas regras de negócio que não sejam da entidade.

10. **Repository Pattern:**  
    Os repositórios herdam uma interface específica, por exemplo:  
    `AgendamentoRepository : IAgendamentoRepository`.  
    Métodos exclusivos ficam na interface específica, mas ela também herda de `IRepository<Agendamento>`, que contém métodos genéricos reutilizáveis.

11. **Azure Service Bus e Worker Service:**  
    Usei Azure Service Bus para registrar quando um agendamento é adicionado. O agendamento é enviado para a Azure, contendo o e-mail do profissional de saúde, e a mensagem é escrita na fila `emailsagendamento`. O projeto WorkerService escuta essa fila, pega o registro e marca o agendamento como se o e-mail tivesse sido enviado.

12. **Testes unitários:**  
    Criei um projeto de testes unitários com xUnit para validar regras de negócio e uso de validação, utilizando Moq para simular repositórios.

# Decisões técnicas RESUMO
1. **JWT Bearer Token**: Para autenticação segura das APIs
2. **Middleware de Logging**: Rastreamento completo de erros
3. **CQRS com MediatR**: Separação clara entre comandos e consultas
4. **FluentValidation**: Validação centralizada e consistente
5. **Repository Pattern**: Abstração do acesso a dados
6. **Azure Service Bus**: Mensageria para notificações de agendamento
7. **Worker Service**: Processamento assíncrono de mensagens
8. **Testes Unitários**: Cobertura de regras de negócio críticas

## 🛠️ Tecnologias Utilizadas

- **.NET 8** - Framework principal
- **MediatR** - Implementação do padrão Mediator
- **FluentValidation** - Validação de entidades
- **Dapper** - Micro ORM para acesso a dados
- **JWT Bearer Token** - Autenticação
- **Azure Service Bus** - Mensageria
- **Swagger/OpenAPI** - Documentação da API
- **xUnit + Moq** - Testes unitários
- **Docker** - Containerização da aplicação



# Arquitetura do projeto DESENHO

┌─────────────────────────────────────────────────────────────┐
│                    Clude.TesteTecnico.API                  │
│                    (Presentation Layer)                     │
├─────────────────────────────────────────────────────────────┤
│                Controllers + Middleware                     │
│                - AuthController                             │
│                - PacienteController                         │
│                - ProfissionalSaudeController                │
│                - AgendaController                           │
│                - RequestLoggingMiddleware                   │
│                - AuthenticationLoggingMiddleware            │
└─────────────────────────────────────────────────────────────┘
                                │
                                ▼
┌─────────────────────────────────────────────────────────────┐
│              Clude.TesteTecnico.API.Application            │
│                   (Application Layer)                       │
├─────────────────────────────────────────────────────────────┤
│  Commands/Queries + Handlers + Validators + DTOs           │
│  - MediatR (CQRS Pattern)                                  │
│  - FluentValidation                                        │
│  - Command/Query Handlers                                  │
│  - Response Models                                         │
└─────────────────────────────────────────────────────────────┘
                                │
                                ▼
┌─────────────────────────────────────────────────────────────┐
│                Clude.TesteTecnico.API.Domain               │
│                    (Domain Layer)                          │
├─────────────────────────────────────────────────────────────┤
│  Entities + Interfaces + Domain Logic                      │
│  - Agendamento, Paciente, ProfissionalSaude               │
│  - IRepository<T>, IAgendamentoRepository, etc.           │
│  - Domain Services & Utils                                 │
└─────────────────────────────────────────────────────────────┘
                                │
                                ▼
┌─────────────────────────────────────────────────────────────┐
│             Clude.TesteTecnico.API.Infrastructure          │
│                  (Infrastructure Layer)                     │
├─────────────────────────────────────────────────────────────┤
│  Repositories + Services + External Integrations           │
│  - Dapper (Data Access)                                    │
│  - Azure Service Bus                                       │
│  - Logging Services                                        │
│  - JWT Authentication                                      │
└─────────────────────────────────────────────────────────────┘



# SCRIPTS DE BANCO
Na pasta Scripts tem o script que cria o banco e as tabelas que usamos.
Para conectar no banco do servidor sql: Server=tcp:cludeapi.database.windows.net,1433; Database=CludeTesteTecnicoAPI; User Id=victor; Password=@Dev2025;Trusted_Connection=False;Encrypt=True;


# LOGS DE ERROS
Ficam na tabela ApplicationMiddlewareLogError, ela intercepta os erros que aconteceram na nossa aplicação através dos nossos middlewares.


