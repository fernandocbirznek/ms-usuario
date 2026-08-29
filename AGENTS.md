# AGENTS.md — ms-usuario

## Visão geral

Este repositório contém o microsserviço de usuários da plataforma **Sociedade da Física**, uma comunidade em que professores publicam conteúdo e alunos acompanham aulas, salvam itens de interesse e participam de fóruns.

O ecossistema completo também possui microsserviços de aulas e fórum e um frontend Angular. Esses componentes não fazem parte deste repositório. Ao alterar contratos HTTP, nomes de campos, IDs ou regras compartilhadas, considere a compatibilidade com esses consumidores e não suponha que seja possível modificá-los junto com este serviço.

Além do cadastro e autenticação de usuários, este serviço atualmente mantém perfis, áreas de interesse, conquistas, sociedades, notícias e favoritos de notícias. Preserve esse escopo existente mesmo quando algum nome parecer pertencer a outro microsserviço.

## Stack e estado técnico

- C# e ASP.NET Core Web API, com `TargetFramework` `net6.0`.
- Entity Framework Core 7 e provider Npgsql 7 para PostgreSQL.
- MediatR para commands, queries e handlers.
- Swagger/OpenAPI por Swashbuckle.
- JWT gerado no fluxo de login.
- Docker com imagens .NET 6.
- Não existe projeto de testes automatizados neste repositório no momento.

Há no estado atual uma aplicação .NET 6 com pacotes EF Core 7. O .NET 6 está fora de suporte, mas não atualize framework ou dependências como efeito colateral de outra tarefa; trate a modernização como trabalho próprio e verifique compatibilidade e implantação.

## Estrutura do repositório

- `Program.cs`: composição da aplicação, CORS, Kestrel, Swagger, MediatR e aplicação automática de migrations.
- `Domains/`: entidades persistidas, entidade-base e enums.
- `Features/<Nome>Feature/`: controllers, commands, queries, requests, responses e handlers agrupados por funcionalidade.
- `Extensions/`: mapeamentos entre requests, domínios e responses, hashing de senha, registro de serviços e helper dos controllers.
- `Interface/IRepository.cs` e `Repositories/Repository.cs`: abstração e implementação genérica de persistência.
- `UsuarioDbContext.cs`: `DbSet`s e composição dos mapeamentos do EF Core.
- `Tables/`: configurações Fluent API das tabelas e relações.
- `Migrations/`: migrations do EF Core e o model snapshot.
- `Helpers/MessageHelper.cs`: mensagens padronizadas de validação.
- `Dockerfile` e `docker-compose.yml`: execução em container.

As funcionalidades atuais são `Usuario`, `UsuarioPerfil`, `UsuarioAreaInteresse`, `UsuarioConquistas`, `UsuarioNoticiaFavoritado`, `AreaInteresse`, `Conquistas`, `Sociedade`, `Noticia` e `NoticiaAreaInteresse`.

## Arquitetura e convenções existentes

Siga o fluxo já usado no projeto:

1. O controller deve permanecer fino, receber `IMediator` e encaminhar a operação com `this.SendAsync(...)`.
2. Cada operação deve ser representada por um command ou query que implemente `IRequest<TResponse>`.
3. O handler concentra validação e orquestração; propague o `CancellationToken` nas chamadas assíncronas.
4. A persistência deve passar por `IRepository<T>` salvo quando houver uma razão técnica explícita para usar o contexto diretamente.
5. Conversões entre request, entidade e response pertencem preferencialmente a uma extensão em `Extensions/`.
6. Novas entidades persistidas precisam de `DbSet`, registro do repositório, configuração de tabela/relações quando necessária e migration.
7. Respeite nomes em português e o namespace raiz `ms_usuario`. Não renomeie rotas ou propriedades públicas somente por preferência estética.

Os endpoints usam `[Route("api/[controller]")]` e segmentos em português como `inserir`, `atualizar`, `excluir` e `selecionar-*`. Mudanças nesses contratos são potencialmente incompatíveis com o frontend Angular e outros microsserviços.

O helper `ControllerExtensions.SendAsync` traduz atualmente:

- `ArgumentNullException` em HTTP 400;
- `InvalidOperationException` em HTTP 403;
- `DuplicateNameException` em HTTP 409;
- sucesso em HTTP 200.

Ao criar código novo, mantenha a compatibilidade com esse comportamento. Se for necessário melhorar o modelo de erros ou códigos HTTP, faça a alteração de maneira transversal, documentada e coberta por testes.

Embora exista referência ao pacote FluentValidation, as validações atuais são feitas manualmente nos handlers. Não introduza um segundo padrão isolado em apenas uma funcionalidade; uma migração para validators deve ser planejada para manter comportamento consistente.

## Banco de dados e migrations

O banco é PostgreSQL e a connection string é lida de `ConnectionStrings:DbContext`. Em variáveis de ambiente, use `ConnectionStrings__DbContext`.

A aplicação executa `Database.Migrate()` durante a inicialização. Portanto, uma migration inválida pode impedir o serviço de subir e pode alterar o banco automaticamente durante o deploy.

Para mudanças de modelo:

1. atualize a entidade e o mapeamento Fluent API correspondente;
2. gere uma migration com nome descritivo;
3. revise tanto os métodos `Up` e `Down` quanto `UsuarioDbContextModelSnapshot.cs`;
4. confirme que relações, nulabilidade, índices e exclusões em cascata refletem a regra de negócio;
5. não edite migrations antigas que já possam ter sido aplicadas em Azure ou em ambientes compartilhados; crie uma nova migration corretiva.

Comandos usuais:

```powershell
dotnet ef migrations add NomeDaMigration --project ms-usuario.csproj
dotnet ef database update --project ms-usuario.csproj
```

O segundo comando altera o banco apontado pela configuração ativa. Confirme o ambiente e a connection string antes de executá-lo; nunca o rode automaticamente contra um banco compartilhado ou de produção.

## Configuração, segurança e compatibilidade

- Não inclua senhas, connection strings reais, chaves JWT, tokens ou dados pessoais em commits, logs, testes ou exemplos.
- Para desenvolvimento, prefira .NET User Secrets ou variáveis de ambiente. Em Azure, use a configuração segura do recurso/serviço.
- Considere `appsettings.json` sensível no estado atual e não reproduza seus valores em documentação ou respostas.
- Não registre senha, salt, token JWT, foto ou payload completo de usuário.
- O hashing atual de senhas (SHA-256 com salt próprio) é legado. Não o altere silenciosamente: uma troca exige estratégia de migração para usuários existentes e algoritmo apropriado para senhas.
- O serviço gera JWT, mas a configuração de autenticação/autorização e os atributos `[Authorize]` estão incompletos ou comentados. Também há uma divergência histórica entre os nomes de claim `usuarioId` e `idUsuario`.
- `ControllerExtensions.IdUsuario` é estático e não deve ser usado como armazenamento confiável por requisição.
- A política CORS atual permite qualquer origem, header e método.

Esses itens são riscos conhecidos, não padrões a serem copiados. Se uma tarefa envolver segurança, corrija o fluxo de ponta a ponta e adicione testes; não faça uma correção parcial que invalide tokens existentes ou bloqueie o frontend sem coordenação.

## Execução local

Pré-requisitos: SDK .NET 6 e uma instância PostgreSQL acessível com uma connection string válida.

```powershell
dotnet restore ms-usuario.sln
dotnet build ms-usuario.sln
dotnet run --project ms-usuario.csproj
```

Apesar dos perfis em `launchSettings.json`, `Program.cs` força atualmente o listener HTTP na porta `5002`. O Swagger fica em `http://localhost:5002/swagger`.

Para container:

```powershell
docker compose up --build
```

O compose publica `8102:5002`, espera uma rede Docker externa chamada `app-network` e usa atualmente o hostname PostgreSQL `ms-aula-postgres`. Valide essa infraestrutura antes de executar; não presuma que ela exista em toda máquina.

## Verificação de mudanças

Para toda alteração:

```powershell
dotnet build ms-usuario.sln
```

Quando houver testes, execute também:

```powershell
dotnet test ms-usuario.sln
```

Como ainda não há suíte automatizada, alterações de comportamento devem preferencialmente adicionar um projeto de testes. No mínimo, valide pelo Swagger ou por chamadas HTTP os caminhos de sucesso, entrada inválida, recurso inexistente, duplicidade e persistência no PostgreSQL. Para autenticação, verifique login, token inválido/expirado e acesso não autorizado.

Não considere a inicialização local concluída apenas porque o build passou: o startup tenta conectar ao PostgreSQL e aplicar migrations.

## Regras para alterações seguras

- Leia o fluxo completo da funcionalidade antes de editar: controller, request/response, handler, extensão, domínio, repositório, mapeamento e migration.
- Preserve alterações locais já existentes e não formate ou reescreva arquivos fora do escopo.
- Evite mudanças amplas de arquitetura junto com uma correção funcional pequena.
- Não exponha entidades EF diretamente em novos contratos se um response específico evitar ciclos, campos sensíveis ou acoplamento de persistência.
- Use métodos assíncronos e propague `CancellationToken`; evite `.Result`, `.Wait()` e consultas N+1.
- Ao carregar navegações, declare os `Include`s necessários via parâmetros do repositório e considere o volume retornado.
- Não retorne `Senha`, `Salt` ou outros campos internos em responses.
- Datas existentes usam o comportamento legado do Npgsql configurado em `Program.cs`; qualquer padronização para UTC exige migration e análise dos consumidores.
- Não altere IDs, enum `TipoUsuarioEnum` ou semântica de contadores de fórum/aula sem avaliar integração com os outros serviços.
- Atualize este arquivo quando comandos, arquitetura, portas, configuração ou responsabilidades do microsserviço mudarem.

## Checklist antes de entregar

- A mudança ficou restrita ao `ms-usuario` e manteve contratos externos ou documentou a quebra?
- Controllers continuam finos e a regra ficou no handler/domínio apropriado?
- Dados sensíveis ficaram fora do código, logs e documentação?
- Alterações no EF incluem uma migration nova e snapshot coerente?
- O build passa sem novos warnings relevantes?
- Os cenários afetados foram testados e as limitações restantes foram registradas?
- Nenhuma alteração local do autor foi sobrescrita?
