using QuizService.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuizService.Services
{
    /******************* This interface is used for define GET methods. *****************************/
    public interface IQuizeService
    {
        public IEnumerable<QuizResponseModel> GetAll();

        public Task<QuizResponseModel> GetAsyncById(int id);
    }
}
