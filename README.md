# Решение задания: Система магазина

## Обзор решения

Реализована система магазина для мультиплеерной игры с использованием современной **Onion Architecture** и принципов SOLID. Система поддерживает покупки за реальные деньги, игровую валюту.

## Архитектура решения (Onion/Layered Architecture)

### Структура проекта:

```
GameShopSystem/
├──GameShopSystem.Domain		      # Ядро бизнес логики
│  ├─Entities/						        # Сущности (Player, ShopItem)
│  ├─DomainEvemts/				        # События 
│  ├─RepositoriesInterfaces/	    # Интерфейсы репозиториев
│  ├─Services/						        # Сервисы бизнес логики
│  │  └─Interfaces					      # Интерфейсы сервисов
│  └─ValueObjects					        # Объекты значения
├──GameShopSystem.Application	    # Сценарии использования
│  ├─Hundlers/						        # Обработчики команд и запросов
│  ├─UseCases/						        # Основные сценарии
│  │  ├Comands/						        # Команды
│  │  └Queries/						        # Запросы
│  ├─DTOs/							          # Data Transfer Object
│  ├─Interfaces/						      # Интерфейсы внешних сервисов
│  └─Extensions/					        # Расширения (для DI)
├──GameShopSystem.Infrastructure	# Реализации
├──GameShopSystem.Presentation	  # Презентация
└─GameShopSystem.Test			        # Тестирование
   ├─Unit/							          # Юнит тесты
   └─Integration/						      # Интеграционные тесты
```
### Диаграмма Onion Architecture:

![Onion Architecture](/docs/Architecture.png)

## Детали реализации

### Архитектурные паттерны:
- **Onion Architecture**: Разделение на слои Domain, Application, Infrastructure, Presentation. 
- **CQRS**: Разделение на команды (изменение данных) и запросы (чтение)
- **Mediator**: Разделение на команды (изменение данных) и запросы (чтение)
- **Dependency Injection**:  Microsoft.Extensions.DependencyInjection

### Типы покупок:
- **Gold**:  Microsoft.Extensions.DependencyInjection
- **Gems**:  Microsoft.Extensions.DependencyInjection
- **RealMoney**:  Microsoft.Extensions.DependencyInjection

## Запуск и тестирование

### Требования:
- .NET 8.0 SDK
- Visual Studio 2022+

## Запуск и тестирование

### Требования:

- .NET 8.0 SDK
- Visual Studio 2022+ или VS Code

### Команды
```bash
# Сборка проекта
dotnet build

# Запуск всех тестов
dotnet test GameShopSystem.Test/GameShopSystem.Test.csproj

# Запуск с покрытием кода
dotnet test  --settings: GameShopSystem.Test/tests.runsettings
```

### Результаты тестирования
- **Всего**: 13;
- **Успешно**: 13;
- **Сбой**: 0;
- **Пропущено**: 0;
- **Длительность**: 5,3 с;
- **Покрытие кода**: 95%;
- **Покрытие веток**:  59%;
