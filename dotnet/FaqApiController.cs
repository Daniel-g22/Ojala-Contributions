using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using Sabio.Models.Requests.UserRequests;
using Sabio.Models;
using Sabio.Models.Requests.FAQs;
using Sabio.Models.Domain.FAQs;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/faqs")]
    [ApiController]
    public class FaqApiController : BaseApiController
    {

        ITestService _testService;
        IFaqService _service = null;
        private IAuthenticationService<int> _authService = null;

        public FaqApiController(ITestService service2, IFaqService service,
            ILogger<FileApiController> logger,
            IAuthenticationService<int> authService) : base(logger)
        {
            _testService = service2;
            _service = service;
            _authService = authService;
        }

        [HttpGet]
        public ActionResult<ItemsResponse<FaqModel>> GetAll()
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                List<FaqModel> faqs = _service.GetAllFaqs();

                if (faqs == null)
                {
                    code = 404;
                    response = new ErrorResponse("Application resource not found.");
                }
                else
                {
                    response = new ItemsResponse<FaqModel> { Items = faqs };
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

        [HttpGet("{categoryId:int}")]
        public ActionResult<ItemsResponse<FaqModel>> GetByCategoryId (int categoryId)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                List<FaqModel> faqs = _service.GetFaqsByCategoryId(categoryId);

                if (faqs == null)
                {
                    code = 404;
                    response = new ErrorResponse("Application resource not found.");
                }
                else
                {
                    response = new ItemsResponse<FaqModel> { Items = faqs };
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

        [HttpPost]
        public ActionResult<ItemResponse<int>> Create(FaqAddRequest model)
        {
            ObjectResult result = null;

            try
            {
                int userId = _authService.GetCurrentUserId();

                int id = _service.AddFaq(model, userId);
                ItemResponse<int> response = new ItemResponse<int>() { Item = id };


                result = Created201(response);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex.ToString());
                ErrorResponse response = new ErrorResponse(ex.Message);

                result = StatusCode(500, response);
            }
            return result;
        }

        [HttpPut("{id:int}")]
        public ActionResult<SuccessResponse> Update(FaqUpdateRequest model, int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                int userId = _authService.GetCurrentUserId();
                
                _service.UpdateFaq(model, userId);

                response = new SuccessResponse();
            }
            catch (Exception ex) {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);

        }

        [HttpDelete("{id:int}")]
        public ActionResult<SuccessResponse> Delete(int id)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                _service.DeleteFaq(id);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);

            }
            return StatusCode(code, response);
        }

    }
}
