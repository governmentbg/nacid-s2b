using Infrastructure;
using Infrastructure.AppSettings;
using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models;
using Infrastructure.DomainValidation.Models.ErrorCodes.Sso;
using Infrastructure.Helpers.Extensions;
using Infrastructure.Token;
using Logs.Enums;
using Logs.Services;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Sc.Models.Dtos.Sso;
using System.Net;
using System.Text;

namespace Integrations.SsoIntegration
{
    public class SsoIntegrationService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly DomainValidatorService domainValidatorService;
        private readonly ErrorLogService errorLogService;

        public SsoIntegrationService(
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            DomainValidatorService domainValidatorService,
            ErrorLogService errorLogService
            )
        {
            this.httpClientFactory = httpClientFactory;
            this.httpContextAccessor = httpContextAccessor;
            this.domainValidatorService = domainValidatorService;
            this.errorLogService = errorLogService;
        }

        public async Task<TokenResponseDto> GetToken(FormUrlEncodedContent loginContent)
        {
            var requestMessage = new HttpRequestMessage
            {
                Content = loginContent,
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{AppSettingsProvider.SsoConfiguration.SsoUri}api/auth/token")
            };

            requestMessage.Headers.AddXForwardedHeaders(httpContextAccessor);

            var client = httpClientFactory.CreateClient();

            using var responseMessage = await client.SendAsync(requestMessage);

            if (responseMessage.IsSuccessStatusCode)
            {
                var tokenResponseDto = JsonConvert.DeserializeObject<TokenResponseDto>(await responseMessage.Content.ReadAsStringAsync());
                return tokenResponseDto;
            }
            else
            {
                if (responseMessage.StatusCode == HttpStatusCode.UnprocessableEntity)
                {
                    var ssoDomainErrorMessage = JsonConvert.DeserializeObject<SsoDomainErrorMessage>(await responseMessage.Content.ReadAsStringAsync());
                    var errorCode = Enum.IsDefined(typeof(SsoErrorCode), ssoDomainErrorMessage.ErrorCode) ? (SsoErrorCode)Enum.Parse(typeof(SsoErrorCode), ssoDomainErrorMessage.ErrorCode) : SsoErrorCode.Auth_UndefinedDomainError;

                    domainValidatorService.ThrowErrorMessage(errorCode, ssoDomainErrorMessage.ErrorAction, ssoDomainErrorMessage.ErrorText, ssoDomainErrorMessage.ErrorCount);
                }
                else
                {
                    await errorLogService.LogError(new Exception("SSO connection problem"), ErrorLogType.IntegrationExceptionLog, httpContextAccessor.HttpContext, null);
                    domainValidatorService.ThrowErrorMessage(SsoErrorCode.Auth_CommunicationExceptionWithSso);
                }

                return null;
            }
        }

        public async Task<UserContext> GetUserContext(HttpContext context)
        {
            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{AppSettingsProvider.SsoConfiguration.SsoUri}api/auth/userinfo")
            };

            requestMessage.Headers.AddXForwardedHeaders(httpContextAccessor);

            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            requestMessage.Headers
                        .Add("Authorization", authHeader);

            var client = httpClientFactory.CreateClient();

            using var responseMessage = await client.SendAsync(requestMessage);

            if (responseMessage.IsSuccessStatusCode)
            {
                var userContext = JsonConvert.DeserializeObject<UserContext>(await responseMessage.Content.ReadAsStringAsync());
                return userContext ?? new UserContext();
            }
            else
            {
                await errorLogService.LogError(new Exception("SSO connection problem"), ErrorLogType.IntegrationExceptionLog, httpContextAccessor.HttpContext, null);
                return new UserContext();
            }
        }

        public async Task<SsoUserDto> SignUp(SsoSignUpDto ssoSignUp, HttpContext context)
        {
            var requestMessage = new HttpRequestMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(ssoSignUp), Encoding.UTF8, "application/json"),
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{AppSettingsProvider.SsoConfiguration.SsoUri}api/auth/signUp")
            };

            requestMessage.Headers.AddXForwardedHeaders(httpContextAccessor);

            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            requestMessage.Headers
                        .Add("Authorization", authHeader);

            var client = httpClientFactory.CreateClient();

            using var responseMessage = await client.SendAsync(requestMessage);

            if (!responseMessage.IsSuccessStatusCode)
            {
                if (responseMessage.StatusCode == HttpStatusCode.UnprocessableEntity)
                {
                    var ssoDomainErrorMessage = JsonConvert.DeserializeObject<SsoDomainErrorMessage>(await responseMessage.Content.ReadAsStringAsync());
                    var errorCode = Enum.IsDefined(typeof(SsoErrorCode), ssoDomainErrorMessage.ErrorCode) ? (SsoErrorCode)Enum.Parse(typeof(SsoErrorCode), ssoDomainErrorMessage.ErrorCode) : SsoErrorCode.Auth_UndefinedDomainError;

                    domainValidatorService.ThrowErrorMessage(errorCode, ssoDomainErrorMessage.ErrorAction, ssoDomainErrorMessage.ErrorText, ssoDomainErrorMessage.ErrorCount);
                }
                else
                {
                    await errorLogService.LogError(new Exception("SSO connection problem"), ErrorLogType.IntegrationExceptionLog, httpContextAccessor.HttpContext, null);
                    domainValidatorService.ThrowErrorMessage(SsoErrorCode.Auth_CommunicationExceptionWithSso);
                }

                return null;
            }
            else
            {
                var ssoUserDto = JsonConvert.DeserializeObject<SsoUserDto>(await responseMessage.Content.ReadAsStringAsync());
                return ssoUserDto;
            }
        }

        public async Task<SsoUserValidateSignUpInfoDto> ValidateSignUp(SsoSignUpDto ssoSignUp)
        {
            var requestMessage = new HttpRequestMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(ssoSignUp), Encoding.UTF8, "application/json"),
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{AppSettingsProvider.SsoConfiguration.SsoUri}api/auth/validateSignUp")
            };

            requestMessage.Headers.AddXForwardedHeaders(httpContextAccessor);

            var client = httpClientFactory.CreateClient();

            using var responseMessage = await client.SendAsync(requestMessage);

            if (!responseMessage.IsSuccessStatusCode)
            {
                if (responseMessage.StatusCode == HttpStatusCode.UnprocessableEntity)
                {
                    var ssoDomainErrorMessage = JsonConvert.DeserializeObject<SsoDomainErrorMessage>(await responseMessage.Content.ReadAsStringAsync());
                    var errorCode = Enum.IsDefined(typeof(SsoErrorCode), ssoDomainErrorMessage.ErrorCode) ? (SsoErrorCode)Enum.Parse(typeof(SsoErrorCode), ssoDomainErrorMessage.ErrorCode) : SsoErrorCode.Auth_UndefinedDomainError;

                    domainValidatorService.ThrowErrorMessage(errorCode, ssoDomainErrorMessage.ErrorAction, ssoDomainErrorMessage.ErrorText, ssoDomainErrorMessage.ErrorCount);
                }
                else
                {
                    await errorLogService.LogError(new Exception("SSO connection problem"), ErrorLogType.IntegrationExceptionLog, httpContextAccessor.HttpContext, null);
                    domainValidatorService.ThrowErrorMessage(SsoErrorCode.Auth_CommunicationExceptionWithSso);
                }

                return new SsoUserValidateSignUpInfoDto();
            }
            else
            {
                var response = JsonConvert.DeserializeObject<SsoUserValidateSignUpInfoDto>(await responseMessage.Content.ReadAsStringAsync());
                return response;
            }
        }

        public async Task ActivateUser(SsoUserActivationDto ssoUserActivation)
        {
            var requestMessage = new HttpRequestMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(ssoUserActivation), Encoding.UTF8, "application/json"),
                Method = HttpMethod.Put,
                RequestUri = new Uri($"{AppSettingsProvider.SsoConfiguration.SsoUri}api/UserActivation/Activate")
            };

            requestMessage.Headers.AddXForwardedHeaders(httpContextAccessor);

            var client = httpClientFactory.CreateClient();

            using var responseMessage = await client.SendAsync(requestMessage);

            if (!responseMessage.IsSuccessStatusCode)
            {
                if (responseMessage.StatusCode == HttpStatusCode.UnprocessableEntity)
                {
                    var ssoDomainErrorMessage = JsonConvert.DeserializeObject<SsoDomainErrorMessage>(await responseMessage.Content.ReadAsStringAsync());
                    var errorCode = Enum.IsDefined(typeof(SsoErrorCode), ssoDomainErrorMessage.ErrorCode) ? (SsoErrorCode)Enum.Parse(typeof(SsoErrorCode), ssoDomainErrorMessage.ErrorCode) : SsoErrorCode.Auth_UndefinedDomainError;

                    domainValidatorService.ThrowErrorMessage(errorCode, ssoDomainErrorMessage.ErrorAction, ssoDomainErrorMessage.ErrorText, ssoDomainErrorMessage.ErrorCount);
                }
                else
                {
                    await errorLogService.LogError(new Exception("SSO connection problem"), ErrorLogType.IntegrationExceptionLog, httpContextAccessor.HttpContext, null);
                    domainValidatorService.ThrowErrorMessage(SsoErrorCode.Auth_CommunicationExceptionWithSso);
                }
            }
        }

        public async Task GenerateRecoverCode(SsoUserEmailDto ssoUserEmail)
        {
            var requestMessage = new HttpRequestMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(ssoUserEmail), Encoding.UTF8, "application/json"),
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{AppSettingsProvider.SsoConfiguration.SsoUri}api/UserRecover/Code")
            };

            requestMessage.Headers.AddXForwardedHeaders(httpContextAccessor);

            var client = httpClientFactory.CreateClient();

            using var responseMessage = await client.SendAsync(requestMessage);

            if (!responseMessage.IsSuccessStatusCode)
            {
                if (responseMessage.StatusCode == HttpStatusCode.UnprocessableEntity)
                {
                    var ssoDomainErrorMessage = JsonConvert.DeserializeObject<SsoDomainErrorMessage>(await responseMessage.Content.ReadAsStringAsync());
                    var errorCode = Enum.IsDefined(typeof(SsoErrorCode), ssoDomainErrorMessage.ErrorCode) ? (SsoErrorCode)Enum.Parse(typeof(SsoErrorCode), ssoDomainErrorMessage.ErrorCode) : SsoErrorCode.Auth_UndefinedDomainError;

                    domainValidatorService.ThrowErrorMessage(errorCode, ssoDomainErrorMessage.ErrorAction, ssoDomainErrorMessage.ErrorText, ssoDomainErrorMessage.ErrorCount);
                }
                else
                {
                    await errorLogService.LogError(new Exception("SSO connection problem"), ErrorLogType.IntegrationExceptionLog, httpContextAccessor.HttpContext, null);
                    domainValidatorService.ThrowErrorMessage(SsoErrorCode.Auth_CommunicationExceptionWithSso);
                }
            }
        }

        public async Task RecoverPassword(SsoUserRecoverPasswordDto ssoUserRecoverPasswordDto)
        {
            var requestMessage = new HttpRequestMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(ssoUserRecoverPasswordDto), Encoding.UTF8, "application/json"),
                Method = HttpMethod.Put,
                RequestUri = new Uri($"{AppSettingsProvider.SsoConfiguration.SsoUri}api/UserRecover")
            };

            requestMessage.Headers.AddXForwardedHeaders(httpContextAccessor);

            var client = httpClientFactory.CreateClient();

            using var responseMessage = await client.SendAsync(requestMessage);

            if (!responseMessage.IsSuccessStatusCode)
            {
                if (responseMessage.StatusCode == HttpStatusCode.UnprocessableEntity)
                {
                    var ssoDomainErrorMessage = JsonConvert.DeserializeObject<SsoDomainErrorMessage>(await responseMessage.Content.ReadAsStringAsync());
                    var errorCode = Enum.IsDefined(typeof(SsoErrorCode), ssoDomainErrorMessage.ErrorCode) ? (SsoErrorCode)Enum.Parse(typeof(SsoErrorCode), ssoDomainErrorMessage.ErrorCode) : SsoErrorCode.Auth_UndefinedDomainError;

                    domainValidatorService.ThrowErrorMessage(errorCode, ssoDomainErrorMessage.ErrorAction, ssoDomainErrorMessage.ErrorText, ssoDomainErrorMessage.ErrorCount);
                }
                else
                {
                    await errorLogService.LogError(new Exception("SSO connection problem"), ErrorLogType.IntegrationExceptionLog, httpContextAccessor.HttpContext, null);
                    domainValidatorService.ThrowErrorMessage(SsoErrorCode.Auth_CommunicationExceptionWithSso);
                }
            }
        }

        public async Task ChangePassword(SsoChangePasswordDto ssoChangerPasswordDto, HttpContext context)
        {
            var requestMessage = new HttpRequestMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(ssoChangerPasswordDto), Encoding.UTF8, "application/json"),
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{AppSettingsProvider.SsoConfiguration.SsoUri}api/User/changePassword")
            };

            requestMessage.Headers.AddXForwardedHeaders(httpContextAccessor);

            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            requestMessage.Headers
                        .Add("Authorization", authHeader);

            var client = httpClientFactory.CreateClient();

            using var responseMessage = await client.SendAsync(requestMessage);

            if (!responseMessage.IsSuccessStatusCode)
            {
                if (responseMessage.StatusCode == HttpStatusCode.UnprocessableEntity)
                {
                    var ssoDomainErrorMessage = JsonConvert.DeserializeObject<SsoDomainErrorMessage>(await responseMessage.Content.ReadAsStringAsync());
                    var errorCode = Enum.IsDefined(typeof(SsoErrorCode), ssoDomainErrorMessage.ErrorCode) ? (SsoErrorCode)Enum.Parse(typeof(SsoErrorCode), ssoDomainErrorMessage.ErrorCode) : SsoErrorCode.Auth_UndefinedDomainError;

                    domainValidatorService.ThrowErrorMessage(errorCode, ssoDomainErrorMessage.ErrorAction, ssoDomainErrorMessage.ErrorText, ssoDomainErrorMessage.ErrorCount);
                }
                else
                {
                    await errorLogService.LogError(new Exception("SSO connection problem"), ErrorLogType.IntegrationExceptionLog, httpContextAccessor.HttpContext, null);
                    domainValidatorService.ThrowErrorMessage(SsoErrorCode.Auth_CommunicationExceptionWithSso);
                }
            }
        }
    }
}
