import React, { useEffect, useState } from "react";
import logger from "sabio-debug";
import usersService from "services/usersService";
import toastr from "toastr";
import PropTypes from "prop-types";
import ProviderAverageRating from "./ProviderAverageRating";

const _logger = logger.extend("ProviderRatings");

function ProviderRatings(props) {
  const [ratings, setRatings] = useState([]);

  useEffect(() => {
    usersService.getRatingsByProvId().then(onSuccess).catch(onError);
  }, []);

  const onSuccess = (response) => {
    _logger("onSuccess fired. Response:::", response);
    toastr.success(`You have a total of ${response.items[0].ratings.length} Ratings`);

    setRatings((prevState) => {
      let newObj = { ...prevState };
      newObj = response.items[0].ratings;

      return newObj;
    });
  };

  useEffect(() => {
    _logger("new state:::", ratings);
  }, [ratings]);

  const onError = (err) => {
    _logger("onError fired. Err:::", err);
    toastr.error("Unable to retrieve Providers to rate");
  };

  return (
    <React.Fragment>
      <div className="d-flex flex-column align-items-center">
        <h1>Welcome {`${props.currentUser.firstName} ${props.currentUser.lastName}`}</h1>
        <img src={props.currentUser.avatarUrl} className="" alt="..." />
        <h2 className="mt-3">You have a total of {ratings.length} ratings </h2>

        <ProviderAverageRating isReadOnly={true} ratings={ratings}></ProviderAverageRating>
      </div>
    </React.Fragment>
  );
}

ProviderRatings.propTypes = {
  currentUser: PropTypes.shape({
    avatarUrl: PropTypes.string,
    role: PropTypes.string,
    firstName: PropTypes.string,
    lastName: PropTypes.string
  })
};

export default ProviderRatings;
