import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { AuthGuard } from "./shared";

const routes: Routes = [
  {
    path: "",
    loadChildren: () =>
      import("./landing-page/landing-page.module").then(
        (m) => m.LandingPageModule
      ),
  },
  {
    path: "sentinel-trader",
    loadChildren: () =>
      import("./layout/layout.module").then((m) => m.LayoutModule),
    canActivate: [AuthGuard],
    canActivateChild: [AuthGuard],
  },
  {
    path: "login",
    loadChildren: () =>
      import("./login/login.module").then((m) => m.LoginModule),
  },
  // {
  //   path: "signup",
  //   loadChildren: () =>
  //     import("./signup/signup.module").then((m) => m.SignupModule),
  // },
  {
    path: "error",
    loadChildren: () =>
      import("./server-error/server-error.module").then(
        (m) => m.ServerErrorModule
      ),
  },
  {
    path: "access-denied",
    loadChildren: () =>
      import("./access-denied/access-denied.module").then(
        (m) => m.AccessDeniedModule
      ),
  },
  {
    path: "not-found",
    loadChildren: () =>
      import("./not-found/not-found.module").then((m) => m.NotFoundModule),
  },
  {
    path: "health",
    loadChildren: () =>
      import("./health/health.module").then((m) => m.HealthModule),
  },
  {
    path: "privacy-policy",
    loadChildren: () =>
      import("./privacy-policy/privacy-policy.module").then(
        (m) => m.PrivacyPolicyModule
      ),
  },
  {
    path: "white-paper",
    loadChildren: () =>
      import("./white-paper/white-paper.module").then(
        (m) => m.WhitePaperModule
      ),
  },
  { path: "**", redirectTo: "not-found" },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {})],
  exports: [RouterModule],
})
export class AppRoutingModule {}
