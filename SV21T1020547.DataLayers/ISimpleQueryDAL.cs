namespace SV21T1020547.DataLayers
{
    public interface ISimpleQueryDAL<T> where T : class
    {
        List<T> List();
    }
}
