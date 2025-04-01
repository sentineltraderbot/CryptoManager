module.exports = (config) => {
  config.resolve.fallback = {
    path: false,
    fs: false,
    os: false,
    crypto: false,
    process: false,
    util: false,
    assert: false,
    stream: false,
    http: require.resolve("stream-http"),
    https: require.resolve("https-browserify"),
    zlib: require.resolve("browserify-zlib"),
    url: require.resolve("url/"),
  };

  return config;
};
