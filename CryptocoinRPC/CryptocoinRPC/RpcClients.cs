using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CryptocoinRPC
{
    /// <summary>
    /// Generic JSON RPC Client for cryptocurrencies derived from Bitcoin.
    /// This class runs synchronously.
    /// </summary>
    public class CryptocoinRpcClient
    {
        private string username;
        private string password;
        private string serviceURL;
        private HttpWebRequest request;

        public CryptocoinRpcClient(string username, string password, string serviceURL)
        {
            this.username = username;
            this.password = password;
            this.serviceURL = serviceURL;

            this.request = HttpWebRequest.CreateHttp(serviceURL);
            this.request.Credentials = new NetworkCredential(username, password);
            this.request.ContentType = "application/json-rpc";
            this.request.Method = "POST";
        }

        private JObject InvokeMethod(string method, params object[] parameters)
        {
            var rpcJObject = new JObject();

            rpcJObject["jsonrpc"] = "1.0";
            rpcJObject["id"] = "1";
            rpcJObject["method"] = method;
            rpcJObject["params"] = new JArray();

            if (parameters != null && parameters.Length > 0)
            {
                foreach (object parameter in parameters)
                {
                    ((JArray)rpcJObject["params"]).Add(parameter);
                }
            }

            string json = JsonConvert.SerializeObject(rpcJObject);
            byte[] jsonBytes = Encoding.UTF8.GetBytes(json);

            request.ContentLength = jsonBytes.Length;

            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(jsonBytes, 0, jsonBytes.Length);
            }

            using (WebResponse response = request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader streamReader = new StreamReader(responseStream))
                    {
                        return JsonConvert.DeserializeObject<JObject>(streamReader.ReadToEnd());
                    }
                }
            }
        }

        public void BackupWallet(string backupDestinationPath)
        {
            InvokeMethod("backupwallet", backupDestinationPath);
        }

        public string GetAccount(string address)
        {
            return InvokeMethod("getaccount", address)["result"].ToString();
        }

        public string GetAccountAddress(string account)
        {
            return InvokeMethod("getaccountaddress", account)["result"].ToString();
        }

        public IEnumerable<string> GetAddressesByAccount(string account)
        {
            return from obj in InvokeMethod("getaddressesbyaccount", account)["result"] select obj.ToString();
        }

        public double GetBalance(string account = null, int minConfirmations = 1)
        {
            if (account == null)
            {
                return (double)InvokeMethod("getbalance")["result"];
            }
            else
            {
                return (double)InvokeMethod("getbalance", account, minConfirmations)["result"];
            }
        }

        public string GetBlockByCount(int height)
        {
            return InvokeMethod("getblockbycount", height)["result"].ToString();
        }

        public int GetBlockCount()
        {
            return (int)InvokeMethod("getblockcount")["result"];
        }

        public int GetBlockNumber()
        {
            return (int)InvokeMethod("getblocknumber")["result"];
        }

        public int GetConnectionCount()
        {
            return (int)InvokeMethod("getconnectioncount")["result"];
        }

        public double GetDifficulty()
        {
            return (double)InvokeMethod("getdifficulty")["result"];
        }

        public bool GetGenerate()
        {
            return (bool)InvokeMethod("getgenerate")["result"];
        }

        public double GetHashesPerSecond()
        {
            return (double)InvokeMethod("gethashespersec")["result"];
        }

        public CryptocoinRPC.Info GetInfo()
        {
            return new CryptocoinRPC.Info((JObject)InvokeMethod("getinfo")["result"]);
        }

        public string GetNewAddress(string account)
        {
            return InvokeMethod("getnewaddress", account)["result"].ToString();
        }

        public double GetReceivedByAccount(string account, int minConfirmations = 1)
        {
            return (double)InvokeMethod("getreceivedbyaccount", account, minConfirmations)["result"];
        }

        public CryptocoinRPC.TransactionOfGet GetTransaction(string transactionID)
        {
            return new CryptocoinRPC.TransactionOfGet(InvokeMethod("gettransaction", transactionID)["result"] as JObject);
        }

        public CryptocoinRPC.Work GetWork()
        {
            return new CryptocoinRPC.Work(InvokeMethod("getwork")["result"] as JObject);
        }

        public bool GetWork(string data)
        {
            return (bool)InvokeMethod("getwork", data)["result"];
        }

        public string GetHelp(string command = "")
        {
            return InvokeMethod("help", command)["result"].ToString();
        }

        public Dictionary<string, double> ListAccounts(int minConfirmations = 1)
        {
            string accountDictionaryJson = JsonConvert.SerializeObject(InvokeMethod("listaccounts", minConfirmations)["result"]);
            return JsonConvert.DeserializeObject<Dictionary<string, double>>(accountDictionaryJson);
        }

        public IEnumerable<CryptocoinRPC.ReceivedByAccount> ListReceivedByAccount(int minConfirmations = 1, bool includeEmpty = false)
        {
            JArray receivedJArray = (JArray)InvokeMethod("listreceivedbyaccount", minConfirmations, includeEmpty)["result"];

            var received = new List<CryptocoinRPC.ReceivedByAccount>(receivedJArray.Count);

            foreach (JObject receivedJObject in receivedJArray)
            {
                string account = receivedJObject["account"].ToString();
                double amount = (double)receivedJObject["amount"];
                int confirmations = (int)receivedJObject["confirmations"];
                received.Add(new CryptocoinRPC.ReceivedByAccount(account, amount, confirmations));
            }

            return received;
        }

        public IEnumerable<CryptocoinRPC.ReceivedByAddress> ListReceivedByAddress(int minConfirmationsirmations = 1, bool includeEmpty = false)
        {
            JArray receivedJArray = (JArray)InvokeMethod("listreceivedbyaddress", minConfirmationsirmations, includeEmpty)["result"];

            var received = new List<CryptocoinRPC.ReceivedByAddress>(receivedJArray.Count);

            foreach (JObject receivedJObject in receivedJArray)
            {
                string account = receivedJObject["address"].ToString();
                double amount = (double)receivedJObject["amount"];
                int confirmations = (int)receivedJObject["confirmations"];
                received.Add(new CryptocoinRPC.ReceivedByAddress(account, amount, confirmations));
            }

            return received;
        }

        public IEnumerable<CryptocoinRPC.TransactionOfList> ListTransactions(string account, int count = 5, int from = 0)
        {
            JArray transactionsJArray = (JArray)InvokeMethod("listtransactions", account, count, from)["result"];

            var transactions = new List<CryptocoinRPC.TransactionOfList>(transactionsJArray.Count);

            foreach (JObject transactionJObject in transactionsJArray)
            {
                transactions.Add(new CryptocoinRPC.TransactionOfList(transactionJObject));
            }

            return transactions;
        }

        /// <summary>
        /// Moves amount from one account to another.
        /// </summary>
        /// <param name="fromAccount"></param>
        /// <param name="toAddress"></param>
        /// <param name="amount"></param>
        /// <param name="minConfirmations"></param>
        /// <param name="comment"></param>
        /// <param name="commentTo"></param>
        /// <returns></returns>
        public bool Move(string fromAccount, string toAddress, double amount, int minConfirmations = 1, string comment = "", string commentTo = "")
        {
            return (bool)InvokeMethod("move", fromAccount, toAddress, amount, minConfirmations, comment, commentTo)["result"];
        }

        /// <summary>
        /// Returns the Transaction ID (TxID) if successful.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="amount"></param>
        /// <param name="comment"></param>
        /// <param name="commentTo"></param>
        /// <returns></returns>
        public string SendToAddress(string address, double amount, string comment = "", string commentTo = "")
        {
            return InvokeMethod("sendtoaddress", address, amount, comment, commentTo)["result"].ToString();
        }

        public void SetAccount(string address, string account)
        {
            InvokeMethod("setaccount", address, account);
        }

        public void SetGenerate(bool generate, int generateProcessLimit = 1)
        {
            InvokeMethod("setgenerate", generate, generateProcessLimit);
        }

        public void Stop()
        {
            InvokeMethod("stop");
        }

        public ValidatedAddress ValidateAddress(string address)
        {
            return new ValidatedAddress(InvokeMethod("validateaddress", address)["result"] as JObject);
        }
    }
}
