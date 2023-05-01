- [ ] ContentAware
  * Хранит список Domains, поддерживаемые Nude.API и парсерами, а также список с `SourceAssociationEntry` структурой, являющаяся SourceType => Domains[ ]
  * `DetectEntryTypeByUrl(url)`
- [ ] `NudeMoonDefaults` и `HentaiChanDefaults` также содержат информацию о свих доменах

- [ ] `ContentKeyGenerator` нужно допилить (шифровать строку с возможность расшифровки Type и Row)
- [ ] `MangaParserResolver` содержит проверку домена