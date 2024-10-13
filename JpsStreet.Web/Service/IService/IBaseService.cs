using JpsStreet.Web.Models;

namespace JpsStreet.Web.Service.IService
{
    public interface IBaseService
    {
        Task<ResponseDTo?> SendAsync(RequestDTo requestDto, bool withBearer = true);
    }
}
