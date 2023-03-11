# Nude

## Infrastructure

```asciidoc
                 ┌──────────────┐
                 │              │
                 │   Database   │
                 │              │
                 └────▲─────────┘
                      │
   ┌──────────┐       │    ┌─────────────────┐
   │          │       │    │                 │
   │ Nude.API ├───────┴────►  Nude-Moon.org  │
   │          │            │                 │
   └──┬───▲───┘            └─────────────────┘
      │   │
┌─────▼───┴──────┐
│                └────► Convert manga images
│  Nude.Tg.Bot   ◄────┘ to Telegraph article
│                │
└───────┬────────┘
        │
        │
   ┌────▼─────────────┐
   │ Telegram Client  │
   └──────────────────┘
```

## TODO

**Nude**

* PuppeterSharp optimization requests

* Handle exceptions and retry requests

* More searching patterns (by name, tags, author)

* Search all linked manga

**Nude.API**

* (потом) TagNormalizeConfiguration

* (потом) RuntimeQueryableFilter

* Users

* ~~ParsingRequest.CallbackUrl~~

* ~~IParsingCallbackService.SendAsync(request)~~

**Nude.Tg.Bot**

* ~~Callback endpoint to receive avilable manga~~

* ~~Add ConvertTickets (id, manga_id, chat_id, status)~~

* ~~Convert images background service~~

* ~~Get random TghManga~~

* ~~Tgh article response as [Read|<tgh>]~~

* TelegramUsers

**CI/CD**

* Buy VPS

* Install Docker

* Configure Docker Compose



Claims

1. `nude.user.default`
   
   * Max 3 parallel processing requests

2. `nude.user.premium`
   
   * Max 10 parallel processing requests
   
   * Create manga

3. `nude.user.admin` 
   
   * No limit parallel processing requests