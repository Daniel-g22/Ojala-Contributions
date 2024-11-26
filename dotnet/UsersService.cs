using Microsoft.AspNetCore.Identity;
using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Domain.ProviderRating;
using Sabio.Models.Domain.Users;
using Sabio.Models.Interfaces;
using Sabio.Models.Requests.ProviderRating;
using Sabio.Models.Requests.UserRequests;
using Sabio.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;



namespace Sabio.Services
{

    public class UsersService : IUsersService
    {

        private IAuthenticationService<int> _authenticationService;
        private IDataProvider _data;
        private ILookUpService _lookUpService;
        private IEmailService _emailService;


        public UsersService(IAuthenticationService<int> authService, IDataProvider data, ILookUpService lookUpService, IEmailService emailService)
        {

            _data = data;
            _lookUpService = lookUpService;
            _emailService = emailService;
            _authenticationService = authService;
        }

        public void AddRating2Provider(ProviderRatingAddRequest model, int consumerId )
        {
            string procName = "[dbo].[ProviderRating_Insert]";

            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection collection)
                {
                    collection.AddWithValue("@ProviderId", model.ProviderId);
                    collection.AddWithValue("@Rating", model.Rating);
                    collection.AddWithValue("@ConsumerId", consumerId);
                }
                );
        }
        public List<ProviderRating> GetProviderRatingsByProvId(int providerId)
        {
            List<ProviderRating> list = null;
            string procName = "[dbo].[ProviderRating_SelectByProviderId]";

            _data.ExecuteCmd(procName,
                inputParamMapper: delegate (SqlParameterCollection collection)
            {
                collection.AddWithValue("@ProviderId", providerId);
            },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    ProviderRating providerRating = MapSingleProviderRatingRecord(reader);

                    if (list == null)
                    {
                        list = new List<ProviderRating>();
                    }

                    list.Add(providerRating);
                }
                );
            return list;
        }

        public List<ProviderRating> GetProviderRatings()
        {
            List<ProviderRating> list = null;
            string procName = "[dbo].[ProviderRating_SelectAll]";

            _data.ExecuteCmd(procName,
                inputParamMapper: null,
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    ProviderRating providerRating = MapSingleProviderRatingRecord(reader);

                    if (list == null)
                    {
                        list = new List<ProviderRating>();
                    }

                    list.Add(providerRating);
                }
                );
            return list;

        }
      
        public List<BasicUser> GetProviderUsers()
        {
            List<BasicUser> list = null;
            string procName = "[dbo].[Users_SelectProviders]";

            _data.ExecuteCmd(procName,
                inputParamMapper: null,
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    BasicUser singleProvider = MapSingleBasicUserRecord(reader);

                    if (list == null)
                    {
                        list = new List<BasicUser>();
                    }

                    list.Add(singleProvider);
                }
                );
            return list;

        }

        public void AddProvider2Consumer(int providerId, int consumerId)
        {
            string procName = "[dbo].[ConsumerProviders_Insert]";

            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection collection)
                {
                    collection.AddWithValue("@ConsumerId", consumerId);
                    collection.AddWithValue("@ProviderId", providerId);
                }
                );
        }

        public List<BasicUser> GetConsumersByProvider(int providerId)
        {
            List<BasicUser> list = null;
            string procName = "[dbo].[SelectByProvider]";

            _data.ExecuteCmd(procName,
                inputParamMapper: delegate (SqlParameterCollection paramCol)
                {
                    paramCol.AddWithValue("@ProviderId", providerId);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    BasicUser singleConsumer = MapSingleBasicUserRecord(reader);

                    if (list == null)
                    {
                        list = new List<BasicUser>();
                    }

                    list.Add(singleConsumer);
                }
                );
            return list;

        }

        public List<BasicUser> GetProviderByConsumer(int consumerId)
        {
            List<BasicUser> list = null;
            string procName = "[dbo].[ConsumerProviders_SelectProviderByConsumer]";

            _data.ExecuteCmd(procName,
                inputParamMapper: delegate (SqlParameterCollection paramCol)
                {
                    paramCol.AddWithValue("@ConsumerId", consumerId);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    BasicUser singleConsumer = MapSingleBasicUserRecord(reader);

                    if (list == null)
                    {
                        list = new List<BasicUser>();
                    }

                    list.Add(singleConsumer);
                }
                );
            return list;

        }

        private BasicUser MapSingleBasicUserRecord(IDataReader reader)
{
    BasicUser user = new BasicUser();
    int startingIndex = 0;

    string userJsonString = "";

    userJsonString = reader.GetString(startingIndex++);
    if (!string.IsNullOrEmpty(userJsonString))
    {
        user = Newtonsoft.Json.JsonConvert.DeserializeObject<BasicUser>(userJsonString);
    }

    return user;
}

private ProviderRating MapSingleProviderRatingRecord(IDataReader reader)
{
    ProviderRating providerRating = new ProviderRating();
    List<Ratings> ratingsList = new List<Ratings>();
    int startingIndex = 0;

    string userJsonString = "";
    userJsonString = reader.GetString(startingIndex++);
    if (!string.IsNullOrEmpty(userJsonString))
    {
        providerRating.Provider = Newtonsoft.Json.JsonConvert.DeserializeObject<BasicUser>(userJsonString);
    }

    string ratingsJsonString = "";
    ratingsJsonString = reader.GetString(startingIndex++);
    if (!string.IsNullOrEmpty(ratingsJsonString))
    {
        ratingsList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Ratings>>(ratingsJsonString);
    }

    providerRating.Ratings = ratingsList;


    return providerRating;
}
}
}
