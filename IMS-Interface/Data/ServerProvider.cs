﻿using IMS_Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMS_Interface.Data
{
    public class ServerProvider
    {
        protected Guid ServerID;
        public Guid CurrentServerID {
            get
            {
                if (ServerID == Guid.Empty)
                {
                    IList<ServerProxy> servers = IMS.Instance.ServerManager.Servers;
                    if (servers.Count > 0)
                    {
                        return ServerID = servers[0].ID;
                    }
                    else
                    {
                        return Guid.Empty;
                    }
                }
                else if(IMS.Instance.ServerManager.GetServer(ServerID) is null)
                {
                    ServerID = Guid.Empty;
                    return CurrentServerID;
                }
                return ServerID;
            }
            set
            {
                ServerID = value;
                OnServerSelectionChange?.Invoke();
            }
        }
        public ServerProxy CurrentServer
        {
            get => IMS.Instance.ServerManager.GetServer(CurrentServerID);
            set => CurrentServerID = value.ID;
        }
        public event Action OnServerSelectionChange;
    }
}
