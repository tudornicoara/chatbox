using System.Threading.Tasks;
using ChatBox.Entities.Identity;

namespace ChatBox.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
    }
}
