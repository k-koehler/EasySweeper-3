using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyWinterface
{
    class Player
    {
        private int? _id;
        private string _user;

        public int? ID => _id;
        public string User => _user;

        public bool OCRFailed => _user == null || _user.ToLower() == "(unknown)";

        public Player(int? id, string user)
        {
            _id = id;
            _user = user;
        }

        public Player(string name)
            : this(null, name)
        { }


    }
}
