

using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface ILikesRespository
    {
        Task<UserLike> GetUserLike(int sourceUserId , int targetUserId);
        Task<AppUser> GetUserWithLikes(int userID);
        Task<PagedList<LikeDto>> GetUsersLikes(LikesParams likesParams);
    }
}