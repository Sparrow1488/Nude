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

* ~~Correct dispose PuppeterSharp~~

* More manga information parsing (Author)

* Search by name

* Search by tags

* Search by author

* Search all linked manga

**Nude.API**

* ~~MangaProvider (that return stored in database manga, if not exists -> add to queue, parse and add to db)~~

* ~~MangaQueueController (stay in queue to get parsed page (used callback))~~

* TagNormalizeConfiguration

* RuntimeQueryableFilter

**Nude.Tg.Bot**:

* Telegraph converter

* Nude.API.Client

* Callback to receive parsed manga

**Database**:

* ~~Postgres in Docker~~

**CI/CD**:

* Buy VPS

* Install Docker

* Configure Docker Compose