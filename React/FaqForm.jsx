import React, { useState, useEffect } from "react";
import { Formik, Form, Field, ErrorMessage } from "formik";
import logger from "sabio-debug";
import faqSchema from "schemas/faqSchema";
import lookUpService from "services/lookUpService";
import { mapLookUpItem } from "helper/utils";
import faqsService from "services/faqsService";
import toastr from "toastr";
import PropTypes from "prop-types";

const _logger = logger.extend("CreateFaq");

const schema = faqSchema;

function FaqForm(props) {
  _logger("props with data from Edit clicked", props);

  const [faq, setFaq] = useState({
    question: "",
    answer: "",
    categoryId: null,
    sortOrder: null,
    id: null
  });

  const [faqCategories, setFaqCategories] = useState("");
  const [title, setTitle] = useState("");

  useEffect(() => {
    if (!!props.faq) {
      _logger("useEffect fired with props from accordion.This is props.faq:::", props.faq);

      setFaq((prevState) => {
        let newObj = { ...prevState };
        newObj.question = props.faq.question;
        newObj.answer = props.faq.answer;
        newObj.categoryId = props.faq.category.id;
        newObj.sortOrder = props.faq.sortOrder;
        newObj.id = props.faq.id;

        return newObj;
      });
      setTitle(() => {
        return "Update";
      });
    } else {
      setTitle(() => {
        return "Create";
      });
    }
  }, [props.faq]);

  _logger("faqState:::", faq);

  useEffect(() => {
    lookUpService.LookUp(["faqcategories"]).then(lookUpSuccess).catch(lookUpError);
  }, []);

  const lookUpSuccess = (response) => {
    _logger("Array of Category Objs:::", response.item.faqcategories);

    setFaqCategories((prevState) => {
      let newObj = [...prevState];
      newObj = response.item.faqcategories.map(mapLookUpItem);

      return newObj;
    });
  };

  const lookUpError = (err) => {
    _logger("lookUpError fired. Error:::", err);
  };

  const submitClicked = (values) => {
    _logger("submitClicked fired, the values:::", values);

    if (!props.faq) {
      faqsService.createFaq(values).then(createFaqSuccess).catch(createFaqError);
    } else if (props.faq) {
      _logger("submitClicked fired. Making PUT call.Values:::", values);
      faqsService.updateFaq(values).then(updateFaqSuccess).catch(updateFaqError);
    }
  };

  const updateFaqSuccess = (response) => {
    _logger("updateFaqSuccess fired. This is response:::", response);
    toastr.success("FAQ updated");
    props.triggerGetFaqs();
  };

  const updateFaqError = (err) => {
    _logger("updateFaqError fired. This is the err:", err);
    toastr.error("Unable to update FAQ");
  };

  const createFaqSuccess = (response) => {
    _logger("createFaqSuccess fired. This is response:::", response);
    toastr.success("FAQ created");
  };

  const createFaqError = (err) => {
    _logger("createFaqError fired. This is the err:", err);
    toastr.error("Unable to create new FAQ");
  };

  return (
    <React.Fragment>
      <div className="container">
        <div className="row">
          <h1> {title} FAQ here:</h1>
        </div>
        <div className="row">
          <div className="col-6">
            <Formik enableReinitialize={true} initialValues={faq} onSubmit={submitClicked} validationSchema={schema}>
              <Form>
                <div className="from-group">
                  <div className="row mt-3">
                    <label htmlFor="question">Question</label>
                    <Field type="text" name="question" />
                    <ErrorMessage name="question" components="div" className="has error"></ErrorMessage>
                  </div>

                  <div className="row mt-3">
                    <label htmlFor="answer">Answer</label>
                    <Field type="text" name="answer" />
                    <ErrorMessage name="answer" components="div" className="has error"></ErrorMessage>
                  </div>

                  <div className="row mt-3">
                    <Field as="select" name="categoryId" className="form-select" aria-label="Default select example">
                      <option value="">FAQ Category Type</option>
                      {faqCategories}
                    </Field>
                  </div>

                  <div className="row mt-3">
                    <label htmlFor="sortOrder">Sort Order</label>
                    <Field type="number" name="sortOrder" />
                    <ErrorMessage name="answer" components="div" className="has error"></ErrorMessage>
                  </div>

                  <button type="submit" className="btn btn-success mt-3">
                    {title}
                  </button>
                </div>
              </Form>
            </Formik>
          </div>
        </div>
      </div>
    </React.Fragment>
  );
}

export default FaqForm;

FaqForm.propTypes = {
  triggerGetFaqs: PropTypes.func,
  faq: PropTypes.shape({
    id: PropTypes.number,
    question: PropTypes.string,
    answer: PropTypes.string,
    sortOrder: PropTypes.number,
    category: PropTypes.number
  })
};
