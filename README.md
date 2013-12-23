CryptocoinRPC.NET
=================

JSON RPC client for cryptocurrencies derived from Bitcoin.

__Example__
```C#
class Program
{
	static void Main(string[] args)
	{
	  var username = "Bit";
	  var password = "Coin";
	  var port = 22555;
	  var client = new CryptocoinRpcClient(username, password, "127.0.0.1:" + port);
	  
	  Console.WriteLine("Difficulty: " + client.GetDifficulty());
	}
}
```
