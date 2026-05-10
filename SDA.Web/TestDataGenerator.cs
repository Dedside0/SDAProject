using SDA.Db.Models;

namespace SDA.Web
{
    public class TestDataGenerator
    {
        public static Ticket GenerateTestTicket()
        {
            var ticketId = Guid.NewGuid();
            var questions = new List<Question>();
            var ticketQuestions = new List<TicketQuestion>();

            // Базовые вопросы ПДД
            var questionsData = new[]
            {
                new { Text = "В каком случае разрешено выполнить обгон на перекрестке?",
                      Answers = new[] { "На главной дороге", "На регулируемом перекрестке", "Только при отсутствии знаков", "Запрещено всегда" },
                      CorrectIndex = 1 },

                new { Text = "Что означает мигание зеленого сигнала светофора?",
                      Answers = new[] { "Предупреждение о скором выключении", "Разрешает движение", "Запрещает движение", "Требует остановки" },
                      CorrectIndex = 0 },

                new { Text = "Разрешен ли разворот на мосту?",
                      Answers = new[] { "Да, всегда", "Нет, запрещён", "Только при отсутствии знака", "Только в светлое время" },
                      CorrectIndex = 1 },

                new { Text = "Какой знак запрещает движение всех транспортных средств?",
                      Answers = new[] { "Кирпич", "Движение запрещено", "Стоп", "Обгон запрещён" },
                      CorrectIndex = 1 },

                new { Text = "Что такое тормозной путь?",
                      Answers = new[] { "Расстояние от начала торможения до полной остановки", "Время реакции", "Дистанция до препятствия", "Скорость авто" },
                      CorrectIndex = 0 },

                new { Text = "При повороте направо вы обязаны уступить дорогу:",
                      Answers = new[] { "Пешеходам, переходящим проезжую часть", "Автобусам", "Трамваю попутного направления", "Велосипедистам" },
                      CorrectIndex = 0 },

                new { Text = "Допустимая максимальная скорость в населенном пункте:",
                      Answers = new[] { "40 км/ч", "60 км/ч", "80 км/ч", "90 км/ч" },
                      CorrectIndex = 1 },

                new { Text = "Что означает знак 'Главная дорога'?",
                      Answers = new[] { "Приоритет перед пересекаемой", "Запрет стоянки", "Начало населенного пункта", "Скоростное шоссе" },
                      CorrectIndex = 0 },

                new { Text = "Какое наказание за управление в нетрезвом виде?",
                      Answers = new[] { "Штраф 5000", "Лишение прав", "Предупреждение", "Штраф 30000 и лишение" },
                      CorrectIndex = 3 },

                new { Text = "Когда следует включать аварийную сигнализацию?",
                      Answers = new[] { "При вынужденной остановке", "При ослеплении фарами", "При буксировке", "Во всех перечисленных случаях" },
                      CorrectIndex = 3 },

                new { Text = "Что такое безопасная дистанция?",
                      Answers = new[] { "Расстояние до впереди идущего автомобиля", "Время 2 секунды", "Половина скорости в метрах", "Не менее 50 метров" },
                      CorrectIndex = 1 },

                new { Text = "Запрещается ли обгон на пешеходном переходе?",
                      Answers = new[] { "Да, запрещён", "Только если есть пешеходы", "Разрешен", "Разрешен без встречки" },
                      CorrectIndex = 0 },

                new { Text = "Как обозначается учебное транспортное средство?",
                      Answers = new[] { "Знак 'У'", "Желтый цвет", "Мигающий маячок", "Наклейка '70'" },
                      CorrectIndex = 0 },

                new { Text = "Водитель обязан уступить дорогу слепому пешеходу?",
                      Answers = new[] { "Да, всегда", "Нет", "Только на переходах", "Если есть трость" },
                      CorrectIndex = 0 },

                new { Text = "Что такое 'Остановка' по ПДД?",
                      Answers = new[] { "Прекращение движения до 5 минут", "Прекращение движения до 15 минут для посадки", "Плановый перерыв", "Длительная стоянка" },
                      CorrectIndex = 1 },

                new { Text = "Разрешено ли водить автомобиль без ОСАГО?",
                      Answers = new[] { "Да", "Нет, запрещено", "Только в экстренных случаях", "Только на такси" },
                      CorrectIndex = 1 },

                new { Text = "Какой знак предписывает движение прямо?",
                      Answers = new[] { "Круглый синий со стрелкой", "Треугольный", "Прямоугольный", "Знак 4.1.1" },
                      CorrectIndex = 0 },

                new { Text = "При ДТП первое действие водителя?",
                      Answers = new[] { "Включить аварийку", "Вызвать ГИБДД", "Убрать авто", "Сфотографировать" },
                      CorrectIndex = 0 },

                new { Text = "Что обозначает красный сигнал светофора?",
                      Answers = new[] { "Запрещает движение", "Разрешает", "Приготовиться", "Уступить дорогу" },
                      CorrectIndex = 0 },

                new { Text = "Обязательна ли шипованная резина зимой?",
                      Answers = new[] { "Только в определенных регионах", "Да, по закону", "Нет, по желанию", "Только для грузовиков" },
                      CorrectIndex = 1 }
            };

            // Создаём вопросы и ответы
            for (int i = 0; i < questionsData.Length; i++)
            {
                var qData = questionsData[i];
                var questionId = Guid.NewGuid();
                var answers = new List<Answer>();

                // Создаём ответы
                for (int j = 0; j < qData.Answers.Length; j++)
                {
                    answers.Add(new Answer
                    {
                        Id = Guid.NewGuid(),
                        QuestionId = questionId,
                        Text = qData.Answers[j],
                        IsRight = (j == qData.CorrectIndex)
                    });
                }

                // Создаём вопрос
                var question = new Question
                {
                    Id = questionId,
                    Text = qData.Text,
                    Answers = answers
                };

                questions.Add(question);

                // Создаём связь вопроса с билетом
                ticketQuestions.Add(new TicketQuestion
                {
                    QuestionId = questionId,
                    Question = question,
                    TicketId = ticketId,
                    Order = i + 1
                });
            }

            // Создаём билет
            var ticket = new Ticket
            {
                Id = ticketId,
                Name = "Билет 1",
                TicketQuestions = ticketQuestions
            };

            return ticket;
        }

        // Альтернативный метод для получения вопросов без привязки к билету (простой список)
        public static List<Question> GetQuestionsOnly()
        {
            var ticket = GenerateTestTicket();
            var questions = new List<Question>();

            foreach (var tq in ticket.TicketQuestions)
            {
                questions.Add(tq.Question);
            }

            return questions;
        }

        // Метод для получения ответов на конкретный вопрос
        public static List<Answer> GetAnswersForQuestion(Guid questionId, Ticket ticket)
        {
            var question = ticket.TicketQuestions?.FirstOrDefault(tq => tq.QuestionId == questionId)?.Question;
            return question?.Answers ?? new List<Answer>();
        }

        // Проверка правильности ответа
        public static bool IsAnswerCorrect(Guid questionId, Guid answerId, Ticket ticket)
        {
            var question = ticket.TicketQuestions?.FirstOrDefault(tq => tq.QuestionId == questionId)?.Question;
            var answer = question?.Answers?.FirstOrDefault(a => a.Id == answerId);
            return answer?.IsRight ?? false;
        }
    }
}
