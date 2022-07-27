#region Using
using EM.Entity;
using EM.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
#endregion

namespace EM.Services.WebForms
{
    /// <summary>
    /// Dynamic Form related all operations
    /// </summary>
    public interface IFormServices
    {
        /// <summary>
        /// Get id
        /// </summary>
        /// <param name="id"></param>
        /// <returns> returns form's id </returns>
        Forms GetById(int id);

        /// <summary>
        /// Get all forms
        /// </summary>
        /// <returns>list of forms</returns>
        IEnumerable<Forms> GetAllForms();

        /// <summary>
        /// Method for create new form
        /// </summary>
        /// <param name="forms"></param>
        /// <returns>Forms Model</returns>
        Forms CreateForm(Forms forms);
       
        /// <summary>
        /// Method for update existing forms 
        /// </summary>
        /// <param name="objForms"></param>
        /// <returns>Forms Model</returns>
        Forms EditForms(Forms objForms);

        /// <summary>
        /// Search forms 
        /// </summary>
        /// <returns>filtered data</returns>
        IEnumerable<Forms> SearchForms(SearchForms forms);

        //////////////////////////////////////////////////////// Manage Fields ////////////////////////////////////////////

        /// <summary>
        /// Get id
        /// </summary>
        /// <param name="id"></param>
        /// <returns> returns field's id </returns>
        FieldDetails GetByFieldId(int id);

        /// <summary>
        /// Get all fields
        /// </summary>
        /// <returns>list of fields</returns>
        IEnumerable<FieldDetails> GetAllFields();

        /// <summary>
        /// Get all fields
        /// </summary>
        /// <returns>list of fields</returns>
        IEnumerable<FieldDetails> GetAllFieldsList(int id);

        /// <summary>
        /// Method for add new field
        /// </summary>
        /// <param name="objFieldDetails"></param>
        /// <returns>FieldDetails Model</returns>
        FieldDetails AddFields(FieldDetails objFieldDetails);

        /// <summary>
        /// Method for update existing forms 
        /// </summary>
        /// <param name="objForms"></param>
        /// <returns>Forms Model</returns>
        FieldDetails EditFields(FieldDetails objFieldDetails);

        //////////////////////////////////////////////////////// Field Options ////////////////////////////////////////////

        /// <summary>
        /// Get id
        /// </summary>
        /// <param name="id"></param>
        /// <returns> returns form's id </returns>
        FieldOptions GetByOptionId(int id);

        /// <summary>
        /// Get all FieldOptions
        /// </summary>
        /// <returns>list of filtered FieldOptions </returns>
        IEnumerable<FieldOptions> GetAllFieldOptions();

        /// <summary>
        /// Get all FieldOptions
        /// </summary>
        /// <returns>list of filtered FieldOptions </returns>
        IEnumerable<FieldOptions> GetFilteredFieldOptions(int id);

        /// <summary>
        /// Method for add new field
        /// </summary>
        /// <param name="objFieldOptions"></param>
        /// <returns>FieldDetails Model</returns>
        FieldOptions AddOptionFields(FieldOptions objFieldOptions);

        /// <summary>
        /// Method for update existing field options 
        /// </summary>
        /// <param name="objFieldOptions"></param>
        /// <returns>FieldOptions Model</returns>
        FieldOptions EditFieldOptions(FieldOptions objFieldOptions);

        /// <summary>
        ///  Remove field option for form 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Remove field options</returns>
        FieldOptions RemoveFieldOption(int id);

        //////////////////////////////////////////////////////// Field Sequence ////////////////////////////////////////////
       
        /// <summary>
        /// Get form for field sequence
        /// </summary>
        /// <returns>get form in list format</returns>
        IEnumerable<Forms> GetFormCardById(int id);

        /// <summary>
        /// Get all saved forms
        /// </summary>
        /// <returns>list of saved forms </returns>
        IEnumerable<FormFields> GetAllSavedForms();

        /// <summary>
        ///  Save form for customer
        /// </summary>
        /// <param name="objFieldOptions"></param>
        /// <returns>Saved form</returns>
        FormFields SaveForm(FormFields objFormFields);

        /// <summary>
        /// Get all fields
        /// </summary>
        /// <returns>list of fields</returns>
        IEnumerable<FieldDetails> GetAllFilteredFieldsList(int id);

        /// <summary>
        /// Get all FieldOptions
        /// </summary>
        /// <returns>list of filtered FieldOptions </returns>
        IEnumerable<FieldDetails> GetSavedFormField(int id);

        //////////////////////////////////////////Send forms in mails to customer ////////////////////////////////////////

        /// <summary>
        /// Get all Client Side Form Details
        /// </summary>
        /// <returns>list of Client Side Form Details </returns>
        IEnumerable<FieldDetails> ClientSideFormDetails(int id);

        /// <summary>
        ///  Save form for customer
        /// </summary>
        /// <param name="objFieldOptions"></param>
        /// <returns>Saved form</returns>
        CustomerForms SaveCustomerForm(CustomerForms objCustomerForms);

        /// <summary>
        ///  Submit Client Form
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Submit Client Form</returns>
        bool SubmitClientForm([FromBody] IEnumerable<CustomerFormData> model);

        //////////////////////////////////////////All customer forms detaisl for admin////////////////////////////////////////

        /// <summary>
        /// Get all customer forms
        /// </summary>
        /// <returns>list of all customer forms</returns>
        IEnumerable<CustomerForms> GetAllCustomerForms();

        /// <summary>
        /// Get all customer forms
        /// </summary>
        /// <returns>list of all customer forms</returns>
        IEnumerable<FormFields> ViewCustomerForm(int id);

        /// <summary>
        /// Get all customer forms
        /// </summary>
        /// <returns>list of all customer forms</returns>
        IEnumerable<CustomerFormData> GetDataOfForm(int id);


        bool GetVerifyCustomer(VerifyClient objVerifyClient);

        CustomerForms GetFormId(int id);
    }
}