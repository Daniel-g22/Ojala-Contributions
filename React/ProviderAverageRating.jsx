import React, { useState, useEffect } from "react";
import logger from "sabio-debug";
import PropTypes from "prop-types";
import { Rating } from "react-simple-star-rating";
import usersService from "services/usersService";
import toastr from "toastr";

const _logger = logger.extend("ProviderAverageRating");

function ProviderAverageRating(props) {
  _logger("ProviderAverageRating props:::", props);

  const [ratings, setRatings] = useState(props.ratings);
  const [avgRating, setAvgRating] = useState(calculateAverageRating(props.ratings));

  useEffect(() => {
    _logger("UseEffect ratings:::", props.ratings);
    const avg = calculateAverageRating(props.ratings);
    _logger("UseEffect avg:::", avg);
    setAvgRating(avg);
  }, [props]);

  function calculateAverageRating(rateCalc) {
    _logger("ratings:::", rateCalc);

    if (!rateCalc || rateCalc.length === 0) {
      return 0;
    }

    let sum = 0;
    for (let i = 0; i < rateCalc.length; i++) {
      sum += rateCalc[i].rating;
    }
    const averageRating = sum / rateCalc.length;

    return averageRating;
  }

  const handleRating = (value, e, index) => {
    if (index) {
      _logger("EVT:::", value, e, index);
      let payload = {
        providerId: props.providerId,
        rating: value
      };
      const RatingAddSuccess = (response) => {
        _logger("RatingAddSuccess fired. This is response:::", response);
        toastr.success("Rating submitted");

        setRatings((prevState) => {
          const newState = [...prevState];
          newState.push({ rating: value });
          _logger("NEWSTATE:::", newState);
          const newAvg = calculateAverageRating(newState);
          setAvgRating(newAvg);
          return newState;
        });
      };
      usersService.addProviderRating(payload).then(RatingAddSuccess).catch(RatingAddError);
    }
  };

  const RatingAddError = (err) => {
    _logger("RatingAddError fired. This is the err:", err);
    toastr.error("Unable to add rating");
  };

  const pointerLeaveHandler = () => {
    setAvgRating((prevState) => {
      let newObj = { ...prevState };
      newObj = calculateAverageRating(ratings);

      return newObj;
    });
  };
  return (
    <React.Fragment>
      <h4>Average Rating</h4>
      <Rating
        initialValue={avgRating}
        onClick={handleRating}
        size={20}
        allowFraction={false}
        readonly={props.isReadOnly}
        onPointerEnter={null}
        onPointerLeave={pointerLeaveHandler}
        onPointerMove={null}
        allowHover={true}
        onMouseLeave={pointerLeaveHandler}
        disableFillHover={false}
      />
      ({avgRating})
    </React.Fragment>
  );
}

ProviderAverageRating.propTypes = {
  providerId: PropTypes.number,
  isReadOnly: PropTypes.bool,
  ratings: PropTypes.arrayOf(
    PropTypes.shape({
      rating: PropTypes.number,
      consumerId: PropTypes.number
    })
  )
};

export default ProviderAverageRating;
