#region Using
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using System;
using EM.Entity;
using ElmahCore;
using EM.Models;
using EM.Common;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Collections.Generic;
#endregion

/// <summary>
///Controller for all Form related activites
/// </summary>
namespace EM.Web.Controllers
{
    /// <summary>
    /// Calling cache from startup.cs
    /// </summary>
    [ResponseCache(CacheProfileName = "Default0")]
    public class FormController : BaseController
    {
        /// <summary>
        /// Constructor of an object 
        /// </summary>
        /// <param name="configuration"></param>
        #region constructor
        public FormController(IConfiguration configuration) : base(configuration)
        {

        }
        #endregion

        /// <summary>
        /// Form Templates
        /// </summary>
        /// <returns>form template view</returns>
        #region FormTemplates(GET)
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult FormTemplates()
        {
            return View();
        }
        #endregion

        /// <summary>
        /// Get all forms 
        /// </summary>
        /// <returns>get all forms in list format</returns>
        #region GetAllForms(GET)
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllForms()
        {
            //Calling BaseController.
            var result = new ApiGenericModel<Forms>();
            result = ApiRequest<Forms>(RequestTypes.Get, "FormApi/GetAllForms").Result;
            return Json(result.GenericList);
        }
        #endregion

        /// <summary>
        /// Method for getting create form model
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Add/edit form model</returns>
        #region OpenCreateForm(GET)
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult OpenCreateForm(int? id)
        {
            HttpContext.RaiseError(new InvalidOperationException("OpenCreateForm"));
            Forms objForms = new Forms();
            try
            {
                if (id > 0)
                {
                    //Calling BaseController.
                    var result = new ApiGenericModel<Forms>();
                    result = ApiRequest<Forms>(RequestTypes.Get, "FormApi/OpenCreateForm/" + id).Result;
                    objForms.FormId = result.GenericModel.FormId;
                    objForms.FormName = result.GenericModel.FormName;
                    objForms.DestinationEmail = result.GenericModel.DestinationEmail;
                    objForms.IsActive = result.GenericModel.IsActive;
                }
            }
            catch (Exception)
            {
                return View("Error");
            }
            return PartialView("_CreateFormPartial", objForms);
        }
        #endregion

        /// <summary>
        ///  Create Form is for creating new form 
        /// </summary>
        /// <param name="objForms"></param>
        /// <returns>Create new form</returns>
        #region Add/Edit CreateForm(POST)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateForm(Forms objForms)
        {
            HttpContext.RaiseError(new InvalidOperationException("CreateForm"));
            try
            {
                if (ModelState.IsValid)
                {
                    var result = new ApiGenericModel<Forms>();
                    //For create new form.
                    if (objForms.FormId == 0)
                    {
                        //Calling BaseController.
                        result = ApiRequest<Forms>(RequestTypes.Post, "FormApi/CreateForm", null, objForms).Result;
                        if (result != null)
                        {
                            objForms = result.GenericModel;
                        }
                        if (true)
                        {
                            if (objForms == null)
                            {
                                return Json(result);
                            }
                            return Json(result);
                        }
                    }
                    //For update existing form.
                    else
                    {
                        result = ApiRequest<Forms>(RequestTypes.Put, "FormApi/EditForm", null, objForms).Result;
                        if (result != null)
                        {
                            objForms = result.GenericModel;
                        }
                        if (objForms == null)
                        {
                            return Json(result);
                        }
                        else
                        {
                            return Json(result);
                        }
                    }
                }
            }
            catch (Exception)
            {
                return View("Error");
            }
            return Content("");
        }
        #endregion

        /// <summary>
        /// Search forms 
        /// </summary>
        /// <returns>filtered data</returns>
        #region SearchForms(POST)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult SearchForms(SearchForms objForms)
        {
            //Calling BaseController.
            var result = new ApiGenericModel<Forms>();
            result = ApiRequest<Forms>(RequestTypes.Post, "FormApi/SearchForms", null, objForms).Result;
            return Json(result.GenericList);
        }
        #endregion

        //////////////////////////////////////////////////////// Manage Fields ////////////////////////////////////////////

        /// <summary>
        /// Get Manage Field(View)
        /// </summary>
        /// <returns>List of fields</returns>
        #region ManageFields(GET)
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult ManageFields(int id)
        {
            return View();
        }
        #endregion

        /// <summary>
        /// Method for getting form model for add and edit
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Add/edit new field model</returns>
        #region Open Add/edit FieldForm(GET)
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult OpenFieldForm(int? id)
        {
            HttpContext.RaiseError(new InvalidOperationException("OpenFieldForm"));
            FieldDetails objFieldDetails = new FieldDetails();
            try
            {
                if (id > 0)
                {
                    //Calling BaseController.
                    var result = new ApiGenericModel<FieldDetails>();
                    result = ApiRequest<FieldDetails>(RequestTypes.Get, "FormApi/OpenFieldForm/" + id).Result;
                    objFieldDetails.FormId = result.GenericModel.FormId;
                    objFieldDetails.FieldDetailsId = result.GenericModel.FieldDetailsId;
                    objFieldDetails.FieldType = result.GenericModel.FieldType;
                    objFieldDetails.FieldHtmlName = result.GenericModel.FieldHtmlName;
                    objFieldDetails.FieldImagePath = result.GenericModel.FieldImagePath;
                    objFieldDetails.IsOptional = result.GenericModel.IsOptional;
                    objFieldDetails.NoOfDatatableColumn = result.GenericModel.NoOfDatatableColumn;
                    objFieldDetails.ColumnOneTitle = result.GenericModel.ColumnOneTitle;
                    objFieldDetails.ColumnTwoTitle = result.GenericModel.ColumnTwoTitle;
                    objFieldDetails.ColumnThreeTitle = result.GenericModel.ColumnThreeTitle;
                    objFieldDetails.FieldValidationType = result.GenericModel.FieldValidationType;
                    objFieldDetails.HelpText = result.GenericModel.HelpText;
                }
            }
            catch (Exception)
            {
                return View("Error");
            }
            return PartialView("_AddFieldPartial", objFieldDetails);
        }
        #endregion

        /// <summary>
        ///  Add new field for form 
        /// </summary>
        /// <param name="objFieldDetails"></param>
        /// <returns>Add/Edit new field</returns>
        #region Add/EditFields(POST)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult AddFields(FieldDetails objFieldDetails)
        {
            HttpContext.RaiseError(new InvalidOperationException("AddFields"));
            try
            {
                if (ModelState.IsValid)
                {
                    var result = new ApiGenericModel<FieldDetails>();
                    //For add new field.
                    if (objFieldDetails.FieldDetailsId == 0)
                    {
                        //Calling BaseController.
                        result = ApiRequest<FieldDetails>(RequestTypes.Post, "FormApi/AddFields", null, objFieldDetails).Result;
                        if (result != null)
                        {
                            objFieldDetails = result.GenericModel;
                        }
                        if (true)
                        {
                            if (objFieldDetails == null)
                            {
                                return Json(result);
                            }
                            return Json(result);
                        }
                    }
                    //For update existing field.
                    else
                    {
                        result = ApiRequest<FieldDetails>(RequestTypes.Put, "FormApi/EditField", null, objFieldDetails).Result;
                        if (result != null)
                        {
                            objFieldDetails = result.GenericModel;
                        }
                        if (objFieldDetails == null)
                        {
                            return Json(result);
                        }
                        else
                        {
                            return Json(result);
                        }
                    }
                }
            }
            catch (Exception)
            {
                return View("Error");
            }
            return Content("");
        }
        #endregion

        /// <summary>
        /// Getting all field list from this method.
        /// </summary>
        /// <returns>all list of fields</returns>
        #region GetFieldList(POST)
        [HttpPost]
        [Authorize(Roles = "Admin, Users")]
        public IActionResult GetFieldList(int id)
        {
            HttpContext.RaiseError(new InvalidOperationException("GetFieldList"));
            try
            {
                if (HttpContext.Session.GetString("JWToken") == null)
                {
                    return Unauthorized();
                }
                else
                {
                    int totalRecord = 0;
                    int filterRecord = 0;

                    JqueryDatatableParam objJqueryDatatableParam = new JqueryDatatableParam();

                    objJqueryDatatableParam.draw = Request.Form["draw"].FirstOrDefault();

                    objJqueryDatatableParam.sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();

                    objJqueryDatatableParam.sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();

                    objJqueryDatatableParam.searchValue = Request.Form["search[value]"].FirstOrDefault();

                    objJqueryDatatableParam.pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");

                    objJqueryDatatableParam.skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");

                    ////Calling BaseController.
                    var result = new ApiGenericModel<FieldDetails>();
                    result = ApiRequest<FieldDetails>(RequestTypes.Post, "FormApi/GetFieldList/" + id, null, objJqueryDatatableParam).Result;

                    var returnObj = new { draw = result.Draw, recordsTotal = result.RecordsTotal, recordsFiltered = result.RecordsFiltered, data = result.GenericList };
                    if (true)
                    {
                        return Json(returnObj);
                    }
                }
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
        #endregion

        //////////////////////////////////////////////////////// Field Options ////////////////////////////////////////////

        /// <summary>
        /// Get all field options
        /// </summary>
        /// <param name="id"></param>
        /// <returns>list of field options</returns>
        #region GetFieldOptions(GET)
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetFieldOptions(int id)
        {
            HttpContext.RaiseError(new InvalidOperationException("GetFieldOptions"));
            try
            {
                //Calling BaseController.
                var result = new ApiGenericModel<FieldOptions>();
                result = ApiRequest<FieldOptions>(RequestTypes.Get, "FormApi/GetFieldOptions/" + id).Result;
                return new JsonResult(result.GenericList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        /// <summary>
        /// Method for getting field options model for add and edit
        /// </summary>
        /// <param name="id"></param>
        /// <returns>open field options model</returns>
        #region OpenFieldOptions(GET)
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult OpenFieldOptions(int? id, int? optionId)
        {
            HttpContext.RaiseError(new InvalidOperationException("OpenFieldOptions"));
            FieldOptions objFieldOptions = new FieldOptions();
            try
            {
                if (optionId > 0)
                {
                    //Calling BaseController.
                    var result = new ApiGenericModel<FieldOptions>();
                    result = ApiRequest<FieldOptions>(RequestTypes.Get, "FormApi/OpenEditFieldOption/" + id).Result;
                    objFieldOptions.FieldDetailsId = result.GenericModel.FieldDetailsId;
                    objFieldOptions.FieldOptionsId = result.GenericModel.FieldOptionsId;
                    objFieldOptions.OptionValue = result.GenericModel.OptionValue;
                }
            }
            catch (Exception)
            {
                return View("Error");
            }

            ViewBag.fieldDetailsId = id;
            return PartialView("_FieldOptionPatial", objFieldOptions);
        }
        #endregion

        /// <summary>
        ///  Add new field option for form 
        /// </summary>
        /// <param name="objFieldOptions"></param>
        /// <returns>Add new field options</returns>
        #region Add/EditFieldOptions(POST)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult AddOptionFields(FieldOptions objFieldOptions)
        {
            HttpContext.RaiseError(new InvalidOperationException("AddOptionFields"));
            try
            {
                if (ModelState.IsValid)
                {
                    var result = new ApiGenericModel<FieldOptions>();
                    //For add new field.
                    if (objFieldOptions.FieldOptionsId == 0)
                    {
                        //Calling BaseController.
                        result = ApiRequest<FieldOptions>(RequestTypes.Post, "FormApi/AddOptionFields", null, objFieldOptions).Result;
                        if (result != null)
                        {
                            objFieldOptions = result.GenericModel;
                        }
                        if (true)
                        {
                            if (objFieldOptions == null)
                            {
                                return Json(result);
                            }
                            return Json(result);
                        }
                    }
                    //For update existing field.
                    else
                    {
                        result = ApiRequest<FieldOptions>(RequestTypes.Put, "FormApi/EditOptionFields", null, objFieldOptions).Result;
                        if (result != null)
                        {
                            objFieldOptions = result.GenericModel;
                        }
                        if (objFieldOptions == null)
                        {
                            return Json(result);
                        }
                        else
                        {
                            return Json(result);
                        }
                    }
                }
            }
            catch (Exception)
            {
                return View("Error");
            }
            return Content("");
        }
        #endregion

        /// <summary>
        ///  Remove field option for form 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Remove field options</returns>
        #region RemoveFieldOption(POST)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult RemoveFieldOption(int id)
        {
            HttpContext.RaiseError(new InvalidOperationException("RemoveFieldOption"));
            try
            {
                //Calling BaseController.
                var result = new ApiGenericModel<FieldOptions>();
                result = ApiRequest<FieldOptions>(RequestTypes.Delete, "FormApi/RemoveFieldOption/" + id).Result;
                if (result.GenericList != null)
                {
                    return Json(result.GenericList);
                }
                else
                {
                    return Json(result);
                }
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
        #endregion

        //////////////////////////////////////////Field Sequence////////////////////////////////////////

        /// <summary>
        /// Get Field Sequence
        /// </summary>
        /// <returns>List of fields sequences</returns>
        #region FieldSequence
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult FieldSequence()
        {
            HttpContext.RaiseError(new InvalidOperationException("FieldSequence"));
            try
            {
                return View();
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
        #endregion

        /// <summary>
        /// Get form for field sequence
        /// </summary>
        /// <returns>get form in list format</returns>
        #region FieldSequenceFormCard(GET)
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult FieldSequenceFormCard(int Id)
        {
            HttpContext.RaiseError(new InvalidOperationException("FieldSequenceFormCard"));
            try
            {
                //Calling BaseController.
                var result = new ApiGenericModel<Forms>();
                result = ApiRequest<Forms>(RequestTypes.Get, "FormApi/FieldSequenceFormCard/" + Id).Result;
                return Json(result.GenericList);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
        #endregion

        /// <summary>
        /// Get fields for field sequence
        /// </summary>
        /// <returns>get field in list format</returns>
        #region FieldSequenceFieldList(GET)
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult FieldSequenceFieldList(int Id)
        {
            HttpContext.RaiseError(new InvalidOperationException("FieldSequenceFieldList"));
            try
            {
                //Calling BaseController.
                var result = new ApiGenericModel<FieldDetails>();
                result = ApiRequest<FieldDetails>(RequestTypes.Get, "FormApi/FieldSequenceFieldList/" + Id).Result;
                if (result.GenericList != null)
                {
                    return Json(result.GenericList);
                }
                else
                {
                    return Json(result);
                }
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
        #endregion

        /// <summary>
        /// Get fields for field sequence
        /// </summary>
        /// <returns>get field in list format</returns>
        #region FieldSequenceSavedFieldList(GET)
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult FieldSequenceSavedFieldList(int Id)
        {
            HttpContext.RaiseError(new InvalidOperationException("FieldSequenceSavedFieldList"));
            try
            {
                //Calling BaseController.
                var result = new ApiGenericModel<FieldDetails>();
                result = ApiRequest<FieldDetails>(RequestTypes.Get, "FormApi/FieldSequenceSavedFieldList/" + Id).Result;
                if (result.GenericList != null)
                {
                    return Json(result.GenericList);
                }
                else
                {
                    return Json(result);
                }
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
        #endregion

        /// <summary>
        ///  Save form for customer
        /// </summary>
        /// <param name="objFieldOptions"></param>
        /// <returns>Saved form</returns>
        #region SaveForm(POST)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult SaveForm(FormFields objFormFields)
        {
            HttpContext.RaiseError(new InvalidOperationException("SaveForm"));
            try
            {
                var result = new ApiGenericModel<FormFields>();
                //Calling BaseController.
                result = ApiRequest<FormFields>(RequestTypes.Post, "FormApi/SaveForm", null, objFormFields).Result;
                if (result != null)
                {
                    objFormFields = result.GenericModel;
                }
                if (true)
                {
                    if (objFormFields == null)
                    {
                        return Json(result.GenericModel);
                    }
                    return Json(result);
                }
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
        #endregion

        //////////////////////////////////////////New Customer Forms////////////////////////////////////////

        /// <summary>
        /// New Customer Forms(View)
        /// </summary>
        /// <returns>List of new customer forms</returns>
        #region NewCustomerForms(GET)
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult NewCustomerForms()
        {
            HttpContext.RaiseError(new InvalidOperationException("NewCustomerForms"));
            try
            {
                return View();
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
        #endregion

        /// <summary>
        /// Getting all field list from this method.
        /// </summary>
        /// <returns>all list of fields</returns>
        #region GetAllFormsData(POST)
        [HttpPost]
        [Authorize(Roles = "Admin, Users")]
        public IActionResult GetAllFormsData()
        {
            HttpContext.RaiseError(new InvalidOperationException("GetAllFormsData"));
            try
            {
                if (HttpContext.Session.GetString("JWToken") == null)
                {
                    return Unauthorized();
                }
                else
                {
                    JqueryDatatableParam objJqueryDatatableParam = new JqueryDatatableParam();

                    objJqueryDatatableParam.draw = Request.Form["draw"].FirstOrDefault();

                    objJqueryDatatableParam.sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();

                    objJqueryDatatableParam.sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();

                    objJqueryDatatableParam.searchValue = Request.Form["search[value]"].FirstOrDefault();

                    objJqueryDatatableParam.pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");

                    objJqueryDatatableParam.skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");

                    ////Calling BaseController.
                    var result = new ApiGenericModel<Forms>();
                    result = ApiRequest<Forms>(RequestTypes.Post, "FormApi/GetAllFormsData ", null, objJqueryDatatableParam).Result;

                    var returnObj = new { draw = result.Draw, recordsTotal = result.RecordsTotal, recordsFiltered = result.RecordsFiltered, data = result.GenericList };
                    if (true)
                    {
                        return Json(returnObj);
                    }
                }
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
        #endregion

        /// <summary>
        /// Method for getting form model for add and edit
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Add/edit new field model</returns>
        #region OpenSendEmailForm(GET)
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult OpenSendEmailForm(int id)
        {
            HttpContext.RaiseError(new InvalidOperationException("OpenSendEmailForm"));
            try
            {
                ViewBag.formID = id;
                return PartialView("_SendMailPartial");
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
        #endregion

        /// <summary>
        ///  Create Form is for creating new form 
        /// </summary>
        /// <param name="objForms"></param>
        /// <returns>Create new form</returns>
        #region SendMail(POST)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult SendMail(CustomerForms objCustomerForms)
        {
            HttpContext.RaiseError(new InvalidOperationException("SendMail"));
            try
            {
                if (ModelState.IsValid)
                {
                    var result = new ApiGenericModel<CustomerForms>();
                    //Calling BaseController.
                    result = ApiRequest<CustomerForms>(RequestTypes.Post, "FormApi/SendMail", null, objCustomerForms).Result;
                    if (result != null)
                    {
                        objCustomerForms = result.GenericModel;
                    }
                    if (true)
                    {
                        if (objCustomerForms == null)
                        {
                            return Json(result);
                        }
                        return Json(result);
                    }
                }
            }
            catch (Exception)
            {
                return View("Error");
            }
            return Content("");
        }
        #endregion

        //////////////////////////////////////////Send forms in mails to customer ////////////////////////////////////////

        /// <summary>
        /// Client Form
        /// </summary>
        /// <returns>Client form view</returns>
        #region ClientForm(GET)
        [HttpGet]
        public IActionResult ClientForm(string formId, string email, string cfi)
        {
            HttpContext.RaiseError(new InvalidOperationException("ClientForm"));
            try
            {
                var formID = EncryptionDecryption.Decrypt(formId);
                var Email = EncryptionDecryption.Decrypt(email);
                var CustomerFormId = EncryptionDecryption.Decrypt(cfi);

                ClientSideFormDetails objClientSideFormDetails = new ClientSideFormDetails();
                VerifyClient obj = new VerifyClient();
                obj.FormId = Convert.ToInt32(formID);
                obj.CustomerEmail = Email;

                var results = new ApiGenericModel<bool>();
                results = ApiRequest<bool>(RequestTypes.Post, "FormApi/GetVerifyCustomer", null, obj).Result;
                if (results.GenericModel == false)
                {
                    return RedirectToAction("RecordExist", "Form");
                }
                else
                {
                    //Calling BaseController.
                    var result = new ApiGenericModel<ClientSideFormDetails>();
                    result = ApiRequest<ClientSideFormDetails>(RequestTypes.Get, "FormApi/ClientSideFormDetails/" + formID).Result;
                    return View(result.GenericList);
                }
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }
        #endregion

        /// <summary>
        /// Get form id for submit form
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>form id</returns>
        #region GetFormID
        public IActionResult GetFormID(string Id)
        {
            var formID = EncryptionDecryption.Decrypt(Id);
            return Json(formID);
        }
        #endregion

        /// <summary>
        ///  Submit Client Form
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Submit Client Form</returns>
        #region SubmitClientForm(POST)
        [HttpPost]
        public IActionResult SubmitClientForm(string customerFormId, [FromBody] IEnumerable<CustomerFormDataValues> model)
        {
            HttpContext.RaiseError(new InvalidOperationException("SubmitClientForm"));
            try
            {
                var CustomerFormID = EncryptionDecryption.Decrypt(customerFormId);
                CustomerFormDataValues newModel = new CustomerFormDataValues();
                //model.Select(x => x.CustomerFormsId).ToList();
                foreach (var item in model)
                {
                    item.CustomerFormsId = Convert.ToInt32(CustomerFormID);
                }
                var test = model;

                var result = new ApiGenericModel<bool>();
                result = ApiRequest<bool>(RequestTypes.Post, "FormApi/SubmitClientForm", null, model).Result;
                if (result.GenericModel == true)
                {
                    return Json(result);
                }
                if (model == null)
                {
                    return Json(result);
                }
                else
                {
                    return Json(result);
                }
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }
        #endregion

        /// <summary>
        /// Success page for form submission
        /// </summary>
        /// <returns>Success page</returns>
        #region SuccessForm
        public IActionResult SuccessForm()
        {
            return View();
        }
        #endregion

        /// <summary>
        /// RecordExist page for form submission
        /// </summary>
        /// <returns>RecordExist page</returns>
        #region RecordExist
        public IActionResult RecordExist()
        {
            return View();
        }
        #endregion

        //////////////////////////////////////////All customer forms detaisl for admin////////////////////////////////////////
        /// <summary>
        /// Alls Customer Forms(View)
        /// </summary>
        /// <returns>List of all customer forms</returns>
        #region CustomerForms(GET)
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult CustomerForms()
        {
            HttpContext.RaiseError(new InvalidOperationException("CustomerForms"));
            try
            {
                return View();
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
        #endregion

        /// <summary>
        /// Getting all customer form list from this method.
        /// </summary>
        /// <returns>all list of customer forms</returns>
        #region GetAllCustomerForms(POST)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllCustomerForms()
        {
            HttpContext.RaiseError(new InvalidOperationException("GetAllCustomerForms"));
            try
            {
                if (HttpContext.Session.GetString("JWToken") == null)
                {
                    return Unauthorized();
                }
                else
                {
                    JqueryDatatableParam objJqueryDatatableParam = new JqueryDatatableParam();

                    objJqueryDatatableParam.draw = Request.Form["draw"].FirstOrDefault();

                    objJqueryDatatableParam.sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();

                    objJqueryDatatableParam.sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();

                    objJqueryDatatableParam.searchValue = Request.Form["search[value]"].FirstOrDefault();

                    objJqueryDatatableParam.pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");

                    objJqueryDatatableParam.skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");

                    ////Calling BaseController.
                    var result = new ApiGenericModel<CustomerForms>();
                    result = ApiRequest<CustomerForms>(RequestTypes.Post, "FormApi/GetAllCustomerForms ", null, objJqueryDatatableParam).Result;

                    var returnObj = new { draw = result.Draw, recordsTotal = result.RecordsTotal, recordsFiltered = result.RecordsFiltered, data = result.GenericList };
                    if (true)
                    {
                        return Json(returnObj);
                    }
                }
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
        #endregion

        /// <summary>
        /// Client Form
        /// </summary>
        /// <returns>Client form view</returns>
        #region ViewCustomerForm(GET)
        [HttpGet]
        public IActionResult ViewCustomerForm(int id)
        {
            HttpContext.RaiseError(new InvalidOperationException("ClientForm"));
            try
            {
                ClientSideFormDetails objClientSideFormDetails = new ClientSideFormDetails();
                //Calling BaseController.
                var result = new ApiGenericModel<ClientSideFormDetails>();
                result = ApiRequest<ClientSideFormDetails>(RequestTypes.Get, "FormApi/ViewClientSideFormDetails/" + id).Result;

                var result1 = new ApiGenericModel<CustomerFormData>();
                result1 = ApiRequest<CustomerFormData>(RequestTypes.Get, "FormApi/GetDataOfForm/" + id).Result;

                foreach (var item in result.GenericList)
                {
                    item.FieldValue = result1.GenericList.Where(x => x.FieldId == item.FieldDetailsId).Select(x => x.FieldValue).FirstOrDefault().ToString();
                    ViewBag.FormName = item.FormName;
                }

                return PartialView("_ViewCustomerFormPartial", result.GenericList);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }
        #endregion

        /// <summary>
        /// Client Form
        /// </summary>
        /// <returns>Client form view</returns>
        #region GetDataOfForm(GET)
        [HttpGet]
        public IActionResult GetDataOfForm(int id)
        {
            HttpContext.RaiseError(new InvalidOperationException("GetDataOfForm"));
            try
            {
                //Calling BaseController.
                var result = new ApiGenericModel<CustomerFormData>();
                result = ApiRequest<CustomerFormData>(RequestTypes.Get, "FormApi/GetDataOfForm/" + id).Result;
                return View(result.GenericList);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }
        #endregion

        //[HttpGet]
        //public IActionResult DownloadPdf()
        //{
        //    //// instantiate a html to pdf converter object
        //    //HtmlToPdf converter = new HtmlToPdf();

        //    //// set converter options
        //    //converter.Options.PdfPageSize = pageSize;
        //    //converter.Options.PdfPageOrientation = pdfOrientation;
        //    //converter.Options.WebPageWidth = webPageWidth;
        //    //converter.Options.WebPageHeight = webPageHeight;

        //    //// create a new pdf document converting an html string
        //    //PdfDocument doc = converter.ConvertHtmlString(htmlString, baseUrl);

        //    //// save pdf document
        //    //doc.Save(Response, false, "Sample.pdf");

        //    //// close pdf document
        //    //doc.Close();
        //    var htmlContent = String.Format("<body>Hello world: {0}</body>",DateTime.Now);
        //    var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();
        //    var pdfBytes = htmlToPdf.GeneratePdf(htmlContent);
        //    return View();
        //}
        //[HttpPost]
        //public string DownloadPdf(string id)
        //{

        //    // convert HTML to PDF Byte[] array and return as Base64 string to AJAX
        //    //string post = "<h2>Here is a test</h2>";
        //    //var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();
        //    //result = Convert.ToBase64String(htmlToPdf.GeneratePdf(post));

        //    //var htmlContent = String.Format("<body>Hello world: {0}</body>", DateTime.Now);
        //    //var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();
        //    //var pdfBytes = htmlToPdf.GeneratePdf(htmlContent);

        //    // read parameters from the webpage
        //    try{
        //        string htmlString = @"<html>
        //                             <body>
        //                              Hello World from selectpdf.com.
        //                             </body>
        //                            </html>
        //                            ";

        //        // instantiate a html to pdf converter object
        //        HtmlToPdf converter = new HtmlToPdf();
        //        // create a new pdf document converting an url
        //        PdfDocument doc = converter.ConvertHtmlString(htmlString);

        //        // save pdf document
        //        byte[] myByte = doc.Save();

        //        // close pdf document
        //        doc.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        //return View("Error");
        //    }
        //    return null;
        //}

    }
}