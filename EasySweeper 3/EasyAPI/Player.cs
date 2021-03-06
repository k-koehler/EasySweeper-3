﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyAPI
{
    public class Player
    {
        private int? _id;
        private string _user;

        public int? ID => _id;
        public string User => _user;

        public bool OCRFailed => _user == null || _user.ToLower() == UNKNOWN_NAME;

        public const string UNKNOWN_NAME = "(unknown)";

        public Player(int? id, string user)
        {
            _id = id;
            _user = user == "" ? UNKNOWN_NAME : user;
        }

        public Player(string name)
            : this(null, name)
        { }
    }
}
