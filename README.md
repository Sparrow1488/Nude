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

**Fix**

* ~~Available domains in configuration~~

* ~~Telegraph article source~~

* Check images format before upload to Telegraph (.*webp*)

* Can connect to callback url

**Nude**

* PuppeterSharp optimization requests

* Handle exceptions and retry requests

* More searching patterns (by name, tags, author)

* Search all linked manga

**Nude.API**

* (feature) TagNormalizeConfiguration

* (feature) RuntimeQueryableFilter

* Users

**Nude.Tg.Bot**

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