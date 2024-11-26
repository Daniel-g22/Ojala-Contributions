using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using Sabio.Models;
using Sabio.Models.Domain.FAQs;
using Sabio.Models.Domain.ProviderRating;
using Sabio.Models.Domain.Users;
using Sabio.Models.Interfaces;
using Sabio.Models.Requests.ProviderRating;
using Sabio.Models.Requests.UserRequests;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Core;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersApiController : BaseApiController
    {
        private IUsersService _service = null;
        private ILogger _logger = null;
        private IEmailService _emailService;
        private IAuthenticationService<int> _authService;

        IOptions<SecurityConfig> _options;

        public UsersApiController( IEmailService emailService, IUsersService service, ILogger<UsersApiController> logger, IOptions<SecurityConfig> options, IAuthenticationService<int> authService) : base(logger)
        {
            _logger = logger;
            _service = service;
            _emailService = emailService;
            _options = options;
            _authService = authService;

        }

        [HttpPost("rating2provider")]
        public ActionResult<SuccessResponse> CreateProviderRating (ProviderRatingAddRequest model)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                int consumerId = _authService.GetCurrentUserId();
                _service.AddRating2Provider(model, consumerId);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);

            }
            return StatusCode(code, response);
        }

        [HttpGet("providerratingsbyid")]
        public ActionResult<ItemsResponse<ProviderRating>> ProviderRatingsByProvId()
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                int providerId = _authService.GetCurrentUserId();
                List<ProviderRating> providerRatings = _service.GetProviderRatingsByProvId(providerId);

                if (providerRatings == null)
                {
                    code = 404;
                    response = new ErrorResponse("Application resource not found.");
                }
                else
                {
                    response = new ItemsResponse<ProviderRating> { Items = providerRatings };
                }
            }
            catch (SqlException sqlEx)
            {
                code = 500;
                response = new ErrorResponse(sqlEx.Message);
                base.Logger.LogError(sqlEx.ToString());
            }
            catch (ArgumentException argEx)
            {
                code = 500;
                response = new ErrorResponse(argEx.Message);
            }
            catch (Exception ex)
            {
                code = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }

        [HttpGet("providerratings")]
        public ActionResult<ItemsResponse<ProviderRating>> ProviderRatings()
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                List<ProviderRating> providerRatings = _service.GetProviderRatings();

                if (providerRatings == null)
                {
                    code = 404;
                    response = new ErrorResponse("Application resource not found.");
                }
                else
                {
                    response = new ItemsResponse<ProviderRating> { Items = providerRatings };
                }
            }
            catch (SqlException sqlEx)
            {
                code = 500;
                response = new ErrorResponse(sqlEx.Message);
                base.Logger.LogError(sqlEx.ToString());
            }
            catch (ArgumentException argEx)
            {
                code = 500;
                response = new ErrorResponse(argEx.Message);
            }
            catch (Exception ex)
            {
                code = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }

        [HttpGet("providers")]
        public ActionResult<ItemsResponse<BasicUser>> GetProviders()
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                List<BasicUser> providers = _service.GetProviderUsers();

                if (providers == null)
                {
                    code = 404;
                    response = new ErrorResponse("Application resource not found.");
                }
                else
                {
                    response = new ItemsResponse<BasicUser> { Items = providers };
                }
            }
            catch (SqlException sqlEx)
            {
                code = 500;
                response = new ErrorResponse(sqlEx.Message);
                base.Logger.LogError(sqlEx.ToString());
            }
            catch (ArgumentException argEx)
            {
                code = 500;
                response = new ErrorResponse(argEx.Message);
            }
            catch (Exception ex)
            {
                code = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }

        [HttpGet("provider2consumer/{providerId:int}")]
        public ActionResult<SuccessResponse> CreateConsumerProvider ( int providerId)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                int consumerId = _authService.GetCurrentUserId();
                _service.AddProvider2Consumer( providerId, consumerId);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);

            }
            return StatusCode(code, response);
        }

        [HttpGet("consumersprovider")]
        public ActionResult<ItemsResponse<BasicUser>> GetConsumerByProvider()
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                int userId = _authService.GetCurrentUserId();
                List<BasicUser> consumers = _service.GetConsumersByProvider(userId);

                if (consumers == null)
                {
                    code = 404;
                    response = new ErrorResponse("Application resource not found.");
                }
                else
                {
                    response = new ItemsResponse<BasicUser> { Items = consumers };
                }
            }
            catch (SqlException sqlEx)
            {
                code = 500;
                response = new ErrorResponse(sqlEx.Message);
                base.Logger.LogError(sqlEx.ToString());
            }
            catch (ArgumentException argEx)
            {
                code = 500;
                response = new ErrorResponse(argEx.Message);
            }
            catch (Exception ex)
            {
                code = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }

        [HttpGet("providersconsumer")]
        public ActionResult<ItemsResponse<BasicUser>> GetProvidersByConsumer()
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                int userId = _authService.GetCurrentUserId();
                List<BasicUser> consumers = _service.GetProviderByConsumer(userId);

                if (consumers == null)
                {
                    code = 404;
                    response = new ErrorResponse("Application resource not found.");
                }
                else
                {
                    response = new ItemsResponse<BasicUser> { Items = consumers };
                }
            }
            catch (SqlException sqlEx)
            {
                code = 500;
                response = new ErrorResponse(sqlEx.Message);
                base.Logger.LogError(sqlEx.ToString());
            }
            catch (ArgumentException argEx)
            {
                code = 500;
                response = new ErrorResponse(argEx.Message);
            }
            catch (Exception ex)
            {
                code = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }
}
}
