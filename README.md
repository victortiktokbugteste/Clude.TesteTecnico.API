# CLUDE TESTE TÉCNICO - API

Esse projeto é responsável por processar toda busca, inserção, atualização, exclusão, de pacientes, profissionais de saúde, agendamentos da nossa clínica CLUDE.
Ele é o motor da nossa aplicação, foi feito aplicando príncipios de Código Limpo, separando bem as responsabilidades de cada classe, 
aplicando as regras de negócio que foram solicitadas no escopo do teste.

!!! O Service Bus eu não publiquei como web application, mas você consegue rodar ele se deixar ele como projeto de inicialização, ele é o "WorkerService".
Também tem o projeto Clude.TesteTecnico.API.Tests com alguns testes implementados.

# Senha que é solicitada caso queira chamar o endpoint api/Auth/login
username:admin, password:123

# Decisões técnicas EXPLICATIVO

1 - Decidi usar Authorize com JWT sendo Bearer Token para tornar nossas controladoras seguras, que elas possam receber apenas requisições autorizadas.
2 - Criei AuthenticationLoggingMiddleware para guardar as requisições e extourar a excessão pra Token inválido/login não autorizado.
3 - Criei RequestLoggingMiddleware para guardar as requisições e extourar as excessões para quaisquer erros que possam ocorrer em nosso servidor, 
desde validação de regra de negócio até à validação de entidades ou de quaisquer outros problemas.
4 - Esses logs dos dois Middlewares são salvos na tabela ApplicationMiddlewareLogError do banco de dados, de uma forma que conseguimos rastrear depois,
porém poderia utilizar facilmente Airbrake pra armazenar esses dados.
5 - Apliquei um middleware que redireciona para o swagger caso chamar a url base da aplicação.
6 - Adicionei o AddCors e UseCors para permitir nosso frontend chamar nossa api.
7 - Toda minha aplicação eu separei as responsabilidades de cada classe, as controllers servem apenas para roteamento, usei o Mediatr para que cada endpoint tenha
seu handler pra executar suas próprias regras de negócio.
8 - Procurei documentar dentro das commands utilizando da biblioteca Swashbuckle e também procurei solicitar somente as propriedades necessárias do front para o backend.
9 - Para separar ainda mais as responsabilidades de cada classe, utilizei o FluentValidator para fazer a validação da própria entidade dentro do Handler, pois dessa forma
o handler só implementa código que realmente seja regra de negócio que não seja da entidade.
10 - Os repositórios herdam uma interface destinada ao próprio repositório exemplo AgendamentoRepository : IAgendamentoRepository, dentro de IAgendamentoRepository tem metódos
que são usados apenas por AgendamentoRepository porém a IAgendamentoRepository ainda herda IRepository<Agendamento> que é uma interface com metódos genéricos que podem ser usados
em vários outros repositórios que tem metódos em comum.
11 - Decidi aplicar Azure Service Bus para registrar quando um agendamento é adicionado, ele envia esse agendamento para a Azure, contendo o e-mail do profissional de saúde,
a mensagem é escrita na fila emailsagendamento, e criei o projeto WorkerService que fica escutando essa fila, pega o registro e marca esse agendamento como se o email tivesse sido enviado.
12 - Criei um projeto de testes unitários xunit para testar alguns casos se estavam lançando a validação do jeito correto, e pra isso utilizei a biblioteca Mock, pra simular repositório.

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
Para conectar no banco: Server=tcp:cludeapi.database.windows.net,1433; Database=CludeTesteTecnicoAPI; User Id=victor; Password=@Dev2025;Trusted_Connection=False;Encrypt=True;


# LOGS DE ERROS
Ficam na tabela ApplicationMiddlewareLogError, ela intercepta os erros que aconteceram na nossa aplicação através dos nossos middlewares.


## ⚙️ Como rodar localmente

1. Clone o repositório: git clone https://github.com/victortiktokbugteste/CludeTest.git
2. Tenha o node LTS 22 instalado em sua máquina
3. Execute o seguinte comando dentro do diretório do projeto: npm install
4. Execute o comando: npm start (ele vai rodar localmente)
5. Hoje ele aponta todas as rotas para https://cludetesteapi.azurewebsites.net que é o backend que já publiquei. 