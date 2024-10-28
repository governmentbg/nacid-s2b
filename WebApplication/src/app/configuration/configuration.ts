import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserContextService } from 'src/auth/services/user-context.service';

@Injectable()
export class Configuration {
    environmentName: string;
    defaultLanguage: string;
    recaptchaLoginAfterAttempt: number;
    scClientId: string;
    hosting: string;
    useAllFunctionalities: boolean;

    constructor(
        private httpClient: HttpClient,
        private userContextService: UserContextService
    ) { }

    load(): Promise<{}> {
        return new Promise(resolve => {
            this.httpClient.get('../../configuration.json')
                .subscribe(config => {
                    this.importSettings(config);
                    this.userContextService.getUserInfo(this.scClientId, this.hosting)
                        .subscribe(() => resolve(true));
                });
        });
    }

    private importSettings(config: any) {
        this.environmentName = config.environmentName;
        this.defaultLanguage = config.defaultLanguage;
        this.recaptchaLoginAfterAttempt = config.recaptchaLoginAfterAttempt;
        this.scClientId = config.scClientId;
        this.hosting = config.hosting;
        this.useAllFunctionalities = config.useAllFunctionalities;
    }
}
