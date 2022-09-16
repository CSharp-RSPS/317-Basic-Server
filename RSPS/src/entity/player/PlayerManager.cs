using RSPS.src.net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.entity.player
{
    public class PlayerManager : EntityManager<Player>
    {


        public Player AddPlayer(string username, string password, Connection connection) {
            return Add(new(username, password, connection));
        }

        public override Player Add(Player entity) {
            base.Add(entity);

            entity.PlayerIndex = Entities.IndexOf(entity);
            return entity;
        }

        public override void Remove(Player entity) {
            //TODO any possible logic we might still need to do
            base.Remove(entity);
        }

        /// <summary>
        /// Logs the player out
        /// </summary>
        /// <param name="player"></param>
        public void Logout(Player player) {
            //TODO logout logic
            Remove(player);
        }

        public override void Dispose() {
            GC.SuppressFinalize(this);

            //TODO logout all players

            base.Dispose();
        }

    }
}
