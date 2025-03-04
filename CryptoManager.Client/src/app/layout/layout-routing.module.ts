import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { LayoutComponent } from "./layout.component";

const routes: Routes = [
  {
    path: "",
    component: LayoutComponent,
    children: [
      {
        path: "",
        redirectTo: "dashboard",
        pathMatch: "full",
      },
      {
        path: "dashboard",
        loadChildren: () =>
          import("./dashboard/dashboard.module").then((m) => m.DashboardModule),
      },
      {
        path: "exchange",
        loadChildren: () =>
          import("./exchange/exchange.module").then((m) => m.ExchangeModule),
      },
      {
        path: "asset",
        loadChildren: () =>
          import("./asset/asset.module").then((m) => m.AssetModule),
      },
      {
        path: "order",
        loadChildren: () =>
          import("./order/order.module").then((m) => m.OrderModule),
      },
      {
        path: "robo-trader-setup",
        loadChildren: () =>
          import("./robo-trader-setup/robo-trader-setup.module").then(
            (m) => m.RoboTraderSetupModule
          ),
      },
      {
        path: "robo-trader-order",
        loadChildren: () =>
          import("./robo-trader-order/robo-trader-order.module").then(
            (m) => m.RoboTraderOrderModule
          ),
      },
      {
        path: "back-test-trader-order",
        loadChildren: () =>
          import("./back-test-trader-order/back-test-trader-order.module").then(
            (m) => m.BackTestTraderOrderModule
          ),
      },
      {
        path: "tables",
        loadChildren: () =>
          import("./tables/tables.module").then((m) => m.TablesModule),
      },
      {
        path: "forms",
        loadChildren: () =>
          import("./form/form.module").then((m) => m.FormModule),
      },
      {
        path: "bs-element",
        loadChildren: () =>
          import("./bs-element/bs-element.module").then(
            (m) => m.BsElementModule
          ),
      },
      {
        path: "grid",
        loadChildren: () =>
          import("./grid/grid.module").then((m) => m.GridModule),
      },
      {
        path: "blank-page",
        loadChildren: () =>
          import("./blank-page/blank-page.module").then(
            (m) => m.BlankPageModule
          ),
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class LayoutRoutingModule {}
