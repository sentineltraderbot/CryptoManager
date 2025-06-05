import { Component, EventEmitter, Input, Output } from "@angular/core";
import {
  AccountService,
  AlertHandlerService,
  AlertType,
  SentinelTraderBotTokenService,
  SolanaWalletService,
  UserDetails,
} from "../../../shared";
import { TranslateService } from "@ngx-translate/core";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { SolanaWalletConnectorModalComponent } from "../../solana-wallet-connector-modal/solana-wallet-connector-modal.component";

@Component({
  selector: "app-token-balance-component",
  templateUrl: "./token-balance.component.html",
  styleUrl: "./token-balance.component.scss",
})
export class TokenBalanceComponent {
  private _userDetails: UserDetails;

  @Input()
  set userDetails(value: UserDetails) {
    this._userDetails = value;
    this.onUserDetailsChanged();
  }

  get userDetails(): UserDetails {
    return this._userDetails;
  }

  onUserDetailsChanged() {
    this.getTokenAccountBalance();
  }
  @Output() userDetailsChanged: EventEmitter<boolean> =
    new EventEmitter<boolean>();

  readonly tokenSymbol = "SENTBOT";
  sentinelTraderBotTokenValue: number = null;

  constructor(
    private translate: TranslateService,
    private alertService: AlertHandlerService,
    private modalService: NgbModal,
    private sentinelTraderBotTokenService: SentinelTraderBotTokenService,
    private accountService: AccountService,
    public solanaWalletService: SolanaWalletService
  ) {}

  ngOnInit() {
    this.getTokenAccountBalance();
  }

  openSolanaWalletConnectorModal() {
    const modalRef = this.modalService.open(
      SolanaWalletConnectorModalComponent
    );

    modalRef.result
      .then((userResponse) => {
        if (userResponse) {
          this.accountService.udpateUserWallet(userResponse).subscribe({
            next: () => {
              this.alertService.createAlert(
                AlertType.Success,
                this.translate.instant("Success")
              );
              this.userDetailsChanged.emit(true);
              this.getTokenAccountBalance();
            },
            error: () => {
              this.alertService.createAlert(
                AlertType.Danger,
                this.translate.instant("CouldNotProcess")
              );
              this.userDetailsChanged.emit(false);
            },
          });
        }
      })
      .catch(() => {});
  }

  getTokenAccountBalance() {
    if (!this.userDetails || !this.userDetails.solanaWalletAddress) {
      return;
    }
    this.sentinelTraderBotTokenService
      .getBalancesBySolanaWallet(this.userDetails.solanaWalletAddress)
      .subscribe({
        next: (data) => {
          this.sentinelTraderBotTokenValue = data.item
            .filter((a) => a.symbol === this.tokenSymbol)
            .map((a) => a.balance)[0];
        },
        error: () => {
          this.alertService.createAlert(
            AlertType.Danger,
            this.translate.instant("CouldNotProcess")
          );
          this.sentinelTraderBotTokenValue = null;
        },
      });
  }
}
