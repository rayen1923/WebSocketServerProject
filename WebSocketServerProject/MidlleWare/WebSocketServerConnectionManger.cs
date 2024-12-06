﻿using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace WebSocketServerProject.MidlleWare
{
    public class WebSocketServerConnectionManger
    {
        private ConcurrentDictionary<String, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();

        public ConcurrentDictionary<String, WebSocket> GetSockets() { return _sockets; }

        public String AddSocket(WebSocket socket)
        {
            string conId = Guid.NewGuid().ToString();
            _sockets.TryAdd(conId, socket);
            return conId;
        }
    }
}
