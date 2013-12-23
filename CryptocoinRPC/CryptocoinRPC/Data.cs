using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptocoinRPC
{
    public class Info
    {
        public int Version { get; private set; }
        public int ProtocolVersion { get; private set; }
        public int Walletversion { get; private set; }
        public double Balance { get; private set; }
        public int Blocks { get; private set; }
        public int Connections { get; private set; }
        public string Proxy { get; private set; }
        public double Difficulty { get; private set; }
        public bool TestNet { get; private set; }
        public int KeyPoolOldest { get; private set; }
        public int KeyPoolSize { get; private set; }
        public double PayTxFee { get; private set; }
        public string Errors { get; private set; }

        public Info(JObject infoJObject)
        {
            this.Version = (int)infoJObject["version"];
            this.ProtocolVersion = (int)infoJObject["protocolversion"];
            this.Walletversion = (int)infoJObject["walletversion"];
            this.Balance = (double)infoJObject["balance"];
            this.Blocks = (int)infoJObject["blocks"];
            this.Connections = (int)infoJObject["connections"];
            this.Proxy = infoJObject["proxy"].ToString();
            this.Difficulty = (double)infoJObject["difficulty"];
            this.TestNet = infoJObject.ToString() == "true" ? true : false;
            this.KeyPoolOldest = (int)infoJObject["keypoololdest"];
            this.KeyPoolSize = (int)infoJObject["keypoolsize"];
            this.PayTxFee = (double)infoJObject["paytxfee"];
            this.Errors = infoJObject.ToString();
        }
    }

    public class TransactionOfGet
    {
        public double Amount { get; private set; }
        public double Fee { get; private set; }
        public int Confirmations { get; private set; }
        public string BlockHash { get; private set; }
        public int BlockIndex { get; private set; }
        public int BlockTime { get; private set; }
        public string TxID { get; private set; }
        public int Time { get; private set; }
        public int TimeReceived { get; private set; }
        public IEnumerable<TransactionOfGet.Detail> Details { get; private set; }

        public TransactionOfGet(JObject transactionJObject)
        {
            this.Amount = (double)transactionJObject["amount"];
            this.Fee = (double)transactionJObject["fee"];
            this.Confirmations = (int)transactionJObject["confirmations"];
            this.BlockHash = transactionJObject["blockhash"].ToString();
            this.BlockIndex = (int)transactionJObject["blockindex"];
            this.BlockTime = (int)transactionJObject["blocktime"];
            this.TxID = transactionJObject["txid"].ToString();
            this.Time = (int)transactionJObject["time"];
            this.TimeReceived = (int)transactionJObject["timereceived"];

            var detailsJArray = (JArray)transactionJObject["details"];
            var details = new List<TransactionOfGet.Detail>(detailsJArray.Count);
            
            foreach (JObject detailJObject in detailsJArray)
            {
                details.Add(new TransactionOfGet.Detail(detailJObject));
            }

            this.Details = details;
        }

        public class Detail
        {
            public Detail(JObject detailJObject)
            {
                this.Account = detailJObject.ToString();
                this.Address = detailJObject.ToString();
                this.Category = detailJObject.ToString();
                this.Amount = (double)detailJObject;
                this.Fee = (double)detailJObject;
            }

            public string Account { get; private set; }
            public string Address { get; private set; }
            public string Category { get; private set; }
            public double Amount { get; private set; }
            public double Fee { get; private set; }
        }
    }

    public class Work
    {
        public string MidState { get; private set; }
        public string Data { get; private set; }
        public string Hash1 { get; private set; }
        public string Target { get; private set; }
        public string Algorithm { get; private set; }

        public Work(JObject workJObject)
        {
            this.MidState = workJObject["midstate"].ToString();
            this.Data = workJObject["data"].ToString();
            this.Hash1 = workJObject["hash1"].ToString();
            this.Target = workJObject["target"].ToString();
            this.Algorithm = workJObject["algorithm"].ToString();
        }
    }

    public class ReceivedByAccount
    {
        public string Account { get; private set; }
        public double Amount { get; private set; }
        public int Confirmations { get; private set; }

        public ReceivedByAccount(string account, double amount, int confirmations)
        {
            this.Account = account;
            this.Amount = amount;
            this.Confirmations = confirmations;
        }
    }

    public class ReceivedByAddress
    {
        public string Address { get; private set; }
        public double Amount { get; private set; }
        public int Confirmations { get; private set; }

        public ReceivedByAddress(string address, double amount, int confirmations)
        {
            this.Address = address;
            this.Amount = amount;
            this.Confirmations = confirmations;
        }
    }

    public class TransactionOfList
    {
        public string Account { get; private set; }
        public string Address { get; private set; }
        public string Category { get; private set; }
        public double Amount { get; private set; }
        public int Confirmations { get; private set; }
        public string BlockHash { get; private set; }
        public int BlockIndex { get; private set; }
        public string TxID { get; private set; }
        public int Time { get; private set; }

        public TransactionOfList(JObject transactionJObject)
        {
            this.Account = transactionJObject["account"].ToString();
            this.Address = transactionJObject["address"].ToString();
            this.Category = transactionJObject["category"].ToString();
            this.Amount = (double)transactionJObject["amount"];
            this.Confirmations = (int)transactionJObject["confirmations"];
            this.BlockHash = transactionJObject["blockhash"].ToString();
            this.BlockIndex = (int)transactionJObject["blockindex"];
            this.TxID = transactionJObject["txid"].ToString();
            this.Time = (int)transactionJObject["time"];
        }
    }
    
    public class ValidatedAddress
    {
        public bool IsValid { get; private set; }
        public string Address { get; private set; }
        public bool IsMine { get; private set; }
        public bool IsScript { get; private set; }
        public string PubKey { get; private set; }
        public bool IsCompressed { get; private set; }
        public string Account { get; private set; }

        public ValidatedAddress(JObject validatedAddressJObject)
        {
            this.IsValid = validatedAddressJObject["isvalid"].ToString() == "true" ? true : false;
            this.Address = validatedAddressJObject["address"].ToString();
            this.IsMine = validatedAddressJObject["ismine"].ToString() == "true" ? true : false;
            this.IsScript = validatedAddressJObject["isscript"].ToString() == "true" ? true : false;
            this.PubKey = validatedAddressJObject["pubkey"].ToString();
            this.IsCompressed = validatedAddressJObject["iscompressed"].ToString() == "true" ? true : false;
            this.Account = validatedAddressJObject["account"].ToString();
        }
    }
}
