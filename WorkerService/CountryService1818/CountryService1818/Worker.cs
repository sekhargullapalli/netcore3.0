using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CountryService1818
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private CountriesContext _context;

        public Worker(ILogger<Worker> logger, CountriesContext context)
        {
            _logger = logger;
            _context = context;
        }

        #region override background service methods
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Country service started (UTC): {DateTimeOffset.UtcNow}");
            return base.StartAsync(cancellationToken);
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Country service stopped (UTC): {DateTimeOffset.UtcNow}");
            return base.StopAsync(cancellationToken);
        }
        public override void Dispose()
        {
            _logger.LogInformation($"Country service stopped (UTC): {DateTimeOffset.UtcNow}");
            base.Dispose();
        }
        #endregion override background service methods

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                TcpListener server = null;
                try
                {
                    IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                    Int32 port = 1818;
                    server = new TcpListener(localAddr, port);
                    //server.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.MaxConnections, 100);
                    server.Start();
                    _logger.LogInformation("Country Details Service Started ..");
                    Byte[] bytes = new Byte[256];
                    String data = null;
                    while (true)
                    {
                        _logger.LogInformation("Wainting for a connection..");
                        TcpClient client = await server.AcceptTcpClientAsync();
                        _logger.LogInformation("Connected!");
                        data = null;
                        NetworkStream stream = client.GetStream();
                        
                        byte[] initmessage = Encoding.ASCII.GetBytes("Enter a query string (max 15 characters) or CRTL+c to quit: ");
                        stream.Write(initmessage, 0, initmessage.Length);
                        
                        int i;
                        while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            data = Encoding.ASCII.GetString(bytes, 0, i);
                            string qs = data.Trim().Length > 15 ? data.Trim().Substring(0, 15) : data.Trim();
                            var countries = _context.Countries.Where(c => c.Name.ToLower().Contains(qs.ToLower())
                            || c.LocalName.ToLower().Contains(qs.ToLower())
                            || c.Code.ToLower().Contains(qs.ToLower())).Select(c => c);
                            StringBuilder replystring = new StringBuilder();
                            replystring.AppendLine();
                            replystring.AppendLine();
                            replystring.AppendLine($"Query String: {qs}");
                            if (countries.Count() == 0)
                            {
                                replystring.AppendLine("No matching countries");
                            }
                            else
                            {
                                int index = 1;
                                foreach (var country in countries.OrderBy(c => c.Name))
                                {
                                    replystring.AppendLine($"Match {index}");
                                    replystring.AppendLine(country.ToString());
                                    replystring.AppendLine("=============================================");
                                    index++;
                                }
                            }
                            replystring.AppendLine();
                            replystring.Append("Enter another query string (max 15 characters) or CRTL+c to quit: ");

                            byte[] msg = Encoding.ASCII.GetBytes(replystring.ToString());
                            stream.Write(msg, 0, msg.Length);
                            _logger.LogInformation($"Sent results for: {qs} [{countries.Count()} matches]");
                        }
                        client.Close();
                    }
                }
                catch (SocketException e)
                {
                    Console.WriteLine($"Socket Exception: {e}");
                }
                finally
                {
                    server.Stop();
                }
            }
        }
    }
}
