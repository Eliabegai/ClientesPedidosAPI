

# Iniciar banco de dados
## Requer Docker

``` bash
docker-compose up -d
```

## Migration
``` bash
dotnet ef migrations add InitialCreate
```

## Aplicar migrations ao banco de dados
``` bash
dotnet ef database update
```

