using System;

namespace Elide.Environment.Configuration
{
    [Serializable]
    public abstract class Config
    {
        protected Config()
        {

        }
     
        public virtual Config Clone()
        {
            return (Config)MemberwiseClone();
        }
    }
}
