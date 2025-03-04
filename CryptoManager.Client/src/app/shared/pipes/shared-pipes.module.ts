import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { AppDatePipe } from "./app-date.pipe";
import { PriceChangePipe } from "./price-change.pipe";

@NgModule({
  imports: [CommonModule],
  declarations: [AppDatePipe, PriceChangePipe],
  providers: [AppDatePipe, PriceChangePipe],
  exports: [AppDatePipe, PriceChangePipe],
})
export class SharedPipesModule {}
