using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoManager.Domain.Contracts.Integration;
using CryptoManager.Domain.DTOs;
using CryptoManager.Domain.IntegrationEntities.Blockchains;
using Solnet.Extensions;
using Solnet.Rpc;
using System.Linq;
using Solnet.Extensions.TokenMint;
using Solnet.Wallet;
using Solnet.Programs;

namespace CryptoManager.Integration.BlockchainIntegrationStrategies;

public class SolanaIntegrationStrategy : IBlockchainIntegrationStrategy
{
    private const string _sentinelTraderBotTokenMintAddress = "GVeaBeaHZDJHji4UTzaPJgB1PRiVeCV2EaArjYyiwNdT";
    private const string _tokenProgramId_2022 = "TokenzQdBNbLqP5VEhdkAS6EPFLC1PHnBqCXEpPxuEb";
    private const int _sentinelTraderBotTokenDecimal = 9;
    private readonly TokenDef SentinelToken = new(_sentinelTraderBotTokenMintAddress, "Sentinel Trader Bot", "SENTBOT", _sentinelTraderBotTokenDecimal);
    public BlockchainIntegratedTypes BlockchainIntegratedType => BlockchainIntegratedTypes.Solana;
    private readonly IRpcClient _rpcClient;
    public SolanaIntegrationStrategy(IRpcClient rpcClient)
    {
        _rpcClient = rpcClient;
        TokenProgram.ProgramIdKey.KeyBytes = new PublicKey(_tokenProgramId_2022).KeyBytes;
        TokenProgram.ProgramIdKey.Key = _tokenProgramId_2022;
    }

    public async Task<ObjectResult<IEnumerable<TickerBalanceDTO>>> GetBalancesAsync(string walletAddress)
    {
        if (!PublicKey.IsValid(walletAddress))
        {
            return ObjectResult<IEnumerable<TickerBalanceDTO>>.Error("Invalid Solana wallet address");
        }
        var publicKey = new PublicKey(walletAddress);
        var tokens = new TokenMintResolver();
        tokens.Add(SentinelToken);

        var wallet = await TokenWallet.LoadAsync(_rpcClient, tokens, publicKey);
        var returnList = wallet.TokenAccounts().Select(account =>
            new TickerBalanceDTO
            {
                Symbol = account.Symbol,
                Name = account.TokenName,
                Balance = account.QuantityDecimal,
                PublicKey = account.PublicKey
            }).ToList();
        returnList.Add(new TickerBalanceDTO{
            Symbol = "SOL",
            Name = "Solana",
            Balance = wallet.Sol,
            PublicKey = walletAddress
        });
        return ObjectResult<IEnumerable<TickerBalanceDTO>>.Success(returnList);
    }

    public async Task<SimpleObjectResult> TransferTokenFundsAsync(BlockchainTokenTransferDTO blockchainTokenTransferDTO)
    {
        var accountFeePayer = new Account(
            blockchainTokenTransferDTO.SenderPrivateKey,
            blockchainTokenTransferDTO.SenderWalletAddress);
            
        var tokens = new TokenMintResolver();
        tokens.Add(SentinelToken);
        var wallet = await TokenWallet.LoadAsync(_rpcClient, tokens, accountFeePayer);
        // find source of funds
        var source = wallet.TokenAccounts().ForToken(SentinelToken).FirstOrDefault();

        // single-line SPL send - sends 12.75 SRM to target wallet ATA 
        // if required, ATA will be created funded by feePayer
        var sig = await wallet.SendAsync(source, blockchainTokenTransferDTO.Amount, blockchainTokenTransferDTO.ReceiverWalletAddress, accountFeePayer.PublicKey, txBuilder => txBuilder.Build(accountFeePayer));

        if (!sig.WasSuccessful)
        {
            return SimpleObjectResult.Error(sig.Reason);
        }
        return SimpleObjectResult.Success();
    }

    public async Task<SimpleObjectResult> TestIntegrationUpAsync()
    {
        try
        {
            return await GetBalancesAsync("GZx1AqsFVPVZqWhoVTp3FxUe8eez9aknaZ9TGKD8aGLb"); //token airdrop wallet
        }
        catch (Exception ex)
        {
            return SimpleObjectResult.Error(ex.Message);
        }
    }
}