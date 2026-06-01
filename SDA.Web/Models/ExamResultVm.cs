using SDA.Db.Models;

namespace SDA.Web.Models
{
    public class ExamResultVm
    {
        public Guid AttemptId { get; set; }
        public bool IsPassed { get; set; }
        public int CorrectCount { get; set; }
        public int WrongCount { get; set; }
        public int TotalQuestions { get; set; }
        public int TimeSpentSeconds { get; set; }
        public DateTime FinishedAt { get; set; }


        public int Accuracy =>
            TotalQuestions > 0
                ? (int)Math.Round(CorrectCount * 100.0 / TotalQuestions)
                : 0;

        public string TimeDisplay =>
            $"{TimeSpentSeconds / 60}:{TimeSpentSeconds % 60:D2}";

        
        public List<int> QuestionTimes { get; set; } = new();

        public List<bool> QuestionCorrect { get; set; } = new();

        public List<TopicStat> TopicStats { get; set; } = new();


        public static ExamResultVm Build(
            ExamAttempt attempt,
            List<QuestionAttempt> questionAttempts)
        {
            var vm = new ExamResultVm
            {
                AttemptId = attempt.Id,
                IsPassed = attempt.IsPassed,
                CorrectCount = attempt.CorrectCount,
                WrongCount = attempt.WrongCount,
                TotalQuestions = attempt.TotalQuestions,
                TimeSpentSeconds = attempt.TimeSpentSeconds,
                FinishedAt = attempt.FinishedAt
            };

            // Сортируем по времени ответа
            var sorted = questionAttempts
                .Where(qa => qa.SelectedAnswerId != null)
                .OrderBy(qa => qa.AnsweredAt)
                .ToList();

            // Вычисляем время на каждый вопрос как разницу между соседними AnsweredAt
            var examStart = attempt.FinishedAt
                                   .AddSeconds(-attempt.TimeSpentSeconds);

            for (int i = 0; i < sorted.Count; i++)
            {
                var prev = i == 0 ? examStart : sorted[i - 1].AnsweredAt;
                var spent = (int)Math.Max(1,
                    (sorted[i].AnsweredAt - prev).TotalSeconds);

                vm.QuestionTimes.Add(spent);
                vm.QuestionCorrect.Add(sorted[i].IsCorrect);
            }

            // Точность по темам
            vm.TopicStats = questionAttempts
                .Where(qa => !string.IsNullOrWhiteSpace(qa.Topic))
                .GroupBy(qa => qa.Topic!)
                .Select(g => new TopicStat
                {
                    Topic = g.Key,
                    Correct = g.Count(x => x.IsCorrect),
                    Total = g.Count()
                })
                .OrderByDescending(t => t.Total)
                .ToList();

            return vm;
        }

        public class TopicStat
        {
            public string Topic { get; set; } = "";
            public int Correct { get; set; }
            public int Total { get; set; }
            public int Accuracy =>
                Total > 0 ? (int)Math.Round(Correct * 100.0 / Total) : 0;
        }
    }
}