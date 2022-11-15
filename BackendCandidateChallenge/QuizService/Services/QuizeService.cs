using Dapper;
using QuizService.Model;
using QuizService.Model.Domain;
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

        public Task<QuizResponseModel> GetAsyncById(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
