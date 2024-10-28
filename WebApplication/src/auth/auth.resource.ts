import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { LoginWithCodeDto } from "./dtos/login/login-with-code.dto";
import { LoginDto } from "./dtos/login/login.dto";
import { TokenResponseDto } from "./dtos/token/token-response.dto";
import { UserContext } from "./dtos/user-context.dto";
import { SsoUserActivationDto } from "./dtos/activation/sso-user-activation.dto";
import { SsoUserEmailDto } from "./dtos/recover/sso-user-email.dto";
import { SsoUserRecoverPasswordDto } from "./dtos/recover/sso-user-recover-password.dto";
import { SignUpDeclarationDto } from "./dtos/sign-up/sign-up-declaration.dto";
import { SupplierDto } from "src/suppliers/dtos/supplier.dto";
import { ssoChangePasswordDto } from "./dtos/change-password/sso-change-password.dto";

@Injectable()
export class AuthResource {

    url = 'api/auth';

    constructor(
        private http: HttpClient
    ) { }

    getUserInfo(): Observable<UserContext> {
        return this.http.get<UserContext>(`${this.url}/userinfo`);
    }

    login(loginDto: LoginDto): Observable<TokenResponseDto> {
        return this.http.post<TokenResponseDto>(`${this.url}/token`, loginDto);
    }

    loginWithCode(loginWithCodeDto: LoginWithCodeDto): Observable<TokenResponseDto> {
        return this.http.post<TokenResponseDto>(`${this.url}/authToken`, loginWithCodeDto);
    }

    signUp(signUpDeclaration: SignUpDeclarationDto): Observable<SupplierDto> {
        return this.http.post<SupplierDto>(`${this.url}/signUp`, signUpDeclaration);
    }

    activateUser(userActivationDto: SsoUserActivationDto): Observable<void> {
        return this.http.put<void>(`${this.url}/activate`, userActivationDto);
    }

    generateRecoverCode(ssoUserEmail: SsoUserEmailDto): Observable<void> {
        return this.http.post<void>(`${this.url}/recover/code`, ssoUserEmail);
    }

    recoverPassword(ssoUserRecoverPasswordDto: SsoUserRecoverPasswordDto): Observable<void> {
        return this.http.put<void>(`${this.url}/recover`, ssoUserRecoverPasswordDto);
    }

    changePassword(ssoChangePasswordDto: ssoChangePasswordDto) : Observable<void> {
        return this.http.post<void>(`${this.url}/changePassword`, ssoChangePasswordDto);
    }
}