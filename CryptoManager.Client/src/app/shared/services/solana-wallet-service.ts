import { computed, Injectable } from "@angular/core";

import {
  ConnectionStore,
  injectConnected,
  injectPublicKey,
  injectWallet,
  injectWallets,
  Wallet,
  WalletStore,
} from "@heavy-duty/wallet-adapter";
import { environment } from "../../../environments/environment";
import { WalletName } from "@solana/wallet-adapter-base";
import { PhantomWalletAdapter } from "@solana/wallet-adapter-phantom";
import { SolflareWalletAdapter } from "@solana/wallet-adapter-solflare";
import { Observable } from "rxjs";

@Injectable()
export class SolanaWalletService {
  public readonly isSolanaWalletConnected = injectConnected();
  public readonly publicKey = injectPublicKey();
  private readonly wallets = injectWallets();
  private readonly wallet = injectWallet();
  private readonly walletName = computed(
    () => this.wallet()?.adapter.name ?? "None"
  );
  constructor(
    private readonly walletStore: WalletStore,
    private readonly hdConnectionStore: ConnectionStore
  ) {
    this.hdConnectionStore.setEndpoint(environment.api.solanaRpcUrl);
  }

  setWalletAdapters() {
    this.walletStore.setAdapters([
      new PhantomWalletAdapter(),
      new SolflareWalletAdapter(),
    ]);
  }

  getWallets(): Wallet[] {
    return this.wallets();
  }

  getWallet(): Wallet {
    return this.wallet();
  }

  getWalletName(): WalletName<string> | "None" {
    return this.walletName();
  }

  selectWallet(walletName: WalletName) {
    this.walletStore.selectWallet(walletName);
  }

  connect(): Observable<void> {
    return this.walletStore.connect();
  }

  disconnect(): Observable<void> {
    return this.walletStore.disconnect();
  }
}
