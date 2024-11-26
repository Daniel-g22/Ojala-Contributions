import React, { useState } from "react";
import { Formik, Form, Field } from "formik";
import logger from "sabio-debug";
import toastr from "toastr";
import PropTypes from "prop-types";
import usersService from "services/usersService";

const _logger = logger.extend("ProviderRatingForm");

function ProviderRatingForm(props) {
  _logger("ProviderRatingForm props (providerID)", props);

  const [rating] = useState({
    providerId: props.providerId,
    rating: 0
  });

  const submitClicked = (values) => {
    _logger("submitClicked fired, the values:::", values);
    usersService.addProviderRating(values).then(RatingAddSuccess).catch(RatingAddError);
  };

  const RatingAddSuccess = (response) => {
    _logger("RatingAddSuccess fired. This is response:::", response);
    toastr.success("Rating submitted");
    props.onRatingSubmitClicked();
  };

  const RatingAddError = (err) => {
    _logger("RatingAddError fired. This is the err:", err);
    toastr.error("Unable to add rating");
    props.onRatingSubmitClicked();
  };

  return (
    <React.Fragment>
      <div className="container">
        <div className="row justify-content-center">
          <h1> Provide Rating:</h1>
        </div>

        <Formik enableReinitialize={true} initialValues={rating} onSubmit={submitClicked}>
          <Form>
            <div className="from-group ">
              <div className="row mt-3 justify-content-center">
                <Field as="select" name="rating" className="form-select" aria-label="Default select example">
                  <option value="">Rating</option>
                  <option value="1">★☆☆☆☆ -1</option>
                  <option value="2">★★☆☆☆ -2</option>
                  <option value="3">★★★☆☆ -3</option>
                  <option value="4">★★★★☆ -4</option>
                  <option value="5">★★★★★ -5</option>
                </Field>
              </div>

              <div className="row mt-3 justify-content-center">
                <button type="submit" className="btn btn-success mt-3">
                  Submit Rating
                </button>
              </div>
            </div>
          </Form>
        </Formik>
      </div>
    </React.Fragment>
  );
}

export default ProviderRatingForm;

ProviderRatingForm.propTypes = {
  providerId: PropTypes.number,
  onRatingSubmitClicked: PropTypes.func
};
