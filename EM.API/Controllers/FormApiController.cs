#region Using
using ElmahCore;
using EM.API.Helpers;
using EM.Common;
using EM.Entity;
using EM.Models;
using EM.Services;
using EM.Services.WebForms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Net;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
#endregion

namespace EM.API.Controllers
{
    /// <summary>
    /// All form related api controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FormApiController : ControllerBase
    {
        /// <summary>
        /// Generate fields for IUsersService, IHostingEnvironment, HttpCacheability,IConfiguration
        /// </summary>
        #region Fields
        private IUsersService _userService;
        private IFormServices _formServices;
        private IConfiguration _configuration;
        [Obsolete]
        private readonly IHostingEnvironment _hostingEnvironment;
        public object HttpCacheability { get; private set; }
        #endregion

        /// <summary>
        /// Constructors for AdminApiController.
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="hostingEnvironment"></param>
        /// <param name="configuration"></param>
        #region Constructors
        [Obsolete]
        public FormApiController(IUsersService userService, IHostingEnvironment hostingEnvironment, IConfiguration configuration, IFormServices formServices)
        {
            _hostingEnvironment = hostingEnvironment;
            _userService = userService;
            _formServices = formServices;
            _configuration = configuration;
        }
        #endregion

        /// <summary>
        /// Get all forms 
        /// </summary>
        /// <returns>get all forms in list format</returns>
        #region GetAllForms(GET)
        [HttpGet("GetAllForms")]
        [Authorize(Roles = "Admin")]
        public ApiResponseModel GetAllForms()
        {
            HttpContext.RaiseError(new InvalidOperationException("GetAllForms"));
            try
            {
                var formDetails = _formServices.GetAllForms();
                if (formDetails != null)
                {
                    //getting value from common helper.
                    return CommonHelper.GetResponse(HttpStatusCode.OK, "", null, formDetails);
                }
                return CommonHelper.GetResponse(HttpStatusCode.BadRequest, "", "");
            }
            catch
            {
                return CommonHelper.GetResponse(HttpStatusCode.InternalServerError, "", "");
            }
        }
        #endregion

        /// <summary>
        /// Method for getting create form model
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Add/edit form model</returns>
        #region OpenCreateForm(GET)
        [HttpGet("OpenCreateForm/{id}")]
        [Authorize(Roles = "Admin")]
        public ApiResponseModel OpenCreateForm(int id)
        {
            HttpContext.RaiseError(new InvalidOperationException("OpenCreateForm"));
            try
            {
                var formDetails = _formServices.GetById(id);
                if (formDetails != null)
                {
                    //getting value from common helper.
                    return CommonHelper.GetResponse(HttpStatusCode.OK, "", formDetails);
                }
                return CommonHelper.GetResponse(HttpStatusCode.BadRequest, "", "");
            }
            catch
            {
                return CommonHelper.GetResponse(HttpStatusCode.InternalServerError, "", "");
            }
        }
        #endregion

        /// <summary>
        ///  Create Form is for creating new form 
        /// </summary>
        /// <param name="objForms"></param>
        /// <returns>Create new form</returns>
        #region AddCreateForm(POST)
        [HttpPost("CreateForm")]
        [Authorize(Roles = "Admin")]
        public ApiResponseModel CreateForm(Forms objForms)
        {
            HttpContext.RaiseError(new InvalidOperationException("CreateForm"));
            try
            {
                if (objForms != null)
                {
                    var createdForms = _formServices.CreateForm(objForms);
                    if (createdForms != null && objForms.FormId != 0)
                    {
                        //getting value from common helper.
                        return CommonHelper.GetResponse(HttpStatusCode.OK, CommonValidations.NewFormCreated, createdForms);
                    }
                }
                return CommonHelper.GetResponse(HttpStatusCode.BadRequest, CommonValidations.RecordExistsMsg, "");
            }
            catch
            {
                return CommonHelper.GetResponse(HttpStatusCode.InternalServerError, CommonValidations.InvalidMsg, "");
            }
        }
        #endregion

        /// <summary>
        ///  Create Form is for creating new form 
        /// </summary>
        /// <param name="objForms"></param>
        /// <returns>Create new form</returns>
        #region EditCreateForm(POST)
        [HttpPut("EditForm")]
        [Authorize(Roles = "Admin")]
        public ApiResponseModel EditForm(Forms objForms)
        {
            HttpContext.RaiseError(new InvalidOperationException("EditForm"));
            try
            {
                if (objForms != null)
                {
                    var editForms = _formServices.EditForms(objForms);
                    //getting value from common helper.
                    return CommonHelper.GetResponse(HttpStatusCode.OK, CommonValidations.UpdateFormDetails, editForms);
                }
                else
                {
                    return CommonHelper.GetResponse(HttpStatusCode.BadRequest, CommonValidations.RecordExistsMsg);
                }
            }
            catch
            {
                return CommonHelper.GetResponse(HttpStatusCode.BadRequest, CommonValidations.InvalidMsg);
            }
        }
        #endregion

        /// <summary>
        /// Search forms 
        /// </summary>
        /// <returns>filtered data</returns>
        #region SearchForms(POST)
        [HttpPost("SearchForms")]
        [Authorize(Roles = "Admin")]
        public ApiResponseModel SearchForms(SearchForms objForms)
        {
            HttpContext.RaiseError(new InvalidOperationException("SearchForms"));
            try
            {
                var filteredFormDetails = _formServices.SearchForms(objForms);
                if (filteredFormDetails != null)
                {
                    //getting value from common helper.
                    return CommonHelper.GetResponse(HttpStatusCode.OK, "", null, filteredFormDetails);
                }
                return CommonHelper.GetResponse(HttpStatusCode.BadRequest, "", "");
            }
            catch
            {
                return CommonHelper.GetResponse(HttpStatusCode.InternalServerError, "", "");
            }
        }
        #endregion

        ////////////////////////////////////////////////////////Manage Fields////////////////////////////////////////////

        /// <summary>
        /// Method for getting form model for add and edit
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Add/edit new field model</returns>
        #region Open Add/edit FieldForm(GET)
        [HttpGet("OpenFieldForm/{id}")]
        [Authorize(Roles = "Admin")]
        public ApiResponseModel OpenFieldForm(int id)
        {
            HttpContext.RaiseError(new InvalidOperationException("OpenFieldForm"));
            try
            {
                var formDetails = _formServices.GetByFieldId(id);
                if (formDetails != null)
                {
                    //getting value from common helper.
                    return CommonHelper.GetResponse(HttpStatusCode.OK, "", formDetails);
                }
                return CommonHelper.GetResponse(HttpStatusCode.BadRequest, "", "");
            }
            catch
            {
                return CommonHelper.GetResponse(HttpStatusCode.InternalServerError, "", "");
            }
        }
        #endregion

        /// <summary>
        ///  Add new field for form 
        /// </summary>
        /// <param name="objFieldDetails"></param>
        /// <returns>Add/Edit new field</returns>
        #region AddFields(POST)
        [HttpPost("AddFields")]
        [Authorize(Roles = "Admin")]
        public ApiResponseModel AddFields(FieldDetails objFieldDetails)
        {
            HttpContext.RaiseError(new InvalidOperationException("AddFields"));
            try
            {
                if (objFieldDetails != null)
                {
                    var createdForms = _formServices.AddFields(objFieldDetails);
                    if (createdForms != null && objFieldDetails.FieldDetailsId != 0)
                    {
                        //getting value from common helper.
                        return CommonHelper.GetResponse(HttpStatusCode.OK, CommonValidations.NewFieldCreated, createdForms);
                    }
                }
                return CommonHelper.GetResponse(HttpStatusCode.BadRequest, CommonValidations.RecordExistsMsg, "");
            }
            catch
            {
                return CommonHelper.GetResponse(HttpStatusCode.InternalServerError, CommonValidations.InvalidMsg, "");
            }
        }
        #endregion

        /// <summary>
        ///  Add new field for form 
        /// </summary>
        /// <param name="objFieldDetails"></param>
        /// <returns>Add/Edit new field</returns>
        #region EditFields(POST)
        [HttpPut("EditField")]
        [Authorize(Roles = "Admin")]
        public ApiResponseModel EditField(FieldDetails objFieldDetails)
        {
            HttpContext.RaiseError(new InvalidOperationException("EditField"));
            try
            {
                if (objFieldDetails != null)
                {
                    var editFields = _formServices.EditFields(objFieldDetails);
                    //getting value from common helper.
                    return CommonHelper.GetResponse(HttpStatusCode.OK, CommonValidations.UpdateFieldDetails, editFields);
                }
                else
                {
                    return CommonHelper.GetResponse(HttpStatusCode.BadRequest, CommonValidations.RecordExistsMsg);
                }
            }
            catch
            {
                return CommonHelper.GetResponse(HttpStatusCode.BadRequest, CommonValidations.InvalidMsg);
            }
        }
        #endregion

        /// <summary>
        /// Getting all field list from this method.
        /// </summary>
        /// <returns>all list of fields</returns>
        #region GetFieldList(POST)
        [HttpPost("GetFieldList/{id}")]
        [Authorize(Roles = "Admin, Users")]
        public ApiResponseModel GetFieldList(JqueryDatatableParam jqueryDatatableParam, int id)
        {
            HttpContext.RaiseError(new InvalidOperationException("GetFieldList"));
            try
            {
                int totalRecord = 0;
                int filterRecord = 0;

                var data = _formServices.GetAllFieldsList(id);
                if (data != null)
                {
                    // get total count of records 
                    totalRecord = data.Count();

                    // search data when search value found
                    if (!string.IsNullOrEmpty(jqueryDatatableParam.searchValue))
                    {
                        data = data.Where(x =>
                          x.FieldHtmlName.ToLower().Contains(jqueryDatatableParam.searchValue.ToLower()));
                    }
                    // get total count of records after search 
                    filterRecord = data.Count();
                    //sort data
                    if (!string.IsNullOrEmpty(jqueryDatatableParam.sortColumn) && !string.IsNullOrEmpty(jqueryDatatableParam.sortColumnDirection))
                        data = data.AsQueryable().OrderBy(jqueryDatatableParam.sortColumn + " " + jqueryDatatableParam.sortColumnDirection);

                    //pagination
                    var empList = data.Skip(jqueryDatatableParam.skip).Take(jqueryDatatableParam.pageSize).ToList();
                    return CommonHelper.GetResponseDataTable(jqueryDatatableParam.draw, totalRecord, filterRecord, empList);
                }
                //getting value from common helper.
                return CommonHelper.GetResponse(HttpStatusCode.BadRequest, "", "");
            }

            catch (Exception)
            {
                return CommonHelper.GetResponse(HttpStatusCode.InternalServerError, "", "");
            }
        }
        #endregion

        ////////////////////////////////////////////////////////Field Options////////////////////////////////////////////

        /// <summary>
        /// Get all field options
        /// </summary>
        /// <param name="id"></param>
        /// <returns>list of field options</returns>
        #region GetFieldOptions(GET)
        [HttpGet("GetFieldOptions/{id}")]
        [Authorize(Roles = "Admin")]
        public ApiResponseModel GetFieldOptions(int id)
        {
            HttpContext.RaiseError(new InvalidOperationException("GetFieldOptions"));
            try
            {
                var getFieldOptions = _formServices.GetFilteredFieldOptions(id);
                //getting value from common helper.
                return CommonHelper.GetResponse(HttpStatusCode.OK, "", "", getFieldOptions);
            }
            catch
            {
                return CommonHelper.GetResponse(HttpStatusCode.BadRequest, "");
            }
        }
        #endregion

        /// <summary>
        /// Method for getting field options model for add and edit
        /// </summary>
        /// <param name="id"></param>
        /// <returns>open field options model</returns>
        #region OpenEditFieldOption(GET)
        [HttpGet("OpenEditFieldOption/{id}")]
        [Authorize(Roles = "Admin")]
        public ApiResponseModel OpenEditFieldOption(int id)
        {
            HttpContext.RaiseError(new InvalidOperationException("OpenFieldOption"));
            try
            {
                var formDetails = _formServices.GetByOptionId(id);
                if (formDetails != null)
                {
                    //getting value from common helper.
                    return CommonHelper.GetResponse(HttpStatusCode.OK, "", formDetails);
                }
                return CommonHelper.GetResponse(HttpStatusCode.BadRequest, "", "");
            }
            catch
            {
                return CommonHelper.GetResponse(HttpStatusCode.InternalServerError, "", "");
            }
        }
        #endregion

        /// <summary>
        /// Method for getting field options model for add and edit
        /// </summary>
        /// <param name="id"></param>
        /// <returns>open field options model</returns>
        #region OpenFieldOptions(GET)
        [HttpPut("OpenFieldOptions")]
        [Authorize(Roles = "Admin")]
        public ApiResponseModel OpenFieldOptions(FieldOptions objFieldOptions)
        {
            HttpContext.RaiseError(new InvalidOperationException("OpenFieldOptions"));
            try
            {
                if (objFieldOptions != null)
                {
                    var openFieldOption = _formServices.EditFieldOptions(objFieldOptions);
                    //getting value from common helper.
                    return CommonHelper.GetResponse(HttpStatusCode.OK, CommonValidations.UpdateFieldDetails, openFieldOption);
                }
                else
                {
                    return CommonHelper.GetResponse(HttpStatusCode.BadRequest, CommonValidations.RecordExistsMsg);
                }
            }
            catch
            {
                return CommonHelper.GetResponse(HttpStatusCode.BadRequest, CommonValidations.InvalidMsg);
            }
        }
        #endregion

        /// <summary>
        ///  Add new field option for form 
        /// </summary>
        /// <param name="objFieldOptions"></param>
        /// <returns>Add new field options</returns>
        #region AddFieldOptions(POST)
        [HttpPost("AddOptionFields")]
        [Authorize(Roles = "Admin")]
        public ApiResponseModel AddOptionFields(FieldOptions objFieldOptions)
        {
            HttpContext.RaiseError(new InvalidOperationException("AddOptionFields"));
            try
            {
                if (objFieldOptions != null)
                {
                    var addFieldOptions = _formServices.AddOptionFields(objFieldOptions);
                    if (addFieldOptions != null && objFieldOptions.FieldOptionsId != 0)
                    {
                        //getting value from common helper.
                        return CommonHelper.GetResponse(HttpStatusCode.OK, CommonValidations.NewFieldCreated, addFieldOptions);
                    }
                }
                return CommonHelper.GetResponse(HttpStatusCode.BadRequest, CommonValidations.RecordExistsMsg, "");
            }
            catch
            {
                return CommonHelper.GetResponse(HttpStatusCode.InternalServerError, CommonValidations.InvalidMsg, "");
            }
        }
        #endregion

        /// <summary>
        ///  Remove field option for form 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Remove field options</returns>
        #region RemoveFieldOption(POST)
        [HttpDelete("RemoveFieldOption/{id}")]
        [Authorize(Roles = "Admin")]
        public ApiResponseModel RemoveFieldOption(int id)
        {
            HttpContext.RaiseError(new InvalidOperationException("RemoveFieldOption"));
            try
            {
                var deleteUser = _formServices.RemoveFieldOption(id);
                //getting value from common helper.
                return CommonHelper.GetResponse(HttpStatusCode.OK, CommonValidations.DeleteUserDetails, deleteUser);
            }
            catch
            {
                return CommonHelper.GetResponse(HttpStatusCode.InternalServerError, CommonValidations.RecordNotExistsMsg, "");
            }
        }
        #endregion

        ////////////////////////////////////////////////////////Field Sequence///////////////////////////////////////

        /// <summary>
        /// Get form for field sequence
        /// </summary>
        /// <returns>get form in list format</returns>
        #region FieldSequenceFormCard(GET)
        [HttpGet("FieldSequenceFormCard/{Id}")]
        [Authorize(Roles = "Admin")]
        public ApiResponseModel FieldSequenceFormCard(int Id)
        {
            HttpContext.RaiseError(new InvalidOperationException("FieldSequenceFormCard"));
            try
            {
                var formDetails = _formServices.GetFormCardById(Id);
                if (formDetails != null)
                {
                    //getting value from common helper.
                    return CommonHelper.GetResponse(HttpStatusCode.OK, "", null, formDetails);
                }
                return CommonHelper.GetResponse(HttpStatusCode.BadRequest, "", "");
            }
            catch
            {
                return CommonHelper.GetResponse(HttpStatusCode.InternalServerError, "", "");
            }
        }
        #endregion

        /// <summary>
        /// Get fields for field sequence
        /// </summary>
        /// <returns>get field in list format</returns>
        #region FieldSequenceFieldList(GET)
        [HttpGet("FieldSequenceFieldList/{Id}")]
        [Authorize(Roles = "Admin")]
        public ApiResponseModel FieldSequenceFieldList(int Id)
        {
            HttpContext.RaiseError(new InvalidOperationException("FieldSequenceFieldList"));
            try
            {
                //var fieldDetails = _formServices.GetAllFieldsList(Id);
                var fieldDetails = _formServices.GetAllFilteredFieldsList(Id);
                if (fieldDetails != null)
                {
                    //getting value from common helper.
                    return CommonHelper.GetResponse(HttpStatusCode.OK, "", null, fieldDetails);
                }
                return CommonHelper.GetResponse(HttpStatusCode.BadRequest, "", "");
            }
            catch
            {
                return CommonHelper.GetResponse(HttpStatusCode.InternalServerError, "", "");
            }
        }
        #endregion

        /// <summary>
        /// Get fields for saved field sequence
        /// </summary>
        /// <returns>get saved field in list format</returns>
        #region FieldSequenceSavedFieldList(GET)
        [HttpGet("FieldSequenceSavedFieldList/{Id}")]
        [Authorize(Roles = "Admin")]
        public ApiResponseModel FieldSequenceSavedFieldList(int Id)
        {
            HttpContext.RaiseError(new InvalidOperationException("FieldSequenceSavedFieldList"));
            try
            {
                var fieldDetails = _formServices.GetSavedFormField(Id);
                if (fieldDetails != null)
                {
                    //getting value from common helper.
                    return CommonHelper.GetResponse(HttpStatusCode.OK, "", null, fieldDetails);
                }
                return CommonHelper.GetResponse(HttpStatusCode.BadRequest, "", "");
            }
            catch
            {
                return CommonHelper.GetResponse(HttpStatusCode.InternalServerError, "", "");
            }
        }
        #endregion

        /// <summary>
        ///  Save form for customer
        /// </summary>
        /// <param name="objFieldOptions"></param>
        /// <returns>Saved form</returns>
        #region SaveForm(POST)
        [HttpPost("SaveForm")]
        [Authorize(Roles = "Admin")]
        public ApiResponseModel SaveForm(FormFields objFormFields)
        {
            HttpContext.RaiseError(new InvalidOperationException("SaveForm"));
            try
            {
                if (objFormFields != null)
                {
                    var createdForms = _formServices.SaveForm(objFormFields);
                    if (createdForms != null)
                    {
                        //getting value from common helper.
                        return CommonHelper.GetResponse(HttpStatusCode.OK, CommonValidations.FormSavedSuccessfully, createdForms);
                    }
                }
                return CommonHelper.GetResponse(HttpStatusCode.BadRequest, CommonValidations.RecordExistsMsg, "");
            }
            catch
            {
                return CommonHelper.GetResponse(HttpStatusCode.InternalServerError, CommonValidations.InvalidMsg, "");
            }
        }
        #endregion

        //////////////////////////////////////////New Customer Forms////////////////////////////////////////

        /// <summary>
        /// Getting all form list from this method.
        /// </summary>
        /// <returns>all list of forms</returns>
        #region GetAllFormsDataForNewCustomers(POST)
        [HttpPost("GetAllFormsData")]
        [Authorize(Roles = "Admin, Users")]
        public ApiResponseModel GetAllFormsData(JqueryDatatableParam jqueryDatatableParam, int id)
        {
            HttpContext.RaiseError(new InvalidOperationException("GetAllFormsData"));
            try
            {
                int totalRecord = 0;
                int filterRecord = 0;

                var data = _formServices.GetAllForms();
                if (data != null)
                {
                    // get total count of records 
                    totalRecord = data.Count();

                    // search data when search value found
                    if (!string.IsNullOrEmpty(jqueryDatatableParam.searchValue))
                    {
                        data = data.Where(x =>
                          x.FormName.ToLower().Contains(jqueryDatatableParam.searchValue.ToLower()));
                    }
                    // get total count of records after search 
                    filterRecord = data.Count();
                    //sort data
                    if (!string.IsNullOrEmpty(jqueryDatatableParam.sortColumn) && !string.IsNullOrEmpty(jqueryDatatableParam.sortColumnDirection))
                        data = data.AsQueryable().OrderBy(jqueryDatatableParam.sortColumn + " " + jqueryDatatableParam.sortColumnDirection);

                    //pagination
                    var empList = data.Skip(jqueryDatatableParam.skip).Take(jqueryDatatableParam.pageSize).ToList();
                    return CommonHelper.GetResponseDataTable(jqueryDatatableParam.draw, totalRecord, filterRecord, empList);
                }
                //getting value from common helper.
                return CommonHelper.GetResponse(HttpStatusCode.BadRequest, "", "");
            }

            catch (Exception)
            {
                return CommonHelper.GetResponse(HttpStatusCode.InternalServerError, "", "");
            }
        }
        #endregion

        /// <summary>
        /// Getting all field list from this method.
        /// </summary>
        /// <returns>all list of fields</returns>
        #region SendMail(POST)
        [HttpPost("SendMail")]
        [Authorize(Roles = "Admin, Users")]
        public ApiResponseModel SendMail(CustomerForms objCustomerForms)
        {
            HttpContext.RaiseError(new InvalidOperationException("SendMail"));
            try
            {
                //var uriBuilder = new UriBuilder
                //{
                //    Scheme = Request.Scheme,
                //    Host = Request.Host.Host,
                //    Port = 7399, //bydefault -1
                //    Path = $"/Form/FormTemplates"
                //};
                var formId = EncryptionDecryption.Encrypt(objCustomerForms.FormId.ToString());
                var email = EncryptionDecryption.Encrypt(objCustomerForms.CustomerEmail.ToString());

                var customerFormDetails = _formServices.SaveCustomerForm(objCustomerForms);
                var customerFormID = EncryptionDecryption.Encrypt(objCustomerForms.CustomerFormsId.ToString());
                var link = "http://localhost:7399/Form/ClientForm?formId=" + formId + "&email="+ email + "&cfi=" + customerFormID;
                var subject = "Sent Form to customer";
                var body = "Hi , <br/> " +
                               "Please click the button to fill out detail in the form" + "<br/> <br/>" +
                               "<a href='"+ link + "'>Open Form</a>  <br/> <br/>" +
                               "If you did not request for form please ignore this mail.";
                MailService.SendEmail(objCustomerForms.CustomerEmail, body, subject);
                return CommonHelper.GetResponse(HttpStatusCode.OK, CommonValidations.EmailSentSuccessfully, "");
            }

            catch (Exception)
            {
                return CommonHelper.GetResponse(HttpStatusCode.InternalServerError, "", "");
            }
        }
        #endregion

        //////////////////////////////////////////Send forms in mails to customer ////////////////////////////////////////

        /// <summary>
        /// Get all Client Side Form Details
        /// </summary>
        /// <returns>list of Client Side Form Details </returns>
        #region ViewClientSideFormDetails(GET)
        [HttpGet("ViewClientSideFormDetails/{Id}")]
        public ApiResponseModel ViewClientSideFormDetails(int id)
        {
            HttpContext.RaiseError(new InvalidOperationException("ViewClientSideFormDetails"));
            try
            {
                var getFormId = _formServices.GetFormId(id);

                var clientSideFormDetails = _formServices.ClientSideFormDetails(getFormId.FormId);

                List<ClientSideFormDetails> clientSideFormDetails1 = new List<ClientSideFormDetails>();
                
                foreach(var item in clientSideFormDetails)
                {
                    ClientSideFormDetails Model = new ClientSideFormDetails();
                    Model.FieldDetailsId = item.FieldDetailsId;
                    Model.fieldTypeName = item.fieldTypeName;
                    Model.FieldHtmlName = item.FieldHtmlName;
                    Model.fieldValidationName = item.fieldValidationName;
                    Model.FieldType = item.FieldType;
                    Model.FormId = item.FormId;
                    Model.IsOptional = item.IsOptional;
                    Model.HelpText = item.HelpText;
                    Model.FormName = item.FormName;

                    var getFieldOptions = _formServices.GetFilteredFieldOptions(Model.FieldDetailsId);
                    Model.FieldOptions = getFieldOptions.ToList();
                    clientSideFormDetails1.Add(Model);
                }

                if (clientSideFormDetails != null)
                {
                    //getting value from common helper.
                    return CommonHelper.GetResponse(HttpStatusCode.OK, "", null, clientSideFormDetails1);
                }
                return CommonHelper.GetResponse(HttpStatusCode.BadRequest, "", "");
            }
            catch
            {
                return CommonHelper.GetResponse(HttpStatusCode.InternalServerError, "", "");
            }
        }
        #endregion
        
        /// <summary>
        /// Get all Client Side Form Details
        /// </summary>
        /// <returns>list of Client Side Form Details </returns>
        #region ClientSideFormDetails(GET)
        [HttpGet("ClientSideFormDetails/{Id}")]
        public ApiResponseModel ClientSideFormDetails(int id)
        {
            HttpContext.RaiseError(new InvalidOperationException("ClientSideFormDetails"));
            try
            {
       

                var clientSideFormDetails = _formServices.ClientSideFormDetails(id);

                List<ClientSideFormDetails> clientSideFormDetails1 = new List<ClientSideFormDetails>();
                
                foreach(var item in clientSideFormDetails)
                {
                    ClientSideFormDetails Model = new ClientSideFormDetails();
                    Model.FieldDetailsId = item.FieldDetailsId;
                    Model.fieldTypeName = item.fieldTypeName;
                    Model.FieldHtmlName = item.FieldHtmlName;
                    Model.fieldValidationName = item.fieldValidationName;
                    Model.FieldType = item.FieldType;
                    Model.FormId = item.FormId;
                    Model.IsOptional = item.IsOptional;
                    Model.HelpText = item.HelpText;

                    var getFieldOptions = _formServices.GetFilteredFieldOptions(Model.FieldDetailsId);
                    Model.FieldOptions = getFieldOptions.ToList();
                    clientSideFormDetails1.Add(Model);
                }

                if (clientSideFormDetails != null)
                {
                    //getting value from common helper.
                    return CommonHelper.GetResponse(HttpStatusCode.OK, "", null, clientSideFormDetails1);
                }
                return CommonHelper.GetResponse(HttpStatusCode.BadRequest, "", "");
            }
            catch
            {
                return CommonHelper.GetResponse(HttpStatusCode.InternalServerError, "", "");
            }
        }
        #endregion

        /// <summary>
        ///  Submit Client Form
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Submit Client Form</returns>
        #region SubmitClientForm(POST)
        [HttpPost("SubmitClientForm")]
        public ApiResponseModel SubmitClientForm([FromBody] IEnumerable<CustomerFormData> model)
        {
            HttpContext.RaiseError(new InvalidOperationException("SubmitClientForm"));
            try
            {
                if (model != null)
                {
                    var submittedForm = _formServices.SubmitClientForm(model);
                    if (submittedForm == true)
                    {
                        //getting value from common helper.
                        return CommonHelper.GetResponse(HttpStatusCode.OK, CommonValidations.FormSavedSuccessfully, submittedForm);
                    }
                }
                return CommonHelper.GetResponse(HttpStatusCode.BadRequest, CommonValidations.RecordExistsMsg, "");
            }
            catch(Exception ex)
            {
                return CommonHelper.GetResponse(HttpStatusCode.InternalServerError, CommonValidations.InvalidMsg, "");
            }
        }
        #endregion

        //////////////////////////////////////////All customer forms detaisl for admin////////////////////////////////////////

        /// <summary>
        /// Getting all customer form list from this method.
        /// </summary>
        /// <returns>all list of customer forms</returns>
        #region GetAllCustomerForms(POST)
        [HttpPost("GetAllCustomerForms")]
        [Authorize(Roles = "Admin")]
        public ApiResponseModel GetAllCustomerForms(JqueryDatatableParam jqueryDatatableParam)
        {
            HttpContext.RaiseError(new InvalidOperationException("GetAllCustomerForms"));
            try
            {
                int totalRecord = 0;
                int filterRecord = 0;

                var data = _formServices.GetAllCustomerForms();
                if (data != null)
                {
                    // get total count of records 
                    totalRecord = data.Count();

                    // search data when search value found
                    if (!string.IsNullOrEmpty(jqueryDatatableParam.searchValue))
                    {
                        data = data.Where(x =>
                          x.CustomerEmail.ToLower().Contains(jqueryDatatableParam.searchValue.ToLower()));
                    }
                    // get total count of records after search 
                    filterRecord = data.Count();
                    //sort data
                    if (!string.IsNullOrEmpty(jqueryDatatableParam.sortColumn) && !string.IsNullOrEmpty(jqueryDatatableParam.sortColumnDirection))
                        data = data.AsQueryable().OrderBy(jqueryDatatableParam.sortColumn + " " + jqueryDatatableParam.sortColumnDirection);

                    //pagination
                    var empList = data.Skip(jqueryDatatableParam.skip).Take(jqueryDatatableParam.pageSize).ToList();
                    return CommonHelper.GetResponseDataTable(jqueryDatatableParam.draw, totalRecord, filterRecord, empList);
                }
                //getting value from common helper.
                return CommonHelper.GetResponse(HttpStatusCode.BadRequest, "", "");
            }

            catch (Exception)
            {
                return CommonHelper.GetResponse(HttpStatusCode.InternalServerError, "", "");
            }
        }
        #endregion

        /// <summary>
        /// Get all Client Side Form Details
        /// </summary>
        /// <returns>list of Client Side Form Details </returns>
        #region ViewCustomerForm(GET)
        [HttpGet("ViewCustomerForm/{Id}")]
        public ApiResponseModel ViewCustomerForm(int id)
        {
            HttpContext.RaiseError(new InvalidOperationException("ViewCustomerForm"));
            try
            {
                var clientSideFormDetails = _formServices.ViewCustomerForm(id);

                if (clientSideFormDetails != null)
                {
                    //getting value from common helper.
                    return CommonHelper.GetResponse(HttpStatusCode.OK, "", null, clientSideFormDetails);
                }
                return CommonHelper.GetResponse(HttpStatusCode.BadRequest, "", "");
            }
            catch
            {
                return CommonHelper.GetResponse(HttpStatusCode.InternalServerError, "", "");
            }
        }
        #endregion


        /// <summary>
        /// Get all Client Side Form Details
        /// </summary>
        /// <returns>list of Client Side Form Details </returns>
        #region GetDataOfForm(GET)
        [HttpGet("GetDataOfForm/{Id}")]
        public ApiResponseModel GetDataOfForm(int id)
        {
            HttpContext.RaiseError(new InvalidOperationException("GetDataOfForm"));
            try
            {
                var clientSideFormDetails = _formServices.GetDataOfForm(id);
                if (clientSideFormDetails != null)
                {
                    //getting value from common helper.
                    return CommonHelper.GetResponse(HttpStatusCode.OK, "", null, clientSideFormDetails);
                }
                return CommonHelper.GetResponse(HttpStatusCode.BadRequest, "", "");
            }
            catch
            {
                return CommonHelper.GetResponse(HttpStatusCode.InternalServerError, "", "");
            }
        }
        #endregion


        /// <summary>
        /// Verify client
        /// </summary>
        /// <returns>verified client </returns>
        #region GetVerifyCustomer(POST)
        [HttpPost("GetVerifyCustomer")]
        public ApiResponseModel GetVerifyCustomer(VerifyClient model)
        {
            HttpContext.RaiseError(new InvalidOperationException("GetVerifyCustomer"));
            try
            {              
                var submittedForm = _formServices.GetVerifyCustomer(model);
                if (submittedForm == true)
                {
                    //getting value from common helper.
                    return CommonHelper.GetResponse(HttpStatusCode.OK, "", submittedForm);
                }
                else if(submittedForm == false)
                {
                    //getting value from common helper.
                    //record already exist
                    return CommonHelper.GetResponse(HttpStatusCode.OK, CommonValidations.RecordExistsMsg, submittedForm);
                }
                return CommonHelper.GetResponse(HttpStatusCode.BadRequest, "", "", "");
            }
            catch
            {
                return CommonHelper.GetResponse(HttpStatusCode.InternalServerError, "", "");
            }
        }
        #endregion
    }
}