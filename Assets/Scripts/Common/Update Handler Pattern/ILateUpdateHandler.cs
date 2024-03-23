namespace Scripts.Common.UpdateHandlerPattern
{
    public interface ILateUpdateHandler
    {
        public void OnLateUpdate(float deltaTime);
    }
}
