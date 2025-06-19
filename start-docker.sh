#!/bin/bash

echo "========================================"
echo "   CLUDE Teste Tecnico - Docker Setup"
echo "========================================"
echo

echo "Verificando se o Docker esta rodando..."
if ! docker info > /dev/null 2>&1; then
    echo "ERRO: Docker nao esta rodando!"
    echo "Por favor, inicie o Docker e tente novamente."
    exit 1
fi

echo "Docker esta rodando!"
echo

echo "Parando containers existentes (se houver)..."
docker-compose down

echo
echo "Construindo e iniciando os containers..."
docker-compose up --build -d

echo
echo "Aguardando os servicos inicializarem..."
sleep 30

echo
echo "========================================"
echo "   Servicos Disponiveis:"
echo "========================================"
echo "API Principal: http://localhost:5000"
echo "Swagger UI:    http://localhost:5000/swagger"
echo "SQL Server:    localhost:1433"
echo
echo "Credenciais:"
echo "Username: admin"
echo "Password: 123"
echo
echo "========================================"
echo "Para ver os logs: docker-compose logs -f"
echo "Para parar:       docker-compose down"
echo "========================================"
echo

read -p "Pressione Enter para abrir o Swagger UI..."

# Abrir navegador (funciona em diferentes sistemas)
if command -v xdg-open > /dev/null; then
    xdg-open http://localhost:5000/swagger
elif command -v open > /dev/null; then
    open http://localhost:5000/swagger
else
    echo "Navegador nao pode ser aberto automaticamente."
    echo "Acesse manualmente: http://localhost:5000/swagger"
fi

echo
echo "Setup concluido! Acesse http://localhost:5000/swagger" 