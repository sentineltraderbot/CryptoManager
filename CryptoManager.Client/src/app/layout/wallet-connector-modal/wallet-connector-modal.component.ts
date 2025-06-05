import { Component, OnInit } from "@angular/core";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { WalletName } from "@solana/wallet-adapter-base";
import { Wallet } from "@heavy-duty/wallet-adapter";
import { WalletService } from "../../shared";
@Component({
  templateUrl: "./wallet-connector-modal.component.html",
  styleUrl: "./wallet-connector-modal.component.scss",
})
export class WalletConnectorModalComponent implements OnInit {
  wallets: Wallet[] = null;
  walletName: WalletName<string> | "None" = null;
  errorMessage: string = null;

  constructor(
    public activeModal: NgbActiveModal,
    public readonly walletService: WalletService
  ) {}

  ngOnInit() {}

  onConnect() {}

  onDisconnect() {}

  onSelectWallet(walletName: WalletName) {}

  onSaveWallet() {}

  isChrome(): boolean {
    const userAgent = navigator.userAgent;
    return /Chrome/.test(userAgent) && !/Edg|OPR/.test(userAgent);
  }

  isMobile(): boolean {
    return /Android|iPhone|iPad|iPod|Windows Phone/i.test(navigator.userAgent);
  }
}
