namespace Infrastructure.DomainValidation.Models.ErrorCodes.Sso
{
    public enum SsoErrorCode
    {
        System_FunctionalityUnavailible,

        Auth_CommunicationExceptionWithSso,
        Auth_UndefinedDomainError,
        Auth_WrongUserNameOrPassword,
        Auth_InvalidRecaptcha,
        Auth_UserLocked,
        Auth_UserDeactivated,
        Auth_UserHasNoSsoApplicationPermissions,
        Auth_UserSsoApplicationLocked,
        Auth_UserSsoApplicationDeactivated,

        User_EmptyUserInfo,
        User_UserNameExists,
        User_EmailExists,
        User_NewPasswordsMismatch,
        User_InvalidPassword,
        User_InvalidFullNameLength,
        User_InvalidEmailLength,
        User_FullNameRequiredCyrillic,
        User_InvalidEmail,
        User_InvalidPhoneNumberLength,
        User_InvalidPhoneNumber,
        User_InvalidAddressLength,
        User_InvalidNotesLength,
        User_InvalidPasswordValidation,
        User_NotFound,
        User_IsNotLocked,
        User_WithGivenActivationCodeNotFound,
        User_ActivationCodeNotExpired,
        User_ActivationCodeExpired,
        User_LockedUserForFailedLoginAttempts,
        User_InvalidOrExpiredTwoFactorAuthCode,
        User_RecoverCodeNotExpired,
        User_InvalidRecoverCode,

        SsoAppUser_UserExistsForSsoApplication
    }
}
