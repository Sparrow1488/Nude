# How to get started

1. Скачать последнюю версию **.NET SDK** ([.NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0))

2. Клонировать репозиторий `Nude` solution

```git
git clone https://github.com/Sparrow1488/Nude.git
```

3. Добавить секреты в проекты

```powershell
dotnet user-secrets init --porject {project-name}
```

**Nude.API**

```json
{
    "Credentials": {
        "NudeMoon": {
            "Login": "{login}",
            "Password": "{password}"
        },
        "HentaiChan": {
            "Login": "{login}",
            "Password": "{password}"
        }
    }
}
```

**Nude.Bot.Tg**

 ```json
 {
     "Credentials": {
         "Telegram": "{token}"
     }
 }
 ```

```powershell
dotnet user-secrets set "{path}" "{value}"
```

4. Запустить БД PostgreSQL

```powershell
docker run --name Nude.DB -p 5432:5432 -e POSTGRES_PASSWORD=secret -d postgres
```

