using SDA.Db.Models;
using SDA.Web.Models;
using SDA.Web.Models.DTO;
using System.Runtime.CompilerServices;
using static SDAProject.Controllers.GeneratedExamController;

namespace SDA.Web
{
    public static class Mapper
    {
        private static Guid NormalizeId(Guid id)
        {
            return id.ToString().StartsWith("00000000") ? Guid.Empty : id;
        }


        //    var ticket = new Ticket
        //    {
        //        Id = dto.Id,
        //        Name = dto.Name,
        //        Theme = dto.Theme,
        //        Description = dto.Description
        //    };

        //    ticket.TicketQuestions = dto.TicketQuestions.Select(tqDto =>
        //            {
        //                Guid realQuestionId = tqDto.QuestionId.ToString().StartsWith("00000000")
        //                    ? Guid.Empty
        //                    : tqDto.QuestionId;

        //    var question = new Question
        //    {
        //        Id = realQuestionId,
        //        Text = tqDto.Text,
        //        ImageUrl = tqDto.ImageUrl,
        //        Explanation = tqDto.Exploration!,
        //        TopicId = tqDto.ThemeId,
        //        Answers = tqDto.Answers.Select(aDto => new Answer
        //        {
        //            Id = aDto.Id.ToString().StartsWith("00000000") ? Guid.Empty : aDto.Id,
        //            Text = aDto.Text,
        //            IsRight = aDto.IsRight,
        //            Order = aDto.Order,
        //        }).ToList()
        //    };

        //                return new TicketQuestion
        //                {
        //        Order = tqDto.Order,
        //                    QuestionId = realQuestionId,
        //                    Question = question,
        //                    Ticket = ticket
        //    };
        //}).ToList();

        public static Ticket ToTicket(this SaveTicketDto dto)
        {
            var ticketId = NormalizeId(dto.Id);
            var ticket = new Ticket
            {
                Id = ticketId,
                Name = dto.Name,
                Theme = dto.Theme,
                Description = dto.Description,
            };

            ticket.TicketQuestions = dto.TicketQuestions.Select(tqDto =>
                {
                    var question = new Question
                    {
                        Id = NormalizeId(tqDto.QuestionId),
                        Text = tqDto.Text,
                        ImageUrl = tqDto.ImageUrl,
                        Explanation = tqDto.Exploration!,
                        TopicId = tqDto.ThemeId,
                        Answers = tqDto.Answers.Select(aDto => new Answer
                        {
                            Id = aDto.Id.ToString().StartsWith("00000000") ? Guid.Empty : aDto.Id,
                            Text = aDto.Text,
                            IsRight = aDto.IsRight,
                            Order = aDto.Order,
                        }).ToList()
                    };

                    return new TicketQuestion
                    {
                        Order = tqDto.Order,
                        QuestionId = question.Id,
                        Question = question,
                        Ticket = ticket,
                        TicketId = ticketId,
                    };
                }).ToList();
            return ticket;
        }

        public static TopicDTO ToTopicDto(this Topic topic)
        {
            return new TopicDTO
            {
                Name = topic.Name,
                Group = topic.Group,
                Id = topic.Id,
            };
        }

    }
}
