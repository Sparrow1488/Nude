# Nude API

## Endpoints

1. Найти мангу по `Id`:

```http
GET /manga/{id} HTTP/1.1
```

Успешно: `Status 200`, `MangaResponse`

Не найдено: `Status 404`

2. Найти мангу по `ContentKey`:

```http
GET /manga?contentKey={contentKey} HTTP/1.1
```

Успешно: `Status 200`, `MangaResponse`

Не найдено: `Status 404`
