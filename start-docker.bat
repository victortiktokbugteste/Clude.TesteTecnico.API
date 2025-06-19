@echo off
echo ========================================
echo    CLUDE Teste Tecnico - Docker Setup
echo ========================================
echo.

echo Verificando se o Docker esta rodando...
docker info >nul 2>&1
if %errorlevel% neq 0 (
    echo ERRO: Docker nao esta rodando!
    echo Por favor, inicie o Docker Desktop e tente novamente.
    pause
    exit /b 1
)

echo Docker esta rodando!
echo.

echo Parando containers existentes (se houver)...
docker-compose down

echo.
echo Construindo e iniciando os containers...
docker-compose up --build -d

echo.
echo Aguardando os servicos inicializarem...
timeout /t 30 /nobreak >nul

echo.
echo ========================================
echo    Servicos Disponiveis:
echo ========================================
echo API Principal: http://localhost:5000
echo Swagger UI:    http://localhost:5000/swagger
echo SQL Server:    localhost:1433
echo.
echo Credenciais:
echo Username: admin
echo Password: 123
echo.
echo ========================================
echo Para ver os logs: docker-compose logs -f
echo Para parar:       docker-compose down
echo ========================================
echo.

echo Pressione qualquer tecla para abrir o Swagger UI...
pause >nul
start http://localhost:5000/swagger

echo.
echo Setup concluido! Acesse http://localhost:5000/swagger
pause 