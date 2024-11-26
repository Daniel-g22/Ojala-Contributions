import React, { useEffect, useState } from "react";
import logger from "sabio-debug";
import usersService from "services/usersService";
import RateProviderCard from "./RateProviderCard";

const _logger = logger.extend("RateProvider");

function RateProvider() {
  const [providers, setProviders] = useState({
    providersData: [],
    providersComponents: []
  });

  useEffect(() => {
    usersService.getProviderRatings().then(onRatingsSuccess).catch(onRatingsError);
  }, []);

  const onRatingsSuccess = (response) => {
    _logger("onRatingsSuccess fired. Response:::", response);

    setProviders((prevState) => {
      let newObj = { ...prevState };
      newObj.providersData = response.items;
      newObj.providersComponents = response.items.map(mapProviders);

      return newObj;
    });
  };

  const mapProviders = (currentProvider) => {
    return <RateProviderCard key={"ListA-" + currentProvider.id} provider={currentProvider} isReadOnly={false} />;
  };

  const onRatingsError = (err) => {
    _logger("onRatingsError fired. Err:::", err);
  };

  return (
    <React.Fragment>
      <div className="row">
        <div className="col-12 text-center">There are a total of {providers.providersData.length} Providers to rate</div>
      </div>
      <div>
        <div className="row mt-5">{providers.providersComponents}</div>
      </div>
    </React.Fragment>
  );
}

export default RateProvider;
