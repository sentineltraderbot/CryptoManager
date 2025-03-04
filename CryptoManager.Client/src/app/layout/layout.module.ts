import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { TranslateModule } from "@ngx-translate/core";
import { NgbDropdownModule } from "@ng-bootstrap/ng-bootstrap";
import { NgbModule } from "@ng-bootstrap/ng-bootstrap";

import { LayoutRoutingModule } from "./layout-routing.module";
import { LayoutComponent } from "./layout.component";
import { SidebarComponent } from "./components/sidebar/sidebar.component";
import { HeaderComponent } from "./components/header/header.component";
import { AlertModule, SharedPipesModule } from "../shared";
import { FormsModule } from "@angular/forms";
import { SolanaWalletConnectorModalComponent } from "./solana-wallet-connector-modal/solana-wallet-connector-modal.component";
import { TokenBalanceComponent } from "./components/token-balance/token-balance.component";

@NgModule({
  imports: [
    CommonModule,
    LayoutRoutingModule,
    TranslateModule,
    NgbDropdownModule,
    NgbModule,
    AlertModule,
    SharedPipesModule,
    FormsModule,
  ],
  declarations: [
    LayoutComponent,
    SidebarComponent,
    HeaderComponent,
    SolanaWalletConnectorModalComponent,
    TokenBalanceComponent,
  ],
})
export class LayoutModule {}
