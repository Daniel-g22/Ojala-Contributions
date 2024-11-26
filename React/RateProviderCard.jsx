import React from "react";
import PropTypes from "prop-types";
import logger from "sabio-debug";
import ProviderAverageRating from "./ProviderAverageRating";

const _logger = logger.extend("RateProviderCard");

function RateProviderCard(props) {
  _logger("RateProviderCard props:::", props);

  const RatingSubmitClicked = () => {
    _logger("RatingSubmitClicked fired.");
  };
  return (
    <React.Fragment>
      <div className="card" style={{ width: "18rem" }}>
        <img src={props.provider.provider.avatarUrl} className="card-img-top" alt="..." />
        <div className="card-body d-flex flex-column align-items-center">
          <h3 className="card-title">{`${props.provider.provider.firstName} ${props.provider.provider.lastName}`}</h3>
          <ProviderAverageRating
            ratings={props.provider.ratings}
            providerId={props.provider.provider.id}
            onRatingSubmitClicked={RatingSubmitClicked}
          ></ProviderAverageRating>
        </div>
      </div>
    </React.Fragment>
  );
}

RateProviderCard.propTypes = {
  provider: PropTypes.shape({
    provider: PropTypes.shape({
      id: PropTypes.number,
      firstName: PropTypes.string,
      lastName: PropTypes.string,
      avatarUrl: PropTypes.string
    }),
    ratings: PropTypes.arrayOf(
      PropTypes.shape({
        rating: PropTypes.number,
        consumerId: PropTypes.number
      })
    )
  })
};

export default RateProviderCard;
