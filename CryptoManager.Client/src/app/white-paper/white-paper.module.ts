import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { WhitePaperComponent } from "./white-paper.component";
import { WhitePaperRoutingModule } from "./white-paper-routing.module";

@NgModule({
  declarations: [WhitePaperComponent],
  imports: [CommonModule, WhitePaperRoutingModule],
})
export class WhitePaperModule {}
