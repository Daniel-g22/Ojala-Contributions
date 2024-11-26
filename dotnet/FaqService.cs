using Sabio.Data.Providers;
using Sabio.Models.Domain.Users;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabio.Data;
using System.Data.SqlClient;
using Sabio.Services.Interfaces;
using Sabio.Models.Requests.FAQs;
using Sabio.Models.Domain.FAQs;
using Sabio.Models.Domain;

namespace Sabio.Services
{
    public class FaqService : IFaqService
    {
        IDataProvider _data = null;

        public FaqService(IDataProvider data)
        {
            _data = data;
        }

        public List<FaqModel> GetAllFaqs()
        {
            List<FaqModel> faqsList = null;
            string procName = "[dbo].[FAQs_SelectAll]";


            _data.ExecuteCmd(procName
                , inputParamMapper: null
                , singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    FaqModel singleFaq = new FaqModel();
                    singleFaq = MapSingleFaqRecord(reader);

                    if (faqsList == null)
                    {
                        faqsList = new List<FaqModel>();
                    }

                    faqsList.Add(singleFaq);
                }
                );

            return faqsList;

        }

        public List<FaqModel> GetFaqsByCategoryId(int categoryId)
        {
            List<FaqModel> faqs = null;
            string procName = "[dbo].[FAQs_Select_ByCategoryId]";

            _data.ExecuteCmd(procName
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@CategoryId", categoryId);
                }
                , singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    FaqModel singleFaq = MapSingleFaqRecord(reader);

                    if (faqs == null)
                    {
                        faqs = new List<FaqModel>();
                    }

                    faqs.Add(singleFaq); 
                }
                );
            return faqs;
        }

        public int AddFaq(FaqAddRequest model, int userId)
        {
            int id = 0;
            string procName = "[dbo].[FAQ_Insert]";

            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection collection)
                {
                    SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                    idOut.Direction = ParameterDirection.Output;

                    collection.Add(idOut);
                    AddCommonParams(model, collection);
                    collection.AddWithValue("@CreatedBy", userId);

                },
                returnParameters: delegate (SqlParameterCollection returnCollection)
                {
                    object objectId = returnCollection["@Id"].Value;
                    int.TryParse(objectId.ToString(), out id);
                }
                );

            return id;
        }

        public void UpdateFaq(FaqUpdateRequest model, int userId)
        {
            string procName = "[dbo].[FAQ_Update]";

            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection collection)
                {
                    AddCommonParams(model, collection);
                    collection.AddWithValue("@ModifiedBy", userId);
                    collection.AddWithValue("@Id", model.Id);
                },
                returnParameters: null);
        }

        public void DeleteFaq(int id)
        {
            string procName = "[dbo].[FAQ_Delete_ById]";

            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@Id", id);
                }
                , returnParameters: null);

        }

        private static void AddCommonParams(FaqAddRequest model, SqlParameterCollection collection)
        {
            collection.AddWithValue("@Question", model.Question);
            collection.AddWithValue("@Answer", model.Answer);
            collection.AddWithValue("@CategoryId", model.CategoryId);
            collection.AddWithValue("@SortOrder", model.SortOrder);

        }
          
        private FaqModel MapSingleFaqRecord(IDataReader reader)
        {
            FaqModel faq = new FaqModel();
            BasicUser createdByUser = new BasicUser();
            BasicUser modifiedByUser = new BasicUser();
            LookUp category = new LookUp();
            
            int startingIndex = 0;

            string createdByJsonString = "";
            string modifiedByJsonString = "";

            faq.Id = reader.GetSafeInt32(startingIndex++);
            faq.Question = reader.GetSafeString(startingIndex++);
            faq.Answer = reader.GetSafeString(startingIndex++);
            faq.SortOrder = reader.GetSafeInt32(startingIndex++);
            faq.DateCreated = reader.GetSafeDateTime(startingIndex++);
            faq.DateModified = reader.GetSafeDateTime(startingIndex++);


            createdByJsonString = reader.GetSafeString(startingIndex++);
            if (!string.IsNullOrEmpty(createdByJsonString))
            {
                faq.CreatedBy = Newtonsoft.Json.JsonConvert.DeserializeObject<BasicUser>(createdByJsonString);
            }

            modifiedByJsonString = reader.GetSafeString(startingIndex++);
            if (!string.IsNullOrEmpty(modifiedByJsonString))
            {
                faq.ModifiedBy = Newtonsoft.Json.JsonConvert.DeserializeObject<BasicUser>(modifiedByJsonString);
            }

            category.Id = reader.GetSafeInt32(startingIndex++);
            category.Name= reader.GetSafeString(startingIndex++);
            faq.Category = category;

            return faq;

        }

    }
}
