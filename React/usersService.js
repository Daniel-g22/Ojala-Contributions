import axios from "axios";
import { onGlobalSuccess, onGlobalError, API_HOST_PREFIX } from "./serviceHelpers";

var usersService = {
  endpoint: `${API_HOST_PREFIX}/api/users`
};

usersService.getProviderRatings = () => {
  const config = {
    method: "GET",
    url: `${usersService.endpoint}/providerratings`,
    crossdomain: true,
    headers: { "Content-Type": "application/json" }
  };

  return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

usersService.getRatingsByProvId = () => {
  const config = {
    method: "GET",
    url: `${usersService.endpoint}/providerratingsbyid/`,
    crossdomain: true,
    headers: { "Content-Type": "application/json" }
  };

  return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

usersService.addProviderRating = (payload) => {
  const config = {
    method: "POST",
    url: `${usersService.endpoint}/rating2provider`,
    data: payload,
    crossdomain: true,
    headers: { "Content-Type": "application/json" }
  };

  return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

