namespace Scripts.Common.Factories
{
    public interface IFactory<in T, out TR>
    {
        public TR Create(T param);
    }
}
