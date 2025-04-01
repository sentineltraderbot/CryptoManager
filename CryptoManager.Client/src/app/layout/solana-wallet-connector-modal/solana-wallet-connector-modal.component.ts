import { Component, OnInit } from "@angular/core";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { WalletName } from "@solana/wallet-adapter-base";
import { SolanaWalletService } from "../../shared/services/solana-wallet-service";
import { Wallet } from "@heavy-duty/wallet-adapter";
@Component({
  templateUrl: "./solana-wallet-connector-modal.component.html",
  styleUrl: "./solana-wallet-connector-modal.component.scss",
})
export class SolanaWalletConnectorModalComponent implements OnInit {
  wallets: Wallet[] = null;
  walletName: WalletName<string> | "None" = null;
  errorMessage: string = null;

  constructor(
    public activeModal: NgbActiveModal,
    public readonly solanaWalletService: SolanaWalletService
  ) {}

  ngOnInit() {
    if (this.isChrome()) {
      this.wallets = this.solanaWalletService.getWallets();
      this.walletName = this.solanaWalletService.getWalletName();
    }
  }

  onConnect() {
    this.solanaWalletService.connect().subscribe({
      next: () => {
        console.log("Wallet connected");
        this.errorMessage = null;
      },
      error: (error) => {
        this.errorMessage = error;
        console.error(error);
      },
    });
  }

  onDisconnect() {
    this.solanaWalletService.disconnect().subscribe({
      next: () => {
        console.log("Wallet disconnected");
        this.walletName = null;
        this.errorMessage = null;
      },
      error: (error) => {
        this.errorMessage = error;
        console.error(error);
      },
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

  isChrome(): boolean {
    const userAgent = navigator.userAgent;
    return /Chrome/.test(userAgent) && !/Edg|OPR/.test(userAgent);
  }

  isMobile(): boolean {
    return /Android|iPhone|iPad|iPod|Windows Phone/i.test(navigator.userAgent);
  }
}
