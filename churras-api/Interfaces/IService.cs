using ChurrasAPI.Data;
using System.Threading.Tasks;

namespace ChurrasAPI.Interfaces
{
    public interface IService<T>
    {
        public void Criar(T obj);
        public void Remover(T obj);
        public T Buscar(params object[] keys);
        public Task<T> BuscarAsync(params object[] keys);
    }
}
