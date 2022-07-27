#region Using
using EM.Entity;
using EM.GenericUnitOfWork.Uow;
using EM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using static EM.Common.GlobalEnum;
#endregion

namespace EM.Services.WebForms
{
    /// <summary>
    /// Dynamic Form related all operations
    /// </summary>
    public class FormServices : IFormServices
    {
        #region Fields
        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        #endregion

        #region Ctor
        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="configuration"></param>
        public FormServices(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            this._unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        #endregion

        /// <summary>
        /// Method for get form by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>return particular form by id</returns>
        #region GetById
        public Forms GetById(int id)
        {
            try
            {
                var repoList = this._unitOfWork.GetRepository<Forms>();
                return repoList.GetByID(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        /// <summary>
        /// Method for get all forms
        /// </summary>
        /// <returns>All form which are active</returns>
        #region GetAllForms
        public IEnumerable<Forms> GetAllForms()
        {
            try
            {
                var repoList = this._unitOfWork.GetRepository<Forms>();
                List<Forms> lstForms = repoList.GetAll().AsNoTracking().ToList();
                if (lstForms != null)
                {
                    return lstForms;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// Create new form 
        /// </summary>
        /// <param name="forms"></param>
        /// <returns>Created new form</returns>
        #region CreateForm(POST)
        public Forms CreateForm(Forms forms)
        {
            try
            {
                var formRepository = _unitOfWork.GetRepository<Forms>();
                Forms verifyForm = new Forms();
                //check that new email is already registered or not 
                verifyForm = this.GetAllForms().FirstOrDefault(x => x.DestinationEmail == forms.DestinationEmail);
                if (verifyForm == null)
                {
                    if (formRepository != null)
                    {
                        forms.CreatedBy = Convert.ToInt32(UserRoles.Admin);
                        forms.CreatedDate = DateTime.Now;
                        formRepository.Add(forms);
                        _unitOfWork.Commit();
                    }
                    return forms;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// Method for update existing forms 
        /// </summary>
        /// <param name="objForms"></param>
        /// <returns>Forms model with modified form details</returns>
        #region EditForms(POST)
        public Forms EditForms(Forms objForms)
        {
            try
            {
                if (objForms != null)
                {
                    var formRepository = _unitOfWork.GetRepository<Forms>();
                    //User updateDetails = new User();
                    var modifiedBy = Convert.ToInt32(UserRoles.Admin);
                    var editFormDetails = this.GetAllForms().FirstOrDefault(x => x.FormId == objForms.FormId);

                    if (formRepository != null)
                    {
                        editFormDetails.FormName = objForms.FormName;
                        var userEmail = this.GetAllForms().FirstOrDefault(x => x.DestinationEmail == objForms.DestinationEmail);
                        if (userEmail == null)
                        {
                            editFormDetails.DestinationEmail = objForms.DestinationEmail;
                        }
                        editFormDetails.IsActive = objForms.IsActive;
                        editFormDetails.UpdatedDate = DateTime.Now;
                        editFormDetails.UpdatedBy = Convert.ToInt32(UserRoles.Admin);
                        _unitOfWork.GetRepository<Forms>().Update(editFormDetails);
                        _unitOfWork.Commit();

                        return objForms;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// Search forms 
        /// </summary>
        /// <returns>filtered data</returns>
        #region SearchForms
        public IEnumerable<Forms> SearchForms(SearchForms forms)
        {
            try
            {
                var repoList = this._unitOfWork.GetRepository<Forms>();
                List<Forms> lstForms = repoList.GetAll().AsNoTracking().ToList();
                lstForms = lstForms.Where(x => (string.IsNullOrEmpty(forms.FormName) || x.FormName.ToLower().Contains(forms.FormName.ToLower())) &&
                                                   (string.IsNullOrEmpty(forms.DestinationEmail) || x.DestinationEmail.ToLower().Contains(forms.DestinationEmail.ToLower()))
                                                   ).ToList();

                if (forms.IsActive || forms.IsActive == null)
                {
                    lstForms = lstForms.Where(x => x.IsActive == true).ToList();
                }
                else
                {
                    lstForms = lstForms.Where(x => x.IsActive == false).ToList();
                }

                if (lstForms != null)
                {
                    return lstForms;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        //////////////////////////////////////////////////////// Manage Fields ////////////////////////////////////////////

        /// <summary>
        /// Method for get field by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>return particular field by id</returns>
        #region GetByFieldId
        public FieldDetails GetByFieldId(int id)
        {
            try
            {
                var repoList = this._unitOfWork.GetRepository<FieldDetails>();
                return repoList.GetByID(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// Get all fields
        /// </summary>
        /// <returns>list of fields</returns>
        #region GetAllFields
        public IEnumerable<FieldDetails> GetAllFields()
        {
            try
            {
                var repoList = this._unitOfWork.GetRepository<FieldDetails>();
                List<FieldDetails> lstFormFields = repoList.GetAll().AsNoTracking().ToList();
                if (lstFormFields != null)
                {
                    return lstFormFields;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// Get all fields
        /// </summary>
        /// <returns>list of fields</returns>
        #region GetAllFieldsList
        public IEnumerable<FieldDetails> GetAllFieldsList(int id)
        {
            try
            {
                var repoList = this._unitOfWork.GetRepository<FieldDetails>();

                List<FieldDetails> lstForms =
                    (from fields in repoList.GetAll()
                     let fieldType = Convert.ToInt32(fields.FieldType)
                     let fieldValidationType = Convert.ToInt32(fields.FieldValidationType)
                     where fields.FormId == id
                     select new FieldDetails
                     {
                         FieldDetailsId = fields.FieldDetailsId,
                         FormId = fields.FormId,
                         FieldType = fields.FieldType,
                         fieldTypeName = fieldType == Convert.ToInt32(fieldTypes.Textbox) ? "TextBox" :
                                         fieldType == Convert.ToInt32(fieldTypes.Textarea) ? "Textarea" :
                                         fieldType == Convert.ToInt32(fieldTypes.Checkbox) ? "Checkbox" :
                                         fieldType == Convert.ToInt32(fieldTypes.Dropdown) ? "Dropdown" :
                                         fieldType == Convert.ToInt32(fieldTypes.Radiobutton) ? "Radiobutton" :
                                         fieldType == Convert.ToInt32(fieldTypes.Datatable) ? "Datatable" : "",
                         FieldHtmlName = fields.FieldHtmlName,
                         FieldValidationType = fields.FieldValidationType,
                         fieldValidationName = fieldValidationType == Convert.ToInt32(fieldValidationTypes.NA) ? "NA" :
                                               fieldValidationType == Convert.ToInt32(fieldValidationTypes.Numeric) ? "Numeric" :
                                               fieldValidationType == Convert.ToInt32(fieldValidationTypes.Email) ? "Email" : "",
                         IsOptional = fields.IsOptional,
                         NoOfDatatableColumn = fields.NoOfDatatableColumn
                     }
                     ).AsNoTracking().ToList();

                if (lstForms != null)
                {
                    return lstForms;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// Method for add new field
        /// </summary>
        /// <param name="objFieldDetails"></param>
        /// <returns>FieldDetails Model</returns>
        #region AddFields(POST)
        public FieldDetails AddFields(FieldDetails objFieldDetails)
        {
            try
            {
                var fieldRepository = _unitOfWork.GetRepository<FieldDetails>();
                FieldDetails fieldDetails = new FieldDetails();

                if (fieldRepository != null)
                {
                    objFieldDetails.NoOfDatatableColumn = 0;
                    fieldRepository.Add(objFieldDetails);
                    _unitOfWork.Commit();
                }
                return objFieldDetails;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// Method for update existing forms 
        /// </summary>
        /// <param name="objForms"></param>
        /// <returns>Forms Model</returns>
        #region EditFields(POST)
        public FieldDetails EditFields(FieldDetails objFieldDetails)
        {
            try
            {
                if (objFieldDetails != null)
                {
                    var fieldRepository = _unitOfWork.GetRepository<FieldDetails>();
                    var editFormDetails = this.GetAllFields().FirstOrDefault(x => x.FieldDetailsId == objFieldDetails.FieldDetailsId && x.FormId == objFieldDetails.FormId);

                    if (fieldRepository != null)
                    {
                        editFormDetails.FieldDetailsId = objFieldDetails.FieldDetailsId;
                        editFormDetails.FormId = objFieldDetails.FormId;
                        editFormDetails.FieldHtmlName = objFieldDetails.FieldHtmlName;
                        editFormDetails.FieldType = objFieldDetails.FieldType;
                        editFormDetails.FieldValidationType = objFieldDetails.FieldValidationType;
                        editFormDetails.IsOptional = objFieldDetails.IsOptional;
                        editFormDetails.NoOfDatatableColumn = objFieldDetails.NoOfDatatableColumn;
                        editFormDetails.ColumnOneTitle = objFieldDetails.ColumnOneTitle;
                        editFormDetails.ColumnTwoTitle = objFieldDetails.ColumnTwoTitle;
                        editFormDetails.ColumnThreeTitle = objFieldDetails.ColumnThreeTitle;
                        editFormDetails.FieldImagePath = objFieldDetails.FieldImagePath;
                        editFormDetails.HelpText = objFieldDetails.HelpText;

                        _unitOfWork.GetRepository<FieldDetails>().Update(editFormDetails);
                        _unitOfWork.Commit();

                        return objFieldDetails;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        //////////////////////////////////////////////////////// Field Options ////////////////////////////////////////////

        /// <summary>
        /// Method for get form by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>return particular form by id</returns>
        #region GetByOptionId
        public FieldOptions GetByOptionId(int id)
        {
            try
            {
                var repoList = this._unitOfWork.GetRepository<FieldOptions>();
                return repoList.GetByID(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// Get all FieldOptions
        /// </summary>
        /// <returns>list of fields</returns>
        #region GetAllFieldOptions
        public IEnumerable<FieldOptions> GetAllFieldOptions()
        {
            try
            {
                var repoList = this._unitOfWork.GetRepository<FieldOptions>();
                List<FieldOptions> lstFieldOptions = repoList.GetAll().AsNoTracking().ToList();
                if (lstFieldOptions != null)
                {
                    return lstFieldOptions;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// Get filtered all FieldOptions
        /// </summary>
        /// <returns>list of filtered fields</returns>
        #region GetFilteredFieldOptions
        public IEnumerable<FieldOptions> GetFilteredFieldOptions(int id)
        {
            try
            {
                var repoList = this._unitOfWork.GetRepository<FieldOptions>();
                List<FieldOptions> lstFieldOptions = repoList.GetAll().Where(x => x.FieldDetailsId == id).AsNoTracking().ToList();
                if (lstFieldOptions != null)
                {
                    return lstFieldOptions;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// Method for add new field
        /// </summary>
        /// <param name="objFieldOptions"></param>
        /// <returns>FieldDetails Model</returns>
        #region AddOptionFields(POST)
        public FieldOptions AddOptionFields(FieldOptions objFieldOptions)
        {
            try
            {
                var fieldOptionRepository = _unitOfWork.GetRepository<FieldOptions>();
                FieldOptions fieldOptions = new FieldOptions();

                if (fieldOptionRepository != null)
                {
                    fieldOptionRepository.Add(objFieldOptions);
                    _unitOfWork.Commit();
                }
                return objFieldOptions;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// Method for update existing field options 
        /// </summary>
        /// <param name="objFieldOptions"></param>
        /// <returns>FieldOptions Model</returns>
        #region EditFieldOptions(POST)
        public FieldOptions EditFieldOptions(FieldOptions objFieldOptions)
        {
            try
            {
                if (objFieldOptions != null)
                {
                    var fieldOptionRepository = _unitOfWork.GetRepository<FieldOptions>();
                    var editFieldOptions = this.GetAllFieldOptions().FirstOrDefault(x => x.FieldOptionsId == objFieldOptions.FieldOptionsId);

                    if (fieldOptionRepository != null)
                    {
                        editFieldOptions.FieldDetailsId = objFieldOptions.FieldDetailsId;
                        editFieldOptions.OptionValue = objFieldOptions.OptionValue;
                        _unitOfWork.GetRepository<FieldOptions>().Update(editFieldOptions);
                        _unitOfWork.Commit();

                        return objFieldOptions;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        ///  Remove field option for form 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Remove field options</returns>
        #region RemoveFieldOption(POST)
        public FieldOptions RemoveFieldOption(int id)
        {
            try
            {
                var userRepository = _unitOfWork.GetRepository<FieldOptions>();
                var deleteAppointment = this.GetAllFieldOptions().FirstOrDefault(x => x.FieldOptionsId == id);
                _unitOfWork.GetRepository<FieldOptions>().HardDelete(deleteAppointment);
                _unitOfWork.Commit();

                return deleteAppointment;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        //////////////////////////////////////////////////////// Field Sequence ////////////////////////////////////////////

        /// <summary>
        /// Get form for field sequence
        /// </summary>
        /// <returns>get form in list format</returns>
        #region GetFormCardById(GET)
        public IEnumerable<Forms> GetFormCardById(int id)
        {
            try
            {
                var repoList = this._unitOfWork.GetRepository<Forms>();
                List<Forms> lstForms = repoList.GetAll().Where(x => x.FormId == id).AsNoTracking().ToList();
                if (lstForms != null)
                {
                    return lstForms;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// Get all saved forms
        /// </summary>
        /// <returns>list of saved forms </returns>
        #region GetAllSavedForms(GET)
        public IEnumerable<FormFields> GetAllSavedForms()
        {
            try
            {
                var repoList = this._unitOfWork.GetRepository<FormFields>();
                List<FormFields> lstFieldOptions = repoList.GetAll().AsNoTracking().ToList();
                if (lstFieldOptions != null)
                {
                    return lstFieldOptions;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        ///  Save form for customer
        /// </summary>
        /// <param name="objFieldOptions"></param>
        /// <returns>Saved form</returns>
        #region SaveForm(POST)
        public FormFields SaveForm(FormFields objFormFields)
        {
            try
            {
                var fieldRepository = _unitOfWork.GetRepository<FormFields>();
                FormFields fieldDetails = new FormFields();

                //Verify form               
                fieldDetails = this.GetAllSavedForms().FirstOrDefault(x => x.FormId == objFormFields.FormId);

                if (fieldDetails == null)
                {
                    fieldRepository.Add(objFormFields);
                    _unitOfWork.Commit();
                }
                else if (fieldDetails != null)
                {
                    fieldDetails.FormId = objFormFields.FormId;
                    fieldDetails.FieldsSequence = objFormFields.FieldsSequence;
                    fieldDetails.IsActive = true;
                    fieldRepository.Update(fieldDetails);
                    _unitOfWork.Commit();
                }
                return objFormFields;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// Get all filtered fields
        /// </summary>
        /// <returns>list of fields(filtered)</returns>
        #region GetAllFilteredFieldsList(For getting list in drag and drop box)
        public IEnumerable<FieldDetails> GetAllFilteredFieldsList(int id)
        {
            try
            {
                var repoList = this._unitOfWork.GetRepository<FormFields>();

                var fieldDetailsList = this._unitOfWork.GetRepository<FieldDetails>();

                var savedFieldList = repoList.GetAll().Where(x => x.FormId == id).Select(x => x.FieldsSequence).FirstOrDefault().Split(",");

                var FinalFieldList = fieldDetailsList.GetAll().Where(x => !savedFieldList.Contains(x.FieldDetailsId.ToString()));

                List<FieldDetails> lstForms =
                    (from fields in FinalFieldList
                     let fieldType = Convert.ToInt32(fields.FieldType)
                     let fieldValidationType = Convert.ToInt32(fields.FieldValidationType)
                     where fields.FormId == id
                     select new FieldDetails
                     {
                         FieldDetailsId = fields.FieldDetailsId,
                         FormId = fields.FormId,
                         FieldType = fields.FieldType,
                         fieldTypeName = fieldType == Convert.ToInt32(fieldTypes.Textbox) ? "TextBox" :
                                         fieldType == Convert.ToInt32(fieldTypes.Textarea) ? "Textarea" :
                                         fieldType == Convert.ToInt32(fieldTypes.Checkbox) ? "Checkbox" :
                                         fieldType == Convert.ToInt32(fieldTypes.Dropdown) ? "Dropdown" :
                                         fieldType == Convert.ToInt32(fieldTypes.Radiobutton) ? "Radiobutton" :
                                         fieldType == Convert.ToInt32(fieldTypes.Datatable) ? "Datatable" : "",
                         FieldHtmlName = fields.FieldHtmlName,
                         FieldValidationType = fields.FieldValidationType,
                         fieldValidationName = fieldValidationType == Convert.ToInt32(fieldValidationTypes.NA) ? "NA" :
                                               fieldValidationType == Convert.ToInt32(fieldValidationTypes.Numeric) ? "Numeric" :
                                               fieldValidationType == Convert.ToInt32(fieldValidationTypes.Email) ? "Email" : "",
                         IsOptional = fields.IsOptional,
                         NoOfDatatableColumn = fields.NoOfDatatableColumn,
                         HelpText = fields.HelpText
                     }
                     ).AsNoTracking().ToList();

                if (lstForms != null)
                {
                    return lstForms;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// Get all saved fields
        /// </summary>
        /// <returns>list of saved fields</returns>
        #region GetSavedFormField(For getting list in save fields drag and drop box)
        public IEnumerable<FieldDetails> GetSavedFormField(int id)
        {
            try
            {
                var repoList = this._unitOfWork.GetRepository<FormFields>();

                var fieldDetailsList = this._unitOfWork.GetRepository<FieldDetails>();

                var savedFieldList = repoList.GetAll().Where(x => x.FormId == id).Select(x => x.FieldsSequence).FirstOrDefault().Split(",");

                var FinalFieldList = fieldDetailsList.GetAll().Where(x => savedFieldList.Contains(x.FieldDetailsId.ToString()));

                List<FieldDetails> lstForms =
                    (from fields in FinalFieldList
                     let fieldType = Convert.ToInt32(fields.FieldType)
                     let fieldValidationType = Convert.ToInt32(fields.FieldValidationType)
                     where fields.FormId == id
                     select new FieldDetails
                     {
                         FieldDetailsId = fields.FieldDetailsId,
                         FormId = fields.FormId,
                         FieldType = fields.FieldType,
                         fieldTypeName = fieldType == Convert.ToInt32(fieldTypes.Textbox) ? "TextBox" :
                                         fieldType == Convert.ToInt32(fieldTypes.Textarea) ? "Textarea" :
                                         fieldType == Convert.ToInt32(fieldTypes.Checkbox) ? "Checkbox" :
                                         fieldType == Convert.ToInt32(fieldTypes.Dropdown) ? "Dropdown" :
                                         fieldType == Convert.ToInt32(fieldTypes.Radiobutton) ? "Radiobutton" :
                                         fieldType == Convert.ToInt32(fieldTypes.Datatable) ? "Datatable" : "",
                         FieldHtmlName = fields.FieldHtmlName,
                         FieldValidationType = fields.FieldValidationType,
                         fieldValidationName = fieldValidationType == Convert.ToInt32(fieldValidationTypes.NA) ? "NA" :
                                               fieldValidationType == Convert.ToInt32(fieldValidationTypes.Numeric) ? "Numeric" :
                                               fieldValidationType == Convert.ToInt32(fieldValidationTypes.Email) ? "Email" : "",
                         IsOptional = fields.IsOptional,
                         NoOfDatatableColumn = fields.NoOfDatatableColumn,
                         HelpText = fields.HelpText
                     }
                     ).AsNoTracking().ToList();

                if (lstForms != null)
                {
                    return lstForms;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// Get all Client Side Form Details
        /// </summary>
        /// <returns>list of Client Side Form Details </returns>
         #region ClientSideFormDetails 
        public IEnumerable<FieldDetails> ClientSideFormDetails(int id)
        {
            try
            {
                var formList = this._unitOfWork.GetRepository<Forms>();

                var getFormName = formList.GetAll().Where(x => x.FormId == id).FirstOrDefault();

                var repoList = this._unitOfWork.GetRepository<FormFields>();

                var fieldDetailsList = this._unitOfWork.GetRepository<FieldDetails>();

                var savedFieldList = repoList.GetAll().Where(x => x.FormId == id).Select(x => x.FieldsSequence).FirstOrDefault().Split(",");

                var fieldOptionList = this._unitOfWork.GetRepository<FieldOptions>();

                var FinalFieldList = fieldDetailsList.GetAll().Where(x => savedFieldList.Contains(x.FieldDetailsId.ToString()));

                List<FieldDetails> lstForms =
                    (from fields in FinalFieldList
                     //join fieldOptions in fieldDetailsList on fields.FieldDetailsId equals fieldDetailsList.
                     let fieldDetailId = fields.FieldDetailsId
                     let fieldType = Convert.ToInt32(fields.FieldType)
                     let fieldValidationType = Convert.ToInt32(fields.FieldValidationType)
                     where fields.FormId == id
                     select new FieldDetails
                     {
                         FieldDetailsId = fields.FieldDetailsId,
                         FormId = fields.FormId,
                         FieldType = fields.FieldType,
                         fieldTypeName = fieldType == Convert.ToInt32(fieldTypes.Textbox) ? "TextBox" :
                                         fieldType == Convert.ToInt32(fieldTypes.Textarea) ? "Textarea" :
                                         fieldType == Convert.ToInt32(fieldTypes.Checkbox) ? "Checkbox" :
                                         fieldType == Convert.ToInt32(fieldTypes.Dropdown) ? "Dropdown" :
                                         fieldType == Convert.ToInt32(fieldTypes.Radiobutton) ? "Radiobutton" :
                                         fieldType == Convert.ToInt32(fieldTypes.Datatable) ? "Datatable" : "",
                         FieldHtmlName = fields.FieldHtmlName,
                         FieldValidationType = fields.FieldValidationType,
                         fieldValidationName = fieldValidationType == Convert.ToInt32(fieldValidationTypes.NA) ? "NA" :
                                               fieldValidationType == Convert.ToInt32(fieldValidationTypes.Numeric) ? "Numeric" :
                                               fieldValidationType == Convert.ToInt32(fieldValidationTypes.Email) ? "Email" : "",
                         IsOptional = fields.IsOptional,
                         NoOfDatatableColumn = fields.NoOfDatatableColumn,
                         HelpText = fields.HelpText,
                         FormName = getFormName.FormName
                     }
                     ).AsNoTracking().ToList();
                 
                        if (lstForms != null)
                        {
                             return lstForms;
                        }
                        else
                        {
                            return null;
                        }
           }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// Method for add new field
        /// </summary>
        /// <param name="objFieldDetails"></param>
        /// <returns>FieldDetails Model</returns>
        #region SaveCustomerForm(POST)
        public CustomerForms SaveCustomerForm(CustomerForms objCustomerForms)
        {
            try
            {
                var fieldRepository = _unitOfWork.GetRepository<CustomerForms>();
                CustomerForms fieldDetails = new CustomerForms();

                if (fieldRepository != null)
                {
                    objCustomerForms.CreatedDate = DateTime.Now;
                    objCustomerForms.IsActive = true;
                    fieldRepository.Add(objCustomerForms);
                    _unitOfWork.Commit();
                }

                return objCustomerForms;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        ///  Submit Client Form
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Submit Client Form</returns>
        #region SubmitClientForm(POST)
        public bool SubmitClientForm([FromBody] IEnumerable<CustomerFormData> model)
        {
            try
            {
                var fieldRepository = _unitOfWork.GetRepository<CustomerFormData>();
                CustomerFormData fieldDetails = new CustomerFormData();
                
                List<CustomerFormData> newList = model.ToList();
                foreach (var item in newList)
                {
                    fieldRepository.Add(item);
                }
                _unitOfWork.Commit();

                var fieldRepository1 = _unitOfWork.GetRepository<CustomerForms>();

                var test = fieldRepository1.GetByID(model.Select(x => x.CustomerFormsId).FirstOrDefault());

                if (test != null)
                {
                    test.SubmitDate = DateTime.Now;
                    test.IsSubmitted = true;
                    fieldRepository1.Update(test);
                    _unitOfWork.Commit();
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        //////////////////////////////////////////All customer forms detaisl for admin////////////////////////////////////////

        /// <summary>
        /// Get all customer forms
        /// </summary>
        /// <returns>list of all customer forms</returns>
        #region GetAllCustomerForms
        public IEnumerable<CustomerForms> GetAllCustomerForms()
        {
            try
            {
                var repoList = this._unitOfWork.GetRepository<CustomerForms>();
                var formList = this._unitOfWork.GetRepository<Forms>();

                var data = (from d in repoList.GetAll() //d = CustomerForms
                            join u in formList.GetAll() on d.FormId equals u.FormId //u = Forms
                            select new CustomerForms()
                            {
                                FormId = d.FormId,
                                FormFieldId = 0,
                                FormName = u.FormName,
                                CustomerFormsId = d.CustomerFormsId,
                                CustomerEmail = d.CustomerEmail,
                                SignatureData = null,
                                IsActive = d.IsActive,
                                IsSubmitted = d.IsSubmitted,
                                CreatedBy = 0,
                                CreatedDate = d.CreatedDate,
                                UpdatedBy = 0,
                                UpdatedDate = d.UpdatedDate,
                                SubmitDate = d.SubmitDate,
                                ExpiryDate = d.ExpiryDate,

                            }).ToList();

                List<CustomerForms> lstForms = data;
                if (lstForms != null)
                {
                    return lstForms;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// Get all saved forms
        /// </summary>
        /// <returns>list of saved forms </returns>
        #region GetAllSavedForms(GET)
        public IEnumerable<FormFields> ViewCustomerForm(int id )
        {
            try
            {
                var repoList = this._unitOfWork.GetRepository<FormFields>();
                List<FormFields> lstFieldOptions = repoList.GetAll().Where(x => x.FormId == id).AsNoTracking().ToList();
                if (lstFieldOptions != null)
                {
                    return lstFieldOptions;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// Get all saved forms
        /// </summary>
        /// <returns>list of saved forms </returns>
        #region GetAllSavedForms(GET)
        public IEnumerable<CustomerFormData> GetDataOfForm(int id)
        {
            try
            {
                var repoList = this._unitOfWork.GetRepository<CustomerFormData>();
                var formList = this._unitOfWork.GetRepository<Forms>();

                List<CustomerFormData> lstFieldOptions = repoList.GetAll().Where(x => x.CustomerFormsId == id).AsNoTracking().ToList();
                if (lstFieldOptions != null)
                {
                    return lstFieldOptions;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// Verify client
        /// </summary>
        /// <returns>verified client </returns>
        #region GetVerifyCustomer(POST)
        public bool GetVerifyCustomer(VerifyClient objVerifyClient)
        {
            try
            {
                var repoList = this._unitOfWork.GetRepository<CustomerForms>();
                var lstFieldOptions = repoList.GetAll().Where(x => x.CustomerEmail == objVerifyClient.CustomerEmail && x.FormId == objVerifyClient.FormId && x.IsSubmitted == true).AsNoTracking().FirstOrDefaultAsync().Result;
                if (lstFieldOptions == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// Get form id from customerform
        /// </summary>
        /// <param name="id"></param>
        /// <returns>form id</returns>
        #region GetFormId
        public CustomerForms GetFormId(int id)
        {
            try
            {
                var repoList = this._unitOfWork.GetRepository<CustomerForms>();
                return repoList.GetByID(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}