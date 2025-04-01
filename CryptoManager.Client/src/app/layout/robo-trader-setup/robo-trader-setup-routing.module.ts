import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { RoboTraderSetupComponent } from "./robo-trader-setup.component";
import { RoboTraderSetupFormComponent } from "./robo-trader-setup-form/robo-trader-setup-form.component";

const routes: Routes = [
  { path: "", component: RoboTraderSetupComponent, pathMatch: "full" },
  { path: "new", component: RoboTraderSetupFormComponent },
  { path: ":id", component: RoboTraderSetupFormComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class RoboTraderSetupRoutingModule {}
