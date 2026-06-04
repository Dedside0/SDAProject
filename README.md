# 🚦 SDA — ПДД Экзаменатор
[![.NET](https://img.shields.io/badge/.NET-8%2B-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-MVC-5C2D91?logo=aspdotnetcore)](https://dotnet.microsoft.com/apps/aspnet)
[![EF Core](https://img.shields.io/badge/Entity%20Framework-Core-0078D4)](https://learn.microsoft.com/ef/core/)
[![C#](https://img.shields.io/badge/C%23-12%2B-239120?logo=csharp)](https://learn.microsoft.com/dotnet/csharp/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

> Современная веб-платформа для подготовки к экзамену по ПДД с аналитикой, тренажёром и админ-панелью.  
> Гибридная архитектура, тёмная тема, адаптивный интерфейс и строгое следование регламенту ГИБДД.

---

## ✨ Особенности
| Модуль | Описание |
|--------|----------|
| 🎓 **Экзамен** | 20 вопросов, таймер 20 мин, правило «2 ошибки → +5 вопросов по слабым темам» |
| 📚 **Тренировка** | По билетам, темам, только ошибки, режим «подряд» с мгновенной проверкой |
| 📊 **Статистика** | История попыток, точность по темам, прогресс-бары, рекомендации по обучению |
| 🛡️ **Админ-панель** | CRUD для билетов/вопросов, модерация, управление пользователями |
| 🌙 **UI/UX** | Кастомная CSS-система с переменными, плавные анимации, полная адаптивность |
| ⚡ **Архитектура** | SSR через Razor + асинхронный `fetch` для мгновенного отклика без перезагрузки |

---

## 🏗 Архитектура проекта
Приложение построено на принципах разделения ответственности и использует **двухпроектную структуру**:

| Проект | Уровень | Состав |
|--------|---------|--------|
| `SDA.Db` | Data / Domain | Сущности, интерфейсы репозиториев, `DbContext`, конфигурации EF Core, миграции |
| `SDA.Web` | Presentation / Logic | Контроллеры MVC, Razor-представления, клиентский JS/CSS, DI, аутентификация |

**Поток данных:**  
`fetch` → `Controller` → `Service/Repository` → `EF Core` → `SQLite` → `JSON/PartialView` → DOM

---

## 🛠 Технологический стек
| Категория | Технологии |
|-----------|------------|
| Backend | .NET 8/9, ASP.NET Core MVC, C# 12, Dependency Injection |
| Data Access | Entity Framework Core, SQL Server / PostgreSQL, Repository Pattern |
| Frontend | Vanilla JS (`fetch` API), CSS Variables, адаптивная сетка, SVG-иконки |
| Auth & Security | ASP.NET Identity, Cookie-аутентификация, CSRF-токены, PBKDF2 |
| Dev Tools | Git, EF CLI, .NET CLI, Swagger (опционально) |

---
