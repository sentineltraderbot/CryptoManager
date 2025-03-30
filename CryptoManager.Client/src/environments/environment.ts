// The file contents for the current environment will overwrite these during build.
// The build system defaults to the dev environment which uses `environment.ts`, but if you do
// `ng build --env=prod` then `environment.prod.ts` will be used instead.
// The list of which env maps to which file can be found in `.angular-cli.json`.

export const environment = {
  api: {
    baseUrl: "https://localhost:62329/api",
    roboTraderBaseUrl: "https://localhost:7170/api",
    solanaRpcUrl: "https://solana.api.onfinality.io/public",
    version: "1.0.0",
  },
  login: {
    path: "login",
    defaultRedirectTo: "dash",
    facebook: {
      appId: "1293953147938399",
    },
    google: {
      appId:
        "385478011304-h6bcrfgf57vp90prfe83el2pp1l2phla.apps.googleusercontent.com",
    },
  },
  production: false,
};
