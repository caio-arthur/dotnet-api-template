## 🛠 Tech Stack

O projeto utiliza **.NET 9**, seguindo os princípios da **Clean Architecture** e **CQRS**:

- **Core:** .NET 9  
- **Arquitetura:** Clean Architecture + CQRS  
- **Banco de Dados:** SQLite (`api.db`) com Entity Framework Core 9  
- **Mensageria Interna:** MediatR (Mediator Pattern)  
- **Consultas Dinâmicas:** Gridify  

---

## 🏗 Estrutura da Arquitetura

A solução é modularizada para garantir separação de responsabilidades:

| Camada        | Responsabilidade |
|---------------|------------------|
| **Domain (Core)** | Entidades, regras de negócio, enums e constantes. Sem dependências externas. |
| **Application**  | Casos de uso (Commands/Queries), DTOs e interfaces. Onde reside o CQRS. |
| **Infrastructure** | Implementação de acesso a dados (EF Core), migrations e serviços externos. |
| **API** | Pontos de entrada (endpoints), configuração de DI e workers. |

---

## 🚀 Como Executar

1. Certifique-se de ter o **.NET SDK 9** instalado.  
2. Clone o repositório.  
3. Rode a aplicação:
```bash
dotnet run --project src/API
```
4. Acesse o Swagger em:
https://localhost:8080/swagger 

---

## 📁 Migrações de Banco de Dados

- Para aplicar migrações do Entity Framework Core, utilize o seguinte comando:

dotnet ef migrations add "Example" -p src/Infrastructure -s src/API -o Migrations

- Para remover a última migração, utilize: 

dotnet ef migrations remove -p src/Infrastructure -s src/API --force
