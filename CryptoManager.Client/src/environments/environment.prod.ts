export const environment = {
  api: {
    baseUrl: "#{cryptomanager_server_base_url}#",
    roboTraderBaseUrl: "#{robotrader_base_url}#",
    version: "1.0.0",
    solanaRpcUrl: "https://api.mainnet-beta.solana.com",
  },
  login: {
    path: "login",
    defaultRedirectTo: "dash",
    facebook: {
      appId: "#{facebook_app_id}#",
    },
    google: {
      appId: "#{google_app_id}#",
    },
  },
  production: true,
};
