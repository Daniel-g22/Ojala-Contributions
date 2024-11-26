import React, { useState, useEffect } from "react";
import logger from "sabio-debug";
import PropTypes from "prop-types";
import faqsService from "services/faqsService";
import FaqAccordion from "./FaqAccordion";

const _logger = logger.extend("FAQs");

function Faqs(props) {
  const [faqs, setFaqs] = useState({
    faqData: [],
    faqComponents: []
  });

  useEffect(() => {
    _logger(props, "props");
    triggerGetFaqs();
  }, []);

  const onFaqSuccess = (response) => {
    _logger("the response:::", response);

    setFaqs((prevState) => {
      let newObj = { ...prevState };
      newObj.faqData = response.items;
      newObj.faqComponents = response.items.map(mapFaq);

      return newObj;
    });
  };
  const mapFaq = (currentFaq) => {
    return <FaqAccordion key={"ListA-" + currentFaq.id} triggerGetFaqs={triggerGetFaqs} faq={currentFaq} cUserRole={props?.currentUser?.role} />;
  };

  const onFaqErr = (err) => {
    _logger("the error:::", err);
  };
  const triggerGetFaqs = () => {
    faqsService.getAll().then(onFaqSuccess).catch(onFaqErr);
  };

  return (
    <React.Fragment>
      <h1>
        <strong>Frequently Asked Questions</strong>
      </h1>
      <div>
        <div className="row">{faqs.faqComponents}</div>
      </div>
    </React.Fragment>
  );
}

Faqs.propTypes = {
  currentUser: PropTypes.shape({
    role: PropTypes.string
  })
};

export default Faqs;
