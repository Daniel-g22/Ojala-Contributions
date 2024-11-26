import React, { useState } from "react";
import PropTypes from "prop-types";
import logger from "sabio-debug";
import FaqForm from "./FaqForm";

const _logger = logger.extend("FAQs");

function FaqAccordion(props) {
  _logger("FaqAccordion props:::", props);

  const [isEditClicked, setIsEditClicked] = useState(false);

  const onEditClick = () => {
    _logger("onEditClick fired.");

    setIsEditClicked(true);
  };

  return (
    <React.Fragment>
      <div className="accordion" id="accordionExample">
        <div className="accordion-item">
          <h2 className="accordion-header" id="headingOne">
            <button
              className="accordion-button"
              type="button"
              data-bs-toggle="collapse"
              data-bs-target="#collapseOne"
              aria-expanded="true"
              aria-controls="collapseOne"
            >
              {props.faq.question}
            </button>
          </h2>
          <div id="collapseOne" className="accordion-collapse collapse show" aria-labelledby="headingOne" data-bs-parent="#accordionExample">
            <div className="accordion-body">{props.faq.answer}</div>
          </div>
        </div>
      </div>
      {props?.cUserRole === "Admin" && <button onClick={onEditClick}>Edit</button>}

      {isEditClicked && <FaqForm triggerGetFaqs={props.triggerGetFaqs} faq={props.faq} />}
    </React.Fragment>
  );
}

FaqAccordion.propTypes = {
  triggerGetFaqs: PropTypes.func,
  faq: PropTypes.shape({
    id: PropTypes.number,
    question: PropTypes.string,
    answer: PropTypes.string,
    sortOrder: PropTypes.number
  }),
  cUserRole: PropTypes.string
};

export default FaqAccordion;
