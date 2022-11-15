using QuizService.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuizService.Services
{
      /*
       This interface is used for define GET methods.
       This arhitecture is most common used
     */
    public interface IQuizeService
    {
        Task<List<QuizResponseModel>> GetAllAsync();

        Task<QuizResponseModel> GetAsyncById(int id);
    }
}
