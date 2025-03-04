import { Component, OnInit } from "@angular/core";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { WalletName, WalletReadyState } from "@solana/wallet-adapter-base";
import { Observable } from "rxjs";
import { SolanaWalletService } from "../../shared/services/solana-wallet-service";
import { Wallet } from "@heavy-duty/wallet-adapter";
import { AlertHandlerService, AlertType } from "../../shared";
@Component({
  templateUrl: "./solana-wallet-connector-modal.component.html",
  styleUrl: "./solana-wallet-connector-modal.component.scss",
})
export class SolanaWalletConnectorModalComponent implements OnInit {
  wallets: Wallet[] = null;
  walletName: WalletName<string> | "None" = null;
  constructor(
    public activeModal: NgbActiveModal,
    public readonly solanaWalletService: SolanaWalletService
  ) {}

  ngOnInit() {
    this.wallets = this.solanaWalletService.getWallets();
    this.walletName = this.solanaWalletService.getWalletName();
  }

  onConnect() {
    this.solanaWalletService.connect().subscribe({
      next: () => console.log("Wallet connected"),
      error: (error) => console.error(error),
    });
  }

  onDisconnect() {
    this.solanaWalletService.disconnect().subscribe({
      next: () => console.log("Wallet disconnected"),
      error: (error) => console.error(error),
    });
  }

  onSelectWallet(walletName: WalletName) {
    this.solanaWalletService.selectWallet(walletName);
  }

  onClaimAirdrop() {
    this.activeModal.close(
      this.solanaWalletService.getWallet().adapter.publicKey.toBase58()
    );
  }
}
