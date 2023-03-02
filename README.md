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

* (потом) ParsingRequest.CallbackUrl

* (потом) IParsingCallbackService.SendAsync(request)

**Nude.Tg.Bot**

* ~~Callback endpoint to receive avilable manga~~

* Add ConvertTickets (id, manga_id, chat_id, status)

* Convert images background service

* Get random TghManga

* (?) Get random Manga

* Add tags to tg response message

* Tgh article response as [Read|<tgh>]

**CI/CD**

* Buy VPS

* Install Docker

* Configure Docker Compose