import { NgModule } from "@angular/core";
import { WhitePaperComponent } from "./white-paper.component";
import { RouterModule, Routes } from "@angular/router";

const routes: Routes = [
  {
    path: "",
    component: WhitePaperComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class WhitePaperRoutingModule {}
