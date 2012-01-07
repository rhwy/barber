
namespace ArtOfNet.ACR
{
    public interface IRtcController
    {
        ControllerResult Execute();
    }

    public abstract class RtcControllerBase<T> : IRtcController
    {
        public T ArgumentModel {get;set;}
        public ApplicationContext Context {get;set;}

        public abstract ControllerResult Execute();
    }
}
