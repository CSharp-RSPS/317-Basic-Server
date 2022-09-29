using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.entity.Mobiles.Players
{
    public class PlayerCredentials
    {

        public int Uid { get; private set; }

        public string Username { get; private set; }

        public string Password { get; set; }

        public long UsernameAsLong => Misc.EncodeBase37(Username);


        public PlayerCredentials(int uid, string username, string password)
        {
            Uid = uid;
            Username = username;
            Password = password;
        }
    }
}
