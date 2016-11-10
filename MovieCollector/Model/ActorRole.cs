using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCollector.Model
{
    public class ActorRole
    {
        private Actor actor;

        public Actor Actor 
        {
            get { return actor; }
            set { actor = value; }
        }

        private string actorName;

        public string ActorName
        {
            get { return actorName; }
            set { actorName = value; }
        }


        private string role;

        public string Role
        {
            get { return role; }
            set { role = value; }
        }

        private string actorPic;

        public string ActorPic
        {
            get { return actorPic; }
            set { actorPic = value; }
        }


        public ActorRole(Actor actor,string role)
        {
            this.actorName = actor.ActorName;
            this.actorPic = actor.ActorPic;
            this.actor = actor;
            this.role = role;
        }
    }
}
