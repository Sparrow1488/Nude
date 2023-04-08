# How to get started

1. Скачать последнюю версию **.NET SDK** ([.NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0))

2. Клонировать репозиторий `Nude` solution

```git
git clone https://github.com/Sparrow1488/Nude.git
```

3. Сконфигурировать `dotnet user-secrets`

```powershell
dotnet user-secrets init --porject {project-name}
```

4. Добавить секреты в проекты.
   
   **Секреты для Nude.Bot.Tg:**
   
   * "*Credentials:Telegram*" "*{token}*"
   
   **Секреты для Nude.API:**
   
   Сейчас в проекте есть 2 ресурса, для которых нужно добавить авторизационные данные, это **NudeMoon** и **HentaiChan**:
   
   * "*Credentials:{resource}:Login*" "*{login}*"
   
   * "*Credentials:{resource}:Password*" "*{password}*"
   
   Секреты устанавливаются следующим образом:

```powershell
dotnet user-secrets set "Credentials:Telegram" "{token}"
```