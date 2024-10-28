import { SharedModule } from "src/shared/shared.module";
import { EAuthSendRequestModalComponent } from "./components/e-auth-send-request-modal.component";
import { EAuthResource } from "./e-auth.resource";
import { NgModule } from "@angular/core";
import { EAuthResponseComponent } from "./components/e-auth-response.component";
import { EAuthRoutingModule } from "./e-auth.routing";

const components = [
    EAuthSendRequestModalComponent,
    EAuthResponseComponent
];

const providers = [
    EAuthResource
];

const commonModules = [
    EAuthRoutingModule,
    SharedModule
]

@NgModule({
    declarations: components,
    providers: providers,
    imports: commonModules,
    exports: [...commonModules, ...components]
})
export class EAuthModule { }