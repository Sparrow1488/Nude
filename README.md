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

* Correct dispose PuppeterSharp

* More manga information parsing

* Search by name

* Search by tags

**Nude.API**

* MangaProvider (that return stored in database manga, if not exists -> add to queue, parse and add to db)

* MangaQueueController (stay in queue to get parsed page (used callback))

**Nude.Tg.Bot**:

* Telegraph converter

* Nude.API.Client

* Callback to receive parsed manga

**Database**:

* Postgres in Docker

**CI/CD**:

* But VPS

* Install Docker

* Configure Docker Compose

* 