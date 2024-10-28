using Infrastructure;
using Infrastructure.Attributes;
using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCodes;
using Infrastructure.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sc.Models.Dtos.Auth;
using Sc.Models.Dtos.Sso;
using Sc.Models.Dtos.Suppliers;
using Sc.Models.Enums.Auth;
using Sc.Models.Enums.Suppliers;
using Sc.Services.ApproveRegistrations;
using Sc.Services.Auth;

namespace Server.Controllers.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserContext userContext;
        private readonly AuthService authService;
        private readonly ApproveRegistrationService approveRegistrationService;
        private readonly DomainValidatorService domainValidatorService;

        public AuthController(
            UserContext userContext,
            AuthService authService,
            ApproveRegistrationService approveRegistrationService,
            DomainValidatorService domainValidatorService
        )
        {
            this.userContext = userContext;
            this.authService = authService;
            this.approveRegistrationService = approveRegistrationService;
            this.domainValidatorService = domainValidatorService;
        }

        [HttpGet("userinfo")]
        public ActionResult<UserContext> GetUserInfo()
        {
            return Ok(userContext);
        }

        [HttpPost("token")]
        public async Task<ActionResult<TokenResponseDto>> Login([FromBody] LoginDto loginDto)
        {
            var tokenResponseDto = await authService.Login(loginDto);
            return Ok(tokenResponseDto);
        }

        [HttpPost("authToken")]
        public async Task<ActionResult<TokenResponseDto>> LoginWithCode([FromBody] LoginWithCodeDto loginWithCodeDto)
        {
            var tokenResponseDto = await authService.LoginWithCode(loginWithCodeDto);
            return Ok(tokenResponseDto);
        }

        [HttpPost("signUp")]
        public async Task<ActionResult<SupplierDto>> SignUp([FromBody] SignUpDeclarationDto signUpDeclarationDto, CancellationToken cancellationToken)
        {
            if (signUpDeclarationDto.SignUp.Type == SignUpType.Supplier)
            {
                if (await approveRegistrationService.ExistsNonDeclined(signUpDeclarationDto.SignUp.SupplierType == SupplierType.Institution 
                    ? signUpDeclarationDto.SignUp.Institution.Id
                    : signUpDeclarationDto.SignUp.Complex.Id, signUpDeclarationDto.SignUp.SupplierType, cancellationToken)) 
                {
                    domainValidatorService.ThrowErrorMessage(ApproveRegistrationErrorCode.ApproveRegistration_Supplier_Exists);
                }

                var supplierDto = await approveRegistrationService.ApplyForRegistration(signUpDeclarationDto, cancellationToken);

                return Ok(supplierDto);
            }
            else if(signUpDeclarationDto.SignUp.Type == SignUpType.Company)
            {
                await authService.SignUp(signUpDeclarationDto.SignUp, null, cancellationToken);
            }

            return Ok();
        }

        [HttpPut("activate")]
        public async Task<ActionResult> ActivateUser([FromBody] SsoUserActivationDto ssoUserActivationDto)
        {
            await authService.ActivateUser(ssoUserActivationDto);

            return Ok();
        }

        [HttpPost("recover/code")]
        public async Task<ActionResult> GenerateRecoverCode([FromBody] SsoUserEmailDto ssoUserEmailDto)
        {
            await authService.GenerateRecoverCode(ssoUserEmailDto);

            return Ok();
        }

        [HttpPut("recover")]
        public async Task<ActionResult> RecoverPassword([FromBody] SsoUserRecoverPasswordDto ssoUserRecoverPasswordDto)
        {
            await authService.RecoverPassword(ssoUserRecoverPasswordDto);

            return Ok();
        }

        [HttpPost("changePassword")]
        [Authorize, ScClient]
        public async Task<ActionResult> ChangePassword([FromBody] SsoChangePasswordDto ssoChangerPasswordDto)
        {
            await authService.ChangePassword(ssoChangerPasswordDto);

            return Ok();
        }
    }
}
