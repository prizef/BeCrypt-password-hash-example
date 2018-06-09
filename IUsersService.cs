using Models.Domain;
using Models.Requests;
using System.Collections.Generic;

namespace Services
{
    public interface IUsersService
    {
        int Create(UserCreateRequest model);
    }
}