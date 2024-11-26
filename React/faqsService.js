import axios from "axios";
import { onGlobalSuccess, onGlobalError, API_HOST_PREFIX } from "./serviceHelpers";

var faqsService = {
  endpoint: `${API_HOST_PREFIX}/api/faqs`
};

faqsService.getAll = () => {
  const config = {
    method: "GET",
    url: `${faqsService.endpoint}`,
    crossdomain: true,
    headers: { "Content-Type": "application/json" }
  };

  return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

faqsService.createFaq = (payload) => {
  const config = {
    method: "POST",
    url: `${faqsService.endpoint}`,
    data: payload,
    crossdomain: true,
    headers: { "Content-Type": "application/json" }
  };

  return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

faqsService.updateFaq = (payload) => {
  const config = {
    method: "PUT",
    url: `${faqsService.endpoint}/${payload.id}`,
    data: payload,
    crossdomain: true,
    headers: { "Content-Type": "application/json" }
  };

  return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

export default faqsService;
