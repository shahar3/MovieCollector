using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCollector.Model
{
    public class Actor
    {
        private string actorName;

        public string ActorName
        {
            get { return actorName; }
            set { actorName = value; }
        }

        private string actorUrl;

        public string ActorUrl
        {
            get { return actorUrl; }
            set { actorUrl = value; }
        }

        private string actorPic;

        public string ActorPic
        {
            get { return actorPic; }
            set { actorPic = value; }
        }


        public Actor(string actorName,string actorUrl,string actorPic="")
        {
            this.actorName = actorName;
            this.actorUrl = actorUrl;
            this.actorPic = actorPic;
        }

        public override string ToString()
        {
            return string.Format("{0}", actorName);
        }
    }
}
