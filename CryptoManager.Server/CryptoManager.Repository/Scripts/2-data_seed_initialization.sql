BEGIN TRANSACTION

IF NOT EXISTS (SELECT * FROM Exchange)
BEGIN
    INSERT INTO Exchange ("Id","APIUrl","ExchangeType","IsEnabled","IsExcluded","Name","RegistryDate","Website") 
    VALUES 
        ('1C544480-7EA3-234C-A9B6-991167B82392','https://api.binance.com/api',0,1,0,'Binance','2018-06-07 12:50:39.3017201','https://www.binance.com'),
        ('E60BD760-1FD1-4A4C-A1F7-085F2DA62254','https://api.hitbtc.com/api',1,1,0,'HitBTC','2018-06-11 17:02:52.8790758','https://hitbtc.com'),
        ('31C3B03F-F7C6-9B4B-964A-77269E36AF0E','https://api.exchange.coinbase.com',2,1,0,'Coinbase','2019-04-13 23:18:20.4237757','https://coinbase.com'),
        ('065A0EBE-0AB7-194E-B704-FB465057BE03','https://api.bitcointrade.com.br',3,1,0,'BitcoinTrade','2019-04-29 19:11:31.735434','https://www.bitcointrade.com.br/'),
        ('2406C310-E84A-4FF3-AC8E-66AD910A7500','https://api.kucoin.com/api',4,1,0,'KuCoin','2021-12-07 10:29:19.3648945','https://www.kucoin.com/')
END

IF NOT EXISTS (SELECT * FROM Asset)
BEGIN

    INSERT INTO Asset ("Id","Description","IsEnabled","IsExcluded","Name","RegistryDate","Symbol")
    VALUES 
        ('1926C8D8-2272-1940-A928-8349FA0E640C','Bitcoin',1,0,'Bitcoin','2018-06-07 12:50:59.4861521','BTC'),
        ('21F809F6-AB7C-0743-926E-DD7DCC70B374','Litecoin',1,0,'Litecoin','2018-06-07 12:51:10.129635','LTC'),
        ('251740F2-83C1-3742-AF74-156ABE66213D','Dolar Tether',1,0,'dolar Tether','2018-06-07 12:51:49.5512384','USDT'),
        ('913C2F21-0024-D84C-BF33-60562A527D16','Tron',1,0,'Tron','2018-06-07 12:52:19.9682691','TRX'),
        ('5BE7E6DA-CF4F-8543-873B-E9B5A9042428','Verge',1,0,'Verge','2018-06-07 12:52:33.7107543','XVG'),
        ('D4BA5F6A-0E5A-CC45-864A-7EA883FB018E','Binance Coin',1,0,'Binance Coin','2018-06-07 12:52:48.2301559','BNB'),
        ('9472110C-2133-3546-9863-F1C6EC54F738','Ripple',1,0,'Ripple','2018-06-07 12:53:06.2467312','XRP'),
        ('5CBFEC26-0064-6E49-91A2-2A7869771AD1','Ethereum',1,0,'Ethereum','2018-06-07 12:54:37.2006801','ETH'),
        ('85D56FE7-FD46-8640-A824-DD7294F0CB4E','British Pound',1,0,'British Pound','2019-04-13 23:17:09.0341332','GBP'),
        ('632BED92-A433-C94B-B1B0-7F4C637A4775','Brazilian Real',1,0,'Brazilian Real','2019-04-29 19:11:57.4235654','BRL'),
        ('6568BE5E-2FC7-F545-890D-5F6504C8EB04','Euro',1,0,'Euro','2019-08-06 07:48:23.051011','EUR'),
        ('F5359A31-1FE6-0245-9AF6-5733E4D69EFB','Stellar Lumens',1,0,'Stellar Lumens','2019-08-06 07:52:36.6722275','XLM'),
        ('EBB8ADCD-76C7-4EE1-917D-047EFE17C804','Polkadot',1,0,'Polkadot','2021-12-25 22:11:33.6973869','DOT'),
        ('E4978B84-1665-4AC4-A77F-21B34C2F96B8','Polygon MATIC',1,0,'Polygon MATIC','2023-04-07 18:27:19.923677','POL'),
        ('3D40159F-B97D-46D4-859F-EB734164141A','Solana',1,0,'Solana','2024-04-26 18:16:36.742764','SOL'),
        ('B4211E5B-7D1B-42E8-9C15-16876399F233','MEME Coin',1,0,'DogeCoin','2025-01-25 09:56:00.957731','DOGE'),
        ('DF13A963-D534-40F4-B787-CEDFBDA7BEE8','Hedera',1,0,'HBAR','2025-01-25 09:59:33.65845','HBAR')

END
Rollback