namespace EM.Common
{
    /// <summary>
    /// Common validation messages for whole project
    /// </summary>
    public static class CommonValidations
    {
        /// <summary>
        /// The required error message....
        /// </summary>
        public const string RequiredErrorMsg = "Please enter {0}.";

        /// <summary>
        /// The select required error message....
        /// </summary>
        public const string SelectRequiredErrorMsg = "Please select {0}.";

        /// <summary>
        /// Defines the RecordExistsMsg.
        /// </summary>
        public const string RecordExistsMsg = "The record already exists.";

        /// <summary>
        /// Defines the RecordExistsMsg.
        /// </summary>
        public const string RecordNotExistsMsg = "The record is not exist.";

        /// <summary>
        /// Defines the InvalidUserMsg.
        /// </summary>
        public const string InvalidUserMsg = "Invalid login credential.";

        /// <summary>
        /// Defines the ValidData.
        /// </summary>
        public const string ValidData = "Please enter valid data.";

        /// <summary>
        /// Defines the PleaseEnterValidEmail.
        /// </summary>
        public const string PleaseEnterValidEmail = "Please enter a valid Email Address.";

        /// <summary>
        /// The required length  error message...
        /// </summary>
        public const string RequiredLengthErrorMsg = "{0} length must be between {2} and {1}.";

        /// <summary>
        /// The invalid mobile error message...
        /// </summary>
        public const string InvalidMobileErrorMsg = "PLease Enter valid Mobile Number";

        /// <summary>
        /// The password regular expression...
        /// </summary>
        public const string PasswordRegex = "^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$";

        /// <summary>
        /// The comparing password message...
        /// </summary>
        public const string ComparePasswordMsg = "Password does not match";

        /// <summary>
        /// The reenter password message...
        /// </summary>
        public const string RetypePasswordMesg = "Please reenter your password";

        /// <summary>
        /// The string regular expression...
        /// </summary>
        public const string StringRegex = "[a-zA-Z0-9]*$";

        /// <summary>
        /// The string max length...
        /// </summary>
        public const string StringLength = "The {0} must be less than {10} characters.";

        /// <summary>
        /// The update password message...
        /// </summary>
        public const string PasswordUpdateMsg = "New password updated successfully";

        /// <summary>
        /// The not update password message...
        /// </summary>
        public const string PasswordNotUpdateMsg = "Something is invalid";

        /// <summary>
        /// The sending link for reset password message...
        /// </summary>
        public const string LinkSendMsg = "Reset password link has been sent to your email id";

        /// <summary>
        /// The sending link for reset password message...
        /// </summary>
        public const string WrongMailMsg = "You entered wrong e-mail address";

        /// <summary>
        /// New user registered successfully message...
        /// </summary>
        public const string NewUserRegisterd = "New user is registered successfully.";

        /// <summary>
        /// User details updated successfully message...
        /// </summary>
        public const string UpdateUserDetails = "User details are updated successfully.";

        /// <summary>
        /// User details delete successfully message...
        /// </summary>
        public const string DeleteUserDetails = "User details are deleted successfully.";

        /// <summary>
        /// Appointment is booked successfully message...
        /// </summary>
        public const string BookedAppointment = "Appointment is booked successfully.";

        /// <summary>
        /// Appointment is updated successfully message...
        /// </summary>
        public const string UpdateAppointment = "Appointment is updated successfully.";

        /// <summary>
        /// if there is time conflict from doctor's side...
        /// </summary>
        public const string DoctorUnavailableMsg = "Doctor is not available for that time, Please select different time.";

        /// <summary>
        /// if any other exception occurs...
        /// </summary>
        public const string InvalidMsg = "Something is invalid.";

        /// <summary>
        /// can't book appointment on past date...
        /// </summary>
        public const string NotDropEventMsg = "Don't Book an appointment of past date.";

        /// <summary>
        /// Doctor details updated successfully message...
        /// </summary>
        public const string UpdateDoctorsDetails = "Doctor details are updated successfully.";

        /// <summary>
        /// New form created successfully message...
        /// </summary>
        public const string NewFormCreated = "New form is created successfully.";

        /// <summary>
        /// Form details updated successfully message...
        /// </summary>
        public const string UpdateFormDetails = "Form details are updated successfully.";

        /// <summary>
        /// New field created successfully message...
        /// </summary>
        public const string NewFieldCreated = "New field is created successfully.";

        /// <summary>
        /// Fields details updated successfully message...
        /// </summary>
        public const string UpdateFieldDetails = "Field details are updated successfully.";

        /// <summary>
        /// Your form is saved successfully message...
        /// </summary>
        public const string FormSavedSuccessfully = "Your form is saved successfully.";
        
        /// <summary>
        /// Your form is saved successfully message...
        /// </summary>
        public const string EmailSentSuccessfully = "Email is sent successfully.";
    }
}