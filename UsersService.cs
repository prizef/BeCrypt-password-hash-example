using Data.Providers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Models.Domain;
using Models.Requests;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Data;

namespace Services
{
    public class UsersService : IUsersService
    {
        private IAuthenticationService authenticationService;
        private IDataProvider dataProvider;

        public UsersService(IAuthenticationService authService, IDataProvider dataProvider)
        {
            authenticationService = authService;
            this.dataProvider = dataProvider;
        }

        public int Create(UserCreateRequest model)
        {
            int userId = 0;
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

            try
            {
                dataProvider.ExecuteNonQuery(
                    "Users_Create",
                    inputParamMapper: (parameters) =>
                    {
                        parameters.AddWithValue("@FirstName", model.FirstName);
                        parameters.AddWithValue("@LastName", model.LastName);
                        parameters.AddWithValue("@Email", model.Email);
                        parameters.AddWithValue("@UserTypeId", model.UserTypeId);
                        parameters.AddWithValue("@Password", passwordHash);
                        parameters.AddWithValue("@SubscribeToNewsletter", model.SubscribeToNewsletter);
                        parameters.Add("@Id", SqlDbType.Int).Direction = ParameterDirection.Output;
                    },
                    returnParameters: (parameters) =>
                    {
                        userId = (int)parameters["@Id"].Value;
                    });
            }
            catch (SqlException ex) when (ex.Number == 2627)
            {
                throw new DuplicateUserException();
            }

            UserAuthData userAuthData = new UserAuthData();
            userAuthData.Id = userId;
            authenticationService.Login(userAuthData, false);

            return userId;
        }
    }
}


