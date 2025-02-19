# Projeto de API para Gerenciamento de Pedidos

Este projeto é uma API desenvolvida em C# com ASP.NET Core e Entity Framework, utilizando PostgreSQL como banco de dados.

## Requisitos

Antes de começar, certifique-se de ter os seguintes requisitos instalados:

- [.NET SDK 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Docker](https://www.docker.com/get-started)
- [PostgreSQL](https://www.postgresql.org/) (opcional, se preferir rodar o banco localmente)

# Configuração do Projeto

1. Clone este repositório:

   ```sh
   git clone https://github.com/Eliabegai/ClientesPedidosAPI.git
   cd ClientesPedidosAPI/ClientesPedidosAPI
   ```

2. Instale as dependências do projeto:

   ```sh
   dotnet restore
   ```

# Iniciar banco de dados

## Configuração do Banco de Dados

O banco de dados pode ser executado em um contêiner Docker. Para isso, execute o seguinte comando:

```sh
docker-compose up -d
```

Caso já tenha um contêiner rodando, você pode iniciá-lo com:

```sh
docker start postgres_clientes_pedidos
```

Se precisar acessar o banco de dados dentro do contêiner, use:

```sh
docker exec -it postgres_clientes_pedidos psql -U admin -d clientes_pedidos
```

## Migration
```bash
dotnet ef migrations add InitialCreate
```

## Aplicar migrations ao banco de dados
```bash
dotnet ef database update
```

# Rodar API
(reinicia automaticamente ao editar o código)
```bash
dotnet run
dotnet watch run
```

A API estará disponível em `http://localhost:5020` ou `https://localhost:5001`.


# Configuração FrontEnd
FrontEnd simples utilizando o Vite para testar alguns endpoints, facilitando a visualização, criação e edição dos pedidos e clientes.

```sh
cd ./pedidosClientes/
npm install
npm run dev
```
O Front estará disponível em `http://localhost:5173`.
