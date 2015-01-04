﻿using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using NBitcoin.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidBase
{
    public class CallbackRepository
    {
        public CallbackRepository(RapidBaseConfiguration configuration)
        {
            Configuration = configuration;
            _Table = new CrudTable<CallbackRegistration>(Configuration.GetCallbackTable());
        }

        CrudTable<CallbackRegistration> _Table;

        public RapidBaseConfiguration Configuration
        {
            get;
            set;
        }
        public CallbackRegistration CreateCallback(string eventName, CallbackRegistration callback)
        {
            callback.Id = null;
            var callbackStr = Serializer.ToString(callback);
            var id = Hash(callbackStr);
            callback.Id = id;
            _Table.Create(eventName, id, callback);
            return callback;
        }

        private string Hash(string data)
        {
            return Hashes.Hash256(Encoding.UTF8.GetBytes(data)).ToString();
        }

        public CallbackRegistration[] GetCallbacks(string eventName)
        {
            return _Table.Read(eventName);
        }

        public bool Delete(string eventName, string id)
        {
            return _Table.Delete(eventName, id);
        }
    }
}