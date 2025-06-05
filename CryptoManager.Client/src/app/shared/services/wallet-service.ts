import { Injectable } from "@angular/core";

// import { AppKit, createAppKit } from "@reown/appkit/react";
// import { SolanaAdapter } from "@reown/appkit-adapter-solana/react";
// import { solana, solanaTestnet, solanaDevnet } from "@reown/appkit/networks";
// import {
//   getPhantomWallet,
//   getSolflareWallet,
// } from "@solana/wallet-adapter-wallets";

// 0. Set up Solana Adapter
// const solanaWeb3JsAdapter = new SolanaAdapter({
//   wallets: [new getPhantomWallet(), new SolflareWalletAdapter()],
// });

// 2. Create a metadata object - optional
const metadata = {
  name: "sentineltraderbot",
  description: "AppKit Example",
  url: "https://reown.com/appkit", // origin must match your domain & subdomain
  icons: ["https://assets.reown.com/reown-profile-pic.png"],
};

// 3. Create modal
// createAppKit({
//   adapters: [solanaWeb3JsAdapter],
//   networks: [solana, solanaTestnet, solanaDevnet],
//   metadata: metadata,
//   projectId,
//   features: {
//     analytics: true, // Optional - defaults to your Cloud configuration
//   },
// });

@Injectable()
export class WalletService {
  projectId = "b56e18d47c72ab683b10814fe9495694"; // this is a public projectId only to use on localhost

  constructor() {}

  // test(): AppKit {
  //   return createAppKit({
  //     adapters: [new SolanaAdapter()],
  //     networks: [solana, solanaDevnet, solanaTestnet],
  //     projectId: this.projectId,
  //     themeMode: "light",
  //     themeVariables: {
  //       "--w3m-accent": "#000000",
  //     },
  //     features: {
  //       analytics: true,
  //     },
  //   });
  // }
}
