import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { RoboTraderSetupRoutingModule } from "./robo-trader-setup-routing.module";
import { NgSelectModule } from "@ng-select/ng-select";
import {
  ErrorsModule,
  PageHeaderModule,
  SharedPipesModule,
} from "../../shared";
import { TranslateModule } from "@ngx-translate/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { NgbModule } from "@ng-bootstrap/ng-bootstrap";
import { RoboTraderSetupFormComponent } from "./robo-trader-setup-form/robo-trader-setup-form.component";
import { RoboTraderSetupComponent } from "./robo-trader-setup.component";
import { IndicatorComponent } from "./robo-trader-setup-form/indicator.component";

@NgModule({
  imports: [
    CommonModule,
    RoboTraderSetupRoutingModule,
    NgSelectModule,
    PageHeaderModule,
    TranslateModule,
    FormsModule,
    NgbModule,
    ReactiveFormsModule,
    SharedPipesModule,
    ErrorsModule,
  ],
  declarations: [
    RoboTraderSetupComponent,
    RoboTraderSetupFormComponent,
    IndicatorComponent,
  ],
})
export class RoboTraderSetupModule {}
