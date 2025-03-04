import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { NgbCarouselModule, NgbAlertModule } from "@ng-bootstrap/ng-bootstrap";

import { DashboardRoutingModule } from "./dashboard-routing.module";
import { DashboardComponent } from "./dashboard.component";
import {
  TimelineComponent,
  NotificationComponent,
  ChatComponent,
} from "./components";
import { SharedPipesModule, StatModule } from "../../shared";
import { TranslateModule } from "@ngx-translate/core";
import { FormsModule } from "@angular/forms";

@NgModule({
  imports: [
    CommonModule,
    NgbCarouselModule,
    NgbAlertModule,
    DashboardRoutingModule,
    StatModule,
    SharedPipesModule,
    TranslateModule,
    FormsModule,
  ],
  declarations: [
    DashboardComponent,
    TimelineComponent,
    NotificationComponent,
    ChatComponent,
  ],
})
export class DashboardModule {}
