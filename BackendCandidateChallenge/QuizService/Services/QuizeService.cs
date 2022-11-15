﻿using Dapper;
using QuizService.Model;
using QuizService.Model.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace QuizService.Services
{
    public class QuizeService : IQuizeService
    {
        private readonly IDbConnection _connection;

        public QuizeService(IDbConnection connection)
        {
            _connection = connection;
        }
        public Task<List<QuizResponseModel>> GetAllAsync()
        {
            const string sql = "SELECT * FROM Quiz;";
            var quizzes = _connection.Query<Quiz>(sql);
            return (Task<List<QuizResponseModel>>)quizzes.Select(quiz =>
                new QuizResponseModel
                {
                    Id = quiz.Id,
                    Title = quiz.Title
                });
        }

        //Todo: I see here, we have a lot of constant (const), so I would move this constant in separate class and call it here ! 
        public async Task<QuizResponseModel> GetAsyncById(int id)
        {
            try
            {
                const string quizSql = "SELECT * FROM Quiz WHERE Id = @Id;";
                var quiz = _connection.QuerySingle<Quiz>(quizSql, new { Id = id });
                const string questionsSql = "SELECT * FROM Question WHERE QuizId = @QuizId;";
                var questions = _connection.Query<Question>(questionsSql, new { QuizId = id });
                const string answersSql = "SELECT a.Id, a.Text, a.QuestionId FROM Answer a INNER JOIN Question q ON a.QuestionId = q.Id WHERE q.QuizId = @QuizId;";
                var answers = _connection.Query<Answer>(answersSql, new { QuizId = id })
                    .Aggregate(new Dictionary<int, IList<Answer>>(), (dict, answer) =>
                    {
                        if (!dict.ContainsKey(answer.QuestionId))
                            dict.Add(answer.QuestionId, new List<Answer>());
                        dict[answer.QuestionId].Add(answer);
                        return dict;
                    });
                return new QuizResponseModel
                {
                    Id = quiz.Id,
                    Title = quiz.Title,
                    Questions = questions.Select(question => new QuizResponseModel.QuestionItem
                    {
                        Id = question.Id,
                        Text = question.Text,
                        Answers = answers.ContainsKey(question.Id)
                            ? answers[question.Id].Select(answer => new QuizResponseModel.AnswerItem
                            {
                                Id = answer.Id,
                                Text = answer.Text
                            })
                            : Array.Empty<QuizResponseModel.AnswerItem>(),
                        CorrectAnswerId = question.CorrectAnswerId
                    }),
                    Links = new Dictionary<string, string>
            {
                {"self", $"/api/quizzes/{id}"},
                {"questions", $"/api/quizzes/{id}/questions"}
            }
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
